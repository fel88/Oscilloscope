using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Oscilloscope
{
    public partial class OledI2CSimulator : Form
    {
        public OledI2CSimulator()
        {
            InitializeComponent();
            pictureBox1.Paint += PictureBox1_Paint;
        }

        private void PictureBox1_Paint(object? sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            gr.Clear(Color.White);
            for (int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    gr.FillRectangle(Pixels[i, j] == 1 ? Brushes.Black : Brushes.White, i * 6, j * 6, 6, 6);
                    gr.DrawRectangle(Pens.Black, i * 6, j * 6, 6, 6);
                }
            }

        }
        byte[,] Pixels = new byte[128, 32];
        internal void Init(I2CMessage[] packets)
        {
            int currentY = 0;
            int currentX = 0;

            foreach (var item in packets)
            {
                if (item.Bytes[0].Data == 0x40)
                {

                    for (int i = 1; i < item.Bytes.Count; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            var val = item.Bytes[i].Data & (1 << j);
                            Pixels[currentX, currentY + j] = (byte)(val>0?1:0);
                        }
                        currentX++;
                        if (currentX >= 128)
                        {
                            currentY += 8;
                            currentX %= 128;
                        }

                    }

                }
            }
        }
    }
}
