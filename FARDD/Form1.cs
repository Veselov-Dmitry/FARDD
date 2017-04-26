// обоснование 4.5 net framework http://help.autodesk.com/view/INVNTOR/2014/ENU/?guid=GUID-738A4668-B0FB-456C-89C9-F83C12CBAD40
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using S4;
using System.Threading;
using System.Text;
using Ionic.Zlib;
using System.Runtime.InteropServices;
using System.IO;

namespace FARDD
{
    public partial class Form1 : Form
    {
        private Inventor.Application inventorApp = null;
        private const string CLSID = "Inventor.Application";
        private Inventor.DrawingDocument Midw = null;
        private Inventor.AssemblyDocument Miam = null;
        //private string mastToPDF = "_screen.pdf";
        private string mastToPDF = ".pdf";
        private Inventor.PartDocument Mipt = null;
        private Inventor.Document doc;
        /// <summary>
        /// список объектов документов инвентора, которые открываются при конверт
        /// </summary>
        private List<Inventor.Document> inventorObjToConvert = new List<Inventor.Document>();
        private S4.TS4App S4App;
        private string[] docIDstring;
        private List<int> docIDs;
        /// <summary>
        /// всего документов для конвертации
        /// </summary>
        private int totalDocAll = 0;
        /// <summary>
        /// всего доп файлов для конвертаци
        /// </summary>
        private int totalAdvAll = 0;
        /// <summary>
        /// всего документов прошло конвертацию
        /// </summary>
        private int totalDoc = 0;
        /// <summary>
        /// всего доп файлов прошло конвертацию
        /// </summary>
        private int totalAdv = 0;
        /// <summary>
        /// всего STEP прошло конвертацию
        /// </summary>
        private int totalAdvStep = 0;
        /// <summary>
        /// всего PDF файлов прошло конветранию
        /// </summary>
        private int totalAdvPDF = 0;
        /// <summary>
        /// приращение для прогрессбара на файлы
        /// </summary>
        private int itr;
        /// <summary>
        /// приращение для прогрессбара на доп файлы
        /// </summary>
        private int itrAdv;
        private static List<int> InventorProcIDs = new List<int>();
        private static List<int> FARDDProcIDs = new List<int>();
        //public static string fin;
        /// <summary>
        /// для таймера перед закрытием программы 10 сек.
        /// </summary>
        private static int n; 
        static int nDoc = 0; //счетчик обработки документов из списка переданных через параметры при запуске
        public string zipPath = @"";

        public ShowOnEditFile Instance
        {
            get;
            set;
        }

        public Form1( string args )
        {
            getArray( args );
            InitializeComponent( );
        }
        
        private void getArray( string args )
        {
            char p = args[ args.Length - 1 ];
            if( char.IsDigit( p ) )
                docIDstring = args.Split( new Char[] { ' ' , ',' , '.' , ':' , '\t' , ';' , '-' } );
            else
                docIDstring = ( args.Substring( 0 , args.Length - 1 ) ).Split( new Char[] { ' ' , ',' , '.' , ':' , '\t' , ';' , '-' } );
        }

