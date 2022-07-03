using System.IO;

namespace Noxico.Engine.Board
{
    /// <summary>
    /// An exit of some sort. Activate them by standing on them and pressing Enter.
    /// </summary>
    public class Warp
    {
        public static int GeneratorCount = 0;

        public Warp()
        {
            ID = GeneratorCount.ToString();
            GeneratorCount++;
        }

        public string ID { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int TargetBoard { get; set; }
        public string TargetWarpID { get; set; }

        public void SaveToFile(BinaryWriter stream)
        {
            stream.Write(ID);
            stream.Write((byte) XPosition);
            stream.Write((byte) YPosition);
            stream.Write(TargetBoard);
            stream.Write(TargetWarpID.OrEmpty());
        }

        public static Warp LoadFromFile(BinaryReader stream)
        {
            var newWarp = new Warp();
            newWarp.ID = stream.ReadString();
            newWarp.XPosition = stream.ReadByte();
            newWarp.YPosition = stream.ReadByte();
            newWarp.TargetBoard = stream.ReadInt32();
            newWarp.TargetWarpID = stream.ReadString();
            return newWarp;
        }
    }
}