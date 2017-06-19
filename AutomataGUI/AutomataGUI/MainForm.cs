using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutomataGUI.Utils;

namespace AutomataGUI
{
    public partial class MainForm : Form
    {
        private DFA_Wrapper src;
        private Drawing.CircleParam _lastCircleLocation;
        private Registry.MouseCondition _lastMouseCondition;
         
        public MainForm()
        {
            InitializeComponent();
            drawingBoard.Image = new Bitmap(drawingBoard.Width, drawingBoard.Height);
            Utils.Registry.FixedImage = (Image)drawingBoard.Image.Clone();

            src = new DFA_Wrapper(drawingBoard);
            src.DFAIsEdited += UpdateDFATable;
            src.stateclicked += UpdateStateClicked;
        }

        private void btnAddState_Click(object sender, EventArgs e)
        {
            Registry.MouseStatus = Registry.MouseCondition.AddState;
        }

        private void btnDeleteState_Click(object sender, EventArgs e)
        {
            Registry.MouseStatus = Registry.MouseCondition.DeleteState;
        }

        private void btnStartState_Click(object sender, EventArgs e)
        {
            Registry.MouseStatus = Registry.MouseCondition.StartState;
        }

        private void btnAcceptState_Click(object sender, EventArgs e)
        {
            Registry.MouseStatus = Registry.MouseCondition.Accept;
        }
        
        private void btnC0_Click(object sender, EventArgs e)
        {
            Registry.MouseStatus = Registry.MouseCondition.ZeroStart;
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            src.RemoveLastTransition();
        }

        private void btnC1_Click(object sender, EventArgs e)
        {
            Registry.MouseStatus = Registry.MouseCondition.OneStart;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            src.Dispose();
            src = new DFA_Wrapper(drawingBoard);
            src.DFAIsEdited += UpdateDFATable;
            src.stateclicked += UpdateStateClicked;

            //drawingBoard.Image.Dispose();
            //drawingBoard.Image = _DefaultImage;
            Utils.Drawing.DrawRectangle(drawingBoard, drawingBoard.Size);
            Registry.FixedImage = (Image)drawingBoard.Image.Clone();

            stateclickedLabel.Text = "";

            UpdateDFATable();
        }

        private void drawingBoard_MouseMove(object sender, MouseEventArgs e)
        {

            Utils.Drawing.CircleParam temp = new Drawing.CircleParam();
            switch (Registry.MouseStatus)
            {
                case Registry.MouseCondition.AddState:
                    temp.Radius = Utils.Registry.Radius;
                    temp.CenterLocation = e.Location;
                    temp.FillColor = Utils.Registry.StateColors.Hovered;
                    temp.OutlineColor = Pens.Black;
                    Utils.Drawing.DrawCircle(drawingBoard, temp, false, Utils.Utils.MapToAlphabet(src.NumStates));
                    _lastCircleLocation = temp;
                    Cursor.Current = Cursors.Hand;
                    break;
                case Registry.MouseCondition.DeleteState:
                    Cursor.Current = Cursors.UpArrow;
                    break;
                case Registry.MouseCondition.StartState:
                    Cursor.Current = Cursors.PanEast;
                    break;
                case Registry.MouseCondition.Accept:
                    Cursor.Current = Cursors.PanWest;
                    break;
                case Registry.MouseCondition.ZeroStart:
                case Registry.MouseCondition.ZeroEnd:
                case Registry.MouseCondition.OneStart:
                case Registry.MouseCondition.OneEnd:
                    Cursor.Current = Cursors.Cross;
                    break;
                default:
                    break;
            }
            _lastMouseCondition = Registry.MouseStatus;
        }

