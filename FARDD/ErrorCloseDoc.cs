using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FARDD
{
    public partial class ErrorCloseDoc : Form
    {
        public  static System.Windows.Forms.Timer  MyTimer = new System.Windows.Forms.Timer();
        public static int counter = 8;
        
        public ErrorCloseDoc( )
        {
            InitializeComponent( );
            counter = 8;

            this.button1.Text = "OK [" + counter + "]";
            MyTimer.Interval = 1000;
            MyTimer.Tick += myTimer_Elapsed;
            MyTimer.Start( );
        }

        private void myTimer_Elapsed( object sender , EventArgs e )
        {
            counter = counter - 1;
            if( counter < 0 )
            {
                this.Hide( );
                this.Close( );
            }
            this.button1.Text = "Закрыть [" + counter + "]";
        }

        private void button1_Click( object sender , EventArgs e )
        {
            this.Close( );
        }
    }
}
