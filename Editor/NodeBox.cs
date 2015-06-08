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
    public class SocketWrapper
    {
        public readonly NodeBox Owner;
        public readonly object Socket;
        public bool Flash;
        public Point Location;

        public SocketWrapper(NodeBox owner, object socket, int x, int y)
        {
            Socket = socket;
            Flash = false;
            Location = new Point(x, y);
            Owner = owner;
        }
    }

    public class ConnectionWrapper
    {
        public readonly SocketWrapper Source;
        public readonly SocketWrapper Destination;
        
        public ConnectionWrapper(SocketWrapper src, SocketWrapper dest)
        {
            Source = src;
            Destination = dest;
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
        protected ToolTip Tip;

        protected bool IsDragged = false;
        protected Size SavedMousePos;

        protected readonly List<SocketWrapper> Sockets;
        protected readonly List<ConnectionWrapper> Connections;

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
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            
            Left = x - DefaultWidth / 2;
            Top = y - DefaultHeight / 2;
            Width = DefaultWidth;
            Height = DefaultHeight;
            BackColor = Color.Transparent;

            Tip = new ToolTip();
            Tip.ShowAlways = false;
            Tip.AutomaticDelay = 500;
            Tip.UseAnimation = true;
            Tip.UseFading = true;
            
            AllowDrop = true;

            Sockets = new List<SocketWrapper>();
            Connections = new List<ConnectionWrapper>();
            NodeUpdateHandler(node);

            Node.OnUpdate = NodeUpdateHandler;
            Node.OnSocketActivity = SocketActivityHandler;
        }
        
        public void Draw(Graphics canvas)
        {
            canvas.FillRectangle(Fill, 0, SocketRadius + 1, Width - SocketRadius * 2 - 2, Height - SocketRadius * 2 - 2);
            canvas.DrawRectangle(Outline, 0, SocketRadius + 1, Width - SocketRadius * 2 - 2, Height - SocketRadius * 2 - 2);
            canvas.DrawString(Node.Name, SystemFonts.DefaultFont, Brushes.Black, ClientRectangle, TextFormat);

            foreach (var socket in Sockets)
            {
                canvas.FillEllipse(socket.Flash ? SocketFlashBrush : SocketStaticBrush, socket.Location.X - SocketRadius, socket.Location.Y - SocketRadius, SocketRadius * 2, SocketRadius * 2);
                canvas.DrawEllipse(Outline, socket.Location.X - SocketRadius, socket.Location.Y - SocketRadius, SocketRadius * 2, SocketRadius * 2);
            }
        }

        public void DrawConnections(Graphics canvas)
        {
            foreach (var conn in Connections)
            {
                canvas.DrawLine(Outline, conn.Source.Location + new Size(Location), conn.Destination.Location + new Size(conn.Destination.Owner.Location));
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
                Sockets.Add(new SocketWrapper(this, socket, (d / 2) + i * d, y));
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

        protected int GetDistance(Point a, Point b)
        {
            return (int)(Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y)));
        }

        private SocketWrapper GetSocketAtPoint(Point location)
        {
            return Sockets.Find(obj => GetDistance(obj.Location, location) <= SocketRadius * 2);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            // Connecting
            var socket = GetSocketAtPoint(e.Location);
            if (socket != null)
            {
                DoDragDrop(new ConnectionContainer(socket), DragDropEffects.Link);
                return;
            }

            // Dragging
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
            
            // Setting tooltip

            var socket = GetSocketAtPoint(e.Location);
            string str = null;
            if (socket == null)
                str = Node.ToString();
            else
                str = socket.Socket.ToString();

            if (str != Tip.GetToolTip(this))
                Tip.SetToolTip(this, str);

            // Dragging

            if (!IsDragged)
                return;

            var offset = e.Location - SavedMousePos;
            Left += offset.X;
            Top += offset.Y;
            Parent.Refresh();
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            base.OnDragOver(drgevent);

            if (!drgevent.Data.GetDataPresent(typeof(ConnectionContainer)))
                return;

            var cont = (ConnectionContainer)drgevent.Data.GetData(typeof(ConnectionContainer));
            var socket = GetSocketAtPoint(PointToClient(new Point(drgevent.X, drgevent.Y)));

            if (socket != null && socket.Socket != cont.Source && ((AbstractSocket)socket.Socket).AcceptsDataType(((AbstractSocket)cont.Source.Socket).DataType)
                    && ((AbstractSocket)cont.Source.Socket).Kind != ((AbstractSocket)socket.Socket).Kind)
                drgevent.Effect = DragDropEffects.Link;
            else
                drgevent.Effect = DragDropEffects.None;
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);

            if (drgevent.Effect != DragDropEffects.Link)
                return;

            var cont = (ConnectionContainer)drgevent.Data.GetData(typeof(ConnectionContainer));
            var socket = GetSocketAtPoint(PointToClient(new Point(drgevent.X, drgevent.Y)));

            var src = ((AbstractSocket)cont.Source.Socket).Kind == SocketKind.Source ? cont.Source : socket;
            var dest = src == socket ? cont.Source : socket;

            Node.Environment.Connect((AbstractSocket)src.Socket, (AbstractSocket)dest.Socket);
            src.Owner.AddConnection(new ConnectionWrapper(src, dest));
            Parent.Refresh();
        }

        public void AddConnection(ConnectionWrapper conn)
        {
            Connections.Add(conn);
        }

        /// <summary>
        /// Verifies validity of connections
        /// </summary>
        public void Validate()
        {
            var list = new List<ConnectionWrapper>();
            foreach (var conn in Connections)
            {
                if (conn.Destination.Owner.Node.Destroyed)
                    list.Add(conn);
            }

            foreach (var conn in list)
                Connections.Remove(conn);
        }
    }
}
