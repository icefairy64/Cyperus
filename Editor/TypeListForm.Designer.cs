﻿namespace Cyperus.Designer
{
    partial class TypeListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TypeListForm));
            this.TypeListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // TypeListBox
            // 
            resources.ApplyResources(this.TypeListBox, "TypeListBox");
            this.TypeListBox.FormattingEnabled = true;
            this.TypeListBox.Name = "TypeListBox";
            this.TypeListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TypesList_MouseDown);
            // 
            // TypeListForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TypeListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TypeListForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox TypeListBox;
    }
}