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
        
        public static Image FixedImage { get; set; }

        public static Bitmap myImage { get; set; }

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
        public static void DrawCircle(PictureBox whereToDraw, Point imageLocation, int radius, Brush fillColor, Pen outlineColor, bool save)
        {


            //Size circleSize = new Size(2 * radius, 2 * radius);
            //Graphics myCircle;
            //myCircle = whereToDraw.CreateGraphics();
            //myCircle.FillEllipse(fillColor, new Rectangle(imageLocation, circleSize));
            //myCircle.DrawEllipse(outlineColor, new Rectangle(imageLocation, circleSize));
            //myCircle.Dispose();



            whereToDraw.Image = (Image)Registry.FixedImage.Clone();

            Size circleSize = new Size(2 * radius, 2 * radius);
            Image currentImage = (Image)whereToDraw.Image.Clone();
            Graphics g = Graphics.FromImage(currentImage);
            g.FillEllipse(fillColor, new Rectangle(imageLocation, circleSize));
            g.DrawEllipse(outlineColor, new Rectangle(imageLocation, circleSize));
            g.Dispose();
            whereToDraw.Image = (Image)currentImage.Clone();
            if (save)
                Registry.FixedImage = (Image)whereToDraw.Image.Clone();
        }
        public static void DrawLine(PictureBox whereToDraw, Point from, Point to, Pen lineColor)
        {
            Graphics myLine;
            myLine = whereToDraw.CreateGraphics();
            myLine.DrawLine(lineColor, from, to);
        }
    }
}
