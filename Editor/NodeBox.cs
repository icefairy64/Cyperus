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
    class NodeBox : Control
    {
        protected Pen Outline;
        protected SolidBrush Fill;
        protected StringFormat TextFormat;

        public int SocketRadius = 4;

        public AbstractNode Node { get; protected set; }

        public Color Color
        {
            get { return Fill.Color; }
            set { Fill.Color = value; }
        }

        public NodeBox(AbstractNode node, int x, int y)
        {
            Node = node;
            Fill = (SolidBrush)Brushes.Aquamarine;
            Outline = Pens.Black;
            TextFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
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
            canvas.FillRectangle(Fill, ClientRectangle);
            canvas.DrawRectangle(Outline, ClientRectangle);
            canvas.DrawString(Node.Name, SystemFonts.DefaultFont, Brushes.Black, ClientRectangle, TextFormat);

            DrawSockets(Node.Inputs, canvas, (int)ClientRectangle.Top);
            DrawSockets(Node.Outputs, canvas, (int)ClientRectangle.Bottom);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Draw(e.Graphics);
        }
    }
}
