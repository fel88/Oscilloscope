namespace Oscilloscope
{
    public class I2CMessage
    {
        public byte Address;
        public bool Ack;
        public bool ReadWrite;
        public List<i2cByte> Bytes = new List<i2cByte>();
    }
}
