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
    public partial class FinishBox : Form
    {
        public  static System.Windows.Forms.Timer  MyTimer = new System.Windows.Forms.Timer();
        public static int counter = 10;

        public FinishBox()
        {
            InitializeComponent( );

        }

        public FinishBox( string message, string error )
        {
            InitializeComponent( );
            this.textReport.Text = message;
            this.textErrorRed.Text = error;
            counter = 10;

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
            this.button1.Text = "OK [" + counter + "]";
        }

        private void button1_Click( object sender , EventArgs e )
        {
            this.Close( );
        }
    }
}
