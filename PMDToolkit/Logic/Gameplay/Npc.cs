using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;

namespace PMDToolkit.Logic.Gameplay {
    public class Npc : ActiveChar {

        public Npc() {
            dead = true;
        }

        public Npc(Loc2D newLoc, Direction8 charDir, int npcIndex) {
            CharLoc = newLoc;
            CharDir = charDir;
            CharData.Species = npcIndex;
            if (npcIndex == 1) {
                Name = "Bandaid Mouse";
                Atk = 0;
                MaxHP = 8;
            } else if (npcIndex == 2) {
                Name = "Slime";
                Ability1 = "Slow Body";
                Atk = -2;
                MaxHP = 4;
            }
            Tactic = new AI(this);
            Initialize();
        }

        
    }
}
