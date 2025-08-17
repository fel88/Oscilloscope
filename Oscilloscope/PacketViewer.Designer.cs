namespace Oscilloscope
{
    partial class PacketViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PacketViewer));
            toolStrip1 = new ToolStrip();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            exportToolStripMenuItem = new ToolStripMenuItem();
            toXmlToolStripMenuItem = new ToolStripMenuItem();
            richTextBox1 = new RichTextBox();
            toolStripDropDownButton2 = new ToolStripDropDownButton();
            oledI2cToolStripMenuItem = new ToolStripMenuItem();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1, toolStripDropDownButton2 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(800, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { exportToolStripMenuItem });
            toolStripDropDownButton1.Image = (Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new Size(29, 22);
            toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // exportToolStripMenuItem
            // 
            exportToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toXmlToolStripMenuItem });
            exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            exportToolStripMenuItem.Size = new Size(108, 22);
            exportToolStripMenuItem.Text = "export";
            // 
            // toXmlToolStripMenuItem
            // 
            toXmlToolStripMenuItem.Name = "toXmlToolStripMenuItem";
            toXmlToolStripMenuItem.Size = new Size(108, 22);
            toXmlToolStripMenuItem.Text = "to xml";
            // 
            // richTextBox1
            // 
            richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.Location = new Point(0, 25);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(800, 425);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            // 
            // toolStripDropDownButton2
            // 
            toolStripDropDownButton2.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton2.DropDownItems.AddRange(new ToolStripItem[] { oledI2cToolStripMenuItem });
            toolStripDropDownButton2.Image = (Image)resources.GetObject("toolStripDropDownButton2.Image");
            toolStripDropDownButton2.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            toolStripDropDownButton2.Size = new Size(65, 22);
            toolStripDropDownButton2.Text = "simulate";
            // 
            // oledI2cToolStripMenuItem
            // 
            oledI2cToolStripMenuItem.Name = "oledI2cToolStripMenuItem";
            oledI2cToolStripMenuItem.Size = new Size(180, 22);
            oledI2cToolStripMenuItem.Text = "oled i2c";
            oledI2cToolStripMenuItem.Click += oledI2cToolStripMenuItem_Click;
            // 
            // PacketViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(richTextBox1);
            Controls.Add(toolStrip1);
            Name = "PacketViewer";
            Text = "PacketViewer";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem toXmlToolStripMenuItem;
        private RichTextBox richTextBox1;
        private ToolStripDropDownButton toolStripDropDownButton2;
        private ToolStripMenuItem oledI2cToolStripMenuItem;
    }
}