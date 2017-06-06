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
        private MagnetPoint _OneSource;
        private MagnetPoint _OneTarget;

        private List<State_Wrapper> _lstStates;
        private List<Transition_Wrapper> _lstTransFunc;

        private struct MagnetPoint
        {
            public State_Wrapper State;
            public Point indexPoint;
            public void NullIt()
            {
                State = null;
                indexPoint = new Point(0, 0);
            }
            public bool IsNull { get { return (State == null); } }
        }

        private struct Transition_Wrapper
        {
            public TransFunc TransitionFunction;
            public MagnetPoint SourceIndex;
            public MagnetPoint DestinationIndex;
            public Transition_Wrapper(TransFunc x, MagnetPoint src, MagnetPoint dst)
            {
                TransitionFunction = x;
                SourceIndex = src;
                DestinationIndex = dst;
            }
            public void NullIt()
            {
                TransitionFunction = null;
                SourceIndex.NullIt();
                DestinationIndex.NullIt();
            }
            public bool IsNull
            { get { return (TransitionFunction == null) && SourceIndex.IsNull && DestinationIndex.IsNull; } }
        }


        public DFA_Wrapper(PictureBox drawingBoard)
        {
            _dfa = new DFA();
            _name_counter = 0;
            _lstStates = new List<State_Wrapper>();
            _lstTransFunc = new List<Transition_Wrapper>();
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
            _state.StateZeroStart += _lstStatesZeroStart;
            _state.StateZeroEnd += _lstStatesZeroEnd;
            _state.StateOneStart += _lstStatesOneStart;
            _state.StateOneEnd += _lstStatesOneEnd;
            Draw(_state, Utils.Registry.StateColors.Default, true);

            AddState(_state);
        }

        private void _lstState_StateHovered(State_Wrapper sender, MouseEventArgs e)
        {
            Draw(sender, Utils.Registry.StateColors.Hovered, true);
            if (sender.IsAcceptState)
                DrawAccept(sender, Utils.Registry.StateColors.Hovered, true);
            if (Utils.Registry.MouseStatus == Utils.Registry.MouseCondition.ZeroStart || Utils.Registry.MouseStatus == Utils.Registry.MouseCondition.ZeroEnd ||
                Utils.Registry.MouseStatus == Utils.Registry.MouseCondition.OneStart || Utils.Registry.MouseStatus == Utils.Registry.MouseCondition.OneEnd)
                sender.ShowConnectingPoint(_drawingBoard, e.Location);
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

        private void _lstStates_StateSetAccept(State_Wrapper sender, MouseEventArgs e)
        {
            sender.IsAcceptState = true;
            _dfa.AddFinalStates(sender.Name);
            _lstState_StateHovered(sender, e);
            Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.Default;
        }

        private void _lstStatesZeroStart(State_Wrapper sender, MouseEventArgs e)
        {
            _ZeroSource.State = sender;
            _ZeroSource.indexPoint = _ZeroSource.State.GetPointIndex(e.Location);
            Transition_Wrapper dummy = DeleteTransitions(_ZeroSource.State, "0");
            if (!dummy.IsNull)
            {
                DrawLine(dummy.SourceIndex, dummy.DestinationIndex, "0", false, true);
            }
            Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.ZeroEnd;
        }

        private void _lstStatesZeroEnd(State_Wrapper sender, MouseEventArgs e)
        {
            _ZeroTarget.State = sender;
            _ZeroTarget.indexPoint = e.Location;
            DrawLine(_ZeroSource, _ZeroTarget, "0", true, true);
            AddTransitions(_ZeroSource, _ZeroTarget, "0");
            Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.Default;
            _ZeroSource.NullIt();
            _ZeroTarget.NullIt();
        }

        private void _lstStatesOneStart(State_Wrapper sender, MouseEventArgs e)
        {
            _OneSource.State = sender;
            _OneSource.indexPoint = _OneSource.State.GetPointIndex(e.Location);
            Transition_Wrapper dummy = DeleteTransitions(_OneSource.State, "1");
            if (!dummy.IsNull)
            {
                DrawLine(dummy.SourceIndex, dummy.DestinationIndex, "1", false, true);
            }
            Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.OneEnd;
        }

        private void _lstStatesOneEnd(State_Wrapper sender, MouseEventArgs e)
        {
            _OneTarget.State = sender;
            _OneTarget.indexPoint = e.Location;
            DrawLine(_OneSource, _OneTarget, "1", true, true);
            AddTransitions(_OneSource, _OneTarget, "1");
            Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.Default;
            _OneSource.NullIt();
            _OneTarget.NullIt();
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

        private void AddTransitions(MagnetPoint prev, MagnetPoint next, string input)
        {
            Transition_Wrapper currTrans = _lstTransFunc.Find(x => x.TransitionFunction.PrevState == prev.State.Name && x.TransitionFunction.Input == input);
            if (!currTrans.IsNull)
                throw new Exception("something not deleted");
            _lstTransFunc.Add(new Transition_Wrapper(new TransFunc(prev.State.Name, input, next.State.Name), prev, next));

            TransFunc[] dummy = new TransFunc[_lstTransFunc.Count];
            for (int i = 0; i < _lstTransFunc.Count; i++)
                dummy[i] = _lstTransFunc[i].TransitionFunction;

            _dfa.Transitions = dummy;
        }
        private Transition_Wrapper DeleteTransitions(State_Wrapper prev, string input)
        {
            Transition_Wrapper currTrans = _lstTransFunc.Find(x => x.TransitionFunction.PrevState == prev.Name && x.TransitionFunction.Input == input);
            if (!currTrans.IsNull)
            {
                _lstTransFunc.Remove(currTrans);
                return currTrans;
            }
            else
                return new Transition_Wrapper(null, new MagnetPoint(), new MagnetPoint());
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

        private void DrawLine(MagnetPoint source, MagnetPoint desti, string input, bool create, bool fix)
        {
            Point ptSrc = source.indexPoint;
            source.State.SetPointIndexUsed(ptSrc, create);
            Point ptDst = desti.indexPoint;
            desti.State.SetPointIndexUsed(ptDst, create);

            Utils.Drawing.LineParam dummy = new Utils.Drawing.LineParam();
            dummy.Source = ptSrc;
            dummy.Destination = ptDst;
            Pen testPen;
            if (create)
            {
                if (input == "0")
                    testPen = new Pen(Color.Black, 4);
                else
                    testPen = new Pen(Color.Blue, 4);
            }
            else
                testPen = new Pen(Color.White, 4);
            testPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            dummy.LineColor = testPen;
            Utils.Drawing.DrawLine(_drawingBoard, dummy, true);
        }
    }
}
