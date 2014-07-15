using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Maps.Floors {
    public class DungeonArrayHall {

        public bool Open { get; set; }

        public List<Loc2D> TurnPoints { get; set; }

        public DungeonArrayHall() {
            TurnPoints = new List<Loc2D>();
        }

    }
}
