using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Logic.Gameplay {
    public class Target {


        public Target(ActiveChar character, Enums.Alignment alignment, int distance) {
            Character = character;
            TargetAlignment = alignment;
            Distance = distance;
        }

        public ActiveChar Character { get; set; }

        public Enums.Alignment TargetAlignment { get; set; }

        public int Distance { get; set; }
    }
}
