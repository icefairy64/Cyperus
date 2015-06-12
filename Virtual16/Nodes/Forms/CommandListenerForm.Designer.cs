namespace Virtual16.Nodes.Forms
{
    partial class CommandListenerForm
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
            this.Log = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // Log
            // 
            this.Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Log.Location = new System.Drawing.Point(12, 12);
            this.Log.Name = "Log";
            this.Log.Size = new System.Drawing.Size(453, 389);
            this.Log.TabIndex = 1;
            this.Log.Text = "";
            // 
            // CommandListenerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 413);
            this.Controls.Add(this.Log);
            this.Name = "CommandListenerForm";
            this.Text = "Command Listener";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.RichTextBox Log;

    }
}