using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cyperus;

namespace Cyperus.Designer
{
    [Serializable]
    public class NodeBox : Control
    {
        protected Pen Outline;
        protected SolidBrush Fill;
        protected StringFormat TextFormat;

        protected bool IsDragged = false;
        protected Size SavedMousePos;

        public int SocketRadius = 4;

        public AbstractNode Node { get; protected set; }

        public Color Color
        {
            get { return Fill.Color; }
            set 
            {
                Fill.Color = value;
                Refresh();
            }
        }

        protected readonly int DefaultWidth = 120;
        protected readonly int DefaultHeight = 50;

        public NodeBox(AbstractNode node, int x, int y)
        {
            Node = node;
            Fill = (SolidBrush)Brushes.Aquamarine;
            Outline = Pens.Black;
            TextFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            Left = x - DefaultWidth / 2;
            Top = y - DefaultHeight / 2;
            Width = DefaultWidth;
            Height = DefaultHeight;
        }

        protected void DrawSockets(IReadOnlyCollection<AbstractSocket> set, Graphics canvas, int y)
        {
            int n = set.Count;
            int d = (int)ClientSize.Width / (n > 0 ? n : 1);

            for (int i = 0; i < n; i++)
            {
                canvas.FillEllipse(Fill, (d / 2) + i * d - SocketRadius, y - SocketRadius, SocketRadius * 2, SocketRadius * 2);
                canvas.DrawEllipse(Outline, (d / 2) + i * d - SocketRadius, y - SocketRadius, SocketRadius * 2, SocketRadius * 2);
            }
        }
        
        public void Draw(Graphics canvas)
        {
            canvas.FillRectangle(Fill, 0, 0, Width - SocketRadius - 1, Height - SocketRadius - 1);
            canvas.DrawRectangle(Outline, 0, 0, Width - SocketRadius - 1, Height - SocketRadius - 1);
            canvas.DrawString(Node.Name, SystemFonts.DefaultFont, Brushes.Black, ClientRectangle, TextFormat);

            DrawSockets(Node.Inputs, canvas, (int)ClientRectangle.Top - SocketRadius - 1);
            DrawSockets(Node.Outputs, canvas, (int)ClientRectangle.Bottom - SocketRadius - 1);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Draw(e.Graphics);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            IsDragged = true;
            SavedMousePos = new Size(e.Location);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            IsDragged = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!IsDragged)
                return;

            var offset = e.Location - SavedMousePos;
            Left += offset.X;
            Top += offset.Y;
        }
    }
}
