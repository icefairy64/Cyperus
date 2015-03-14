using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Designer
{
    class Configurable : Attribute
    {
        public Form SettingsForm { get; protected set; }

        public Configurable(Form settingsForm)
        {
            SettingsForm = settingsForm;
        }
    }
}
