namespace Cyperus.Designer
{
    partial class MainForm
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
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadEnvironmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveEnvironmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ManageAssemblies = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(535, 24);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadEnvironmentToolStripMenuItem,
            this.saveEnvironmentToolStripMenuItem,
            this.ManageAssemblies});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadEnvironmentToolStripMenuItem
            // 
            this.loadEnvironmentToolStripMenuItem.Name = "loadEnvironmentToolStripMenuItem";
            this.loadEnvironmentToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.loadEnvironmentToolStripMenuItem.Text = "Load environment...";
            // 
            // saveEnvironmentToolStripMenuItem
            // 
            this.saveEnvironmentToolStripMenuItem.Name = "saveEnvironmentToolStripMenuItem";
            this.saveEnvironmentToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveEnvironmentToolStripMenuItem.Text = "Save environment...";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // ManageAssemblies
            // 
            this.ManageAssemblies.Name = "ManageAssemblies";
            this.ManageAssemblies.Size = new System.Drawing.Size(186, 22);
            this.ManageAssemblies.Text = "Manage assemblies...";
            this.ManageAssemblies.Click += new System.EventHandler(this.ManageAssemblies_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 409);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Name = "MainForm";
            this.Text = "Cyperus Designer";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadEnvironmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveEnvironmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ManageAssemblies;
    }
}

