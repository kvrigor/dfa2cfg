namespace AutomataGUI
{
    partial class frmMainGUI
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblMouseStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStateCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMousePos = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatesViewer = new System.Windows.Forms.DataGridView();
            this.ts_btnAddState = new System.Windows.Forms.ToolStripButton();
            this.ts_btnDeleteState = new System.Windows.Forms.ToolStripButton();
            this.ts_btnConnect1 = new System.Windows.Forms.ToolStripButton();
            this.ts_btnConnect0 = new System.Windows.Forms.ToolStripButton();
            this.ts_btnAcceptState = new System.Windows.Forms.ToolStripButton();
            this.ts_btnProcess = new System.Windows.Forms.ToolStripButton();
            this.DiagramArea = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StatesViewer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiagramArea)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(711, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ts_btnAddState,
            this.ts_btnDeleteState,
            this.ts_btnConnect1,
            this.ts_btnConnect0,
            this.ts_btnAcceptState,
            this.ts_btnProcess});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(711, 27);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblMouseStatus,
            this.lblStateCount,
            this.lblMousePos});
            this.statusStrip1.Location = new System.Drawing.Point(0, 396);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(711, 25);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblMouseStatus
            // 
            this.lblMouseStatus.AutoSize = false;
            this.lblMouseStatus.Name = "lblMouseStatus";
            this.lblMouseStatus.Size = new System.Drawing.Size(100, 20);
            this.lblMouseStatus.Text = "Default";
            // 
            // lblStateCount
            // 
            this.lblStateCount.Name = "lblStateCount";
            this.lblStateCount.Size = new System.Drawing.Size(13, 20);
            this.lblStateCount.Text = "0";
            // 
            // lblMousePos
            // 
            this.lblMousePos.AutoSize = false;
            this.lblMousePos.Name = "lblMousePos";
            this.lblMousePos.Size = new System.Drawing.Size(70, 20);
            this.lblMousePos.Text = "0,0";
            // 
            // StatesViewer
            // 
            this.StatesViewer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StatesViewer.Location = new System.Drawing.Point(398, 51);
            this.StatesViewer.Name = "StatesViewer";
            this.StatesViewer.Size = new System.Drawing.Size(313, 345);
            this.StatesViewer.TabIndex = 4;
            // 
            // ts_btnAddState
            // 
            this.ts_btnAddState.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ts_btnAddState.Image = global::AutomataGUI.Properties.Resources.imgAddState;
            this.ts_btnAddState.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ts_btnAddState.Name = "ts_btnAddState";
            this.ts_btnAddState.Size = new System.Drawing.Size(24, 24);
            this.ts_btnAddState.Text = "Add State";
            this.ts_btnAddState.Click += new System.EventHandler(this.ts_btnAddState_Click);
            // 
            // ts_btnDeleteState
            // 
            this.ts_btnDeleteState.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ts_btnDeleteState.Image = global::AutomataGUI.Properties.Resources.imgDeleteState;
            this.ts_btnDeleteState.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ts_btnDeleteState.Name = "ts_btnDeleteState";
            this.ts_btnDeleteState.Size = new System.Drawing.Size(24, 24);
            this.ts_btnDeleteState.Text = "Delete State";
            this.ts_btnDeleteState.Click += new System.EventHandler(this.ts_btnDeleteState_Click);
            // 
            // ts_btnConnect1
            // 
            this.ts_btnConnect1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ts_btnConnect1.Image = global::AutomataGUI.Properties.Resources.imgConnect1;
            this.ts_btnConnect1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ts_btnConnect1.Name = "ts_btnConnect1";
            this.ts_btnConnect1.Size = new System.Drawing.Size(24, 24);
            this.ts_btnConnect1.Text = "Connect 1";
            this.ts_btnConnect1.Click += new System.EventHandler(this.ts_btnConnect1_Click);
            // 
            // ts_btnConnect0
            // 
            this.ts_btnConnect0.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ts_btnConnect0.Image = global::AutomataGUI.Properties.Resources.imgConnect0;
            this.ts_btnConnect0.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ts_btnConnect0.Name = "ts_btnConnect0";
            this.ts_btnConnect0.Size = new System.Drawing.Size(24, 24);
            this.ts_btnConnect0.Text = "Connect 0";
            this.ts_btnConnect0.Click += new System.EventHandler(this.ts_btnConnect0_Click);
            // 
            // ts_btnAcceptState
            // 
            this.ts_btnAcceptState.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ts_btnAcceptState.Image = global::AutomataGUI.Properties.Resources.imgAcceptState;
            this.ts_btnAcceptState.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ts_btnAcceptState.Name = "ts_btnAcceptState";
            this.ts_btnAcceptState.Size = new System.Drawing.Size(24, 24);
            this.ts_btnAcceptState.Text = "toolStripButton1";
            this.ts_btnAcceptState.Click += new System.EventHandler(this.tsbtnTest_Click);
            // 
            // ts_btnProcess
            // 
            this.ts_btnProcess.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ts_btnProcess.Image = global::AutomataGUI.Properties.Resources.imgProcess;
            this.ts_btnProcess.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ts_btnProcess.Name = "ts_btnProcess";
            this.ts_btnProcess.Size = new System.Drawing.Size(24, 24);
            this.ts_btnProcess.Text = "toolStripButton1";
            this.ts_btnProcess.Click += new System.EventHandler(this.ts_btnProcess_Click);
            // 
            // DiagramArea
            // 
            this.DiagramArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DiagramArea.BackColor = System.Drawing.Color.White;
            this.DiagramArea.Location = new System.Drawing.Point(0, 51);
            this.DiagramArea.Margin = new System.Windows.Forms.Padding(2);
            this.DiagramArea.Name = "DiagramArea";
            this.DiagramArea.Size = new System.Drawing.Size(537, 345);
            this.DiagramArea.TabIndex = 0;
            this.DiagramArea.TabStop = false;
            this.DiagramArea.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DiagramArea_MouseClick);
            this.DiagramArea.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DiagramArea_MouseMove);
            // 
            // frmMainGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 421);
            this.Controls.Add(this.StatesViewer);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.DiagramArea);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximumSize = new System.Drawing.Size(727, 459);
            this.MinimumSize = new System.Drawing.Size(583, 459);
            this.Name = "frmMainGUI";
            this.Text = "AutomataGUI";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StatesViewer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiagramArea)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ts_btnAddState;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblMouseStatus;
        private System.Windows.Forms.ToolStripButton ts_btnDeleteState;
        private System.Windows.Forms.ToolStripButton ts_btnConnect1;
        private System.Windows.Forms.ToolStripButton ts_btnConnect0;
        private System.Windows.Forms.ToolStripStatusLabel lblStateCount;
        private System.Windows.Forms.PictureBox DiagramArea;
        private System.Windows.Forms.ToolStripButton ts_btnAcceptState;
        private System.Windows.Forms.ToolStripStatusLabel lblMousePos;
        private System.Windows.Forms.ToolStripButton ts_btnProcess;
        private System.Windows.Forms.DataGridView StatesViewer;
    }
}

