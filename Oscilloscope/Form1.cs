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
            pictureBox1.MouseDoubleClick += PictureBox1_MouseDoubleClick;

        }

        private void PictureBox1_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            var pos = pictureBox1.PointToClient(Cursor.Position);
            var maxX = Math.Min(diapX + minX, channels[0].Values.Count - 1);
            var realDiapX = maxX - minX;
            var currenXX = (pos.X / (double)pictureBox1.Width) * realDiapX + minX;

            minX = (int)currenXX;
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

        bool SkipSteps = true;
        private void PictureBox1_Paint(object? sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
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
            if (SkipSteps)
            {
                var realStep = realDiapX / (float)(pictureBox1.Width * 3);
                incr = (int)realStep;

                if (incr < 1)
                    incr = 1;
            }

            Pen[] pens =
            [
                new Pen(Color.Blue),
                new Pen(Color.Red),
                new Pen(Color.Green),
                new Pen(Color.Orange),
            ];

            for (int i1 = 0; i1 < channels.Count; i1++)
            {
                Channel? channel = channels[i1];
                if (!channel.Visible)
                    continue;

                for (int i = minX + 1; i < maxX; i += incr, xx++)
                {
                    var realX = (float)((i - 1 - minX) / (double)realDiapX) * pictureBox1.Width;
                    var realX2 = (float)((i - minX) / (double)realDiapX) * pictureBox1.Width;

                    var v1 = channel.Values[i - 1] + channel.OffsetY;

                    var v2 = channel.Values[i] + channel.OffsetY;
                    double accum = 0;
                    int cntr = 0;
                    for (int j = 0; j < incr; j++)
                    {
                        if ((i + j) > channel.Values.Count - 1)
                            continue;

                        var v3 = channel.Values[i + j] + channel.OffsetY;
                        accum += v3;
                        cntr++;
                    }
                    v2 = accum / cntr;

                    gr.DrawLine(pens[i1], realX, pictureBox1.Height - (float)(pictureBox1.Height * ((v1 - channel.minY) / channel.diap)), realX2 + 1,
                      pictureBox1.Height - (float)(((v2 - channel.minY) / channel.diap) * pictureBox1.Height));

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
        private static readonly char[] separator = new char[] { ',' };

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            foreach (var item in channels)
            {
                item.Markers.Clear();
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            var d = AutoDialog.DialogHelpers.StartDialog();

            for (int i = 0; i < channels.Count; i++)
            {
                Channel? item = channels[i];
                d.AddStringField("chName" + i, "Name #" + i, item.Name);
                d.AddBoolField("visible" + i, "Visible #" + i, item.Visible);
                d.AddNumericField("maxY" + i, "MaxY #" + i, item.maxY, 1000, -1000);
                d.AddNumericField("minY" + i, "MinY #" + i, item.minY, 1000, -1000);
                d.AddNumericField("offsetY" + i, "OffsetY #" + i, item.OffsetY, 1000, -1000);
            }

            if (!d.ShowDialog())
                return;

            for (int i = 0; i < channels.Count; i++)
            {
                Channel? item = channels[i];
                item.Name = d.GetStringField("chName" + i);
                item.Visible = d.GetBoolField("visible" + i);
                item.maxY = d.GetNumericField("maxY" + i);
                item.minY = d.GetNumericField("minY" + i);
                item.OffsetY = d.GetNumericField("offsetY" + i);
            }
        }

        bool IntegralFilterEnabled = false;
        double IntegralFilterKoef = 0.9;

        private void rigolFormatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            if (ofd.FileName.ToLower().EndsWith(".wfm"))
            {


            }
            else if (ofd.FileName.ToLower().EndsWith(".csv"))
            {
                var d = AutoDialog.DialogHelpers.StartDialog();

                d.AddBoolField("IntegralFilterEnabled", "IntegralFilterEnabled", IntegralFilterEnabled);
                d.AddNumericField("IntegralFilterKoef", "IntegralFilterKoef", IntegralFilterKoef);                

                if (!d.ShowDialog())
                    return;

                IntegralFilterEnabled = d.GetBoolField("IntegralFilterEnabled");
                IntegralFilterKoef = d.GetNumericField("IntegralFilterKoef");

                channels.Clear();                
                using var file = File.OpenRead(ofd.FileName);
                using var reader = new StreamReader(file);
                var line1 = reader.ReadLine();
                var spl0 = line1.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                var chNames = spl0.Where(z => z.StartsWith("CH")).ToArray();
                var line2 = reader.ReadLine();
                for (int i = 0; i < chNames.Length; i++)
                {
                    channels.Add(new Channel() { Name = chNames[i], OffsetY = 10 * i });
                }

                double[] integralVals = new double[channels.Count];
                bool first = true;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var spl = line.Split([','], StringSplitOptions.RemoveEmptyEntries);
                    if (first && IntegralFilterEnabled)
                    {
                        first = false;

                        for (int i = 0; i < chNames.Length; i++)
                        {
                            var val = float.Parse(spl[1 + i]);
                            integralVals[i] = val;

                        }
                    }


                    for (int i = 0; i < chNames.Length; i++)
                    {

                        var val = float.Parse(spl[1 + i]);
                        if (IntegralFilterEnabled)
                        {
                            integralVals[i] = IntegralFilterKoef * integralVals[i] + (1.0 - IntegralFilterKoef) * val;
                            channels[i].Values.Add(integralVals[i]);
                        }
                        else
                        {

                            channels[i].Values.Add(val);
                        }
                    }
                }
            }
        }

        
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            var d = AutoDialog.DialogHelpers.StartDialog();

            d.AddBoolField("IntegralFilterEnabled", "IntegralFilterEnabled", IntegralFilterEnabled);
            d.AddNumericField("IntegralFilterKoef", "IntegralFilterKoef", IntegralFilterKoef);

            if (!d.ShowDialog())
                return;

            IntegralFilterEnabled = d.GetBoolField("IntegralFilterEnabled");
            IntegralFilterKoef = d.GetNumericField("IntegralFilterKoef");
        }
    }
}
