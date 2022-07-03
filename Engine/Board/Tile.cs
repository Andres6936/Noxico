using System;
using System.Collections.Specialized;
using System.IO;

namespace Noxico.Engine.Board
{
    /// <summary>
    /// A single tile on a board.
    /// </summary>
    public class Tile
    {
        public int Index { get; set; }

        public Fluids Fluid { get; set; }
        public bool Shallow { get; set; }
        public int BurnTimer { get; set; }
        public bool Seen { get; set; }
        public Color SlimeColor { get; set; }
        public int InherentLight { get; set; }

        public bool SolidToWalker
        {
            get { return Definition.SolidToWalker; }
        }

        public bool SolidToDryWalker
        {
            get { return Definition.SolidToWalker || (Fluid != Fluids.Dry && !Shallow); }
        }

        public bool SolidToFlyer
        {
            get { return Definition.SolidToFlyer; }
        }

        public bool SolidToProjectile
        {
            get { return Definition.SolidToProjectile; }
        }

        public TileDefinition Definition
        {
            get { return TileDefinition.Find(Index, true); }
            set { Index = value.Index; }
        }

        public void SaveToFile(BinaryWriter stream)
        {
            stream.Write((UInt16) Index);

            var bits = new BitVector32();
            bits[32] = Shallow;
            bits[64] = (InherentLight > 0);
            bits[128] = Seen;
            stream.Write((byte) ((byte) bits.Data | (byte) Fluid));
            stream.Write((byte) BurnTimer);
            if (SlimeColor == null)
                SlimeColor = Color.Transparent;
            SlimeColor.SaveToFile(stream);
            if (InherentLight > 0)
                stream.Write((byte) InherentLight);
        }

        public void LoadFromFile(BinaryReader stream)
        {
            Index = stream.ReadUInt16();

            var set = stream.ReadByte();
            var bits = new BitVector32(set);
            Shallow = bits[32];
            Seen = bits[128];
            BurnTimer = stream.ReadByte();
            Fluid = (Fluids) (set & 7);
            SlimeColor = Toolkit.LoadColorFromFile(stream);
            if (bits[64])
                InherentLight = stream.ReadByte();
        }

        public Tile Clone()
        {
            return new Tile
            {
                Index = this.Index,
                Fluid = this.Fluid,
                Shallow = this.Shallow,
                BurnTimer = this.BurnTimer,
                Seen = this.Seen,
                SlimeColor = this.SlimeColor,
                InherentLight = this.InherentLight
            };
        }

        public override string ToString()
        {
            if (Fluid != Fluids.Dry)
                return string.Format("{0} - {1}", Definition, Fluid);
            return Definition.ToString();
        }
    }
}