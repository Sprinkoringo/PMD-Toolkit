using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;

namespace PMDToolkit.Logic.Gameplay {
    public class PathTile {

        public Loc2D Location { get; set; }
        public bool Seen { get; set; }
        public bool Traversed { get; set; }
        public double Cost { get; set; }
        public double Heuristic { get; set; }
        public PathTile BackReference { get; set; }

        public PathTile(Loc2D location, bool seen, bool traversed, int cost, int heuristic, PathTile backReference) {
            Location = location;
            Seen = seen;
            Traversed = traversed;
            Cost = cost;
            Heuristic = heuristic;
            BackReference = backReference;
        }
    }
}
