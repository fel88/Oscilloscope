using System.Net.Http.Headers;

namespace Oscilloscope
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Paint += PictureBox1_Paint;
            MouseWheel += Form1_MouseWheel;
            pictureBox1.MouseClick += PictureBox1_MouseClick;
        }

        public List<int> globalMarkers = new List<int>();
        private void PictureBox1_MouseClick(object? sender, MouseEventArgs e)
        {
            var pos = pictureBox1.PointToClient(Cursor.Position);
            var xx = (pos.X / (double)pictureBox1.Width) * diapX + minX;
            if (enableMarkerAdd)
            {
                enableMarkerAdd = false;
                globalMarkers.Add((int)xx);
            }

        }

        private void Form1_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                diapX /= 2;
            }
            else
            {
                diapX *= 2;
            }

        }

        int diapX = 111500;
        int minX = 0;

        private void PictureBox1_Paint(object? sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            gr.Clear(Color.White);

            if (channels.Count == 0)
                return;

            /* double maxY = 15;
             double minY = -5;
             double maxY2 = 25;
             double minY2 = -25;*/
            ///  var diap = maxY - minY;
            //  var diap2 = maxY2 - minY2;
            var maxX = Math.Min(diapX + minX, channels[0].Values.Count - 1);
            var realDiapX = maxX - minX;
            var xx = 0;
            int incr = 1;

            var realStep = realDiapX / (float)(pictureBox1.Width*3);
            incr = (int)realStep;
            
            if (incr < 1) 
                incr = 1;            

            Pen[] pens =
            [
                new Pen(Color.Blue),
                new Pen(Color.Red)
            ];

            for (int i1 = 0; i1 < channels.Count; i1++)
            {
                Channel? channel = channels[i1];
                for (int i = minX + 1; i < maxX; i += incr, xx++)
                {
                    var realX = (float)((i - 1 - minX) / (double)realDiapX) * pictureBox1.Width;
                    var realX2 = (float)((i - minX) / (double)realDiapX) * pictureBox1.Width;

                    gr.DrawLine(pens[i1], realX, pictureBox1.Height - (float)(pictureBox1.Height * ((channel.Values[i - 1] - channel.minY) / channel.diap)), realX2 + 1,
                      pictureBox1.Height - (float)(((channel.Values[i] - channel.minY) / channel.diap) * pictureBox1.Height));

                    //   gr.DrawLine(new Pen(Color.Red), realX, pictureBox1.Height - (float)(pictureBox1.Height * ((values2[i - 1] - minY2) / diap2)), realX2 + 1,
                    //   pictureBox1.Height - (float)(((values2[i] - minY2) / diap2) * pictureBox1.Height));
                }
                foreach (var marker in channel.Markers)
                {
                    var realX = (float)((marker.Position - minX) / (double)realDiapX) * pictureBox1.Width;
                    gr.DrawLine(Pens.Green, realX, 0, realX, pictureBox1.Height);

                }
            }


            var pos = pictureBox1.PointToClient(Cursor.Position);
            gr.DrawLine(Pens.Green, pos.X, 0, pos.X, pictureBox1.Height);

            var currenXX = (pos.X / (double)pictureBox1.Width) * diapX + minX;
            gr.DrawString($"Total : {channels[0].Values.Count}  current: {minX}", SystemFonts.DefaultFont, Brushes.Black, 10, 10);
            gr.DrawString(diapX.ToString(), SystemFonts.DefaultFont, Brushes.Black, 10, 40);
            gr.DrawString(currenXX.ToString(), SystemFonts.DefaultFont, Brushes.Black, 10, 70);

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            using var file = File.OpenRead(ofd.FileName);
            using var reader = new StreamReader(file);
            reader.ReadLine();
            reader.ReadLine();
            channels.Add(new Channel());
            channels.Add(new Channel());
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var spl = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var val = float.Parse(spl[1]);
                var val2 = float.Parse(spl[2]);
                channels[0].Values.Add(val);
                channels[1].Values.Add(val2);
            }

            channels[1].maxY = 25;
            channels[1].minY = -25;

            /* var min = values.Min();
             var max = values.Max();
             minX = values.IndexOf(min) - diapX / 2;*/
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            minX += diapX / 2;
            minX = Math.Min(minX, channels[0].Values.Count - diapX);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            minX -= diapX / 2;
            minX = Math.Max(minX, 0);
        }

        bool enableMarkerAdd = false;
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            enableMarkerAdd = true;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            foreach (var c in channels)
            {
                c.ExtractEdgesMarkers();
            }
        }

        List<Channel> channels = new List<Channel>();
        public class ChannelMarker
        {
            public int Position;
            public Channel Parent;
        }
        public class Channel
        {
            public string Name { get; set; }
            public List<ChannelMarker> Markers = new List<ChannelMarker>();
            public void AddMarker(int pos)
            {
                Markers.Add(new ChannelMarker() { Position = pos, Parent = this });
            }
            public List<float> Values = new List<float>();
            public void ExtractEdgesMarkers()
            {
                bool edgeWait = true;
                for (int i = 1; i < Values.Count; i++)
                {
                    if (Values[i - 1] < 1.8 && Values[i] > 1.8 && edgeWait)
                    {
                        edgeWait = false;
                        AddMarker(i);
                    }
                    if (Values[i] < 0.5)
                        edgeWait = true;
                }
            }
            public double maxY = 15;
            public double minY = -5;

            public double diap => maxY - minY;
        }

        public byte ToByte(IReadOnlyList<byte> bits)
        {
            byte accum = 0;
            bits = bits.Reverse().ToArray();
            for (int j = 0; j < bits.Count(); j++)
            {
                accum |= (byte)(bits[j] << j);
            }
            return accum;
        }

        private void i2CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<byte[]> bits = new List<byte[]>();

            var clock = channels[0];
            var data = channels[1];

            clock.Markers.Clear();
            data.Markers.Clear();

            clock.ExtractEdgesMarkers();
            data.ExtractEdgesMarkers();

            var common = clock.Markers.Concat(data.Markers).OrderBy(z => z.Position).ToArray();
            var terminates = data.Markers.Where(z => clock.Values[z.Position] > 1.8).ToArray();
            var markersRemains = clock.Markers.ToList();
            for (int i = 0; i < terminates.Length; i++)
            {
                List<byte> accum = new List<byte>();

                var t = terminates[i];
                var get1 = markersRemains.Where(z => z.Position < t.Position).ToArray();
                foreach (var item in get1)
                {
                    markersRemains.Remove(item);
                }
                foreach (var item in get1)
                {
                    if (data.Values[item.Position] > 1.8)
                        accum.Add(1);
                    else
                        accum.Add(0);

                }
                bits.Add(accum.ToArray());
            }


            var packets = I2C_Decode(bits);



            PacketViewer f = new PacketViewer();
            f.Init(packets);
            //f.Controls.Add(new RichTextBox() { Dock = DockStyle.Fill, Text = string.Join(", ", bytes.Select(z => z.ToString("X2"))) });
            f.Show();

        }


        private I2CMessage[] I2C_Decode(List<byte[]> bits)
        {
            List<I2CMessage> ret = new List<I2CMessage>();
            foreach (var send in bits)
            {
                I2CMessage m = new I2CMessage();
                m.Address = ToByte(send.Take(7).ToArray());
                m.ReadWrite = ToByte(send.Skip(7).Take(1).ToArray()) > 0;
                m.Ack = ToByte(send.Skip(8).Take(1).ToArray()) > 0;
                var qty = (send.Length - 9) / 9;
                for (int i = 0; i < qty; i++)
                {
                    i2cByte b = new i2cByte();
                    b.Data = ToByte(send.Skip(9 + i * 9).Take(8).ToArray());
                    b.Ack = ToByte(send.Skip(9 + i * 9 + 1).Take(1).ToArray()) > 0;
                    m.Bytes.Add(b);
                }
                ret.Add(m);
            }

            return ret.ToArray();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            foreach (var item in channels)
            {
                item.Markers.Clear();
            }
        }
    }
}
