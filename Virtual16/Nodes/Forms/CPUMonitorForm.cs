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
    public partial class CPUMonitorForm : Form
    {
        public CPUMonitorForm()
        {
            InitializeComponent();
        }

        protected string ToHex(int val, int limit)
        {
            var tmp = "";
            int bs = 16;
            int i = 0;
            while (i < limit)
            {
                int x = val % bs;
                val /= bs;
                tmp = (x > 9 ? (char)((int)'A' + x - 10) : (char)((int)'0' + x)) + tmp;
                i++;
            }
            return tmp;
        }

        protected void InternalUpdate(V16CPU src)
        {
            RegisterStatusLabel.Text = String.Format("${0}\n${1}\n${2}\n${3}\n${4}\n${5}\n{6}\n{7}",
                ToHex(src.A, 2),
                ToHex(src.BC, 4),
                ToHex(src.DE, 4),
                ToHex(src.HL, 4),
                ToHex(src.SP, 4),
                ToHex(src.IP, 4),
                src.Carry ? 1 : 0,
                src.Zero ? 1 : 0);
            RegisterStatusLabel.Refresh();
        }

        public void UpdateInfo(V16CPU src)
        {
            Invoke(new MethodInvoker(() => InternalUpdate(src)));
        }
    }
}
