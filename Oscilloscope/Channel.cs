using static Oscilloscope.Form1;

namespace Oscilloscope
{
    public class Channel
    {
        public string Name { get; set; }
        public bool Visible { get; set; } = true;
        public double OffsetY { get; set; } 
        public List<ChannelMarker> Markers = new List<ChannelMarker>();
        public void AddMarker(int pos)
        {
            Markers.Add(new ChannelMarker() { Position = pos, Parent = this });
        }
        public List<double> Values = new List<double>();
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
}
