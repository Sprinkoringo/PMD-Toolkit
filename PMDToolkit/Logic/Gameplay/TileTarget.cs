using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;

namespace PMDToolkit.Logic.Gameplay {
    public class TileTarget {

        public TileTarget(Loc2D tileLoc, int distance) {
            TileLoc = tileLoc;
            Distance = distance;
        }

        public Loc2D TileLoc { get; set; }

        public int Distance { get; set; }
    }
}
