using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataGUI
{
    public static class Globals
    {
        public static MouseCondition MouseStatus { get; set; }
        
        public static GraphicsState PanelDisplayGraphics { get; set; }

        public enum MouseCondition
        {
            Default,
            Hovered,
            AddState,
            DeleteState,
            MoveState,
            ConnectZero,
            ConnectOne,
            Selected
        }
    }
}
