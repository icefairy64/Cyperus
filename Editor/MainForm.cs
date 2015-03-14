using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cyperus;

namespace Cyperus.Designer
{
    public partial class MainForm : Form
    {
        AssemblyManager AssemblyManagerForm;
        Environment NodeEnvironment;
        
        public MainForm()
        {
            InitializeComponent();

            NodeEnvironment = new Environment();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void ManageAssemblies_Click(object sender, EventArgs e)
        {
            if (AssemblyManagerForm == null || AssemblyManagerForm.IsDisposed)
            {
                AssemblyManagerForm = new AssemblyManager();
            }

            AssemblyManagerForm.Show();
        }
    }
}
