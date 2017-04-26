using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S4;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;

namespace ConsoleApplication1
{
    class Consoles
    {
        static void Main( string[] args )
        {

            S4.TS4App S4App = null;
            S4App = new TS4App( );

            //Process.Start( @"\\sql - main\IM\SEARCH\imlogin.exe", "username = 52673 password = 654321" );

            ProcessStartInfo startInfo = new ProcessStartInfo( @"\\sql - main\IM\SEARCH\imlogin.exe" );
            startInfo.WindowStyle = ProcessWindowStyle.Minimized;

            Process.Start( startInfo );

            startInfo.Arguments = "username = 52673 password = 654321";

            Process.Start( startInfo );



            int log = S4App.Login( );
            if( log != 1 )
                Console.WriteLine( "Ошибка" );
            else
                Console.WriteLine( "Успешно" );
            Console.ReadKey( );
        }
    }
}

