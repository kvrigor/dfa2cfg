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

        private int cnt = 0;
        private const int _stateMaxCount = 5;

        private Timer _ticker = new Timer();


        public frmMainGUI()
        {
            InitializeComponent();
            DiagramArea.Image = new Bitmap(DiagramArea.Width, DiagramArea.Height);
            Registry.FixedImage = (Image)DiagramArea.Image.Clone();
            _ticker.Interval = 100;
            _ticker.Tick += ticker_Ticked;
            _ticker.Start();
        }

        private void ts_btnAddState_Click(object sender, EventArgs e)
        {
            if (State.StateCollection.Count >= _stateMaxCount)
                MessageBox.Show("You already achieved the maximum number of states which is " + _stateMaxCount.ToString(), "Maximum number of states is achieved.");
            else
                Registry.MouseStatus = Registry.MouseCondition.AddState;
        }

        private void DiagramArea_MouseMove(object sender, MouseEventArgs e)
        {
            switch (Registry.MouseStatus)
            {
                case Registry.MouseCondition.AddState:
                    Utils.DrawCircle(DiagramArea, new Point(e.Location.X - 25, e.Location.Y - 25), 25, Brushes.LightBlue, Pens.Black, false);
                    break;
                default:
                    break;
            }
            lblMousePos.Text = e.Location.X.ToString() + "," + e.Location.Y.ToString();
        }

        private void DiagramArea_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (Registry.MouseStatus)
                {
                    case Registry.MouseCondition.Default:
                        break;
                    case Registry.MouseCondition.AddState:
                        Point pointedAt = e.Location;
                        cnt++;
                        string name = cnt.ToString();
                        State.StateCollection.Add(name, new State(name, pointedAt, DiagramArea));
                        DiagramArea.MouseMove += State.StateCollection[name].MouseHovered;
                        DiagramArea.MouseDown += State.StateCollection[name].MouseDowned;
                        DiagramArea.MouseUp += State.StateCollection[name].MouseReleased;
                        DiagramArea.MouseClick += State.StateCollection[name].MouseClicked;
                        State.StateCollection[name].DrawIn(DiagramArea);
                        Registry.MouseStatus = Registry.MouseCondition.Default;
                        _newStateSet = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private void ts_btnDeleteState_Click(object sender, EventArgs e)
        {
            Registry.MouseStatus = Registry.MouseCondition.DeleteState;
        }

        private void ts_btnConnect1_Click(object sender, EventArgs e)
        {
            Registry.MouseStatus = Registry.MouseCondition.ConnectOne;
        }

        private void ts_btnConnect0_Click(object sender, EventArgs e)
        {
            Registry.MouseStatus = Registry.MouseCondition.ConnectZero;
        }

        private void ticker_Ticked(object sender, EventArgs e)
        {
            lblMouseStatus.Text = Registry.MouseStatus.ToString();
        }

        private void tsbtnTest_Click(object sender, EventArgs e)
        {
            
        }
    }
}
