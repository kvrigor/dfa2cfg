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
        private Panel drawingBoard;

        private Point m_CenterLocation;
        private State m_TargetOne;
        private State m_TargetZero;
        private DateTime timepressed;

        //For MoveState
        private Rectangle _moveState = new Rectangle();
        private bool _blnMoveSet = false;


        private static State StateHovered;
        public static Dictionary<string, State> StateCollection = new Dictionary<string, State>();
        public static void DrawAllStates(Panel panelArea, State exception = null)
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

        public State(string _name, Point centerPoint)
        {
            StateName = _name;
            ImageLocation = new Point(centerPoint.X - radius, centerPoint.Y - radius);
            CenterLocation = centerPoint;
            
            
        }

        public void DrawIn(Panel panelArea)
        {
            drawingBoard = panelArea;
            Graphics myCircle;
            Pen myPen = new Pen(Color.Black);
            Brush myBrush = Brushes.DarkBlue;
            myCircle = panelArea.CreateGraphics();
            myCircle.FillEllipse(myBrush, new Rectangle(ImageLocation, circleSize));
            myCircle.DrawEllipse(myPen, new Rectangle(ImageLocation, circleSize));
            
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
            var panelArea = sender as Panel;
            switch (Globals.MouseStatus)
            {
                case Globals.MouseCondition.Default:
                    if (IsInRange(pointer.Location) && StateHovered != this)
                    {
                        Graphics myCircle;
                        Pen myPen = new Pen(Color.Black);
                        Brush myBrush = Brushes.Blue;
                        myCircle = panelArea.CreateGraphics();
                        myCircle.FillEllipse(myBrush, new Rectangle(ImageLocation, circleSize));
                        myCircle.DrawEllipse(myPen, new Rectangle(ImageLocation, circleSize));
                        StateHovered = this;
                        Globals.MouseStatus = Globals.MouseCondition.Hovered;
                    }
                    else if (!IsInRange(pointer.Location) && StateHovered == this)
                    {
                        Graphics myCircle;
                        Pen myPen = new Pen(Color.Black);
                        Brush myBrush = Brushes.DarkBlue;
                        myCircle = panelArea.CreateGraphics();
                        myCircle.FillEllipse(myBrush, new Rectangle(ImageLocation, circleSize));
                        myCircle.DrawEllipse(myPen, new Rectangle(ImageLocation, circleSize));
                        StateHovered = null;
                    }
                    break;
                case Globals.MouseCondition.Hovered:
                    if (!IsInRange(pointer.Location) && StateHovered == this)
                    {
                        Graphics myCircle;
                        Pen myPen = new Pen(Color.Black);
                        Brush myBrush = Brushes.DarkBlue;
                        myCircle = panelArea.CreateGraphics();
                        myCircle.FillEllipse(myBrush, new Rectangle(ImageLocation, circleSize));
                        myCircle.DrawEllipse(myPen, new Rectangle(ImageLocation, circleSize));
                        StateHovered = null;
                        Globals.MouseStatus = Globals.MouseCondition.Default;
                    }
                    break;
                case Globals.MouseCondition.MoveState:
                    if (StateHovered == this)
                    {

                        if (!_blnMoveSet)
                        {
                            Graphics tempG = panelArea.CreateGraphics();
                            Brush tempB = Brushes.White;
                            Pen tempP = Pens.White;
                            tempG.FillEllipse(tempB, _moveState);
                            tempG.DrawEllipse(tempP, _moveState);
                        }
                        // draw existing states
                        State.DrawAllStates(panelArea, this);

                        _moveState.Location = new Point(e.Location.X - 25, e.Location.Y - 25);
                        _moveState.Size = circleSize;
                        Graphics g = panelArea.CreateGraphics();
                        Brush b = Brushes.LightBlue;
                        Pen p = Pens.Black;
                        g.FillEllipse(b, _moveState);
                        g.DrawEllipse(p, _moveState);
                    }
                    break;
                default:
                    //if (IsInRange(pointer.Location))
                    //{
                    //    Graphics myCircle;
                    //    Pen myPen = new Pen(Color.Black);
                    //    Brush myBrush = Brushes.Blue;
                    //    myCircle = panelArea.CreateGraphics();
                    //    myCircle.FillEllipse(myBrush, new Rectangle(ImageLocation, circleSize));
                    //    myCircle.DrawEllipse(myPen, new Rectangle(ImageLocation, circleSize));
                    //}
                    //else if (!IsInRange(pointer.Location))
                    //{
                    //    Graphics myCircle;
                    //    Pen myPen = new Pen(Color.Black);
                    //    Brush myBrush = Brushes.DarkBlue;
                    //    myCircle = panelArea.CreateGraphics();
                    //    myCircle.FillEllipse(myBrush, new Rectangle(ImageLocation, circleSize));
                    //    myCircle.DrawEllipse(myPen, new Rectangle(ImageLocation, circleSize));
                    //}
                    break;
            }
        }

        public void MouseDowned(object sender, MouseEventArgs e)
        {
            if (StateHovered == this)
            {
                if (e.Button == MouseButtons.Left)
                {
                    switch (Globals.MouseStatus)
                    {
                        case Globals.MouseCondition.Selected:
                            Globals.MouseStatus = Globals.MouseCondition.MoveState;
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
                    switch (Globals.MouseStatus)
                    {
                        case Globals.MouseCondition.MoveState:
                            if ((DateTime.Now - timepressed).TotalMilliseconds < 100)
                            {
                                Globals.MouseStatus = Globals.MouseCondition.Default;
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                            ImageLocation = new Point(pointer.Location.X - radius, pointer.Location.Y - radius);
                            CenterLocation = pointer.Location;
                            Globals.MouseStatus = Globals.MouseCondition.Default;
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
                    switch (Globals.MouseStatus)
                    {
                        case Globals.MouseCondition.Default:
                            break;
                        case Globals.MouseCondition.Hovered:
                            Globals.MouseStatus = Globals.MouseCondition.Selected;
                            break;
                        case Globals.MouseCondition.AddState:
                            break;
                        case Globals.MouseCondition.DeleteState:
                            State.StateCollection[StateName].Dispose();
                            State.StateCollection.Remove(StateName);
                            panelArea.Refresh();
                            Globals.MouseStatus = Globals.MouseCondition.Default;
                            break;
                        case Globals.MouseCondition.MoveState:
                            break;
                        case Globals.MouseCondition.ConnectOne:
                            if (State.StateHovered == null)
                            {
                                State.StateHovered = this;
                            }
                            else if (StateHovered != this)
                            {
                                Graphics oneline = panelArea.CreateGraphics();
                                oneline.DrawLine(Pens.Red, StateHovered.CenterLocation, this.CenterLocation);
                                StateHovered.m_TargetOne = this;
                                Globals.MouseStatus = Globals.MouseCondition.Default;
                                StateHovered = null;
                            }
                            break;
                        case Globals.MouseCondition.ConnectZero:
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
