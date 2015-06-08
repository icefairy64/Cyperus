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
    public partial class RenameForm : Form
    {
        readonly NodeBox Target;

        public RenameForm(NodeBox target)
        {
            InitializeComponent();
            Target = target;
        }

        private void RenameForm_Load(object sender, EventArgs e)
        {
            NewNameBox.Text = Target.Node.Name;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Target.Node.Name = NewNameBox.Text;
            Target.Refresh();
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
