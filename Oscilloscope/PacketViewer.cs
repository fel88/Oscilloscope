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
    public partial class PacketViewer : Form
    {
        public PacketViewer()
        {
            InitializeComponent();
        }

        internal void Init(I2CMessage[] packets)
        {
            foreach (var item in packets)
            {
                richTextBox1.AppendText($"address: {item.Address.ToString("X2")}{Environment.NewLine}");
                richTextBox1.AppendText($"bytes: {item.Bytes.Count}{Environment.NewLine}");
                richTextBox1.AppendText(string.Join(", ", item.Bytes.Select(z => z.Data.ToString("X2"))) + Environment.NewLine);
            }
        }
    }
}
