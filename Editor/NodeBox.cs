using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cyperus;

namespace Cyperus.Designer
{
    class SocketWrapper
    {
        public readonly object Socket;
        public bool Flash;
        public Point Location;

        public SocketWrapper(object socket, int x, int y)
        {
            Socket = socket;
            Flash = false;
            Location = new Point(x, y);
        }
    }
    
    [Serializable]
    public class NodeBox : Control
    {
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

        protected Pen Outline;
        protected SolidBrush Fill;
        protected StringFormat TextFormat;

        protected bool IsDragged = false;
        protected Size SavedMousePos;

        private List<SocketWrapper> Sockets;

        protected readonly int DefaultWidth = 120;
        protected readonly int DefaultHeight = 50;
        protected readonly int SocketRadius = 4;

        static readonly int SocketFlashDelay = 40;
        static readonly SolidBrush SocketStaticBrush = new SolidBrush(Color.White);
        static readonly SolidBrush SocketFlashBrush = new SolidBrush(Color.LimeGreen);

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

            Sockets = new List<SocketWrapper>();
            NodeUpdateHandler(node);

            Node.OnUpdate = NodeUpdateHandler;
            Node.OnSocketActivity = SocketActivityHandler;
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
            canvas.FillRectangle(Fill, 0, SocketRadius + 1, Width - SocketRadius * 2 - 2, Height - SocketRadius * 2 - 2);
            canvas.DrawRectangle(Outline, 0, SocketRadius + 1, Width - SocketRadius * 2 - 2, Height - SocketRadius * 2 - 2);
            canvas.DrawString(Node.Name, SystemFonts.DefaultFont, Brushes.Black, ClientRectangle, TextFormat);

            //DrawSockets(Node.Inputs, canvas, (int)ClientRectangle.Top + SocketRadius + 1);
            //DrawSockets(Node.Outputs, canvas, (int)ClientRectangle.Bottom - SocketRadius - 1);
            foreach (var socket in Sockets)
            {
                canvas.FillEllipse(socket.Flash ? SocketFlashBrush : SocketStaticBrush, socket.Location.X - SocketRadius, socket.Location.Y - SocketRadius, SocketRadius * 2, SocketRadius * 2);
                canvas.DrawEllipse(Outline, socket.Location.X - SocketRadius, socket.Location.Y - SocketRadius, SocketRadius * 2, SocketRadius * 2);
            }
        }

        protected void NodeUpdateHandler(AbstractNode sender)
        {
            Sockets.Clear();
            AddSockets(Node.Inputs, SocketRadius + 1);
            AddSockets(Node.Outputs, ClientRectangle.Bottom - SocketRadius - 1);
        }

        protected void AddSockets(IReadOnlyCollection<AbstractSocket> set, int y)
        {
            int n = set.Count;
            int d = (int)ClientSize.Width / (n > 0 ? n : 1);
            int i = 0;
            foreach (var socket in set)
            {
                Sockets.Add(new SocketWrapper(socket, (d / 2) + i * d, y));
                i++;
            }
        }

        protected async Task SocketActivityHandler(AbstractNode sender, object socket)
        {
            var target = Sockets.Find(obj => obj.Socket == socket);
            target.Flash = true;
            Invoke(new MethodInvoker(() => Refresh()));
            await Task.Run(() => Thread.Sleep(SocketFlashDelay));
            target.Flash = false;
            Invoke(new MethodInvoker(() => Refresh()));
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
