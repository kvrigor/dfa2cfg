using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomataGUI.Utils
{
    public static class Registry
    {
        public static MouseCondition MouseStatus { get; set; }

        public static Image FixedImage { get; set; }

        public static State_Wrapper LastClickedState { get; set; }
                
        public enum MouseCondition
        {
            Default,
            Hovered,
            AddState,
            DeleteState,
            StartState,
            MoveState,
            ConnectZero,
            ConnectOne,
            ZeroStart,
            ZeroEnd,
            OneStart,
            OneEnd,
            Selected,
            Accept
        }

        public static int Radius = 25;

        public static class StateColors
        {
            public static Brush Default { get { return Brushes.Aqua; } }
            public static Brush Hovered { get { return Brushes.AliceBlue; } }
            public static Brush Delete { get { return Brushes.Red; } }
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
        public static string MapToAlphabet(int input) { return ((char)(65 + input)).ToString(); }
        
        //public static void DrawLineAndCircle(PictureBox whereToDraw, Registry.CircleParam[] circles, Registry.LineParam[] lines, bool save)
        //{
        //    whereToDraw.Image = (Image)Registry.FixedImage.Clone();

        //    Image currentImage = (Image)whereToDraw.Image.Clone();
        //    foreach (Registry.CircleParam circle in circles)
        //    {
        //        Size circleSize = new Size(2 * circle.Radius, 2 * circle.Radius);
        //        Graphics myCircle = Graphics.FromImage(currentImage);
        //        myCircle.FillEllipse(circle.FillColor, new Rectangle(circle.ImageLocation, circleSize));
        //        myCircle.DrawEllipse(circle.OutlineColor, new Rectangle(circle.ImageLocation, circleSize));
        //        myCircle.Dispose();
        //    }
            
        //    foreach (Registry.LineParam line in lines)
        //    {
        //        Graphics myLine = Graphics.FromImage(currentImage);
        //        myLine.DrawLine(line.LineColor, line.Source, line.Destination);
        //        myLine.Dispose();
        //    }

        //    whereToDraw.Image = (Image)currentImage.Clone();
        //    if (save)
        //        Registry.FixedImage = (Image)whereToDraw.Image.Clone();
        //}
    }

    public static class Drawing
    {
        public struct CircleParam
        {
            public int Radius;
            public Point CenterLocation;
            public Brush FillColor;
            public Pen OutlineColor;

            public Point ImageLocation { get { return new Point(CenterLocation.X - Radius, CenterLocation.Y - Radius); } }
        }

        public struct LineParam
        {
            public Point Source;
            public Point Destination;
            public Pen LineColor;
        }

        public static void DrawCircle(PictureBox whereToDraw, CircleParam circles, bool save, string label = "")
        {
            whereToDraw.Image = (Image)Registry.FixedImage.Clone();

            Size circleSize = new Size(2 * circles.Radius, 2 * circles.Radius);
            Image currentImage = (Image)whereToDraw.Image.Clone();
            Graphics g = Graphics.FromImage(currentImage);
            Rectangle boundRect = new Rectangle(circles.ImageLocation, circleSize);
          
            g.FillEllipse(circles.FillColor, boundRect);
            g.DrawEllipse(circles.OutlineColor, new Rectangle(circles.ImageLocation, circleSize));
            if (!String.IsNullOrEmpty(label))
                g.DrawString(label, new Font("Arial", 22), new SolidBrush(Color.Black), boundRect, new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center});
            g.Dispose();

            whereToDraw.Image = (Image)currentImage.Clone();
            if (save)
                Registry.FixedImage = (Image)whereToDraw.Image.Clone();
        }

        public static void UnDrawCircle(PictureBox whereToDraw, CircleParam circles)
        {
            whereToDraw.Image = (Image)Registry.FixedImage.Clone();
            Size circleSize = new Size(0, 0);
            Image currentImage = (Image)whereToDraw.Image.Clone();
            Graphics g = Graphics.FromImage(currentImage);  
            g.FillEllipse(circles.FillColor, new Rectangle(circles.ImageLocation, circleSize));
            g.DrawEllipse(circles.OutlineColor, new Rectangle(circles.ImageLocation, circleSize));
            g.Dispose();
            whereToDraw.Image = (Image)currentImage.Clone();
        }

        public static void UnDrawLine(PictureBox whereToDraw, LineParam lineInfo)
        {
            whereToDraw.Image = (Image)Registry.FixedImage.Clone();

            Image currentImage = (Image)whereToDraw.Image.Clone();
            Graphics myLine = Graphics.FromImage(currentImage);
            myLine.DrawLine(lineInfo.LineColor, lineInfo.Source, lineInfo.Source);
            myLine.Dispose();
            whereToDraw.Image = (Image)currentImage.Clone();
        }

        public static void DrawCircles(PictureBox whereToDraw, CircleParam[] circles, bool save)
        {
            whereToDraw.Image = (Image)Registry.FixedImage.Clone();
            Image currentImage = (Image)whereToDraw.Image.Clone();

            foreach (CircleParam item in circles)
            {
                Size circleSize = new Size(2 * item.Radius, 2 * item.Radius);
                Graphics g = Graphics.FromImage(currentImage);
                g.FillEllipse(item.FillColor, new Rectangle(item.ImageLocation, circleSize));
                g.DrawEllipse(item.OutlineColor, new Rectangle(item.ImageLocation, circleSize));
                g.Dispose();
            }

            whereToDraw.Image = (Image)currentImage.Clone();
            if (save)
                Registry.FixedImage = (Image)whereToDraw.Image.Clone();
        }

        public static void DrawLine(PictureBox whereToDraw, LineParam lineInfo, bool save)
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
        public static void DrawArc(PictureBox whereToDraw, LineParam arcInfo, bool istop, bool save)
        {
            whereToDraw.Image = (Image)Registry.FixedImage.Clone();

            int tension = 25;
            if (istop)
                tension = -25;
            Image currentImage = (Image)whereToDraw.Image.Clone();
            Graphics myArc = Graphics.FromImage(currentImage);
            Point[] pts = new Point[4];
            pts[0] = arcInfo.Source;
            pts[1] = new Point(arcInfo.Source.X, arcInfo.Source.Y + tension);
            pts[2] = new Point(arcInfo.Destination.X, arcInfo.Destination.Y + tension);
            pts[3] = arcInfo.Destination;
            myArc.DrawCurve(arcInfo.LineColor, pts, 1.2F);
            myArc.Dispose();

            whereToDraw.Image = (Image)currentImage.Clone();
            if (save)
                Registry.FixedImage = (Image)whereToDraw.Image.Clone();
        }

        public static void DrawRectangle(PictureBox whereToDraw, Size sss)
        {
            whereToDraw.Image = (Image)Registry.FixedImage.Clone();

            Image currentImage = (Image)whereToDraw.Image.Clone();
            Graphics g = Graphics.FromImage(currentImage);
            g.FillRectangle(Brushes.White, new Rectangle(new Point(0, 0), sss));
            g.Dispose();

            whereToDraw.Image = (Image)currentImage.Clone();
            Registry.FixedImage = (Image)whereToDraw.Image.Clone();
        }
    }
}
