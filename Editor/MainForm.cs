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
            var node = (AbstractNode)Activator.CreateInstance(nodeType, nodeType.Name, NodeEnvironment);
            return node;
        }

        private void NodeContextMenu_Opening(object sender, CancelEventArgs e)
        {
            var box = NodeContextMenu.SourceControl as NodeBox;
            if (box == null)
                return;

            propertiesToolStripMenuItem.Enabled = GetNodePropertiesForm(box.Node) != null;

            if (box.Node is Producer)
            {
                toolStripSeparator1.Visible = true;
                startToolStripMenuItem.Visible = true;
                pauseToolStripMenuItem.Visible = true;
                stopToolStripMenuItem.Visible = true;

                var prod = box.Node as Producer;

                if (prod.Paused)
                    pauseToolStripMenuItem.Text = "Continue";
                else
                    pauseToolStripMenuItem.Text = "Pause";

                if (prod.Active)
                {
                    startToolStripMenuItem.Enabled = false;
                    pauseToolStripMenuItem.Enabled = true;
                    stopToolStripMenuItem.Enabled = true;
                }
                else
                {
                    startToolStripMenuItem.Enabled = true;
                    pauseToolStripMenuItem.Enabled = false;
                    stopToolStripMenuItem.Enabled = false;
                }
            }
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

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var box = NodeContextMenu.SourceControl as NodeBox;
            if (box == null)
                return;

            var prod = box.Node as Producer;
            prod.Start();
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var box = NodeContextMenu.SourceControl as NodeBox;
            if (box == null)
                return;

            var prod = box.Node as Producer;
            prod.Paused = !prod.Paused;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var box = NodeContextMenu.SourceControl as NodeBox;
            if (box == null)
                return;

            var prod = box.Node as Producer;
            prod.Stop();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            NodeEnvironment.Destroy();
        }
    }
}
