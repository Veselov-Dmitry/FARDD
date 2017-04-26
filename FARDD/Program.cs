using System;
using System.Windows.Forms;

namespace FARDD
{
    static class Program
    { 
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles( );
            Application.SetCompatibleTextRenderingDefault( false );
            StartOrNotForm son = new StartOrNotForm( );
            son.TopMost = false;
            son.TopMost = true;
            if(DialogResult.OK ==  son.ShowDialog( ) )
            {
                string inputParams = "";
                try
                {
                    try
                    {
                        inputParams = args[ 0 ];
                    }

                    catch { }
                    finally
                    {
                        //проверка на пустой входной параметр
                        if( String.IsNullOrWhiteSpace( inputParams ) )
                        {
                            Form2 fTwo = new Form2( );
                            //цикл на постое поле в форме по ОК
                            do
                            {
                                fTwo.ShowDialog( );
                                if( fTwo.DialogResult == DialogResult.OK )
                                {
                                    inputParams = fTwo.textBox1.Text;
                                }
                                else
                                    System.Environment.Exit( 0 );
                            }
                            while( String.IsNullOrWhiteSpace( inputParams ) );
                        }
                        //проверка  на ввод не числа в массиве
                        foreach( string op in inputParams.Split( new Char[] { ' ' , ',' , '.' , ':' , '\t' , ';' , '-' } ) )
                        {
                            int n;
                            if( !String.IsNullOrEmpty( op ) & !int.TryParse( op , out n ) )
                            {
                                throw new Exception( "" );
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show( "Программа не работает без входных параметров\n в виде инвентарных номеров документов\n(тип string,  возможные разделители: ' ', ',', '.', ':', '\\t', ';', '-' )" , "Входные параметры" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                    System.Environment.Exit( 0 );
                }
                Application.Run( new Form1( inputParams ) );
            }
        }
    }

            
            
}
