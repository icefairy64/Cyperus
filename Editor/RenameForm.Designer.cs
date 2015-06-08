namespace Cyperus.Designer
{
    partial class RenameForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.NewNameBox = new System.Windows.Forms.TextBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButtonM = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter new name:";
            // 
            // NewNameBox
            // 
            this.NewNameBox.Location = new System.Drawing.Point(15, 37);
            this.NewNameBox.Name = "NewNameBox";
            this.NewNameBox.Size = new System.Drawing.Size(222, 20);
            this.NewNameBox.TabIndex = 1;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(243, 12);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButtonM.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButtonM.Location = new System.Drawing.Point(243, 37);
            this.CancelButtonM.Name = "CancelButton";
            this.CancelButtonM.Size = new System.Drawing.Size(75, 23);
            this.CancelButtonM.TabIndex = 3;
            this.CancelButtonM.Text = "Cancel";
            this.CancelButtonM.UseVisualStyleBackColor = true;
            this.CancelButtonM.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // RenameForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 69);
            this.Controls.Add(this.CancelButtonM);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.NewNameBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RenameForm";
            this.Text = "Rename";
            this.Load += new System.EventHandler(this.RenameForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NewNameBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButtonM;
    }
}