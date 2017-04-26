using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace FARDD
{
    public partial class ShowOnEditFile : Form
    {
        public  static System.Windows.Forms.Timer  MyTimer = new System.Windows.Forms.Timer();    
        public static int counter = 10;
        public ShowOnEditFile( string args )
        {
            InitializeComponent( );
            this.textBox1.Text = args .Replace( "\n" , Environment.NewLine );
            this.button1.Text = "Закрыть [" + counter + "]";
            MyTimer.Interval = 1000;
            MyTimer.Tick +=  myTimer_Elapsed ;
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
