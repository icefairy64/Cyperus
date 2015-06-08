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
            this.components = new System.ComponentModel.Container();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadEnvironmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveEnvironmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ManageAssemblies = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NodeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.СolorChooser = new System.Windows.Forms.ColorDialog();
            this.changeColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu.SuspendLayout();
            this.NodeContextMenu.SuspendLayout();
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
            // ManageAssemblies
            // 
            this.ManageAssemblies.Name = "ManageAssemblies";
            this.ManageAssemblies.Size = new System.Drawing.Size(186, 22);
            this.ManageAssemblies.Text = "Manage assemblies...";
            this.ManageAssemblies.Click += new System.EventHandler(this.ManageAssemblies_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // NodeContextMenu
            // 
            this.NodeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertiesToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.changeColorToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.NodeContextMenu.Name = "NodeContextMenu";
            this.NodeContextMenu.Size = new System.Drawing.Size(155, 92);
            this.NodeContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.NodeContextMenu_Opening);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.propertiesToolStripMenuItem.Text = "Properties...";
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.renameToolStripMenuItem.Text = "Rename...";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // changeColorToolStripMenuItem
            // 
            this.changeColorToolStripMenuItem.Name = "changeColorToolStripMenuItem";
            this.changeColorToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.changeColorToolStripMenuItem.Text = "Change color...";
            this.changeColorToolStripMenuItem.Click += new System.EventHandler(this.changeColorToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 409);
            this.Controls.Add(this.MainMenu);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.MainMenu;
            this.Name = "MainForm";
            this.Text = "Cyperus Designer";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.NodeContextMenu.ResumeLayout(false);
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
        private System.Windows.Forms.ContextMenuStrip NodeContextMenu;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeColorToolStripMenuItem;
        internal System.Windows.Forms.ColorDialog СolorChooser;
    }
}

