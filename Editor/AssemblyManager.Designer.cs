namespace Cyperus.Designer
{
    partial class AssemblyManager
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
            this.AssembliesList = new System.Windows.Forms.ListBox();
            this.AssembliesBox = new System.Windows.Forms.GroupBox();
            this.ShowEmptyCheckbox = new System.Windows.Forms.CheckBox();
            this.LoadAssemblyButton = new System.Windows.Forms.Button();
            this.TypesBox = new System.Windows.Forms.GroupBox();
            this.TypesList = new System.Windows.Forms.ListBox();
            this.LoadAssemblyDialog = new System.Windows.Forms.OpenFileDialog();
            this.AssembliesBox.SuspendLayout();
            this.TypesBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // AssembliesList
            // 
            this.AssembliesList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.AssembliesList.FormattingEnabled = true;
            this.AssembliesList.Location = new System.Drawing.Point(15, 19);
            this.AssembliesList.Name = "AssembliesList";
            this.AssembliesList.Size = new System.Drawing.Size(225, 264);
            this.AssembliesList.TabIndex = 0;
            this.AssembliesList.SelectedIndexChanged += new System.EventHandler(this.AssembliesList_SelectedIndexChanged);
            // 
            // AssembliesBox
            // 
            this.AssembliesBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.AssembliesBox.Controls.Add(this.ShowEmptyCheckbox);
            this.AssembliesBox.Controls.Add(this.LoadAssemblyButton);
            this.AssembliesBox.Controls.Add(this.AssembliesList);
            this.AssembliesBox.Location = new System.Drawing.Point(12, 12);
            this.AssembliesBox.MinimumSize = new System.Drawing.Size(342, 296);
            this.AssembliesBox.Name = "AssembliesBox";
            this.AssembliesBox.Size = new System.Drawing.Size(342, 296);
            this.AssembliesBox.TabIndex = 1;
            this.AssembliesBox.TabStop = false;
            this.AssembliesBox.Text = "Assemblies";
            // 
            // ShowEmptyCheckbox
            // 
            this.ShowEmptyCheckbox.AutoSize = true;
            this.ShowEmptyCheckbox.Location = new System.Drawing.Point(246, 48);
            this.ShowEmptyCheckbox.Name = "ShowEmptyCheckbox";
            this.ShowEmptyCheckbox.Size = new System.Drawing.Size(84, 17);
            this.ShowEmptyCheckbox.TabIndex = 2;
            this.ShowEmptyCheckbox.Text = "Show empty";
            this.ShowEmptyCheckbox.UseVisualStyleBackColor = true;
            this.ShowEmptyCheckbox.CheckedChanged += new System.EventHandler(this.ShowEmptyCheckbox_CheckedChanged);
            // 
            // LoadAssemblyButton
            // 
            this.LoadAssemblyButton.Location = new System.Drawing.Point(246, 19);
            this.LoadAssemblyButton.Name = "LoadAssemblyButton";
            this.LoadAssemblyButton.Size = new System.Drawing.Size(90, 23);
            this.LoadAssemblyButton.TabIndex = 1;
            this.LoadAssemblyButton.Text = "Load...";
            this.LoadAssemblyButton.UseVisualStyleBackColor = true;
            this.LoadAssemblyButton.Click += new System.EventHandler(this.LoadAssemblyButton_Click);
            // 
            // TypesBox
            // 
            this.TypesBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TypesBox.Controls.Add(this.TypesList);
            this.TypesBox.Location = new System.Drawing.Point(360, 12);
            this.TypesBox.MinimumSize = new System.Drawing.Size(220, 296);
            this.TypesBox.Name = "TypesBox";
            this.TypesBox.Size = new System.Drawing.Size(224, 296);
            this.TypesBox.TabIndex = 2;
            this.TypesBox.TabStop = false;
            this.TypesBox.Text = "Provided types";
            // 
            // TypesList
            // 
            this.TypesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TypesList.FormattingEnabled = true;
            this.TypesList.Location = new System.Drawing.Point(6, 19);
            this.TypesList.Name = "TypesList";
            this.TypesList.Size = new System.Drawing.Size(212, 264);
            this.TypesList.TabIndex = 0;
            // 
            // LoadAssemblyDialog
            // 
            this.LoadAssemblyDialog.Filter = ".NET assemblies|*.dll;*.exe";
            this.LoadAssemblyDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.LoadAssemblyDialog_FileOk);
            // 
            // AssemblyManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 320);
            this.Controls.Add(this.TypesBox);
            this.Controls.Add(this.AssembliesBox);
            this.MinimumSize = new System.Drawing.Size(612, 359);
            this.Name = "AssemblyManager";
            this.Text = "Assembly Manager";
            this.AssembliesBox.ResumeLayout(false);
            this.AssembliesBox.PerformLayout();
            this.TypesBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox AssembliesList;
        private System.Windows.Forms.GroupBox AssembliesBox;
        private System.Windows.Forms.Button LoadAssemblyButton;
        private System.Windows.Forms.CheckBox ShowEmptyCheckbox;
        private System.Windows.Forms.GroupBox TypesBox;
        private System.Windows.Forms.ListBox TypesList;
        private System.Windows.Forms.OpenFileDialog LoadAssemblyDialog;
    }
}