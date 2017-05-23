using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Codecs.Languages.Regular;

namespace AutomataGUI
{
    public class DFA_Wrapper
    {
        private DFA _dfa;
        private int _name_counter;
        private PictureBox _drawingBoard;

        private List<State_Wrapper> _lstStates;

        public DFA_Wrapper(PictureBox drawingBoard)
        {
            _dfa = new DFA();
            _name_counter = 0;
            _lstStates = new List<State_Wrapper>();
            _drawingBoard = drawingBoard;
        }

        public void AddState(Point center)
        {
            _name_counter++;
            State_Wrapper _state = new State_Wrapper(_drawingBoard, "State" + _name_counter.ToString(), center);
            _state.StateHovered += _lstState_StateHovered;
            _state.StateLeaveHovered += _lstState_StateLeaveHovered;
            _state.StateDeleted += _lstState_StateDeleted;
            _state.StateSetStart += _lstState_StateSetStart;
            _state.StateSetAccept += _lstStates_StateSetAccept;
            Draw(_state, Utils.Registry.StateColors.Default, true);

            AddState(_state);
        }

        private void _lstState_StateHovered(State_Wrapper sender, EventArgs e)
        {
            Draw(sender, Utils.Registry.StateColors.Hovered, true);
            if (sender.IsAcceptState)
                DrawAccept(sender, Utils.Registry.StateColors.Hovered, true);
        }

        private void _lstState_StateLeaveHovered(State_Wrapper sender, EventArgs e)
        {
            Draw(sender, Utils.Registry.StateColors.Default, true);
            if (sender.IsAcceptState)
                DrawAccept(sender, Utils.Registry.StateColors.Default, true);
        }

        private void _lstState_StateDeleted(State_Wrapper sender, EventArgs e)
        {
            sender.Dispose(_drawingBoard);
            DrawRemove(sender, true);
            RemoveState(sender);            
            Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.Default;
        }

        private void _lstState_StateSetStart(State_Wrapper sender, EventArgs e)
        {
            if (_dfa.StartState != "")
            {
                //remove the old StartState
                State_Wrapper xyz = GetState(_dfa.StartState);
                Utils.Drawing.LineParam olddummy = new Utils.Drawing.LineParam();
                olddummy.Source = new Point(xyz.SetStartPoint.X - 25, xyz.SetStartPoint.Y);
                olddummy.Destination = xyz.SetStartPoint;
                Pen oldtestPen = new Pen(Color.White, 4);
                oldtestPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                olddummy.LineColor = oldtestPen;
                Utils.Drawing.DrawLine(_drawingBoard, olddummy, true);
            }
            Utils.Drawing.LineParam dummy = new Utils.Drawing.LineParam();
            dummy.Source = new Point(sender.SetStartPoint.X - 25, sender.SetStartPoint.Y);
            dummy.Destination = sender.SetStartPoint;
            Pen testPen = new Pen(Color.Black, 4);
            testPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            dummy.LineColor = testPen;
            Utils.Drawing.DrawLine(_drawingBoard, dummy, true);
            _dfa.StartState = sender.Name;
            Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.Default;
        }

        private void _lstStates_StateSetAccept(State_Wrapper sender, EventArgs e)
        {
            sender.IsAcceptState = true;
            _dfa.AddFinalStates(sender.Name);
            Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.Default;
        }

        private void AddState(State_Wrapper dfa_state)
        {
            _lstStates.Add(dfa_state);
            string[] _state = new string[_lstStates.Count];
            for (int i = 0; i < _lstStates.Count; i++)
                _state[i] = _lstStates[i].Name;
            _dfa.States = _state;
        }

        private void RemoveState(State_Wrapper dfa_state)
        {
            _lstStates.Remove(dfa_state);
            string[] _state = new string[_lstStates.Count];
            for (int i = 0; i < _lstStates.Count; i++)
                _state[i] = _lstStates[i].Name;
            _dfa.States = _state;
            if (_dfa.AcceptStates.Contains(dfa_state.Name))
            {
                string[] accept_state = new string[_dfa.AcceptStates.Length - 1];
                int x = 0;
                for (int i = 0; i < _dfa.AcceptStates.Length; i++)
                    if (_dfa.AcceptStates[i] != dfa_state.Name)
                    {
                        accept_state[x] = _dfa.AcceptStates[i];
                        x++;
                    }
                _dfa.AcceptStates = accept_state;
            }
        }

        private State_Wrapper GetState(string name)
        {
            for (int i = 0; i < _lstStates.Count; i++)
                if (_lstStates[i].Name == name)
                    return _lstStates[i];
            return null;
        }

        private void Draw(State_Wrapper _state, Brush _fillcolor, bool fix)
        {
            Utils.Drawing.CircleParam dummy = new Utils.Drawing.CircleParam();
            dummy.CenterLocation = _state.CenterLocation;
            dummy.FillColor = _fillcolor;
            dummy.OutlineColor = Pens.Black;
            dummy.Radius = Utils.Registry.Radius;
            Utils.Drawing.DrawCircle(_drawingBoard, dummy, fix);
        }

        private void DrawAccept(State_Wrapper _state, Brush _fillcolor, bool fix)
        {
            Utils.Drawing.CircleParam dummy = new Utils.Drawing.CircleParam();
            dummy.CenterLocation = _state.CenterLocation;
            dummy.FillColor = _fillcolor;
            dummy.OutlineColor = Pens.Black;
            dummy.Radius = Utils.Registry.Radius - 5;
            Utils.Drawing.DrawCircle(_drawingBoard, dummy, fix);
        }

        private void DrawRemove(State_Wrapper _state, bool fix)
        {
            Utils.Drawing.CircleParam dummy = new Utils.Drawing.CircleParam();
            dummy.CenterLocation = _state.CenterLocation;
            dummy.FillColor = Brushes.White;
            dummy.OutlineColor = Pens.White;
            dummy.Radius = Utils.Registry.Radius;
            Utils.Drawing.DrawCircle(_drawingBoard, dummy, fix);
        }
    }
}
