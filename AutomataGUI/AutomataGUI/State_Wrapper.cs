using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomataGUI
{
    public class State_Wrapper
    {
        private string _name;
        private Point _centerLocation;
        private bool _ishovered;
        private bool _isaccept;

        public delegate void StateEvents(State_Wrapper sender, EventArgs e);
        public StateEvents StateHovered;
        public StateEvents StateLeaveHovered;
        public StateEvents StateDeleted;
        public StateEvents StateSetStart;
        public StateEvents StateSetAccept;


        public string Name { get { return _name; } }
        public bool IsAcceptState { get { return _isaccept; } set { _isaccept = value; } }
        public Point CenterLocation { get { return _centerLocation; } }
        public Point ImageLocation { get { return new Point(_centerLocation.X - Utils.Registry.Radius, _centerLocation.Y - Utils.Registry.Radius); } }
        public Point SetStartPoint { get { return new Point(_centerLocation.X - Utils.Registry.Radius, _centerLocation.Y); } }

        public State_Wrapper(PictureBox drawingBoard, string name, Point center)
        {
            drawingBoard.MouseMove += drawingBoard_MouseMove;
            drawingBoard.MouseClick += drawingBoard_MouseClicked;
            _name = name;
            _centerLocation = center;
        }

        public void Dispose(PictureBox drawingBoard)
        {
            drawingBoard.MouseMove -= drawingBoard_MouseMove;
            drawingBoard.MouseClick -= drawingBoard_MouseClicked;
        }

        private void drawingBoard_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsInRange(e.Location))
            {
                if (!_ishovered)
                {
                    StateHovered?.Invoke(this, e);
                    _ishovered = true;
                }
            }
            else
            {
                if (_ishovered)
                {
                    StateLeaveHovered?.Invoke(this, e);
                    _ishovered = false;
                }
            }
        }

        private void drawingBoard_MouseClicked(object sender, MouseEventArgs e)
        {
            if (!IsInRange(e.Location))
                return;
            if (e.Button == MouseButtons.Left)
            {
                switch (Utils.Registry.MouseStatus)
                {
                    case Utils.Registry.MouseCondition.DeleteState:
                        StateDeleted?.Invoke(this, e);
                        break;
                    case Utils.Registry.MouseCondition.StartState:
                        StateSetStart?.Invoke(this, e);
                        break;
                    case Utils.Registry.MouseCondition.Accept:
                        StateSetAccept?.Invoke(this, e);
                        break;
                    default:
                        break;
                }
            }
        }

        private bool IsInRange(Point mouse_position)
        {
            double Ax, Ay;
            double Bx, By;
            double distance;
            Ax = _centerLocation.X;
            Ay = _centerLocation.Y;
            Bx = mouse_position.X;
            By = mouse_position.Y;
            distance = Math.Sqrt(Math.Pow(Ax - Bx, 2) + Math.Pow(Ay - By, 2));
            if (distance <= Utils.Registry.Radius)
                return true;
            else
                return false;
        }

    }
}
