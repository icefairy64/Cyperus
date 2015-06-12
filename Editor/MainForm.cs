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
        internal AssemblyManager AssemblyManagerForm;
        internal TypeListForm TypeListForm;
        internal Environment NodeEnvironment;
        
        public MainForm()
        {
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            NodeEnvironment = new Environment();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            foreach (var control in Controls)
            {
                var box = control as NodeBox;
                if (box != null)
                    box.DrawConnections(e.Graphics);
            }
        }

        private void ManageAssemblies_Click(object sender, EventArgs e)
        {
            if (AssemblyManagerForm == null || AssemblyManagerForm.IsDisposed)
                AssemblyManagerForm = new AssemblyManager(this);

            AssemblyManagerForm.Show();
            AssemblyManagerForm.BringToFront();
        }

        private void showTypesListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TypeListForm == null || TypeListForm.IsDisposed)
                TypeListForm = new TypeListForm(this);

            TypeListForm.Show();
            TypeListForm.BringToFront();
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

        #region Reflection

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

        private void SetNodePropertiesForm(AbstractNode node, object form)
        {
            var type = node.GetType();
            foreach (var field in type.GetFields())
            {
                if (Attribute.IsDefined(field, typeof(PropertiesFormAttribute)))
                {
                    field.SetValue(node, form);
                    return;
                }
            }

            foreach (var property in type.GetProperties())
            {
                if (Attribute.IsDefined(property, typeof(PropertiesFormAttribute)))
                {
                    property.SetValue(node, form);
                    return;
                }
            }
        }

        private Type GetNodePropertiesFormType(AbstractNode node)
        {
            var type = node.GetType();
            foreach (var field in type.GetFields())
            {
                if (Attribute.IsDefined(field, typeof(PropertiesFormAttribute)))
                    return field.FieldType;
            }

            foreach (var property in type.GetProperties())
            {
                if (Attribute.IsDefined(property, typeof(PropertiesFormAttribute)))
                    return property.PropertyType;
            }

            return null;
        }

        private AbstractNode SpawnNode(Type nodeType)
        {
            var node = (AbstractNode)Activator.CreateInstance(nodeType, nodeType.Name, NodeEnvironment);
            return node;
        }

        #endregion

        #region Context menu

        private void NodeContextMenu_Opening(object sender, CancelEventArgs e)
        {
            var box = NodeContextMenu.SourceControl as NodeBox;
            if (box == null)
                return;

            if (GetNodePropertiesForm(box.Node) != null)
                propertiesToolStripMenuItem.Enabled = true;
            else
            {
                var type = GetNodePropertiesFormType(box.Node);
                if (type != null)
                {
                    SetNodePropertiesForm(box.Node, Activator.CreateInstance(type));
                    propertiesToolStripMenuItem.Enabled = true;
                }
                else
                    propertiesToolStripMenuItem.Enabled = false;
            }

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
            else
            {
                toolStripSeparator1.Visible = false;
                startToolStripMenuItem.Visible = false;
                pauseToolStripMenuItem.Visible = false;
                stopToolStripMenuItem.Visible = false;
            }
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

            foreach (var obj in Controls)
            {
                var ob = obj as NodeBox;
                if (ob != null)
                    ob.Validate();
            }

            Refresh();
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

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var box = NodeContextMenu.SourceControl as NodeBox;
            if (box == null)
                return;

            var form = GetNodePropertiesForm(box.Node);

            if (form == null || form.IsDisposed)
            {
                form = (Form)Activator.CreateInstance(GetNodePropertiesFormType(box.Node));
            }

            form.Show();
            form.BringToFront();
        }

        #endregion

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            NodeEnvironment.Destroy();
        }
    }
}
