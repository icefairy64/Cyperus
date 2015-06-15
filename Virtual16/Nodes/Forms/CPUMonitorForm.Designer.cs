namespace Virtual16.Nodes.Forms
{
    partial class CPUMonitorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelStatus = new System.Windows.Forms.Label();
            this.RegisterStatusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelStatus.Location = new System.Drawing.Point(12, 9);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(19, 88);
            this.labelStatus.TabIndex = 0;
            this.labelStatus.Text = "A\r\nBC\r\nDE\r\nHL\r\nSP\r\nIP\r\nC\r\nZ";
            // 
            // RegisterStatusLabel
            // 
            this.RegisterStatusLabel.AutoSize = true;
            this.RegisterStatusLabel.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RegisterStatusLabel.Location = new System.Drawing.Point(40, 9);
            this.RegisterStatusLabel.Name = "RegisterStatusLabel";
            this.RegisterStatusLabel.Size = new System.Drawing.Size(40, 88);
            this.RegisterStatusLabel.TabIndex = 0;
            this.RegisterStatusLabel.Text = "$00\r\n$0000\r\n$0000\r\n$0000\r\n$EFFF\r\n$0000\r\n0\r\n0";
            // 
            // CPUMonitorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(94, 106);
            this.Controls.Add(this.RegisterStatusLabel);
            this.Controls.Add(this.labelStatus);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "CPUMonitorForm";
            this.Text = "CPUMonitorForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label RegisterStatusLabel;
    }
}