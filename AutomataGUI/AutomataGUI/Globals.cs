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

        public struct CircleParam
        {
            public int Radius;
            public Point ImageLocation;
            public Brush FillColor;
            public Pen OutlineColor;
        }

        public struct LineParam
        {
            public Point Source;
            public Point Destination;
            public Pen LineColor;
        }
    }

    public static class Utils
    {
        public static void DrawCircle(PictureBox whereToDraw, Point imageLocation, int radius, Brush fillColor, Pen outlineColor, bool save)
        {
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
        public static void DrawLine(PictureBox whereToDraw, Registry.LineParam lineInfo, bool save)
        {
            whereToDraw.Image = (Image)Registry.FixedImage.Clone();

            Image currentImage = (Image)whereToDraw.Image.Clone();
            Graphics myLine = Graphics.FromImage(currentImage);
            myLine.DrawLine(lineInfo.LineColor, lineInfo.Source, lineInfo.Destination);
            myLine.Dispose();
            whereToDraw.Image = (Image)currentImage.Clone();
            if (save)
                Registry.FixedImage = (Image)whereToDraw.Image.Clone();
        }
        public static void DrawLineAndCircle(PictureBox whereToDraw, Registry.CircleParam[] circles, Registry.LineParam[] lines, bool save)
        {
            whereToDraw.Image = (Image)Registry.FixedImage.Clone();

            Image currentImage = (Image)whereToDraw.Image.Clone();
            foreach (Registry.CircleParam circle in circles)
            {
                Size circleSize = new Size(2 * circle.Radius, 2 * circle.Radius);
                Graphics myCircle = Graphics.FromImage(currentImage);
                myCircle.FillEllipse(circle.FillColor, new Rectangle(circle.ImageLocation, circleSize));
                myCircle.DrawEllipse(circle.OutlineColor, new Rectangle(circle.ImageLocation, circleSize));
                myCircle.Dispose();
            }
            
            foreach (Registry.LineParam line in lines)
            {
                Graphics myLine = Graphics.FromImage(currentImage);
                myLine.DrawLine(line.LineColor, line.Source, line.Destination);
                myLine.Dispose();
            }

            whereToDraw.Image = (Image)currentImage.Clone();
            if (save)
                Registry.FixedImage = (Image)whereToDraw.Image.Clone();
        }
    }
}