        private void Go( object sender , EventArgs e )
        {
            try
            {
                DialogResult errorAnswer = DialogResult.Cancel;
                do
                {
                    try
                    {
                        inventorApp = null;
                        S4App = null;
                        CreateObjs( );
                        //тихий режим в инвенторе
                        inventorApp.SilentOperation = true;
                        progBar.Value = 32;
                        itr = System.Convert.ToInt32( ( 100 - progBar.Value ) / docIDs.Count );
                        editCheck( );
                        try
                        {
                            for( nDoc = 0 ; nDoc < docIDs.Count ; nDoc++ )
                            {
                                processMakeConvertation( nDoc );
                            }
                        }
                        catch( Exception exfor )
                        {
                            MessageBox.Show( "Произошла ошибка в цикле перехода по документам: " + exfor.Message + "\nin line(" + exfor.StackTrace.Substring( exfor.StackTrace.Length - 3 ) + ")\n" ,
                                           "Ошибка при выполнении" ,
                                           MessageBoxButtons.RetryCancel ,
                                           MessageBoxIcon.Error );

                        }
                        inventorApp.SilentOperation = false;
                        textStatus.Text = "Готово";
                        progBar.Value = 100;
                        //Thread myThread = new Thread( MyTimer );
                        //myThread.Start( );
                        string message =
@"Готово!
Обработано документов - [" + totalDoc + @"]
Файлов исполнений - [" + totalAdv + @"]            
Всего файлов *.step - [" + totalAdvStep + @"]                            
Всего файлов *.pdf - [" + totalAdvPDF + @"]   
                         
Нажмите ОК либо подождите 10 секунд, окно закроется автоматически";
                        string error = " ";
                        if( ( totalDocAll - totalDoc > 0 ) | ( totalAdvAll - totalAdv > 0 ) )
                            error =
@"Неудалось сконвертировать:
файлов [" + ( totalDocAll - totalDoc ) + @"]
доп. файлов  [" + ( totalAdvAll - totalAdv ) + "]";

                        FinishBox fB = new FinishBox( message , error );
                        fB.ShowDialog( );
                        CorrectExit( );

                    }
                    catch( Exception er )
                    {
                        errorAnswer = MessageBox.Show( "Произошла ошибка при переходе к документу: " + er.Message ,
                            "Ошибка при выполнении" ,
                            MessageBoxButtons.RetryCancel ,
                            MessageBoxIcon.Error );
                        if( errorAnswer == DialogResult.Cancel )
                            continue;
                    }
                } while( errorAnswer == DialogResult.Retry);

            }catch(Exception exgo )
            {
                MessageBox.Show( "Произошла ошибка в процессе перехода по документам: " + exgo.Message + "\nin line(" + exgo.StackTrace.Substring( exgo.StackTrace.Length-3) + ")\n",
                               "Ошибка при выполнении" ,
                               MessageBoxButtons.RetryCancel ,
                               MessageBoxIcon.Error );

            }
        }
        /// <summary>
        /// проверка на редактирование документа и вывод если есть
        /// </summary>
        private void editCheck()
        {

            StringBuilder sbList = new StringBuilder( );
            sbList.Append( "Документы взятые на изменение:\n\n" );
            bool isShow = false;
            foreach( int op in docIDs )
            {
                S4App.OpenDocument( op );
                int stat = S4App.GetDocStatus( );
                if( stat != 0 )
                {
                    sbList.Append( "\"" + Path.GetFileNameWithoutExtension( S4App.GetDocFilename( op ) ) + "\"(" + S4App.GetUserFullName_ByUserID( stat ) + ")\n" );
                    isShow = true;
                }
                S4App.CloseDocument( );
            }
            if( isShow )
            {
                ShowOnEditFile soef = new ShowOnEditFile( sbList.ToString( ) );
                soef.ShowDialog( );
            }
        }

