using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inventor;

namespace ConsoleApplication_INVENTOR
{
    class Program
    {
        static void Main( string[] args )
        {
            string path = @"C:\Users\52758\AppData\Local\Temp\temp1207\20161021101318\test 29 СБ_2D.idw";
            ApprenticeServerComponent oApprentice = new ApprenticeServerComponent( );
            ApprenticeServerDocument oDoc = oApprentice.Open( path );

            PropertySets oPropertySets = oDoc.PropertySets;// ( "{F29F85E0-4FF9-1068-AB91-08002B27B3D9}" )
            foreach( PropertySet op in oPropertySets )
            {
                Console.WriteLine( "{0}" , op.InternalName );
                Console.ReadKey( );
            }
        }
    }
}
