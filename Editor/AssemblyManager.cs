using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Cyperus;

namespace Cyperus.Designer
{
    public partial class AssemblyManager : Form
    {
        protected MainForm Main;
        
        string GetAssemblyTitle(Assembly assembly)
        {
            string name = assembly.FullName.Substring(0, assembly.FullName.IndexOf(','));

            return String.Format("{0} ({1})", name, Program.TypesByAssembly[assembly].Length);
        }

        void RebuildAssebmbliesList()
        {
            AssembliesList.Items.Clear();

            Program.RebuildVisibleAssembliesList();

            var nameQuery =
                from a in Program.VisibleAssemblies select GetAssemblyTitle(a);

            AssembliesList.Items.AddRange(nameQuery.ToArray());
        }

        void AssemblyLoadedHandler(object sender, EventArgs e)
        {
            RebuildAssebmbliesList();
        }

        public AssemblyManager(MainForm main)
        {
            InitializeComponent();
            RebuildAssebmbliesList();
            Main = main;

            Program.AssemblyLoaded += AssemblyLoadedHandler;
            ShowEmptyCheckbox.Checked = Program.ShowEmptyAssemblies;
        }

        private void ShowEmptyCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Program.ShowEmptyAssemblies = ShowEmptyCheckbox.Checked;
            RebuildAssebmbliesList();
        } 

        private void AssembliesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AssembliesList.SelectedIndex < 0)
                return;

            TypesList.Items.Clear();
            TypesList.Items.AddRange(Program.TypesByAssembly[Program.VisibleAssemblies[AssembliesList.SelectedIndex]]);
        }

        private void LoadAssemblyButton_Click(object sender, EventArgs e)
        {
            LoadAssemblyDialog.ShowDialog();
        }

        private void LoadAssemblyDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                Assembly.LoadFrom(LoadAssemblyDialog.FileName);
                RebuildAssebmbliesList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Could not load assembly ({0}): {1}", ex.GetType(), ex.Message));
            }
        }

        ~AssemblyManager()
        {
            Program.AssemblyLoaded -= AssemblyLoadedHandler;
        }

        private void TypesList_MouseDown(object sender, MouseEventArgs e)
        {
            if (TypesList.SelectedIndex < 0)
                return;

            DoDragDrop(new NodeCreationContainer((Type)Program.TypesByAssembly[Program.VisibleAssemblies[AssembliesList.SelectedIndex]][TypesList.SelectedIndex]), DragDropEffects.Move);
        }
    }
}
