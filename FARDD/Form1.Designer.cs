namespace FARDD
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.progBar = new System.Windows.Forms.ProgressBar();
            this.textStatus = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // progBar
            // 
            this.progBar.BackColor = System.Drawing.SystemColors.Window;
            this.progBar.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.progBar.ForeColor = System.Drawing.Color.YellowGreen;
            this.progBar.Location = new System.Drawing.Point(12, 12);
            this.progBar.Maximum = 101;
            this.progBar.Name = "progBar";
            this.progBar.Size = new System.Drawing.Size(311, 55);
            this.progBar.TabIndex = 0;
            this.progBar.UseWaitCursor = true;
            this.progBar.Value = 5;
            // 
            // textStatus
            // 
            this.textStatus.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.textStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textStatus.Enabled = false;
            this.textStatus.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textStatus.Location = new System.Drawing.Point(12, 67);
            this.textStatus.Multiline = true;
            this.textStatus.Name = "textStatus";
            this.textStatus.ReadOnly = true;
            this.textStatus.Size = new System.Drawing.Size(311, 29);
            this.textStatus.TabIndex = 4;
            this.textStatus.TabStop = false;
            this.textStatus.Text = "состояние...";
            this.textStatus.UseWaitCursor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(334, 92);
            this.Controls.Add(this.textStatus);
            this.Controls.Add(this.progBar);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(20, 20);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Converting files...";
            this.UseWaitCursor = true;
            this.Load += new System.EventHandler(this.Start);
            this.Shown += new System.EventHandler(this.Go);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progBar;
        private System.Windows.Forms.TextBox textStatus;
    }
}

