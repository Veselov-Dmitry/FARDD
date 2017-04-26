using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main( string[] args )
        {
            DateTime date1 = DateTime.Now;
            Console.WriteLine( System.IO.Path.GetTempPath( ) );
            Console.WriteLine( "Year " + date1.Year );
            Console.WriteLine( "Month " + date1.Month );
            Console.WriteLine( "Day " + date1.Day );
            Console.WriteLine( "Hour " + date1.Hour );
            Console.WriteLine( "Minute " + date1.Minute );
            Console.WriteLine( "Second " + date1.Second );
            Console.WriteLine( "" + date1.Year + date1.Month + date1.Day + date1.Hour + date1.Minute + date1.Second );
            Console.WriteLine( "" + date1.Year + point( date1.Month ) + point( date1.Day) + point( date1.Hour ) + point( date1.Minute ) + point( date1.Second ) );
            
            Console.ReadKey( );
        }

        public static string point( int a )
        {
            //string b = null;
            //if( a < 99 )
            //{
            //    double c = a * 1.0 / 99;
            //    b = c.ToString( ).Substring(2,2);
            //}
            //return b;
            return ( ( a < 99 ) & ( a > 0 ) ) ? ( a * 1.0 / 99 ).ToString( ).Substring( 2 , 2 ):"00";

        }
    }
}
