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
using System.Diagnostics;

namespace AutomataGUI
{
    public partial class MainForm : Form
    {
        private DFA_Wrapper src;
        private Drawing.CircleParam _lastCircleLocation;
        Utils.Drawing.LineParam _lastGuideLine;
        private Registry.MouseCondition _lastMouseCondition;
        private bool _clearWorkspace;
        private Point? _zeroStart, _oneStart;

        public MainForm()
        {
            InitializeComponent();
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileMajorPart + "." + fvi.FileMinorPart;
            this.Text = "DFA2CFG v" + version + " Beta";
            
            drawingBoard.Image = new Bitmap(drawingBoard.Width, drawingBoard.Height);
            Utils.Registry.FixedImage = (Image)drawingBoard.Image.Clone();

            src = new DFA_Wrapper(drawingBoard);
            src.DFAIsEdited += UpdateDFATable;
            src.stateclicked += UpdateStateClicked;
            _clearWorkspace = false;
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
            if (MessageBox.Show("Are you sure you want to clear the drawing pane?", "DFA2CFG", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                src.Dispose();
                src = new DFA_Wrapper(drawingBoard);
                src.DFAIsEdited += UpdateDFATable;
                src.stateclicked += UpdateStateClicked;
                Utils.Drawing.DrawRectangle(drawingBoard, drawingBoard.Size);
                Registry.FixedImage = (Image)drawingBoard.Image.Clone();    
                _clearWorkspace = true;
                UpdateDFATable();
                _clearWorkspace = false;
                drawingBoard_MouseClick(sender, new MouseEventArgs(MouseButtons.Right, 1, 1, 1, 1));
            }
        }

        private void drawingBoard_MouseMove(object sender, MouseEventArgs e)
        {

       
            switch (Registry.MouseStatus)
            {
                case Registry.MouseCondition.AddState:
                    Utils.Drawing.CircleParam temp = new Drawing.CircleParam();
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
                    Cursor.Current = Cursors.Cross;
                    statusLabel.Text = "Set source state for transition 0";            
                    break;
                case Registry.MouseCondition.OneStart:
                    Cursor.Current = Cursors.Cross;
                    statusLabel.Text = "Set source state for transition 1";      
                    break;
                case Registry.MouseCondition.ZeroEnd:
                    Cursor.Current = Cursors.Cross;
                    statusLabel.Text = "Selected source state is " + Registry.LastClickedState.Name + "; set destination state for transition 0";
                    DrawGuideLine(e, true);
                    break;
                case Registry.MouseCondition.OneEnd:
                    Cursor.Current = Cursors.Cross;
                    statusLabel.Text = "Selected source state is " + Registry.LastClickedState.Name + "; set destination state for transition 1";
                    DrawGuideLine(e, false);
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
                        _zeroStart = e.Location;
                        break;
                    case Registry.MouseCondition.OneStart:
                        _oneStart = e.Location;
                        break;
                    case Registry.MouseCondition.ZeroEnd:
                        _zeroStart = null;
                        break;
                    case Registry.MouseCondition.OneEnd:
                        _oneStart = null;
                        break;
                    default:
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
                        btnC0.Checked = false;
                        break;
                    case Registry.MouseCondition.ZeroEnd:
                        Utils.Drawing.UnDrawLine(drawingBoard, _lastGuideLine);
                        btnC0.Checked = false;
                        break;
                    case Registry.MouseCondition.OneStart:
                        btnC1.Checked = false;
                        break;
                    case Registry.MouseCondition.OneEnd:
                        Utils.Drawing.UnDrawLine(drawingBoard, _lastGuideLine);
                        btnC1.Checked = false;
                        break;
                    default:
                        break;
                }
                if (lstvwDFATable.BackColor == Color.MistyRose)
                    statusLabel.Text = "DFA diagram is incomplete";
                else if (lstvwDFATable.BackColor == Color.LightGreen)
                    statusLabel.Text = "DFA to CFG conversion successful";
                else
                    statusLabel.Text = "";
                _lastMouseCondition = Registry.MouseCondition.Default;
                Registry.MouseStatus = Registry.MouseCondition.Default;
                _zeroStart = null;
                _oneStart = null;
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
                            else if ((_lastMouseCondition == Registry.MouseCondition.ZeroEnd) || (_lastMouseCondition == Registry.MouseCondition.OneEnd))
                                Utils.Drawing.UnDrawLine(drawingBoard, _lastGuideLine);
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
            else if ((_lastMouseCondition == Registry.MouseCondition.ZeroEnd) || (_lastMouseCondition == Registry.MouseCondition.OneEnd))
            {
                Utils.Drawing.UnDrawLine(drawingBoard, _lastGuideLine);
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
                    btnUndo_Click(sender, e);
                    break;
                case Keys.Escape:
                    //clear drawing pane
                    Registry.MouseStatus = Registry.MouseCondition.Default;
                    btnClear_Click(sender, e);
                    break;
            }
        }

        private void UpdateStateClicked()
        {
            //if (Registry.LastClickedState != null)
            //    stateclickedLabel.Text = Registry.LastClickedState.Name;
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
                if (!_clearWorkspace)
                    lstvwDFATable.BackColor = Color.MistyRose;
                else
                    lstvwDFATable.BackColor = Color.White;
                richTextBox1.Clear();
            }
        }

        private void DrawGuideLine(MouseEventArgs e, bool isTransZero)
        {
            if (isTransZero && _zeroStart != null)
            {
                Utils.Drawing.LineParam guideLine = new Utils.Drawing.LineParam();
                guideLine.Source = (Point)_zeroStart;
                Pen testPen = new Pen(Color.Black, 4);
                testPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                guideLine.Destination = e.Location;
                guideLine.LineColor = testPen;
                Utils.Drawing.DrawLine(drawingBoard, guideLine, false);
                _lastGuideLine = guideLine;
            }
            else if (!isTransZero && _oneStart != null)
            {
                Utils.Drawing.LineParam guideLine = new Utils.Drawing.LineParam();
                guideLine.Source = (Point)_oneStart;
                Pen testPen = new Pen(Color.Blue, 4);
                testPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                guideLine.Destination = e.Location;
                guideLine.LineColor = testPen;
                Utils.Drawing.DrawLine(drawingBoard, guideLine, false);
                _lastGuideLine = guideLine;
            }           
        }
    }
}
