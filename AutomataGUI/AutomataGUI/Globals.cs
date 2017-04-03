using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataGUI
{
    public static class Globals
    {
        public static Point TargetZero;


        public static MouseCondition MouseStatus;

        public enum MouseCondition
        {
            Default,
            Hovered,
            AddState,
            DeleteState,
            MoveState,
            ConnectZero,
            ConnectOne
        }
    }
}
