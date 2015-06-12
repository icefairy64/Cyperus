using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cyperus.Designer
{
    public partial class TypeListForm : Form
    {
        protected MainForm Main;

        public TypeListForm(MainForm main)
        {
            InitializeComponent();

            Main = main;
            Program.AssemblyLoaded += AssemblyLoadedHandler;

            foreach (var type in Program.Types)
                TypeListBox.Items.Add(type);
        }

        void AssemblyLoadedHandler(object sender, EventArgs e)
        {
            TypeListBox.Items.Clear();
            foreach (var type in Program.Types)
                TypeListBox.Items.Add(type);
        }

        private void TypesList_MouseDown(object sender, MouseEventArgs e)
        {
            if (TypeListBox.SelectedIndex < 0)
                return;

            DoDragDrop(new NodeCreationContainer((Type)Program.Types[TypeListBox.SelectedIndex]), DragDropEffects.Move);
        }
    }
}
