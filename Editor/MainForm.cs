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
            AssemblyManagerForm.BringToFront();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(NodeCreationContainer)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(NodeCreationContainer)))
                return;
            
            // Creating node from type contained in received container
            var container = (NodeCreationContainer)(e.Data.GetData(typeof(NodeCreationContainer)));
            var node = SpawnNode(container.NodeType);
            var box = new NodeBox(node, e.X - Left, e.Y - Top);
            box.ContextMenuStrip = NodeContextMenu;
            Controls.Add(box);
        }

        private AbstractNode SpawnNode(Type nodeType)
        {
            return (AbstractNode)Activator.CreateInstance(nodeType, "obj", NodeEnvironment);
        }

        private void NodeContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (!(NodeContextMenu.SourceControl is NodeBox))
                return;

            propertiesToolStripMenuItem.Enabled = GetNodePropertiesForm((NodeContextMenu.SourceControl as NodeBox).Node) != null;
        }

        private Form GetNodePropertiesForm(AbstractNode node)
        {
            var type = node.GetType();
            foreach (var field in type.GetFields())
            {
                if (Attribute.IsDefined(field, typeof(PropertiesFormAttribute)))
                    return (Form)field.GetValue(node);
            }

            foreach (var property in type.GetProperties())
            {
                if (Attribute.IsDefined(property, typeof(PropertiesFormAttribute)))
                    return (Form)property.GetValue(node);
            }

            return null;
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var box = NodeContextMenu.SourceControl as NodeBox;
            if (box == null)
                return;

            new RenameForm(box).Show();
        }

        private void changeColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var box = NodeContextMenu.SourceControl as NodeBox;
            if (box == null)
                return;

            // Apparently, VS doesn't want me to use dialog that is already on the form (ColorChooser)
            var colorSel = new ColorDialog();
            if (colorSel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                box.Color = colorSel.Color;

            colorSel.Dispose();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var box = NodeContextMenu.SourceControl as NodeBox;
            if (box == null)
                return;

            box.Node.Destroy();
            Controls.Remove(box);
        }
    }
}