        private void drawingBoard_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (Registry.MouseStatus)
                {
                    case Registry.MouseCondition.AddState:
                        src.AddState(e.Location);
                        break;
                    case Registry.MouseCondition.ZeroStart:
                        statusLabel.Text = "Set destination state for transition 0";
                        break;
                    case Registry.MouseCondition.ZeroEnd:
                        statusLabel.Text = "Set source state for transition 0";
                        break;
                    case Registry.MouseCondition.OneStart:
                        statusLabel.Text = "Set destination state for transition 1";
                        break;
                    case Registry.MouseCondition.OneEnd:
                        statusLabel.Text = "Set source state for transition 1";
                        break;
                }
            }
            else
            {
                switch (Registry.MouseStatus)
                {
                    case Registry.MouseCondition.AddState:
                        btnAddState.Checked = false;
                        Utils.Drawing.UnDrawCircle(drawingBoard, _lastCircleLocation);
                        break;
                    case Registry.MouseCondition.DeleteState:
                        btnDeleteState.Checked = false;
                        break;
                    case Registry.MouseCondition.StartState:
                         btnStartState.Checked = false;
                        break;
                    case Registry.MouseCondition.Accept:
                         btnAcceptState.Checked = false;
                        break;
                    case Registry.MouseCondition.ZeroStart:
                    case Registry.MouseCondition.ZeroEnd:
                        btnC0.Checked = false;
                        break;
                    case Registry.MouseCondition.OneStart:
                    case Registry.MouseCondition.OneEnd:
                        btnC1.Checked = false;
                        break;
                    default:
                        break;
                }
                _lastMouseCondition = Registry.MouseCondition.Default;
                Registry.MouseStatus = Registry.MouseCondition.Default;
                statusLabel.Text = "DFA diagram is incomplete";
            }
                
        }

        private void drawingBoard_SizeChanged(object sender, EventArgs e)
        {
            if (drawingBoard.Image != null)
            {
                Bitmap test = new Bitmap(drawingBoard.Width, drawingBoard.Height);
                Graphics g = Graphics.FromImage(test);
                g.DrawImage(drawingBoard.Image, 0, 0);
                drawingBoard.Image = test;
                Utils.Registry.FixedImage = (Image)drawingBoard.Image.Clone();
            }
        }

        private void toolstripButtons_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender.GetType() == typeof(ToolStripButton)) && ((ToolStripButton)sender).Checked)
            {
                foreach (ToolStripItem item in toolStrip1.Items)
                {
                    if (item.GetType() == typeof(ToolStripButton))
                    {
                        if (((ToolStripButton)item).Name != ((ToolStripButton)sender).Name)
                        {
                            ((ToolStripButton)item).Checked = false;
                            if (_lastMouseCondition == Registry.MouseCondition.AddState)
                                Utils.Drawing.UnDrawCircle(drawingBoard, _lastCircleLocation);
                        }
                        else
                        {
                            switch (((ToolStripButton)item).Name)
                            {
                                case "btnAddState":
                                    statusLabel.Text = "Add a new state on the drawing pane";
                                    break;
                                case "btnDeleteState":
                                    statusLabel.Text = "Select state to be deleted";
                                    break;
                                case "btnStartState":
                                    statusLabel.Text = "Select which state to become the start state";
                                    break;
                                case "btnAcceptState":
                                    statusLabel.Text = "Select which state/s to become the accept state/s";
                                    break;
                                case "btnC0":
                                    statusLabel.Text = "Set source state for transition 0";
                                    break;
                                case "btnC1":
                                    statusLabel.Text = "Set source state for transition 1";
                                    break;
                            }
                            
                        }
                    }
                }
            }
        }

        private void drawingBoard_MouseLeave(object sender, EventArgs e)
        {
            if (_lastMouseCondition == Registry.MouseCondition.AddState)
            {
                Utils.Drawing.UnDrawCircle(drawingBoard, _lastCircleLocation);
                _lastMouseCondition = Registry.MouseCondition.Default;
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
           switch (e.KeyCode)
            {
                case Keys.A:
                    //add new state
                    Registry.MouseStatus = Registry.MouseCondition.AddState;
                    btnAddState.Checked = true;
                    break;
                case Keys.S:
                    //set start state
                    Registry.MouseStatus = Registry.MouseCondition.StartState;
                    btnStartState.Checked = true;
                    break;
                case Keys.D:
                    //delete state
                    Registry.MouseStatus = Registry.MouseCondition.DeleteState;
                    btnDeleteState.Checked = true;
                    break;
                case Keys.F:
                    //set accept state
                    Registry.MouseStatus = Registry.MouseCondition.Accept;
                    btnAcceptState.Checked = true;
                    break;
                case Keys.O:
                    //connect 0
                    Registry.MouseStatus = Registry.MouseCondition.ZeroStart;
                    btnC0.Checked = true;
                    break;
                case Keys.I:
                    //connect 1
                    Registry.MouseStatus = Registry.MouseCondition.OneStart;
                    btnC1.Checked = true;
                    break;
                case Keys.Back:
                    //undo last connection
                    Registry.MouseStatus = Registry.MouseCondition.Default;
                    btnUndo_Click(sender, e);
                    break;
            }
        }

        private void UpdateStateClicked()
        {
            if (Registry.LastClickedState != null)
                stateclickedLabel.Text = "State " + Registry.LastClickedState.Name;
        }

        private void UpdateDFATable()
        {
            lstvwDFATable.Items.Clear();

            int count = src.DFAObject.States.Length;
            for (int i = 0; i < count; i++)
            {
                string[] info = new string[5];
                ListViewItem anItem;

                string get0;
                string get1;
                try
                {
                    get0 = src.DFAObject.Transitions.Where(x => x.PrevState == src.DFAObject.States[i] && x.Input == "0").First().NextState;
                }
                catch (Exception)
                {
                    get0 = "";
                }
                try
                {
                    get1 = src.DFAObject.Transitions.Where(x => x.PrevState == src.DFAObject.States[i] && x.Input == "1").First().NextState;
                }
                catch (Exception)
                {
                    get1 = "";
                }

                info[0] = (src.DFAObject.StartState == src.DFAObject.States[i]).ToString(); // isstart
                info[1] = src.DFAObject.States[i]; // statename
                info[2] = get0; // 0 trans
                info[3] = get1; // 1 trans
                info[4] = (src.DFAObject.AcceptStates.ToList().Contains(src.DFAObject.States[i])).ToString(); // isaccept
                anItem = new ListViewItem(info);
                lstvwDFATable.Items.Add(anItem);
            }

            try
            {
                Codecs.Languages.ContextFree.CFG test = Codecs.Utils.DFAExtensions.ToCFG(src.DFAObject);
                richTextBox1.Text = test.ToString();
                lstvwDFATable.BackColor = Color.LightGreen;
            }
            catch (Exception)
            {
                lstvwDFATable.BackColor = Color.MistyRose;
            }
        }
    }
}
