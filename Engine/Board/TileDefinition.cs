using System;
using System.Collections.Generic;
using System.Linq;

namespace Noxico.Engine.Board
{
    public class TileDefinition
    {
        private static Dictionary<int, TileDefinition> defs;

        static TileDefinition()
        {
            defs = new Dictionary<int, TileDefinition>();
            var tml = Mix.GetTokenTree("tiles.tml", true);
            foreach (var tile in tml.Where(t => t.Name == "tile"))
            {
                var i = (int) tile.Value;
                var def = new TileDefinition
                {
                    Index = i,
                    Name = tile.GetToken("id").Text,
                    Glyph = (int) tile.GetToken("char").Value,
                    Background = Color.FromName(tile.GetToken("back").Text),
                    Foreground = tile.HasToken("fore")
                        ? Color.FromName(tile.GetToken("fore").Text)
                        : Color.FromName(tile.GetToken("back").Text).Darken(),
                    Wall = tile.HasToken("wall"),
                    Ceiling = tile.HasToken("ceiling"),
                    Cliff = tile.HasToken("cliff"),
                    Fence = tile.HasToken("fence"),
                    Grate = tile.HasToken("grate"),
                    CanBurn = tile.HasToken("canburn"),
                    FriendlyName = tile.HasToken("_n") ? tile.GetToken("_n").Text : null,
                    Description = tile.HasToken("description") ? tile.GetToken("description").Text : null,
                    Variants = tile.HasToken("variants") ? tile.GetToken("variants") : new Token("variants"),
                    VariableWall = tile.HasToken("varwall") ? (int) tile.GetToken("varwall").Value : 0,
                };
                def.MultiForeground =
                    tile.HasToken("mult") ? Color.FromName(tile.GetToken("mult").Text) : def.Foreground;
                defs.Add(i, def);
            }
        }

        public string Name { get; private set; }
        public int Index { get; private set; }
        public int Glyph { get; private set; }
        public Color Foreground { get; private set; }
        public Color Background { get; private set; }
        public Color MultiForeground { get; private set; }
        public bool Wall { get; private set; }
        public bool Ceiling { get; private set; }
        public bool Cliff { get; private set; }
        public bool Fence { get; private set; }
        public bool Grate { get; private set; }
        public bool CanBurn { get; private set; }
        public string FriendlyName { get; private set; }
        public string Description { get; private set; }
        public Token Variants { get; private set; }
        public int VariableWall { get; private set; }

        public bool IsVariableWall
        {
            get { return VariableWall > 0; }
        }

        public bool SolidToWalker
        {
            get { return Wall || Fence || Cliff; }
        }

        public bool SolidToFlyer
        {
            get { return Ceiling || Wall; }
        }

        public bool SolidToProjectile
        {
            get { return (Wall && !Grate); }
        }

        public static TileDefinition Find(string tileName, bool noVariants)
        {
            var def = defs
                .FirstOrDefault(t => t.Value.Name.Equals(tileName, StringComparison.InvariantCultureIgnoreCase)).Value;
            if (def == null)
                return defs[0];
            if (!noVariants && def.Variants.Tokens.Count > 0)
            {
                var iant = def.Variants.Tokens.PickOne();
                if (Random.NextDouble() > iant.Value)
                    def = TileDefinition.Find(iant.Name, false);
            }

            return def;
        }

        public static TileDefinition Find(string tileName)
        {
            return Find(tileName, false);
        }

        public static TileDefinition Find(int index, bool noVariants)
        {
            if (defs.ContainsKey(index))
            {
                var def = defs[index];
                if (!noVariants && def.Variants.Tokens.Count > 0)
                {
                    var iant = def.Variants.Tokens.PickOne();
                    if (Random.NextDouble() > iant.Value)
                        def = TileDefinition.Find(iant.Name);
                }

                return def;
            }
            else
                return null;
        }

        public static TileDefinition Find(int index)
        {
            return Find(index, false);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}