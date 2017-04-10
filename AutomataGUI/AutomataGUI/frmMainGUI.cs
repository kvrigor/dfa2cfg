using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomataGUI
{
    public partial class frmMainGUI : Form
    {
        // Add State variables
        private Size _circleSize = new Size(50, 50);
        private Rectangle _addNewState = new Rectangle();
        private bool _newStateSet = false;

        private const int _stateMaxCount = 5;



        public frmMainGUI()
        {
            InitializeComponent();
        }

        private void DiagramArea_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (Globals.MouseStatus)
                {
                    case Globals.MouseCondition.Default:
                        break;
                    case Globals.MouseCondition.AddState:
                        Point pointedAt = e.Location;
                        string name = State.StateCollection.Count.ToString();
                        State.StateCollection.Add(name, new State(name, pointedAt));
                        DiagramArea.MouseMove += State.StateCollection[name].MouseHovered;
                        DiagramArea.MouseDown += State.StateCollection[name].MouseDowned;
                        DiagramArea.MouseUp += State.StateCollection[name].MouseReleased;
                        DiagramArea.MouseClick += State.StateCollection[name].MouseClicked;
                        State.StateCollection[name].DrawIn(DiagramArea);
                        Globals.MouseStatus = Globals.MouseCondition.Default;
                        _newStateSet = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private void DiagramArea_MouseMove(object sender, MouseEventArgs e)
        {
            switch (Globals.MouseStatus)
            {
                case Globals.MouseCondition.AddState:
                    if (!_newStateSet)
                    {
                        Graphics tempG = DiagramArea.CreateGraphics();
                        Brush tempB = Brushes.White;
                        Pen tempP = Pens.White;
                        tempG.FillEllipse(tempB, _addNewState);
                        tempG.DrawEllipse(tempP, _addNewState);
                    }
                    // draw existing states
                    State.DrawAllStates(DiagramArea);

                    _addNewState.Location = new Point(e.Location.X - 25, e.Location.Y - 25);
                    _addNewState.Size = _circleSize;
                    Graphics g = DiagramArea.CreateGraphics();
                    Brush b = Brushes.LightBlue;
                    Pen p = Pens.Black;
                    g.FillEllipse(b, _addNewState);
                    g.DrawEllipse(p, _addNewState);
                    break;
                default:
                    break;
            }
        }

        private void ts_btnAddState_Click(object sender, EventArgs e)
        {
            if (State.StateCollection.Count >= _stateMaxCount)
                MessageBox.Show("You already achieved the maximum number of states which is " + _stateMaxCount.ToString(), "Maximum number of states is achieved.");
            else
                Globals.MouseStatus = Globals.MouseCondition.AddState;
        }

        private void ts_btnDeleteState_Click(object sender, EventArgs e)
        {
            Globals.MouseStatus = Globals.MouseCondition.DeleteState;
        }

        private void ts_btnConnect1_Click(object sender, EventArgs e)
        {
            Globals.MouseStatus = Globals.MouseCondition.ConnectOne;
        }

        private void ts_btnConnect0_Click(object sender, EventArgs e)
        {
            Globals.MouseStatus = Globals.MouseCondition.ConnectZero;
        }

        private void frmMainGUI_MouseMove(object sender, MouseEventArgs e)
        {
            lblMouseStatus.Text = Globals.MouseStatus.ToString();
        }
    }
}
