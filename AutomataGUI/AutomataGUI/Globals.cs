using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomataGUI
{
    public static class Registry
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

    public static class Utils
    {
        public static void DrawCircle(PictureBox whereToDraw, Point imageLocation, int radius, Brush fillColor, Pen outlineColor)
        {
            Size circleSize = new Size(2 * radius, 2 * radius);
            Graphics myCircle;
            myCircle = whereToDraw.CreateGraphics();
            myCircle.FillEllipse(fillColor, new Rectangle(imageLocation, circleSize));
            myCircle.DrawEllipse(outlineColor, new Rectangle(imageLocation, circleSize));
        }
        public static void DrawLine(PictureBox whereToDraw, Point from, Point to, Pen lineColor)
        {
            Graphics myLine;
            myLine = whereToDraw.CreateGraphics();
            myLine.DrawLine(lineColor, from, to);
        }
    }
}
