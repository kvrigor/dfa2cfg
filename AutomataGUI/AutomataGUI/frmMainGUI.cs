using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomataGUI
{
    public partial class frmMainGUI : Form
    {
        private int cnt = 0;
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
                        if (State.StateCollection.Count < 5)
                        {
                            Point pointedAt = e.Location;
                            cnt++;
                            string name = cnt.ToString();
                            State.StateCollection.Add(name, new State(name, pointedAt));
                            DiagramArea.MouseMove += State.StateCollection[name].MouseHovered;
                            DiagramArea.MouseDown += State.StateCollection[name].MouseDowned;
                            DiagramArea.MouseUp += State.StateCollection[name].MouseReleased;
                            DiagramArea.MouseClick += State.StateCollection[name].MouseClicked;
                            State.StateCollection[name].DrawIn(DiagramArea);
                            DiagramArea.Refresh();
                        }
                        else
                        {
                            MessageBox.Show("number state is at limit.");
                        }
                        Globals.MouseStatus = Globals.MouseCondition.Default;
                        break;
                    default:
                        break;
                }
            }
        }

        private void ts_btnAddState_Click(object sender, EventArgs e)
        {
            Globals.MouseStatus = Globals.MouseCondition.AddState;
        }

        private void DiagramArea_Paint(object sender, PaintEventArgs e)
        {
            foreach (KeyValuePair<string, State> item in State.StateCollection)
            {
                item.Value.DrawIn(DiagramArea);
            }
            lblStateCount.Text = State.StateCollection.Count.ToString();
        }

        private void DiagramArea_MouseMove(object sender, MouseEventArgs e)
        {
            switch (Globals.MouseStatus)
            {
                case Globals.MouseCondition.AddState:
                    DiagramArea.Refresh();
                    Point pointedAt = new Point(e.Location.X - 25, e.Location.Y - 25);
                    Graphics myCircle;
                    Pen myPen = new Pen(Color.Black);
                    Brush myBrush = Brushes.DarkBlue;
                    myCircle = DiagramArea.CreateGraphics();
                    myCircle.FillEllipse(myBrush, new Rectangle(pointedAt, new Size(50, 50)));
                    myCircle.DrawEllipse(myPen, new Rectangle(pointedAt, new Size(50, 50)));
                    break;
                default:
                    break;
            }
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
