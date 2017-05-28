namespace AutomataGUI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddState = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteState = new System.Windows.Forms.ToolStripButton();
            this.btnStartState = new System.Windows.Forms.ToolStripButton();
            this.btnAcceptState = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnC0 = new System.Windows.Forms.ToolStripButton();
            this.btnC1 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.listView1 = new System.Windows.Forms.ListView();
            this.drawingBoard = new System.Windows.Forms.PictureBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.drawingBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddState,
            this.btnDeleteState,
            this.btnStartState,
            this.btnAcceptState,
            this.toolStripSeparator1,
            this.btnC0,
            this.btnC1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(724, 42);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAddState
            // 
            this.btnAddState.CheckOnClick = true;
            this.btnAddState.Image = global::AutomataGUI.Properties.Resources.imgAddState;
            this.btnAddState.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddState.Name = "btnAddState";
            this.btnAddState.Size = new System.Drawing.Size(62, 39);
            this.btnAddState.Text = "Add State";
            this.btnAddState.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAddState.CheckedChanged += new System.EventHandler(this.toolstripButtons_CheckedChanged);
            this.btnAddState.Click += new System.EventHandler(this.btnAddState_Click);
            // 
            // btnDeleteState
            // 
            this.btnDeleteState.CheckOnClick = true;
            this.btnDeleteState.Image = global::AutomataGUI.Properties.Resources.imgDeleteState;
            this.btnDeleteState.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteState.Name = "btnDeleteState";
            this.btnDeleteState.Size = new System.Drawing.Size(73, 39);
            this.btnDeleteState.Text = "Delete State";
            this.btnDeleteState.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDeleteState.CheckedChanged += new System.EventHandler(this.toolstripButtons_CheckedChanged);
            this.btnDeleteState.Click += new System.EventHandler(this.btnDeleteState_Click);
            // 
            // btnStartState
            // 
            this.btnStartState.CheckOnClick = true;
            this.btnStartState.Image = global::AutomataGUI.Properties.Resources.imgStartState;
            this.btnStartState.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStartState.Name = "btnStartState";
            this.btnStartState.Size = new System.Drawing.Size(64, 39);
            this.btnStartState.Text = "Start State";
            this.btnStartState.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnStartState.CheckedChanged += new System.EventHandler(this.toolstripButtons_CheckedChanged);
            this.btnStartState.Click += new System.EventHandler(this.btnStartState_Click);
            // 
            // btnAcceptState
            // 
            this.btnAcceptState.CheckOnClick = true;
            this.btnAcceptState.Image = global::AutomataGUI.Properties.Resources.imgAcceptState;
            this.btnAcceptState.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAcceptState.Name = "btnAcceptState";
            this.btnAcceptState.Size = new System.Drawing.Size(77, 39);
            this.btnAcceptState.Text = "Accept State";
            this.btnAcceptState.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAcceptState.CheckedChanged += new System.EventHandler(this.toolstripButtons_CheckedChanged);
            this.btnAcceptState.Click += new System.EventHandler(this.btnAcceptState_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 42);
            // 
            // btnC0
            // 
            this.btnC0.CheckOnClick = true;
            this.btnC0.Image = global::AutomataGUI.Properties.Resources.imgConnect0;
            this.btnC0.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnC0.Name = "btnC0";
            this.btnC0.Size = new System.Drawing.Size(65, 39);
            this.btnC0.Text = "Connect 0";
            this.btnC0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnC0.CheckedChanged += new System.EventHandler(this.toolstripButtons_CheckedChanged);
            this.btnC0.Click += new System.EventHandler(this.btnC0_Click);
            // 
            // btnC1
            // 
            this.btnC1.CheckOnClick = true;
            this.btnC1.Image = global::AutomataGUI.Properties.Resources.imgConnect1;
            this.btnC1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnC1.Name = "btnC1";
            this.btnC1.Size = new System.Drawing.Size(65, 39);
            this.btnC1.Text = "Connect 1";
            this.btnC1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnC1.CheckedChanged += new System.EventHandler(this.toolstripButtons_CheckedChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(0, 327);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(724, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Right;
            this.listView1.Location = new System.Drawing.Point(424, 42);
            this.listView1.Margin = new System.Windows.Forms.Padding(2);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(300, 285);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // drawingBoard
            // 
            this.drawingBoard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.drawingBoard.BackColor = System.Drawing.Color.White;
            this.drawingBoard.Location = new System.Drawing.Point(0, 41);
            this.drawingBoard.Margin = new System.Windows.Forms.Padding(2);
            this.drawingBoard.Name = "drawingBoard";
            this.drawingBoard.Size = new System.Drawing.Size(419, 280);
            this.drawingBoard.TabIndex = 0;
            this.drawingBoard.TabStop = false;
            this.drawingBoard.SizeChanged += new System.EventHandler(this.drawingBoard_SizeChanged);
            this.drawingBoard.MouseClick += new System.Windows.Forms.MouseEventHandler(this.drawingBoard_MouseClick);
            this.drawingBoard.MouseMove += new System.Windows.Forms.MouseEventHandler(this.drawingBoard_MouseMove);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 349);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.drawingBoard);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(740, 384);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.drawingBoard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox drawingBoard;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ToolStripButton btnAddState;
        private System.Windows.Forms.ToolStripButton btnDeleteState;
        private System.Windows.Forms.ToolStripButton btnStartState;
        private System.Windows.Forms.ToolStripButton btnAcceptState;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnC0;
        private System.Windows.Forms.ToolStripButton btnC1;
    }
}