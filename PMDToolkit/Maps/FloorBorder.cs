using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Maps {
    public class FloorBorder {

        public Loc2D? Location { get; set; }

        public Direction3D LinkDir { get; set; }

        public FloorBorder(Loc2D? location, Direction3D linkDir) {
            Location = location;
            LinkDir = linkDir;
        }

    }
}
