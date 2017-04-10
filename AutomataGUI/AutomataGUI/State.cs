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
        private Point ImageLocation { get; set; }
        public Point CenterLocation { get { return m_CenterLocation; } set { m_CenterLocation = value; } }

        public State(string _name, Point centerPoint, PictureBox source)
        {
            StateName = _name;
            ImageLocation = new Point(centerPoint.X - radius, centerPoint.Y - radius);
            CenterLocation = centerPoint;
            drawingBoard = source;
        }

        public void DrawIn(PictureBox panelArea)
        {
            Utils.DrawCircle(panelArea, ImageLocation, radius, Brushes.DarkBlue, Pens.Black);
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
                        Utils.DrawCircle(panelArea, ImageLocation, radius, Brushes.Blue, Pens.Black);
                        StateHovered = this;
                        Registry.MouseStatus = Registry.MouseCondition.Hovered;
                    }
                    else if (!IsInRange(pointer.Location) && StateHovered == this)
                    {
                        Utils.DrawCircle(panelArea, ImageLocation, radius, Brushes.DarkBlue, Pens.Black);
                        StateHovered = null;
                    }
                    break;
                case Registry.MouseCondition.Hovered:
                    if (!IsInRange(pointer.Location) && StateHovered == this)
                    {
                        Utils.DrawCircle(panelArea, ImageLocation, radius, Brushes.DarkBlue, Pens.Black);
                        StateHovered = null;
                        Registry.MouseStatus = Registry.MouseCondition.Default;
                    }
                    break;
                case Registry.MouseCondition.MoveState:
                    if (StateHovered == this)
                    {
                        if (!_blnMoveSet)
                        {
                            Utils.DrawCircle(panelArea, _moveState.Location, radius, Brushes.White, Pens.White);
                        }
                        // draw existing states
                        State.DrawAllStates(panelArea, this);

                        _moveState.Location = new Point(e.Location.X - 25, e.Location.Y - 25);
                        _moveState.Size = circleSize;
                        Utils.DrawCircle(panelArea, _moveState.Location, radius, Brushes.LightBlue, Pens.Black);
                    }
                    break;
                case Registry.MouseCondition.DeleteState:
                    if (IsInRange(pointer.Location))
                        Utils.DrawCircle(panelArea, ImageLocation, radius, Brushes.Blue, Pens.Black);
                    else
                        Utils.DrawCircle(panelArea, ImageLocation, radius, Brushes.DarkBlue, Pens.Black);
                    break;
                case Registry.MouseCondition.ConnectOne:
                    if (State.StateHovered == this)
                    {
                        if (IsInRange(pointer.Location))
                        {
                            //do nothing
                        }
                        else
                        {
                            if (_oldConn1s != null)
                            {
                                Utils.DrawLine(panelArea, _oldConn1s, _oldConn1d, Pens.White);
                            }
                            double angle;
                            int x = 0;
                            int y = 0;
                            try
                            {
                                double numer = (double)(pointer.Y - CenterLocation.Y);
                                double denom = (double)(pointer.X - CenterLocation.X);
                                angle = Math.Atan(numer / denom);
                                x = (int)Math.Round(radius * Math.Cos(angle), 0);
                                y = (int)Math.Round(radius * Math.Sin(angle), 0);
                                if (denom < 0)
                                { x = x * -1; y = y * -1; }
                            }
                            catch (Exception ex)
                            {
                                if (pointer.X == CenterLocation.X)
                                {
                                    if (pointer.Y > CenterLocation.Y)
                                        angle = Math.PI / 2;
                                    else
                                        angle = 0 - Math.PI / 2;
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                                    throw;
                                }
                            }
                            Point source = new Point(CenterLocation.X + x, CenterLocation.Y + y);
                            Utils.DrawLine(panelArea, source, pointer.Location, Pens.Red);
                            _oldConn1s = source;
                            _oldConn1d = pointer.Location;
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
                if (e.Button == MouseButtons.Left)
                {
                    switch (Registry.MouseStatus)
                    {
                        case Registry.MouseCondition.Selected:
                            Registry.MouseStatus = Registry.MouseCondition.MoveState;
                            Cursor.Current = Cursors.SizeAll;
                            timepressed = DateTime.Now;
                            break;
                        default:
                            break;
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
                            if ((DateTime.Now - timepressed).TotalMilliseconds < 100)
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
            if (IsInRange(pointer.Location))
            {
                if (e.Button == MouseButtons.Left)
                {
                    switch (Registry.MouseStatus)
                    {
                        case Registry.MouseCondition.Default:
                            break;
                        case Registry.MouseCondition.Hovered:
                            Registry.MouseStatus = Registry.MouseCondition.Selected;
                            break;
                        case Registry.MouseCondition.AddState:
                            break;
                        case Registry.MouseCondition.DeleteState:
                            State.StateCollection[StateName].Dispose();
                            State.StateCollection.Remove(StateName);
                            panelArea.Refresh();
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
                                //Graphics oneline = panelArea.CreateGraphics();
                                //oneline.DrawLine(Pens.Red, StateHovered.CenterLocation, this.CenterLocation);
                                StateHovered.m_TargetOne = this;
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


    }
}
