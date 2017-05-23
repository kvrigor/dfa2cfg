﻿using System;
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

        private Codecs.Languages.Regular.DFA test;

        public frmMainGUI()
        {
            InitializeComponent();
            DiagramArea.Image = new Bitmap(DiagramArea.Width, DiagramArea.Height);
            Utils.Registry.FixedImage = (Image)DiagramArea.Image.Clone();
            _ticker.Interval = 100;
            _ticker.Tick += ticker_Ticked;
            _ticker.Start();

            test = new Codecs.Languages.Regular.DFA();
            Codecs.Languages.Regular.TransFunc xyz;
        }

        private void ts_btnAddState_Click(object sender, EventArgs e)
        {
            if (State.StateCollection.Count >= _stateMaxCount)
                MessageBox.Show("You already achieved the maximum number of states which is " + _stateMaxCount.ToString(), "Maximum number of states is achieved.");
            else
                Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.AddState;
        }

        private void DiagramArea_MouseMove(object sender, MouseEventArgs e)
        {
            switch (Utils.Registry.MouseStatus)
            {
                case Utils.Registry.MouseCondition.AddState:
                    Utils.Utils.DrawCircle(DiagramArea, new Point(e.Location.X - 25, e.Location.Y - 25), 25, Brushes.LightBlue, Pens.Black, false);
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
                switch (Utils.Registry.MouseStatus)
                {
                    case Utils.Registry.MouseCondition.Default:
                        break;
                    case Utils.Registry.MouseCondition.AddState:
                        Point pointedAt = e.Location;
                        cnt++;
                        string name = "STATE" + cnt.ToString();
                        State.StateCollection.Add(name, new State(name, pointedAt, DiagramArea));
                        DiagramArea.MouseMove += State.StateCollection[name].MouseHovered;
                        DiagramArea.MouseDown += State.StateCollection[name].MouseDowned;
                        DiagramArea.MouseUp += State.StateCollection[name].MouseReleased;
                        DiagramArea.MouseClick += State.StateCollection[name].MouseClicked;
                        State.StateCollection[name].DrawIn(DiagramArea);
                        Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.Default;
                        _newStateSet = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private void ts_btnDeleteState_Click(object sender, EventArgs e)
        {
            Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.DeleteState;
        }

        private void ts_btnConnect1_Click(object sender, EventArgs e)
        {
            Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.ConnectOne;
        }

        private void ts_btnConnect0_Click(object sender, EventArgs e)
        {
            Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.ConnectZero;
        }

        private void ticker_Ticked(object sender, EventArgs e)
        {
            lblMouseStatus.Text = Utils.Registry.MouseStatus.ToString();
        }

        private void tsbtnTest_Click(object sender, EventArgs e)
        {
            Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.Accept;
        }

        private void ts_btnProcess_Click(object sender, EventArgs e)
        {
            StatesViewer.Columns.Clear();
            StatesViewer.Columns.Add("StateName", "State");
            StatesViewer.Columns.Add("Input1", "1");
            StatesViewer.Columns.Add("Input0", "0");
            foreach (KeyValuePair<string,State> item in State.StateCollection)
            {
                StatesViewer.Rows.Add(item.Key, item.Value.m_TargetOne.Name, item.Value.m_TargetZero.Name);
            }
        }
    }
}
