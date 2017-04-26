namespace FARDD
{
    partial class FinishBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose( );
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.textErrorRed = new System.Windows.Forms.TextBox();
            this.textReport = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(78, 274);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 27);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textErrorRed
            // 
            this.textErrorRed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textErrorRed.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textErrorRed.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textErrorRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textErrorRed.ForeColor = System.Drawing.Color.Red;
            this.textErrorRed.Location = new System.Drawing.Point(12, 12);
            this.textErrorRed.Multiline = true;
            this.textErrorRed.Name = "textErrorRed";
            this.textErrorRed.Size = new System.Drawing.Size(253, 55);
            this.textErrorRed.TabIndex = 1;
            this.textErrorRed.Text = "Текст";
            // 
            // textReport
            // 
            this.textReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textReport.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textReport.ForeColor = System.Drawing.Color.Black;
            this.textReport.Location = new System.Drawing.Point(12, 73);
            this.textReport.Multiline = true;
            this.textReport.Name = "textReport";
            this.textReport.Size = new System.Drawing.Size(253, 195);
            this.textReport.TabIndex = 2;
            this.textReport.Text = "Текст";
            // 
            // FinishBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 313);
            this.Controls.Add(this.textReport);
            this.Controls.Add(this.textErrorRed);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FinishBox";
            this.ShowIcon = false;
            this.Text = "Complite";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textErrorRed;
        private System.Windows.Forms.TextBox textReport;
    }
}