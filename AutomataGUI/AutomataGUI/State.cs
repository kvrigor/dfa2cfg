using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomataGUI
{
    public class State : IDisposable
    {
        private const int radius = 25;
        private Size circleSize = new Size(2 * radius, 2 * radius);
        private PictureBox drawingBoard;

        private Point m_CenterLocation;
        private State m_TargetOne;
        private State m_TargetZero;
        private DateTime timepressed;

        //For MoveState
        private Rectangle _moveState = new Rectangle();
        private bool _blnMoveSet = false;

        //Connect
        private Point _oldConn1s;
        private Point _oldConn1d;
        
        private static State StateHovered;
        public static Dictionary<string, State> StateCollection = new Dictionary<string, State>();
        public static void DrawAllStates(PictureBox panelArea, State exception = null)
        {
            foreach (KeyValuePair<string, State> item in State.StateCollection)
            {
                if (exception != null)
                {
                    if (exception.StateName != item.Value.StateName)
                        item.Value.DrawIn(panelArea);
                }
                else
                    item.Value.DrawIn(panelArea);
            }
        }

        private string StateName;

        private struct MagnetPoint
        {
            public Point Location;
            public bool Available;
            public MagnetPoint(Point _point, bool _avail)
            {
                Location = _point;
                Available = _avail;
            }
        }
        private Dictionary<string, MagnetPoint> _AvailablePoints;
        private Point ImageLocation { get; set; }
        public Point CenterLocation { get { return m_CenterLocation; }
            set
            {
                m_CenterLocation = value;
                if (_AvailablePoints.Count == 0)
                {
                    _AvailablePoints.Add("1", new MagnetPoint(new Point(m_CenterLocation.X, m_CenterLocation.Y - radius), false));
                    _AvailablePoints.Add("2", new MagnetPoint(new Point(m_CenterLocation.X + (int)(radius / Math.Sqrt(2)), m_CenterLocation.Y - (int)(radius / Math.Sqrt(2))), false));
                    _AvailablePoints.Add("3", new MagnetPoint(new Point(m_CenterLocation.X + radius, m_CenterLocation.Y), false));
                    _AvailablePoints.Add("4", new MagnetPoint(new Point(m_CenterLocation.X + (int)(radius / Math.Sqrt(2)), m_CenterLocation.Y + (int)(radius / Math.Sqrt(2))), false));
                    _AvailablePoints.Add("5", new MagnetPoint(new Point(m_CenterLocation.X, m_CenterLocation.Y + radius), false));
                    _AvailablePoints.Add("6", new MagnetPoint(new Point(m_CenterLocation.X - (int)(radius / Math.Sqrt(2)), m_CenterLocation.Y + (int)(radius / Math.Sqrt(2))), false));
                    _AvailablePoints.Add("7", new MagnetPoint(new Point(m_CenterLocation.X - radius, m_CenterLocation.Y), false));
                    _AvailablePoints.Add("8", new MagnetPoint(new Point(m_CenterLocation.X - (int)(radius / Math.Sqrt(2)), m_CenterLocation.Y - (int)(radius / Math.Sqrt(2))), false));
                }
                else
                {
                    _AvailablePoints["1"] = new MagnetPoint(new Point(m_CenterLocation.X, m_CenterLocation.Y - radius), false);
                    _AvailablePoints["2"] = new MagnetPoint(new Point(m_CenterLocation.X + (int)(radius / Math.Sqrt(2)), m_CenterLocation.Y - (int)(radius / Math.Sqrt(2))), false);
                    _AvailablePoints["3"] = new MagnetPoint(new Point(m_CenterLocation.X + radius, m_CenterLocation.Y), false);
                    _AvailablePoints["4"] = new MagnetPoint(new Point(m_CenterLocation.X + (int)(radius / Math.Sqrt(2)), m_CenterLocation.Y + (int)(radius / Math.Sqrt(2))), false);
                    _AvailablePoints["5"] = new MagnetPoint(new Point(m_CenterLocation.X, m_CenterLocation.Y + radius), false);
                    _AvailablePoints["6"] = new MagnetPoint(new Point(m_CenterLocation.X - (int)(radius / Math.Sqrt(2)), m_CenterLocation.Y + (int)(radius / Math.Sqrt(2))), false);
                    _AvailablePoints["7"] = new MagnetPoint(new Point(m_CenterLocation.X - radius, m_CenterLocation.Y), false);
                    _AvailablePoints["8"] = new MagnetPoint(new Point(m_CenterLocation.X - (int)(radius / Math.Sqrt(2)), m_CenterLocation.Y - (int)(radius / Math.Sqrt(2))), false);
                }
            }
        }

        public State(string _name, Point centerPoint, PictureBox source)
        {
            StateName = _name;
            _AvailablePoints = new Dictionary<string, MagnetPoint>();
            ImageLocation = new Point(centerPoint.X - radius, centerPoint.Y - radius);
            CenterLocation = centerPoint;
            drawingBoard = source;
        }

        public void DrawIn(PictureBox panelArea)
        {
            Utils.DrawCircle(panelArea, ImageLocation, radius, Brushes.DarkBlue, Pens.Black, true);
        }

        public bool IsInRange(Point xy)
        {
            double Ax, Ay;
            double Bx, By;
            double distance;
            Ax = CenterLocation.X;
            Ay = CenterLocation.Y;
            Bx = xy.X;
            By = xy.Y;
            distance = Math.Sqrt(Math.Pow(Ax - Bx, 2) + Math.Pow(Ay - By, 2));
            if (distance <= radius)
                return true;
            else
                return false;
        }

        public void MouseHovered(object sender, MouseEventArgs e)
        {
            var pointer = e as MouseEventArgs;
            var panelArea = sender as PictureBox;
            switch (Registry.MouseStatus)
            {
                case Registry.MouseCondition.Default:
                    if (IsInRange(pointer.Location) && StateHovered != this)
                    {
                        Utils.DrawCircle(panelArea, ImageLocation, radius, Brushes.Blue, Pens.Black, false);
                        StateHovered = this;
                        Registry.MouseStatus = Registry.MouseCondition.Hovered;
                    }
                    //else if (!IsInRange(pointer.Location) && StateHovered == this)
                    //{
                    //    Utils.DrawCircle(panelArea, ImageLocation, radius, Brushes.DarkBlue, Pens.Black, true);
                    //    StateHovered = null;
                    //}
                    break;
                case Registry.MouseCondition.Hovered:
                    if (!IsInRange(pointer.Location) && StateHovered == this)
                    {
                        Utils.DrawCircle(panelArea, ImageLocation, radius, Brushes.DarkBlue, Pens.Black, true);
                        StateHovered = null;
                        Registry.MouseStatus = Registry.MouseCondition.Default;
                    }
                    break;
                case Registry.MouseCondition.MoveState:
                    if (StateHovered == this)
                    {
                        _moveState.Location = new Point(e.Location.X - 25, e.Location.Y - 25);
                        _moveState.Size = circleSize;
                        
                        if (m_TargetOne == null)
                        {
                            Utils.DrawCircle(panelArea, _moveState.Location, radius, Brushes.LightBlue, Pens.Black, false);
                        }
                        else
                        {
                            Registry.CircleParam _circle = new Registry.CircleParam();
                            _circle.ImageLocation = _moveState.Location;
                            _circle.Radius = radius;
                            _circle.FillColor = Brushes.LightBlue;
                            _circle.OutlineColor = Pens.Black;

                            ImageLocation = _moveState.Location;
                            CenterLocation = e.Location;

                            string source = GetNearestPointIndex(m_TargetOne.CenterLocation);
                            Registry.LineParam _line = new Registry.LineParam();
                            _line.Source = _AvailablePoints[source].Location;
                            _line.Destination = m_TargetOne.GetMagnetPoint(_moveState.Location);
                            Pen asd = new Pen(Color.Red, 3);
                            asd.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                            _line.LineColor = asd;

                            Utils.DrawLineAndCircle(panelArea, _circle, _line, false);
                        }
                    }
                    break;
                case Registry.MouseCondition.DeleteState:
                    if (IsInRange(pointer.Location))
                        Utils.DrawCircle(panelArea, ImageLocation, radius, Brushes.Blue, Pens.Black, false);
                    else
                        Utils.DrawCircle(panelArea, ImageLocation, radius, Brushes.DarkBlue, Pens.Black, false);
                    break;
                case Registry.MouseCondition.ConnectOne:
                    if (StateHovered == null)
                    {
                        if (IsInRange(pointer.Location))
                            Utils.DrawCircle(panelArea, ImageLocation, radius, Brushes.Blue, Pens.Black, true);
                        else
                            Utils.DrawCircle(panelArea, ImageLocation, radius, Brushes.DarkBlue, Pens.Black, true);
                    }
                    else if (State.StateHovered == this)
                    {
                        if (IsInRange(pointer.Location))
                        {
                            //do nothing
                        }
                        else
                        {
                            string source = GetNearestPointIndex(pointer.Location);
                            Registry.LineParam _line = new Registry.LineParam();
                            _line.Source = _AvailablePoints[source].Location;
                            _line.Destination = pointer.Location;
                            Pen _pen = new Pen(Color.Red, 3);
                            _pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                            _line.LineColor = _pen;
                            Utils.DrawLine(panelArea, _line, false);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void MouseDowned(object sender, MouseEventArgs e)
        {
            if (StateHovered == this)
            {
                if (IsInRange(e.Location))
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        switch (Registry.MouseStatus)
                        {
                            case Registry.MouseCondition.Selected:
                                Registry.MouseStatus = Registry.MouseCondition.MoveState;
                                Cursor.Current = Cursors.SizeAll;
                                timepressed = DateTime.Now;
                                //erase
                                Utils.DrawCircle(drawingBoard, ImageLocation, radius, Brushes.White, Pens.White, true);
                                Utils.DrawCircle(drawingBoard, ImageLocation, radius, Brushes.LightBlue, Pens.Black, false);
                                string myindex = GetNearestPointIndex(m_TargetOne.CenterLocation);
                                Registry.LineParam _line = new Registry.LineParam();
                                _line.Source = _AvailablePoints[myindex].Location; 
                                _line.Destination = m_TargetOne.GetMagnetPoint(CenterLocation);
                                Pen _pen = new Pen(Color.White, 3);
                                _pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                                _line.LineColor = _pen;
                                Utils.DrawLine(drawingBoard, _line, true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        public void MouseReleased(object sender, MouseEventArgs e)
        {
            if (StateHovered == this)
            {
                var pointer = e as MouseEventArgs;
                var panelArea = sender as Panel;
                if (e.Button == MouseButtons.Left)
                {
                    switch (Registry.MouseStatus)
                    {
                        case Registry.MouseCondition.MoveState:
                            if ((DateTime.Now - timepressed).TotalMilliseconds < 200)
                            {
                                Registry.MouseStatus = Registry.MouseCondition.Default;
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                            ImageLocation = new Point(pointer.Location.X - radius, pointer.Location.Y - radius);
                            CenterLocation = pointer.Location;
                            Registry.MouseStatus = Registry.MouseCondition.Default;
                            Cursor.Current = Cursors.Default;
                            StateHovered = null;
                            _blnMoveSet = false;

                            if (m_TargetOne != null)
                            {
                                string myindex = GetNearestPointIndex(m_TargetOne.CenterLocation);
                                Registry.LineParam _line = new Registry.LineParam();
                                _line.Source = _AvailablePoints[myindex].Location;
                                _line.Destination = m_TargetOne.GetMagnetPoint(CenterLocation);
                                Pen _pen = new Pen(Color.Red, 3);
                                _pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                                _line.LineColor = _pen;
                                Utils.DrawLine(drawingBoard, _line, true);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void MouseClicked(object sender, MouseEventArgs e)
        {
            var pointer = e as MouseEventArgs;
            var panelArea = sender as Panel;
            if (e.Button == MouseButtons.Left)
            {
                if (IsInRange(pointer.Location))
                {
                    switch (Registry.MouseStatus)
                    {
                        case Registry.MouseCondition.Selected:
                            break;
                        case Registry.MouseCondition.Hovered:
                            Registry.MouseStatus = Registry.MouseCondition.Selected;
                            break;
                        case Registry.MouseCondition.AddState:
                            break;
                        case Registry.MouseCondition.DeleteState:
                            Utils.DrawCircle(drawingBoard, ImageLocation, radius, Brushes.White, Pens.White, true);
                            State.StateCollection[StateName].Dispose();
                            State.StateCollection.Remove(StateName);
                            Registry.MouseStatus = Registry.MouseCondition.Default;
                            break;
                        case Registry.MouseCondition.MoveState:
                            break;
                        case Registry.MouseCondition.ConnectOne:
                            if (State.StateHovered == null)
                            {
                                State.StateHovered = this;
                            }
                            else if (StateHovered != this)
                            {
                                StateHovered.m_TargetOne = this;
                                string myindex = GetNearestPointIndex(StateHovered.CenterLocation);
                                Registry.LineParam _line = new Registry.LineParam();
                                _line.Source = StateHovered.GetMagnetPoint(CenterLocation);
                                _line.Destination = _AvailablePoints[myindex].Location; 
                                Pen _pen = new Pen(Color.Red, 3);
                                _pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                                _line.LineColor = _pen;
                                Utils.DrawLine(drawingBoard, _line, true);
                                Registry.MouseStatus = Registry.MouseCondition.Default;
                                StateHovered = null;
                            }
                            break;
                        case Registry.MouseCondition.ConnectZero:
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (Registry.MouseStatus)
                    {
                        case Registry.MouseCondition.Selected:
                            if (StateHovered == this)
                            {
                                Utils.DrawCircle(drawingBoard, ImageLocation, radius, Brushes.DarkBlue, Pens.Black, true);
                                StateHovered = null;
                                Registry.MouseStatus = Registry.MouseCondition.Default;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    drawingBoard.MouseMove -= this.MouseHovered;
                    drawingBoard.MouseDown -= this.MouseDowned;
                    drawingBoard.MouseUp -= this.MouseReleased;
                    drawingBoard.MouseClick -= this.MouseClicked;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~State() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        private string GetNearestPointIndex(Point pt)
        {
            double MinDist = 99999;
            string MinDistKey = "-1";
            double distance;
            foreach (KeyValuePair<string,MagnetPoint> pont in _AvailablePoints)
            {
                Point index = pont.Value.Location;
                distance = Math.Sqrt(Math.Pow(index.X - pt.X, 2) + Math.Pow(index.Y - pt.Y, 2));
                if (distance < MinDist)
                {
                    MinDist = distance;
                    MinDistKey = pont.Key;
                }
            }
            return MinDistKey;
        }

        private Point GetMagnetPoint(Point pt)
        {
            double MinDist = 99999;
            Point MinDistKey = new Point();
            double distance;
            foreach (KeyValuePair<string, MagnetPoint> pont in _AvailablePoints)
            {
                Point index = pont.Value.Location;
                distance = Math.Sqrt(Math.Pow(index.X - pt.X, 2) + Math.Pow(index.Y - pt.Y, 2));
                if (distance < MinDist)
                {
                    MinDist = distance;
                    MinDistKey = pont.Value.Location;
                }
            }
            return MinDistKey;
        }

    }
}
