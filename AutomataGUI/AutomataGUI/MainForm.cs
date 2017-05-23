﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutomataGUI.Utils;

namespace AutomataGUI
{
    public partial class MainForm : Form
    {
        private DFA_Wrapper src;
        private Cursor _tempCursor;

        public MainForm()
        {
            InitializeComponent();
            drawingBoard.Image = new Bitmap(drawingBoard.Width, drawingBoard.Height);
            Utils.Registry.FixedImage = (Image)drawingBoard.Image.Clone();

            src = new DFA_Wrapper(drawingBoard);
            
        }

        private void btnAddState_Click(object sender, EventArgs e)
        {
            Registry.MouseStatus = Registry.MouseCondition.AddState;
        }

        private void btnDeleteState_Click(object sender, EventArgs e)
        {
            Registry.MouseStatus = Registry.MouseCondition.DeleteState;
        }

        private void btnStartState_Click(object sender, EventArgs e)
        {
            Registry.MouseStatus = Registry.MouseCondition.StartState;
        }

        private void btnAcceptState_Click(object sender, EventArgs e)
        {
            Registry.MouseStatus = Registry.MouseCondition.Accept;
        }

        private void drawingBoard_MouseMove(object sender, MouseEventArgs e)
        {
            Utils.Drawing.CircleParam temp = new Drawing.CircleParam();
            switch (Registry.MouseStatus)
            {
                case Registry.MouseCondition.AddState:
                    temp.Radius = 25;
                    temp.CenterLocation = e.Location;
                    temp.FillColor = Utils.Registry.StateColors.Hovered;
                    temp.OutlineColor = Pens.Black;
                    Utils.Drawing.DrawCircle(drawingBoard, temp, false);
                    break;
                case Registry.MouseCondition.DeleteState:
                    _tempCursor = Cursor.Current;
                    Cursor.Current = Cursors.Cross;
                    break;
                default:
                    break;
            }
        }

        private void drawingBoard_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (Registry.MouseStatus)
                {
                    case Registry.MouseCondition.AddState:
                        src.AddState(e.Location);
                        Registry.MouseStatus = Registry.MouseCondition.Default;
                        break;
                    case Registry.MouseCondition.DeleteState:
                        Cursor.Current = _tempCursor;
                        break;
                    default:
                        break;
                }
            }
        }

        private void drawingBoard_SizeChanged(object sender, EventArgs e)
        {
            if (drawingBoard.Image != null)
            {
                Bitmap test = new Bitmap(drawingBoard.Width, drawingBoard.Height);
                Graphics g = Graphics.FromImage(test);
                g.DrawImage(drawingBoard.Image, 0, 0);
                drawingBoard.Image = test;
                Utils.Registry.FixedImage = (Image)drawingBoard.Image.Clone();
            }
        }

    }
}
