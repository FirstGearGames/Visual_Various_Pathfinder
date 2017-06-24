namespace Various_MazeRunner
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
            this.LoadMazeButton = new System.Windows.Forms.Button();
            this.NodeVisualsPanel = new System.Windows.Forms.Panel();
            this.StartSearchButton = new System.Windows.Forms.Button();
            this.CancelSearchButton = new System.Windows.Forms.Button();
            this.SearchAlgorithmCombo = new System.Windows.Forms.ComboBox();
            this.AllowDiagonalCheckbox = new System.Windows.Forms.CheckBox();
            this.DontCrossCornersCheckbox = new System.Windows.Forms.CheckBox();
            this.VisualizationSpeedTrackbar = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.VisualizationSpeedTrackbar)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadMazeButton
            // 
            this.LoadMazeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.LoadMazeButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.LoadMazeButton.ForeColor = System.Drawing.Color.Black;
            this.LoadMazeButton.Location = new System.Drawing.Point(668, 12);
            this.LoadMazeButton.Name = "LoadMazeButton";
            this.LoadMazeButton.Size = new System.Drawing.Size(263, 58);
            this.LoadMazeButton.TabIndex = 4;
            this.LoadMazeButton.Text = "Load Maze Text";
            this.LoadMazeButton.UseVisualStyleBackColor = false;
            this.LoadMazeButton.Click += new System.EventHandler(this.but_LoadMazeText_Click);
            // 
            // NodeVisualsPanel
            // 
            this.NodeVisualsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.NodeVisualsPanel.Location = new System.Drawing.Point(12, 10);
            this.NodeVisualsPanel.Name = "NodeVisualsPanel";
            this.NodeVisualsPanel.Size = new System.Drawing.Size(650, 650);
            this.NodeVisualsPanel.TabIndex = 5;
            this.NodeVisualsPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NodeVisualsPanel_MouseUp);
            // 
            // StartSearchButton
            // 
            this.StartSearchButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.StartSearchButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.StartSearchButton.ForeColor = System.Drawing.Color.Black;
            this.StartSearchButton.Location = new System.Drawing.Point(668, 76);
            this.StartSearchButton.Name = "StartSearchButton";
            this.StartSearchButton.Size = new System.Drawing.Size(129, 58);
            this.StartSearchButton.TabIndex = 6;
            this.StartSearchButton.Text = "Start Search";
            this.StartSearchButton.UseVisualStyleBackColor = false;
            this.StartSearchButton.Click += new System.EventHandler(this.StartSearchButton_Click);
            // 
            // CancelSearchButton
            // 
            this.CancelSearchButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CancelSearchButton.Enabled = false;
            this.CancelSearchButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.CancelSearchButton.ForeColor = System.Drawing.Color.Black;
            this.CancelSearchButton.Location = new System.Drawing.Point(803, 76);
            this.CancelSearchButton.Name = "CancelSearchButton";
            this.CancelSearchButton.Size = new System.Drawing.Size(129, 58);
            this.CancelSearchButton.TabIndex = 7;
            this.CancelSearchButton.Text = "Cancel Search";
            this.CancelSearchButton.UseVisualStyleBackColor = false;
            this.CancelSearchButton.Click += new System.EventHandler(this.ResetSearchButton_Click);
            // 
            // SearchAlgorithmCombo
            // 
            this.SearchAlgorithmCombo.BackColor = System.Drawing.Color.White;
            this.SearchAlgorithmCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SearchAlgorithmCombo.ForeColor = System.Drawing.Color.Black;
            this.SearchAlgorithmCombo.FormattingEnabled = true;
            this.SearchAlgorithmCombo.Location = new System.Drawing.Point(669, 141);
            this.SearchAlgorithmCombo.Name = "SearchAlgorithmCombo";
            this.SearchAlgorithmCombo.Size = new System.Drawing.Size(262, 21);
            this.SearchAlgorithmCombo.TabIndex = 8;
            // 
            // AllowDiagonalCheckbox
            // 
            this.AllowDiagonalCheckbox.AutoSize = true;
            this.AllowDiagonalCheckbox.Checked = true;
            this.AllowDiagonalCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AllowDiagonalCheckbox.ForeColor = System.Drawing.Color.Black;
            this.AllowDiagonalCheckbox.Location = new System.Drawing.Point(671, 279);
            this.AllowDiagonalCheckbox.Name = "AllowDiagonalCheckbox";
            this.AllowDiagonalCheckbox.Size = new System.Drawing.Size(169, 19);
            this.AllowDiagonalCheckbox.TabIndex = 9;
            this.AllowDiagonalCheckbox.Text = "Allow Diagonal Movement";
            this.AllowDiagonalCheckbox.UseVisualStyleBackColor = true;
            // 
            // DontCrossCornersCheckbox
            // 
            this.DontCrossCornersCheckbox.AutoSize = true;
            this.DontCrossCornersCheckbox.Checked = true;
            this.DontCrossCornersCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DontCrossCornersCheckbox.ForeColor = System.Drawing.Color.Black;
            this.DontCrossCornersCheckbox.Location = new System.Drawing.Point(670, 304);
            this.DontCrossCornersCheckbox.Name = "DontCrossCornersCheckbox";
            this.DontCrossCornersCheckbox.Size = new System.Drawing.Size(144, 19);
            this.DontCrossCornersCheckbox.TabIndex = 10;
            this.DontCrossCornersCheckbox.Text = "Do Not Cross Corners";
            this.DontCrossCornersCheckbox.UseVisualStyleBackColor = true;
            // 
            // VisualizationSpeedTrackbar
            // 
            this.VisualizationSpeedTrackbar.Location = new System.Drawing.Point(668, 198);
            this.VisualizationSpeedTrackbar.Name = "VisualizationSpeedTrackbar";
            this.VisualizationSpeedTrackbar.Size = new System.Drawing.Size(262, 50);
            this.VisualizationSpeedTrackbar.TabIndex = 11;
            this.VisualizationSpeedTrackbar.Value = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(668, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "Visualization Speed (fast - slow)";
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(668, 237);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(262, 39);
            this.label2.TabIndex = 13;
            this.label2.Text = "Load Maze Text must be used for options below to take effect.";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(667, 450);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(271, 44);
            this.label3.TabIndex = 14;
            this.label3.Text = "Left click in a loaded maze to change the Start position. Right click to change t" +
    "he end position.";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(665, 597);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(271, 63);
            this.label4.TabIndex = 15;
            this.label4.Text = "Algorithms are not optimized for speed, but rather to demonstrate how they work.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(942, 672);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.VisualizationSpeedTrackbar);
            this.Controls.Add(this.DontCrossCornersCheckbox);
            this.Controls.Add(this.AllowDiagonalCheckbox);
            this.Controls.Add(this.SearchAlgorithmCombo);
            this.Controls.Add(this.CancelSearchButton);
            this.Controls.Add(this.StartSearchButton);
            this.Controls.Add(this.NodeVisualsPanel);
            this.Controls.Add(this.LoadMazeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Various MazeRunner";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.VisualizationSpeedTrackbar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button LoadMazeButton;
        public System.Windows.Forms.Panel NodeVisualsPanel;
        private System.Windows.Forms.Button StartSearchButton;
        private System.Windows.Forms.Button CancelSearchButton;
        private System.Windows.Forms.ComboBox SearchAlgorithmCombo;
        private System.Windows.Forms.CheckBox AllowDiagonalCheckbox;
        private System.Windows.Forms.CheckBox DontCrossCornersCheckbox;
        private System.Windows.Forms.TrackBar VisualizationSpeedTrackbar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

