using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;

namespace PMDToolkit.Logic.Gameplay {
    public class Player : ActiveChar {
        
        public Player() {
            dead = true;
        }

        public Player(Loc2D newLoc, Direction8 charDir) {
            CharLoc = newLoc;
            CharDir = charDir;
            dead = false;
            Name = "Hero";
            Initialize();
        }
        
    }
}
