using Inventor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace FARDD
{
    public partial class Form1
    {
        private void errorWriter( string v )
        {
            string pathToLog = System.IO.Path.GetTempPath( ) + @"temp1207\";
            string nameToLog = "1207Erros.txt";
            string fullFileName = pathToLog + nameToLog;
            if( !Directory.Exists( pathToLog ) )
                Directory.CreateDirectory( pathToLog );
            if( !System.IO.File.Exists( fullFileName ) )
                // Create a new file 
                using( FileStream fs = System.IO.File.Create( fullFileName ) )
                {
                    fs.Flush( );
                    fs.Close( );
                }
            string text = "=={0}============================================\r\n " + DateTime.Now.ToString( ) + v;
            byte[] buff = Encoding.Default.GetBytes( text );
            using( System.IO.FileStream fs = new FileStream( fullFileName , FileMode.Append , FileAccess.Write ) )
            {
                fs.Write( buff , 0 , buff.Length );
                fs.Flush( );
                fs.Close( );
            }
        }

        //private void MyTimer()
        //{
        //    System.Timers.Timer tmr = new System.Timers.Timer( );
        //    tmr.Elapsed += new ElapsedEventHandler( OnTimedEvent );
        //    tmr.Interval = 1000; //Устанавливаем интервал в 1 сек.
        //    tmr.Enabled = true; //Вкючаем таймер.
        //    while( n != 10 ); //Таймер тикает 10 раза.
        //    S4App.ShowSearch( );
        //    CorrectExit( );
        //}

        private static void OnTimedEvent( object sender , ElapsedEventArgs e )
        {
            n++;
        }

        /// <summary>
        /// Основное событие прораммы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        /// Прверяет наличие расширения Inventor'а и имени файла в переданном массиве путей
        /// </summary>
        /// <param name="partPath">имя главного файла</param>
        /// <param name="listAdv">массив путей фалов для проверки</param>
        /// <returns>количество путей удовлетворяющих условию</returns>
        private int SelectTypeConvert( string partPath , string[] listAdv )
        {
            int res = 0;
            string end;
            string mask = partPath.Substring( 0 , partPath.Length - 4 );
            //нет допфайлов
            if( listAdv[ 0 ] == "-1" )
                return res;
            //в допфайлах есть файлы с расширением ipt iam и имени присутствует название головного файла
            foreach( string op in listAdv )
            {
                end = op.ToLower( ).Substring( op.Length - 3 );
                if( ( ( end == "iam" ) | ( end == "ipt" ) ) & ( -1 != op.IndexOf( mask , StringComparison.Ordinal ) ) )
                {

                    return res = 1;
                }
            }
            return res;

        }

        /// <summary>
        /// создет индивидуальную папку в %TEMP%\temp1207\%дата в строку%
        /// </summary>
        /// <param name="partPath"></param>
        /// <returns></returns>
        private string makeIndividualFolder( )
        {
            string folder = null;
            try 
            {

                DateTime date1 = DateTime.Now;
                folder = System.IO.Path.GetTempPath( ) + @"temp1207\" + date1.Year.ToString( );
                int[] dateMassive = new int[] { date1.Month , date1.Day , date1.Hour , date1.Minute , date1.Second };
                foreach( int a in dateMassive )
                    folder += ( ( a < 99 ) & ( a > 0 ) ) ? ( a * 1.0 / 99 ).ToString( ).Substring( 2 , 2 ) : "00";
                DirectoryInfo individualFolder = Directory.CreateDirectory( folder );
                folder = individualFolder.FullName;
                System.IO.File.SetAttributes( folder , FileAttributes.Normal );
            }
            catch
            {
                MessageBox.Show( "Не могу создать папку: " + folder , "Файл не сконвертируется" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                return null;
            }
            return folder;
        }

        /// <summary>
        /// Сохранение документа открытого в инвенторе
        /// </summary>
        /// <param name="tempPath"></param>
        private string SaveDoс( string tempPath, string openPath )
        {
            DialogResult RetryCancel = DialogResult.Cancel;
            do
            {
                try
                {
                    RetryCancel = DialogResult.Cancel;
                    string p = tempPath.ToLower( ).Substring( tempPath.Length - 4 , 4 );
                    if(( p == "step" )&( ( Mipt != null ) | ( Miam != null ) ) ){
                        textStatus.Text = "Сохрание 3D модели...         " + tempPath.ToString( );
                        ModelSaveAs( tempPath.ToString( ) );
                        Mipt = null;
                        Miam = null;
                        totalAdvStep++;
                    }
                    else if(( p == ".pdf" ) &( Midw != null ))
                    {
                        textStatus.Text = "Сохрание 2D чертежа...              " + tempPath.ToString( );
                        MidwSaveAs( tempPath.ToString( ) );
                        Midw = null;
                        totalAdvPDF++;
                    }
                }
                catch( Exception ex )
                {
                    
                    inventorApp.Visible = true;
                    string customMes = "Ошибка при сохранении: \n" +
                        ex.Message +
                        "\n\nФайл: " + tempPath +
                        "" +
                        "\n\nЕсли проблемма повториться, то необходимо обратиться в БСАПР ОАСУ 23-49 либо 22-35";
                    RetryCancel = MessageBox.Show( customMes , "Обработка" , MessageBoxButtons.RetryCancel , MessageBoxIcon.Error );
                    inventorApp.Visible = false;
                    if( RetryCancel == DialogResult.Cancel )
                        return null;
                    CloseDoc( );
                    textStatus.Text = "Завершение процесов Inventor'a";
                    Process[] localByName = Process.GetProcessesByName( "Inventor" );
                    foreach( var op in localByName ){ if( InventorProcIDs.IndexOf( op.Id ) == -1 ) op.Kill( );}
                    System.Type inventorType = System.Type.GetTypeFromProgID( CLSID );
                    textStatus.Text = "Создание объекта Inventor";
                    inventorApp = ( Inventor.Application )Activator.CreateInstance( inventorType );
                    tempPath = makeIndividualFolder( );
                    string trying = Convert( openPath , tempPath );
                }
                if( System.IO.File.Exists( tempPath ) )
                {
                    FileAttributes attributes = System.IO.File.GetAttributes( tempPath );
                        if( ( attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly )
                        {
                            attributes = attributes & FileAttributes.Normal;
                            System.IO.File.SetAttributes( tempPath , attributes );
                        }
                }
        } while( ( RetryCancel == DialogResult.Retry ) );

            return tempPath;
        }

        private void ModelSaveAs( string v )
        {
            Inventor.Document oDoc = inventorApp.ActiveDocument;
            inventorApp.ActiveDocument.SaveAs( v , true );
        }

        /// <summary>
        /// Сбор опций чертежа для сохранения в ПДФ
        /// </summary>
        /// <returns></returns>
        private NameValueMap GetSheetOptions()
        {
            NameValueMap oSheets = inventorApp.TransientObjects.CreateNameValueMap( );
            foreach( Sheet sheet in Midw.Sheets )
            {
                NameValueMap s1 = inventorApp.TransientObjects.CreateNameValueMap( );
                s1.Add( "Name" , sheet.Name );
                s1.Add( "3DModel" , false );
                oSheets.Add( string.Format( "Sheet{0}" , oSheets.Count + 1 ) , s1 );
            }
            return oSheets;
        }

        /// <summary>
        /// Сохранение PDF форматата с учетом нескольких листов
        /// </summary>
        /// <param name="p"></param>
        private void MidwSaveAs( string fullFileName )
        {
            try
            {
                TranslatorAddIn oPDFTrans = ( TranslatorAddIn )inventorApp.ApplicationAddIns.ItemById[ "{0AC6FD96-2F4D-42CE-8BE0-8AEA580399E4}" ];
                if( null == oPDFTrans )
                {
                    MessageBox.Show( "Could not load PDF Translator addin" , "Export Error" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                    return;
                }
                TranslationContext oContext = inventorApp.TransientObjects.CreateTranslationContext( );
                NameValueMap oOptions = inventorApp.TransientObjects.CreateNameValueMap( );
                if( oPDFTrans.HasSaveCopyAsOptions[ Midw , oContext , oOptions ] )
                {
                    oContext.Type = IOMechanismEnum.kFileBrowseIOMechanism;
                    DataMedium oData = inventorApp.TransientObjects.CreateDataMedium( );
                    oOptions.Value[ "Sheet_Range" ] = PrintRangeEnum.kPrintAllSheets;
                    oOptions.Value[ "Vector_Resolution" ] = 1200;
                    oOptions.Value[ "All_Color_AS_Black" ] = false;
                    oOptions.Value[ "Sheets" ] = GetSheetOptions( );
                    oData.FileName = fullFileName;
                    oPDFTrans.SaveCopyAs( Midw , oContext , oOptions , oData );
                    //HideExportDialog( "PDF created" ,  "Export Complete" ); 
                }
            }
            catch( Exception ex )
            {
                MessageBox.Show( "Ошибка при сохранении PDF: " + ex.Message + "\n\nФайл: " + fullFileName + "\n\nЕсли проблемма повториться, то необходимо обратиться в БСАПР ОАСУ 23-49 либо 22-35" , "Обработка" , MessageBoxButtons.OK , MessageBoxIcon.Error );

            }
        }

        /// <summary>
        /// При обрарушении в массиве путей ПДФ или ЗИП удаляет из и убирает из допфайлов
        /// </summary>
        /// <param name="spisAllAdv">массив путей к допфайлам</param>
        /// <param name="partPath">полный путь к файлу</param>
        private void KillOldStepPDF( string[] spisAllAdv , string partPath , string pattern )
        {

            string fName = GetPath( partPath )[ 1 ];
            string based = fName.Substring( 0 , fName.Length - 4 );
            partPath = GetPath( partPath )[ 0 ];
            foreach( string op in spisAllAdv )
            {

                if( op.IndexOf( based + ".zip" ) > -1 )
                {
                    S4App.RemoveAdvanFile( op );
                    DelFromDisk( partPath + @"\" + op );
                }
                //else if( op.IndexOf( "(screen).pdf" ) > -1 ) старое - УДАЛИТЬ
                //else if( op.IndexOf( mastToPDF ) > -1 ) старое - УДАЛИТЬ
                else if( op.IndexOf( pattern + ".pdf" ) > -1 )

                {
                    S4App.RemoveAdvanFile( op );
                    DelFromDisk( partPath + @"\" + op );
                }
            }
            return;
        }

        /// <summary>
        /// Удаляет файлы подлежащие замене в результате работы программы
        /// </summary>
        /// <param name="op">путь к удалеемому файлу</param>
        private void DelFromDisk( string op )
        {
            if( System.IO.File.Exists( op ) )
            {
                FileInfo source = new FileInfo( op );
                FileAttributes attributes = System.IO.File.GetAttributes( op );
                if( ( source.Exists ) )
                {
                    if( ( attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly )
                    {
                        attributes = attributes & FileAttributes.Normal;
                        System.IO.File.SetAttributes( op , attributes );
                    }
                }
                source.Delete( );
            }
        }

        /// <summary>
        /// Работа со строкой, Возвращает 0- элемент Путь к файлу 1- элемент Имя файла 
        /// </summary>
        /// <param name="fullPath">путь для разбиения</param>
        /// <returns> 0- элемент Путь к файлу 1- элемент Имя файла </returns>
        private string[] GetPath( string fullPath )
        {
            int pos = fullPath.LastIndexOf( '\\' );
            string[] part = { fullPath.Substring( 0 , pos ) , fullPath.Substring( pos + 1 , fullPath.Length - pos - 1 ) };
            return part;
        }

        /// <summary>
        /// Конвертирует файл в зависимости от расширения
        /// </summary>
        /// <param name="p">Путь для файла Inventor на конвертацию</param>
        /// <returns name="tempPath">вазвращает путь полученного при конвертыции файла</returns>
        private string Convert( string p , string tempDirToConvert )
        {
            string end;
            string[] partPath;
            string tempName = null;
            string nameConvertDoc = "";
            try
            {
                partPath = GetPath( p );
                string based = partPath[ 1 ].Substring( 0 , partPath[ 1 ].Length - 4 );//имя для допфайла
                tempName = based + ".step"; //имя сохранения степ

                end = p.ToLower( ).Substring( p.Length - 3 , 3 );
                switch( end )
                {
                    case "iam":
                        {
                            textStatus.Text = "Документ 3D сборки открывается      " + p;
                            doc = inventorApp.Documents.Open( p.ToString( ) , true );
                            Miam = doc as Inventor.AssemblyDocument;
                            nameConvertDoc = Miam.FullDocumentName;
                            break;
                        }
                    case "ipt":
                        {
                            textStatus.Text = "Документ 3D детали открывается        " + p;
                            doc = inventorApp.Documents.Open( p.ToString( ) , true );
                            Mipt = doc as Inventor.PartDocument;
                            nameConvertDoc = Mipt.FullDocumentName;
                            break;
                        }
                    case "idw":
                        {
                            try
                            {
                                
                                int keytoSign = 0;// ключ для проверки выгрузки документов на диск в временную папку
                                tempName = based + mastToPDF;
                                
                                try
                                {
                                    textStatus.Text = "Документ 2D чертеж сохраняется на диск      " + tempDirToConvert;
                                    S4App.SaveToDisk( tempDirToConvert , false );
                                }
                                catch
                                {
                                    keytoSign = -1;
                                    MessageBox.Show( "Не удалось сохранить файлы на диск по документу: \"" + p + "\"" , "Пропуск файла" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                                    tempName = null;
                                    break;
                                }
                                if( keytoSign == 0 )
                                {
                                    textStatus.Text = "Выгружаются на диск подписи и другие параметры для 2D чертежа      " + tempDirToConvert;
                                    string resLoadSigns = S4App.writeSignsAndParams2Doc( tempDirToConvert );
                                    //не удалось записать подписи в файл
                                    if( String.IsNullOrEmpty( resLoadSigns ) )
                                    {
                                        tempDirToConvert = partPath[ 0 ];
                                        MessageBox.Show( "Не удалось выгрузить подписи по документу: \"" + p + "\"" );
                                    }
                                }

                                textStatus.Text = "Документ 2D чертеж открывается        " + tempDirToConvert + @"\" + partPath[ 1 ];
                                doc = inventorApp.Documents.Open( tempDirToConvert + @"\" + partPath[ 1 ] , false );
                                //Midw = doc as Inventor.DrawingDocument;
                                Midw = editTextBox( tempDirToConvert + @"\" + partPath[ 1 ] );
                                nameConvertDoc = Midw.FullDocumentName;
                                break;
                            }
                            catch( Exception errr )
                            {
                                MessageBox.Show( errr.Message , "Конвертация PDF" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                                break;
                            }



                        }
                    default:
                        break;
                }
                Inventor.Documents docs = inventorApp.Documents;
                Document docInventor =  docs.get_ItemByName( nameConvertDoc );
                inventorObjToConvert.Add( docInventor );

            }
            catch( Exception ex )
            {
                DialogResult op = MessageBox.Show( "Ошибка во время открытия в Inventor: " + ex.StackTrace , "Обработка" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                if( DialogResult.OK == op )
                    CorrectExit( );
            }
            return tempName;
        }
        /// <summary>
        /// Открываем чертеж и удаляем записи о подписях и из даты
        /// </summary>
        /// <param name="firctPath"></param>
        /// <returns></returns>
        private DrawingDocument editTextBox( string firctPath )
        {
            doc = inventorApp.Documents.Open( firctPath , false );
            Inventor.DrawingDocument drawingDocument = doc as Inventor.DrawingDocument;
            Inventor.Sheets shts = drawingDocument.Sheets;
            Inventor.Sheet sht = drawingDocument.ActiveSheet;
            //находим Необходимый ресурс
            TitleBlock tBlock = sht.TitleBlock;
            TitleBlockDefinition tBlockDiff = tBlock.Definition;

            DrawingSketch dwSketch = null;
            //берем на редактирование эскиз
            tBlockDiff.Edit( out dwSketch );
            TextBoxes tBoxes = dwSketch.TextBoxes;
            string[] pattern = {
                "Разработал_ЭЦП" , "Разработал_ДАТА" ,
                "Проверил_ЭЦП" , "Проверил_ДАТА" ,
                "Т.контр._ЭЦП" , "Т.контр._ДАТА" ,
                "Нач. Бюро_ЭЦП" , "Нач. Бюро_ДАТА" ,
                "Начальник отдела_ЭЦП" , "Начальник отдела_ДАТА" ,
                "Н.контр._ЭЦП" , "Н.контр._ДАТА" ,
                "Утв._ЭЦП" , "Утв._ДАТА" };

            TransientObjects oTOs = inventorApp.TransientObjects;

            Inventor.Color oTopColor = oTOs.CreateColor( 0 , 0 , 0 );
            double fSize = 0;
            foreach( Inventor.TextBox op in tBoxes )
            {

                fSize =(fSize == 0) ? op.Style.FontSize : fSize;
                foreach( string pattItem in pattern )
                {
                    if( op.Text.IndexOf( pattItem ) > 0 )
                    {
                        if( op.Text.IndexOf( "Разработал_ДАТА" ) > 0 )
                        {
                            op.Style.Color = oTopColor;
                            op.Style.FontSize = 0.3;
                            op.Text = DateTime.Now.ToString( "dd.MM.yyyy" );
                        }
                        else
                            op.Text = " ";
                        break;
                    }
                }
            }
            tBlockDiff.ExitEdit( true );
            return drawingDocument;
        }

        /// <summary>
        /// Возвращает массив  допфайлов открытого документа
        /// </summary>
        private string[] CheckAddFile()
        {
            string list = S4App.GetAdvanFilesList( );
            string formatStr;
            string[] spisDopov = { "-1" };
            if( !String.IsNullOrEmpty( list ) )
            {
                formatStr = list.Substring( 0 , list.Length - 2 );//убирает "\r\n" вконце строки
                spisDopov = formatStr.Split( new string[] { "\r\n" } , StringSplitOptions.None );
            }
            return spisDopov;
        }

        /// <summary>
        /// Метод для коректного завершения
        /// Убивает процесс согданного INVENTOR.exe
        /// </summary>
        private void CorrectExit()
        {
            try
            {
                if( inventorApp != null )
                {
                    inventorApp.Visible = true;
                    CloseDoc( );
                }
                Process[] localByName = Process.GetProcessesByName( "Inventor" );

                foreach( var op in localByName )
                {
                    if( InventorProcIDs.IndexOf( op.Id ) == -1 )
                        op.Kill( );
                }
            }
            catch(   Exception exex)
            {
                //MessageBox.Show( "Произошла ошибка при во время корректного выхода: " + exex.Message + "\nin line(" + exex.StackTrace.Substring( exex.StackTrace.Length-3) + ")\n" , "Ошибка при завершении" , MessageBoxButtons.OK , MessageBoxIcon.Information );
            }
            System.Environment.Exit( 0 );
        }

        /// <summary>
        /// Закрывает документ открытый в инвенторе
        /// </summary>
        private void CloseDoc()
        {
            try
            {
                Inventor.Documents docs;
                docs = inventorApp.Documents;
                if( 0 != docs.VisibleDocuments.Count )
                {
                    foreach( Document op in inventorObjToConvert )
                    {
                        op.Close( false );
                    }
                }
            }
            catch
            {
                ErrorCloseDoc ecd = new ErrorCloseDoc( );
                ecd.ShowDialog( );
                //MessageBox.Show( "Не могу корректно закрыть файл","Уведомление",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Подготовка приложания к работе, запись работающих процессов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start( object sender , EventArgs e )
        {
            using( Graphics g = CreateGraphics( ) )
            {
                int W = ( int )( Screen.PrimaryScreen.Bounds.Width );
                int H = ( int )( Screen.PrimaryScreen.Bounds.Height );
                this.Location = new System.Drawing.Point( ( W - 341 ) / 2 , ( H - 102 ) / 2 );
            }
            progBar.Value = 5;
            ProcInventor( );
            protMultyStart( );

        }

        /// <summary>
        /// Защита от запуска нескольких версий программы конвертации
        /// </summary>
        private void protMultyStart()
        {
            DialogResult kok;
            do
            {
                kok = DialogResult.No;
                ProcFARDD( );
                if( FARDDProcIDs.Count > 1 )
                {
                    string mess = "Найденo \"" + FARDDProcIDs.Count + "\" процесс(а) конвертирования\n\r" +
                    "необходимо из завершить или дождаться их выполнения\n\n\r" +
                    "Выберите дейтвие:\n\n\r" +
                    "[YES] - принудительно завершить ранее запущенные процесы(процессы \"зависли\")\n\n\r" +
                    "[NO] - ничего не закрывать и попробовать еще раз(процесы уже сами завершились)\n\n\r" +
                    "[Cansel] - отказаться от конвертации файлов запущенных по текущему моршруту\n\n\r";
                    kok = MessageBox.Show( mess , "Внимание!" , MessageBoxButtons.YesNoCancel , MessageBoxIcon.Warning );
                    if( kok == DialogResult.Yes )
                    {
                        KillProsess( FARDDProcIDs );
                    }
                    if( kok == DialogResult.Cancel )
                        CorrectExit( );
                }
            } while( ( FARDDProcIDs.Count > 1 ) & ( kok == DialogResult.No ) );
        }

        private void KillProsess( List<int> fARDDProcIDs )
        {
            try
            {
                Process currentProcess = Process.GetCurrentProcess( );
                foreach( int op in fARDDProcIDs )
                {
                    if( op != currentProcess.Id )
                    {
                        Process killMe = Process.GetProcessById( op );
                        killMe.Kill( );
                    }

                }
            }
            catch( Exception erProc )
            {
                MessageBox.Show( erProc.Message , "" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                CorrectExit( );
            }
        }

        /// <summary>
        /// Проверка и поиск процессов инвентора, 
        /// </summary>
        private void ProcInventor()
        {
            getProcessInventor( );
            if( InventorProcIDs.Count > 1 )
            {   
                MultiInventorFoundForm miff = new MultiInventorFoundForm( );
                if( DialogResult.OK == miff.ShowDialog( ) )
                {
                    foreach( int op in InventorProcIDs )
                    {
                        Process.GetProcessById( op ).CloseMainWindow();
                    }
                    KillProsess( InventorProcIDs );
                }
            }

        }
        
        private void getProcessInventor()
        {
            try
            {
                Process[] localByName = Process.GetProcessesByName( "Inventor" );
                if( ( localByName != null ) & ( localByName.Length > 0 ) )
                {
                    foreach( var op in localByName )
                    {
                        InventorProcIDs.Add( op.Id );
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Проверка и поиск процессо 1207.exe
        /// </summary>
        private void ProcFARDD()
        {
            try
            {
                Process[] localByName = Process.GetProcessesByName( "1207" );
                if( ( localByName != null ) & ( localByName.Length > 0 ) )
                {
                    foreach( var op in localByName )
                    {
                        FARDDProcIDs.Add( op.Id );
                    }
                }
            }
            catch
            {

            }
        }

        private Inventor.Application InitializeInventor( bool makeVisible )
        {
            Inventor.Application application = null;
            if( inventorApp != null )
            {
                inventorApp.Visible = true;
                CloseDoc( );
                Process[] localByName = Process.GetProcessesByName( "Inventor" );

                foreach( var op in localByName )
                {
                    if( InventorProcIDs.IndexOf( op.Id ) == -1 )
                        op.Kill( );
                }
            }
            else
            {
                // Try to get running instance
                try
                {
                    application = ( Inventor.Application )System.Runtime.InteropServices.Marshal.GetActiveObject( "Inventor.Application" );
                }
                catch( Exception ex )
                {
                    System.Diagnostics.Debug.WriteLine( ex.Message );
                }
                // no running instance was found or if you want a new instance, create one
                if( null == application )
                {
                    try
                    {
                        System.Type inventorType = System.Type.GetTypeFromProgID( CLSID );
                        if( null == inventorType )
                        {
                            // Cannot find registered typeID in the registery for Inventor
                            return null;
                        }
                        application = ( Inventor.Application )Activator.CreateInstance( inventorType );
                    }
                    catch( Exception ex )
                    {
                        System.Diagnostics.Debug.WriteLine( ex.Message );
                    }
                }
            }
            if( null != application )
            {
                application.Visible = makeVisible;
            }
            else
                throw new Exception( "объект Inventor не был создан");
            return application;
        }

        private int getDocEndUserEditing()
        {
            int res = 0;
            // Get the active documents.

            Inventor.Documents docs;
            docs = inventorApp.Documents;
            DialogResult dialogResult;
            if( 0 != docs.VisibleDocuments.Count )
            {
                inventorApp.Visible = true;
                string dosumOpenList = null;
                foreach( Document op in docs )
                {
                    dosumOpenList += op.DisplayName + "\n";

                }

                // MessageBox.Show( "открыто " + docs.VisibleDocuments.Count + " документ(а)\n" + dosumOpenList );

                dialogResult = MessageBox.Show( "У Вас открыто " + docs.VisibleDocuments.Count + @" документ(а) в Inventor 2014
для корректной работы необходимо закрыть эти документы.
Продолжить?

Закройте эти документы и нажмите Да.

Если Вы отказываетесь от конвертации нажмите Нет.


" , "Внимание!" , MessageBoxButtons.YesNo , MessageBoxIcon.Warning );
                if( dialogResult == DialogResult.No )
                {
                    dialogResult = MessageBox.Show( "Пропустить конвертыцию?" , "Подтверждение" , MessageBoxButtons.YesNo , MessageBoxIcon.Hand );
                    if( dialogResult == DialogResult.Yes )
                    {
                        CorrectExit( );
                    }
                }
                res = -1;

            }

            return res;
        }


    }
}
