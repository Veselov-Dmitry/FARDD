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
    public partial class StartOrNotForm : Form
    {
        public StartOrNotForm()
        {
            InitializeComponent( );
        }

        private void button2_Click( object sender , EventArgs e )
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click( object sender , EventArgs e )
        {
            DialogResult = DialogResult.OK;
            this.Close( );
        }
    }
}
