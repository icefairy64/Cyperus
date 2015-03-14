using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyperus;

namespace Cyperus.Designer
{
    [Serializable]
    class NodeBox
    {
        protected Pen Outline;
        protected SolidBrush Fill;
        protected RectangleF Rect;
        protected StringFormat TextFormat;

        public int SocketRadius = 4;

        public AbstractNode Node { get; protected set; }

        public Color Color
        {
            get { return Fill.Color; }
            set { Fill.Color = value; }
        }

        public float X
        {
            get { return Rect.X; }
            set { Rect.X = value; }
        }

        public float Y
        {
            get { return Rect.Y; }
            set { Rect.Y = value; }
        }

        public float Width
        {
            get { return Rect.Width; }
            set 
            { 
                if (value < 100)
                {
                    return;
                }

                Rect.Width = value; 
            }
        }

        public float Height
        {
            get { return Rect.Height; }
            set 
            {
                if (value < 80)
                {
                    return;
                }
                
                Rect.Height = value;
            }
        }

        public NodeBox(AbstractNode node, int x, int y)
        {
            Node = node;
            Fill = (SolidBrush)Brushes.Aquamarine;
            Outline = Pens.Black;
            TextFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            Rect = new RectangleF(x, y, 100, 80);
        }

        protected void DrawSockets(ICollection<AbstractSocket> set, Graphics canvas, int y)
        {
            int n = set.Count;
            int d = (int)Width / (n > 0 ? n : 1);

            for (int i = 0; i < n; i++)
            {
                canvas.FillEllipse(Fill, (d / 2) + i * d - SocketRadius, y - SocketRadius, SocketRadius * 2, SocketRadius * 2);
                canvas.DrawEllipse(Outline, (d / 2) + i * d - SocketRadius, y - SocketRadius, SocketRadius * 2, SocketRadius * 2);
            }
        }
        
        public void Draw(Graphics canvas)
        {
            canvas.FillRectangle(Fill, Rect);
            canvas.DrawRectangle(Outline, Rect.X, Rect.Y, Rect.Width, Rect.Height);
            canvas.DrawString(Node.Name, SystemFonts.DefaultFont, Brushes.Black, Rect, TextFormat);

            DrawSockets(Node.Inputs, canvas, (int)Y);
            DrawSockets(Node.Outputs, canvas, (int)(Y + Width));
        }
    }
}