        private void processMakeConvertation( int i )
        {
            textStatus.Text = "Обработка инвентарного номера документа...";
            string[] listAdv = { "-1" };
            string[] partPath;
            string advFile = "";
            string fullPathDocID = null;
            string temp1207WhithDate = null;
            int select;
            DialogResult openDoc = DialogResult.Ignore;
            if( inventorApp == null )
                CorrectExit( );
            do
            {
                string query = "SELECT COUNT(DOC_ID) AS ANSWER FROM Search.dbo.DOCLIST WHERE DOC_ID = '" + docIDs[ i ] + "'";
                S4App.OpenQuery( query );
                //проверка на существование
                if( Int32.Parse( S4App.QueryFieldByName( "ANSWER" ) ) == 0 )
                {
                    MessageBox.Show( "Документа с инвентарным номером \"" + docIDs[ i ] + "\" не существует в Search" );
                    return;
                }
                S4App.OpenDocument( docIDs[ i ] );
                fullPathDocID = S4App.GetDocFilename( docIDs[ i ] );
                if( String.IsNullOrEmpty( fullPathDocID ) )
                    openDoc = MessageBox.Show( "Не найден " + ( i + 1 ) + " документ с инвентарным номером:\"" + docIDs[ i ] + "\"" , "Ошибка" , MessageBoxButtons.AbortRetryIgnore , MessageBoxIcon.Error );
            } while( DialogResult.Retry == openDoc );
            if( DialogResult.Abort == openDoc )
                CorrectExit( );
            else
            {
                partPath = GetPath( fullPathDocID );
                try
                {
                    textStatus.Text = "Синхронизация документа...";
                    int sync = S4App.SyncDocument( );
                    if( sync == 0 )
                    {
                        //синхронизация ненужна
                    }
                    else if( sync == 1 )
                    {
                        //синхронизация произведена успешно
                    }
                    else
                    {
                        throw new Exception( "синхронизация завершилаь с результатом :\"" + sync + "\"" );
                    }

                }
                catch( Exception syncEr )
                {
                    MessageBox.Show( "Документ инв№:  \"" + docIDs[ i ] + "\" не может быть синхронизирован.\n\rПричина :" + syncEr.Message , "Пропуск файла" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                    return;
                }

                listAdv = CheckAddFile( );
                //почистим список допфайлов от отличных от  "iam" "idw" "ipt"
                if( listAdv[ 0 ] != "-1" )
                {
                    textStatus.Text = "Проверка списка доп. файлов...";
                    KillOldStepPDF( listAdv , S4App.GetDocFilename( docIDs[ i ] ), partPath[0].Substring(0 , partPath[ 0 ].Length - 1) );
                    listAdv = CheckAddFile( );
                }
                temp1207WhithDate = makeIndividualFolder( );
                if( String.IsNullOrWhiteSpace( temp1207WhithDate ) )
                    return;
                select = SelectTypeConvert( partPath[ 1 ] , listAdv );
                if( select != 0 )
                //в допфайлах есть файлы для конвертации - -работаем только с ними
                {
                    int cnt = 0;
                    itrAdv = itr / listAdv.Length;
                    foreach( string op in listAdv )
                    {
                        string p = op.ToLower( ).Substring( op.Length - 3 , 3 );
                        if( ( p == "iam" ) | ( p == "idw" )| ( p == "ipt" ) )
                        {
                            totalAdvAll++;
                            textStatus.Text = "Открытие доп. файлa " + totalAdv + " из " + listAdv.Length;
                            string openPath = partPath[ 0 ] + @"\" + op;
                            advFile = Convert( openPath , temp1207WhithDate );
                            if( !String.IsNullOrWhiteSpace( advFile ) )
                            {
                                string appendFile = SaveDoс( temp1207WhithDate + @"\" + advFile, openPath );
                                cnt++;
                                progBar.Value += itrAdv ;
                                if( appendFile == null )
                                    continue;
                                else
                                {
                                    totalAdv++;
                                }
                            }
                        }
                    }
                    zipPath = partPath[ 0 ] + @"\" + partPath[ 1 ].Substring( 0 , partPath[ 1 ].Length - 3 ) + "zip";
                    try
                    {
                        if( cnt > 0)
                        {
                            textStatus.Text = "Создание архива";
                            using( var zipFile = new Ionic.Zip.ZipFile( zipPath , Encoding.GetEncoding( "cp866" ) ) )
                            {
                                zipFile.AlternateEncoding = System.Text.Encoding.GetEncoding( 866 );
                                zipFile.CompressionLevel = CompressionLevel.None;
                                zipFile.AddItem( temp1207WhithDate );
                                zipFile.Save( );
                            }
                        }
                    }
                    catch( Exception zipEx )
                    {
                        MessageBox.Show( "ошибка при создании zip архива " + zipPath + ".\n\r Причина:" + zipEx.Message + "\"" , "Пропуск файла" );
                    }
                    textStatus.Text = "Добавление доп. файла в Search";
                    S4App.AppendAdvanFile2( zipPath , 2 );
                    DelFromDisk( zipPath );
                }
                else
                //нет допфайлов для конвертации - работает с одним головным фалом
                {
                    string p = partPath[ 1 ].ToLower( ).Substring( partPath[ 1 ].Length - 3 , 3 );
                    if( ( p == "iam" ) | ( p == "idw" ) /*| ( p == "ipt" ) */)
                    {
                        totalDocAll++;
                        textStatus.Text = "Открытие файлa " + nDoc + " из " + docIDs.Count;
                        string openPath = partPath[ 0 ] + @"\" + partPath[ 1 ];
                        advFile = Convert( openPath , temp1207WhithDate );
                        if( !String.IsNullOrWhiteSpace( advFile ) )
                        {
                            string appendFile = SaveDoс( temp1207WhithDate + @"\" + advFile , openPath  );
                            if( appendFile != null )
                            {
                                try
                                {
                                    File.Copy( appendFile , partPath[ 0 ] + @"\" + Path.GetFileName( appendFile ) , true );
                                }
                                catch { }
                                //
                                textStatus.Text = "Добавление доп. файла в Search";
                                S4App.AppendAdvanFile2( appendFile , 2 );
                                totalDoc++;
                            }
                            //DelFromDisk( appendFile );
                        }
                    }
                    
                    progBar.Value += itr;
                }


                CloseDoc( );
                S4App.CloseDocument( );
            }
        }



        /// <summary>
        /// Создание COM объектов 
        /// </summary>
        private void CreateObjs()
        {
            try
            {
                docIDs = new List<int>(); 
                foreach (string op in docIDstring)
                    docIDs.Add(int.Parse(op));
                progBar.Value = 10;
                textStatus.Visible = true;
                textStatus.Text = "Создание объекта Search...";
                S4App = new TS4App();
                int log = S4App.Login();
                if (log != 1) 
                    throw new Exception("Не создан COM объект Search!");
                progBar.Value = 12;

                textStatus.Text = " Создание объектa Inventor...";
                inventorApp = InitializeInventor( false/*Visible*/ );
                progBar.Value = 30;
                if( inventorApp == null )
                {
                    throw new Exception( "Объект Inventor не создан");
                }

            }
            catch (Exception er)
            {
                MessageBox.Show("Ошибка создания объектов. \nError:" + er.Message + "\nin line(" + er.StackTrace.Substring( er.StackTrace.Length - 3 ) + ")\n" , "Er");
                CorrectExit();
            }
            return;
        }

    }
}
