namespace UI
{
    partial class Form1
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
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playerVsPCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pCVsPCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remotePCVsPCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.boardPanel = new System.Windows.Forms.Panel();
            this.depthCombo = new System.Windows.Forms.ComboBox();
            this.playersTurn = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playerVsPCToolStripMenuItem,
            this.pCVsPCToolStripMenuItem,
            this.remotePCVsPCToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // playerVsPCToolStripMenuItem
            // 
            this.playerVsPCToolStripMenuItem.Name = "playerVsPCToolStripMenuItem";
            this.playerVsPCToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.playerVsPCToolStripMenuItem.Text = "player vs PC";
            this.playerVsPCToolStripMenuItem.Click += new System.EventHandler(this.playerVsPCToolStripMenuItem_Click);
            // 
            // pCVsPCToolStripMenuItem
            // 
            this.pCVsPCToolStripMenuItem.Name = "pCVsPCToolStripMenuItem";
            this.pCVsPCToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.pCVsPCToolStripMenuItem.Text = "PC vs PC";
            this.pCVsPCToolStripMenuItem.Click += new System.EventHandler(this.pCVsPCToolStripMenuItem_Click);
            // 
            // remotePCVsPCToolStripMenuItem
            // 
            this.remotePCVsPCToolStripMenuItem.Name = "remotePCVsPCToolStripMenuItem";
            this.remotePCVsPCToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.remotePCVsPCToolStripMenuItem.Text = "Remote PC vs PC...";
            this.remotePCVsPCToolStripMenuItem.Click += new System.EventHandler(this.remotePCVsPCToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(666, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.depthCombo);
            this.panel2.Controls.Add(this.playersTurn);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(666, 40);
            this.panel2.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 650);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(666, 80);
            this.panel4.TabIndex = 5;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(666, 40);
            this.panel5.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(626, 64);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(40, 586);
            this.panel1.TabIndex = 6;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 64);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(40, 586);
            this.panel3.TabIndex = 7;
            // 
            // boardPanel
            // 
            this.boardPanel.Location = new System.Drawing.Point(39, 64);
            this.boardPanel.Name = "boardPanel";
            this.boardPanel.Size = new System.Drawing.Size(586, 586);
            this.boardPanel.TabIndex = 8;
            // 
            // depthCombo
            // 
            this.depthCombo.FormattingEnabled = true;
            this.depthCombo.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.depthCombo.Location = new System.Drawing.Point(104, 10);
            this.depthCombo.Name = "depthCombo";
            this.depthCombo.Size = new System.Drawing.Size(85, 21);
            this.depthCombo.TabIndex = 10;
            // 
            // playersTurn
            // 
            this.playersTurn.AutoSize = true;
            this.playersTurn.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.playersTurn.Location = new System.Drawing.Point(496, 16);
            this.playersTurn.Name = "playersTurn";
            this.playersTurn.Size = new System.Drawing.Size(66, 15);
            this.playersTurn.TabIndex = 9;
            this.playersTurn.Text = "                   ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(408, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Players Turn :";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 730);
            this.Controls.Add(this.boardPanel);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Damka";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playerVsPCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pCVsPCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel boardPanel;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ToolStripMenuItem remotePCVsPCToolStripMenuItem;
        private System.Windows.Forms.ComboBox depthCombo;
        private System.Windows.Forms.Label playersTurn;
        private System.Windows.Forms.Label label1;

    }
}

