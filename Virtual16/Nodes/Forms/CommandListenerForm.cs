using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Virtual16.Nodes.Forms
{
    public partial class CommandListenerForm : Form
    {
        public CommandListenerForm()
        {
            InitializeComponent();
        }

        public void AddLine(string line)
        {
            Invoke(new MethodInvoker(() => Log.Text += line));
        }
    }
}
