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
    public class DFA_Wrapper : IDisposable
    {
        private DFA _dfa;
        private int _name_counter;
        private PictureBox _drawingBoard;
        private MagnetPoint _ZeroSource;
        private MagnetPoint _ZeroTarget;
        private MagnetPoint _OneSource;
        private MagnetPoint _OneTarget;
        private Stack<TransitionInfo> _lastTransitionHistory;
        private List<State_Wrapper> _lstStates;
        private List<Transition_Wrapper> _lstTransFunc;
        private Utils.Drawing.LineParam _startStatePos;

        public delegate void DFAChangedEvents();
        public DFAChangedEvents DFAIsEdited;
        public DFAChangedEvents stateclicked;

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

        private struct TransitionInfo
        {
            public MagnetPoint SourceState;
            public MagnetPoint TargetState;
            public string ConnectionType;
            public bool IsTransition0;
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
            _dfa.InputSymbols = new string[] { "0", "1" };
            _name_counter = 0;
            _lstStates = new List<State_Wrapper>();
            _lstTransFunc = new List<Transition_Wrapper>();
            _drawingBoard = drawingBoard;
            _lastTransitionHistory = new Stack<TransitionInfo>();
        }

        public int NumStates { get { return _name_counter; } }

        public DFA DFAObject { get { return _dfa; } }

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
            _state.StateOneStart += _lstStatesOneStart;
            _state.StateOneEnd += _lstStatesOneEnd;
            _state.OnRepaint += _lstState_OnRePaint;
            _state.StateClicked += triggerupdate;
            Draw(_state, Utils.Registry.StateColors.Default, true);
            AddState(_state);
            Utils.Registry.LastClickedState = _state;
            stateclicked?.Invoke();
            if (_lstStates.Count == 1)
                _lstState_StateSetStart(_state, new EventArgs());
        }

        private void triggerupdate(State_Wrapper sender, MouseEventArgs e)
        {
            Utils.Registry.LastClickedState = sender;
            stateclicked?.Invoke();
        }

        public void RemoveLastTransition()
        {
            if (_lastTransitionHistory.Count > 0)
            {
                TransitionInfo ti = _lastTransitionHistory.Pop();
                Transition_Wrapper tw;

                if (ti.ConnectionType == "Line")
                {
                    DrawLine(ti.SourceState, ti.TargetState, ti.IsTransition0 ? "0" : "1", false, true);
                    tw = DeleteTransitions(ti.SourceState.State, ti.IsTransition0 ? "0" : "1");
                }                
                else if (ti.ConnectionType == "Arc")
                {
                    DrawArcToSelf(ti.SourceState, false, ti.IsTransition0, true);
                    tw = DeleteTransitions(ti.SourceState.State, ti.IsTransition0 ? "0" : "1");
                }
                  
                RepaintAllObjects();
                DFAIsEdited?.Invoke();
            }
        }

        private void RemoveAssociatedTransitions(State_Wrapper state)
        {
            if (_lastTransitionHistory.Count > 0)
            {
                bool removedTransitions = false;
                Stack<TransitionInfo> transitionsRetained = new Stack<TransitionInfo>();
                Transition_Wrapper tw;
                while (_lastTransitionHistory.Count > 0)
                {
                    TransitionInfo ti = _lastTransitionHistory.Pop();
                    if (ti.ConnectionType == "Line")
                    {
                        if ((state.Name == ti.SourceState.State.Name) || (state.Name == ti.TargetState.State.Name))
                        {
                            DrawLine(ti.SourceState, ti.TargetState, ti.IsTransition0 ? "0" : "1", false, true);
                            tw = DeleteTransitions(ti.SourceState.State, ti.IsTransition0 ? "0" : "1");
                            removedTransitions = true;
                        }
                        else
                            transitionsRetained.Push(ti);
                    }
                    else if (ti.ConnectionType == "Arc")
                    {
                        if (state.Name == ti.SourceState.State.Name)
                        {
                            DrawArcToSelf(ti.SourceState, false, ti.IsTransition0, true);
                            tw = DeleteTransitions(ti.SourceState.State, ti.IsTransition0 ? "0" : "1");
                            removedTransitions = true;
                        }
                        else
                            transitionsRetained.Push(ti);
                    }
                }
                while (transitionsRetained.Count > 0)
                    _lastTransitionHistory.Push(transitionsRetained.Pop());
                if (removedTransitions)
                {
                    RepaintAllObjects();
                    DFAIsEdited?.Invoke();
                }
              
            }
        }

        private void RemoveAssociatedTransition(State_Wrapper state, Transition_Wrapper trans, bool isTransition0)
        {
            if (_lastTransitionHistory.Count > 0)
            {
                bool removedTransitions = false;
                Stack<TransitionInfo> transitionsRetained = new Stack<TransitionInfo>();

                while (_lastTransitionHistory.Count > 0)
                {
                    TransitionInfo ti = _lastTransitionHistory.Pop();
                    if ((state.Name == ti.SourceState.State.Name) && (isTransition0 == ti.IsTransition0))
                    {
                        if (ti.ConnectionType == "Line")
                            DrawLine(trans.SourceIndex, trans.DestinationIndex, ti.IsTransition0 ? "0" : "1", false, true);
                        else if (ti.ConnectionType == "Arc")
                            DrawArcToSelf(trans.SourceIndex, false, ti.IsTransition0, true);
                        removedTransitions = true;
                    }
                    else
                        transitionsRetained.Push(ti);
                }
                while (transitionsRetained.Count > 0)
                    _lastTransitionHistory.Push(transitionsRetained.Pop());

                if (removedTransitions)
                {
                    RepaintAllObjects();
                    DFAIsEdited?.Invoke();
                }

            }
        }

        private void RepaintAllObjects()
        {
            //draw transitions
            List<TransitionInfo> allTransitions = _lastTransitionHistory.ToList();
            foreach (TransitionInfo ti in allTransitions)
            {
                if (ti.ConnectionType == "Line")
                    DrawLine(ti.SourceState, ti.TargetState, ti.IsTransition0 ? "0" : "1", true, true);
                else if (ti.ConnectionType == "Arc")
                    DrawArcToSelf(ti.SourceState, true, ti.IsTransition0, true);
            }
            //draw states
            bool hasStartState = false;
            foreach (State_Wrapper state in _lstStates)
            {
                state.RePaint();
                hasStartState = hasStartState || state.IsStartState;
            }
                
            //draw start state
            if (hasStartState)
                Utils.Drawing.DrawLine(_drawingBoard, _startStatePos, true);
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

        private void _lstState_OnRePaint(State_Wrapper sender, MouseEventArgs e)
        {
            Draw(sender, Utils.Registry.StateColors.Default, true);
            if (sender.IsAcceptState)
                DrawAccept(sender, Utils.Registry.StateColors.Default, true);
        }

        private void _lstState_StateLeaveHovered(State_Wrapper sender, EventArgs e)
        {
            Draw(sender, Utils.Registry.StateColors.Default, true);
            if (sender.IsAcceptState)
                DrawAccept(sender, Utils.Registry.StateColors.Default, true);
        }

        private void _lstState_StateDeleted(State_Wrapper sender, EventArgs e)
        {
            if (sender.IsStartState)
            {
                _startStatePos.LineColor.Color = Color.White;
                Utils.Drawing.DrawLine(_drawingBoard, _startStatePos, true);
            }
            RemoveAssociatedTransitions(sender);
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
            _startStatePos = dummy;
            _dfa.StartState = sender.Name;
            sender.IsStartState = true;
            DFAIsEdited?.Invoke();
        }

        private void _lstStates_StateSetAccept(State_Wrapper sender, MouseEventArgs e)
        {
            if (sender.IsAcceptState)
            {
                sender.IsAcceptState = false;
                List<string> lstAccept = _dfa.AcceptStates.ToList();
                lstAccept.Remove(sender.Name);
                _dfa.AcceptStates = lstAccept.ToArray();
                _lstState_StateHovered(sender, e);
            }
            else
            {
                sender.IsAcceptState = true;
                _dfa.AddFinalStates(sender.Name);
                _lstState_StateHovered(sender, e);
            }
            DFAIsEdited?.Invoke();
        }

        private void _lstStatesZeroStart(State_Wrapper sender, MouseEventArgs e)
        {
            _ZeroSource.State = sender;
            _ZeroSource.indexPoint = _ZeroSource.State.GetPointIndex(e.Location);
            Transition_Wrapper dummy = DeleteTransitions(_ZeroSource.State, "0");
            if (!dummy.IsNull)
                RemoveAssociatedTransition(sender, dummy, true);
            Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.ZeroEnd;
        }

        private void _lstStatesZeroEnd(State_Wrapper sender, MouseEventArgs e)
        {
            _ZeroTarget.State = sender;
            _ZeroTarget.indexPoint = _ZeroTarget.State.GetPointIndex(e.Location);
           
            if (_ZeroSource.State == _ZeroTarget.State)
            {
                DrawArcToSelf(_ZeroSource, true, true, true);
                _lastTransitionHistory.Push(new TransitionInfo() { ConnectionType = "Arc", SourceState = _ZeroSource, IsTransition0 = true });
            }               
            else
            {
                DrawLine(_ZeroSource, _ZeroTarget, "0", true, true);
                _lastTransitionHistory.Push(new TransitionInfo() { ConnectionType = "Line", SourceState = _ZeroSource, TargetState = _ZeroTarget, IsTransition0 = true });
            }
                
            AddTransitions(_ZeroSource, _ZeroTarget, "0");
            RepaintAllObjects();
            Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.OneStart;
            _ZeroSource.NullIt();
            _ZeroTarget.NullIt();
        }

        private void _lstStatesOneStart(State_Wrapper sender, MouseEventArgs e)
        {
            _OneSource.State = sender;
            _OneSource.indexPoint = _OneSource.State.GetPointIndex(e.Location);
            Transition_Wrapper dummy = DeleteTransitions(_OneSource.State, "1");
            if (!dummy.IsNull)
                RemoveAssociatedTransition(sender, dummy, false);
            Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.OneEnd;
        }

        private void _lstStatesOneEnd(State_Wrapper sender, MouseEventArgs e)
        {
            _OneTarget.State = sender;
            _OneTarget.indexPoint = _OneTarget.State.GetPointIndex(e.Location);
            if (_OneSource.State == _OneTarget.State)
            {
                DrawArcToSelf(_OneSource, true, false, true);
                _lastTransitionHistory.Push(new TransitionInfo() { ConnectionType = "Arc", SourceState = _OneSource, IsTransition0 = false });
            }              
            else
            {
                DrawLine(_OneSource, _OneTarget, "1", true, true);
                _lastTransitionHistory.Push(new TransitionInfo() { ConnectionType = "Line", SourceState = _OneSource, TargetState = _OneTarget, IsTransition0 = false });
            }
               
            AddTransitions(_OneSource, _OneTarget, "1");
            RepaintAllObjects();
            Utils.Registry.MouseStatus = Utils.Registry.MouseCondition.ZeroStart;
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
            DFAIsEdited?.Invoke();
        }

        private void RemoveState(State_Wrapper dfa_state)
        {
            if (dfa_state.IsStartState)
                _dfa.StartState = "";
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
            DFAIsEdited?.Invoke();
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
            DFAIsEdited?.Invoke();
        }

        private Transition_Wrapper DeleteTransitions(State_Wrapper prev, string input)
        {
            Transition_Wrapper currTrans = _lstTransFunc.Find(x => x.TransitionFunction.PrevState == prev.Name && x.TransitionFunction.Input == input);
            if (!currTrans.IsNull)
            {
                _lstTransFunc.Remove(currTrans);

                TransFunc[] dummy = new TransFunc[_lstTransFunc.Count];
                for (int i = 0; i < _lstTransFunc.Count; i++)
                    dummy[i] = _lstTransFunc[i].TransitionFunction;

                _dfa.Transitions = dummy;
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
            Utils.Drawing.DrawLine(_drawingBoard, dummy, fix);       
        }
        
        private void DrawArcToSelf(MagnetPoint source, bool create, bool iszro, bool fix)
        {

            Utils.Drawing.LineParam dummy = new Utils.Drawing.LineParam();
            Pen testPen;
            if (create)
            {
                if (iszro)
                    testPen = new Pen(Color.Black, 4);
                else
                    testPen = new Pen(Color.Blue, 4);
            }
            else
                testPen = new Pen(Color.White, 4);

            Point reference = source.State.CenterLocation;
            if (iszro)
            {
                Point left = new Point(reference.X - 5, reference.Y - 20);
                Point right = new Point(reference.X + 5, reference.Y - 20);
                dummy.Source = source.State.GetPointIndex(left);
                dummy.Destination = source.State.GetPointIndex(right);
            }
            else
            {
                Point left = new Point(reference.X - 5, reference.Y + 20);
                Point right = new Point(reference.X + 5, reference.Y + 20);
                dummy.Source = source.State.GetPointIndex(left);
                dummy.Destination = source.State.GetPointIndex(right);
            }
            testPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            dummy.LineColor = testPen;
            Utils.Drawing.DrawArc(_drawingBoard, dummy, iszro, fix);        
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    foreach (State_Wrapper item in _lstStates)
                    {
                        item.StateHovered -= _lstState_StateHovered;
                        item.StateLeaveHovered -= _lstState_StateLeaveHovered;
                        item.StateDeleted -= _lstState_StateDeleted;
                        item.StateSetStart -= _lstState_StateSetStart;
                        item.StateSetAccept -= _lstStates_StateSetAccept;
                        item.StateZeroStart -= _lstStatesZeroStart;
                        item.StateZeroEnd -= _lstStatesZeroEnd;
                        item.StateOneStart -= _lstStatesOneStart;
                        item.StateOneEnd -= _lstStatesOneEnd;
                        item.OnRepaint -= _lstState_OnRePaint;
                        item.StateClicked -= triggerupdate;
                    }

                    _lstStates.Clear();
                    _lstTransFunc.Clear();

                    _dfa = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DFA_Wrapper() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
