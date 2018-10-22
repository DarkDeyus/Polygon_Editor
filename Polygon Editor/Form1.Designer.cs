namespace Polygon_Editor
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.createNewPolygonToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.insertPredefinedPolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.createNewPolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setADefaultPolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createNewPolygonToolStripMenuItem1,
            this.insertPredefinedPolygonToolStripMenuItem,
            this.clearToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // createNewPolygonToolStripMenuItem1
            // 
            this.createNewPolygonToolStripMenuItem1.Name = "createNewPolygonToolStripMenuItem1";
            this.createNewPolygonToolStripMenuItem1.Size = new System.Drawing.Size(125, 20);
            this.createNewPolygonToolStripMenuItem1.Text = "Create new polygon";
            this.createNewPolygonToolStripMenuItem1.Click += new System.EventHandler(this.createNewPolygonToolStripMenuItem_Click);
            // 
            // insertPredefinedPolygonToolStripMenuItem
            // 
            this.insertPredefinedPolygonToolStripMenuItem.Name = "insertPredefinedPolygonToolStripMenuItem";
            this.insertPredefinedPolygonToolStripMenuItem.Size = new System.Drawing.Size(155, 20);
            this.insertPredefinedPolygonToolStripMenuItem.Text = "Insert predefined polygon";
            this.insertPredefinedPolygonToolStripMenuItem.Click += new System.EventHandler(this.setADefaultPolygonToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem1
            // 
            this.clearToolStripMenuItem1.Name = "clearToolStripMenuItem1";
            this.clearToolStripMenuItem1.Size = new System.Drawing.Size(46, 20);
            this.clearToolStripMenuItem1.Text = "Clear";
            this.clearToolStripMenuItem1.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // createNewPolygonToolStripMenuItem
            // 
            this.createNewPolygonToolStripMenuItem.Name = "createNewPolygonToolStripMenuItem";
            this.createNewPolygonToolStripMenuItem.Size = new System.Drawing.Size(125, 20);
            this.createNewPolygonToolStripMenuItem.Text = "Create new polygon";
            this.createNewPolygonToolStripMenuItem.Click += new System.EventHandler(this.createNewPolygonToolStripMenuItem_Click);
            // 
            // setADefaultPolygonToolStripMenuItem
            // 
            this.setADefaultPolygonToolStripMenuItem.Name = "setADefaultPolygonToolStripMenuItem";
            this.setADefaultPolygonToolStripMenuItem.Size = new System.Drawing.Size(131, 20);
            this.setADefaultPolygonToolStripMenuItem.Text = "Set a default polygon";
            this.setADefaultPolygonToolStripMenuItem.Click += new System.EventHandler(this.setADefaultPolygonToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 424);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createNewPolygonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setADefaultPolygonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem createNewPolygonToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem insertPredefinedPolygonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem1;
    }
}

