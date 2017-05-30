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
        private MagnetPoint _ZeroSource;
        private MagnetPoint _ZeroTarget;

        private List<State_Wrapper> _lstStates;
        private List<TransFunc> _lstTransFunc;

        private struct MagnetPoint
        {
            public State_Wrapper State;
            public Point indexPoint;
            public void NullIt()
            {
                State = null;
                indexPoint = new Point(0, 0);
            }
        }

        public DFA_Wrapper(PictureBox drawingBoard)
        {
            _dfa = new DFA();
            _name_counter = 0;
            _lstStates = new List<State_Wrapper>();
            _lstTransFunc = new List<TransFunc>();
            _drawingBoard = drawingBoard;
        }

        public int NumStates { get { return _name_counter; } }

        public void AddState(Point center)
        {
            State_Wrapper _state = new State_Wrapper(_drawingBoard, Utils.Utils.MapToAlphabet(_name_counter++), center);
            _state.StateHovered += _lstState_StateHovered;
            _state.StateLeaveHovered += _lstState_StateLeaveHovered;
            _state.StateDeleted += _lstState_StateDeleted;
            _state.StateSetStart += _lstState_StateSetStart;
            _state.StateSetAccept += _lstStates_StateSetAccept;
            _state.StateZeroStart += _lstStatesZeroStart;
            _state.StateZeroEnd += _lstStatesZeroEnd;
            Draw(_state, Utils.Registry.StateColors.Default, true);      
            AddState(_state);
        }

        private void _lstState_StateHovered(State_Wrapper sender, EventArgs e)
        {
            Draw(sender, Utils.Registry.StateColors.Hovered, true);
            if (sender.IsAcceptState)
                DrawAccept(sender, Utils.Registry.StateColors.Hovered, true);
            if (Utils.Registry.MouseStatus == Utils.Registry.MouseCondition.ZeroStart || Utils.Registry.MouseStatus == Utils.Registry.MouseCondition.ZeroEnd)
                sender.ShowConnectingPoint(_drawingBoard);
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
        }

        private void _lstStates_StateSetAccept(State_Wrapper sender, EventArgs e)
        {
            sender.IsAcceptState = true;
            _dfa.AddFinalStates(sender.Name);
            _lstState_StateHovered(sender, e);
        }

        private void _lstStatesZeroStart(State_Wrapper sender, MouseEventArgs e)
        {
            _ZeroSource.State = sender;
            _ZeroSource.indexPoint = _ZeroSource.State.GetPointIndex(e.Location);
            TransFunc dummy = DeleteTransitions(_ZeroSource.State, "0");
            if (dummy != null)
            {

            }
            Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.ZeroEnd;
        }

        private void _lstStatesZeroEnd(State_Wrapper sender, EventArgs e)
        {
            _ZeroTarget.State = sender;
            DrawLine(_ZeroSource.State, _ZeroTarget.State, true);
            AddTransitions(_ZeroSource.State, _ZeroTarget.State, "0");
            Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.Default;
            _ZeroSource.NullIt();
            _ZeroTarget.NullIt();
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

        private void AddTransitions(State_Wrapper prev, State_Wrapper next, string input)
        {
            TransFunc currTrans = _lstTransFunc.Find(x => x.PrevState == prev.Name && x.Input == input);
            if (currTrans != null)
                throw new Exception("something not deleted");
            _lstTransFunc.Add(new TransFunc(prev.Name, input, next.Name));

            _dfa.Transitions = _lstTransFunc.ToArray();
        }

        private TransFunc DeleteTransitions(State_Wrapper prev, string input)
        {
            TransFunc currTrans = _lstTransFunc.Find(x => x.PrevState == prev.Name && x.Input == input);
            if (currTrans != null)
            {
                _lstTransFunc.Remove(currTrans);
                return currTrans;
            }
            else
                return null;
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
            Utils.Drawing.DrawCircle(_drawingBoard, dummy, fix, _state.Name);
        }

        private void DrawAccept(State_Wrapper _state, Brush _fillcolor, bool fix)
        {
            Utils.Drawing.CircleParam dummy = new Utils.Drawing.CircleParam();
            dummy.CenterLocation = _state.CenterLocation;
            dummy.FillColor = _fillcolor;
            dummy.OutlineColor = Pens.Black;
            dummy.Radius = Utils.Registry.Radius - 5;
            Utils.Drawing.DrawCircle(_drawingBoard, dummy, fix, _state.Name);
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

        private void DrawLine(State_Wrapper source, State_Wrapper desti, bool fix)
        {
            Point ptSrc = source.GetPointIndex(desti.CenterLocation);
            Point ptDst = desti.GetPointIndex(source.CenterLocation);

            Utils.Drawing.LineParam dummy = new Utils.Drawing.LineParam();
            dummy.Source = ptSrc;
            dummy.Destination = ptDst;
            Pen testPen = new Pen(Color.Black, 4);
            testPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            dummy.LineColor = testPen;
            Utils.Drawing.DrawLine(_drawingBoard, dummy, true);
        }
    }
}
