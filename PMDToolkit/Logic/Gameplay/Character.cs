using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;

namespace PMDToolkit.Logic.Gameplay {

    public class Character {

        public string BaseName { get; set; }

        public FormData BaseCharData;

        public int Level { get; set; }

        public Move[] BaseMoves { get; set; }

        //stat boosts from vitamins
        public int MaxHPBonus { get; set; }
        public int AtkBonus { get; set; }
        public int DefBonus { get; set; }
        public int SpAtkBonus { get; set; }
        public int SpDefBonus { get; set; }
        public int SpeedBonus { get; set; }

        
        public Character() {
            BaseMoves = new Move[Processor.MAX_MOVE_SLOTS];
            for (int i = 0; i < Processor.MAX_MOVE_SLOTS; i++)
            {
                BaseMoves[i] = new Move();
            }
        }
        
        public Character(Loc2D newLoc, Direction8 charDir) {
            //clean variables
            BaseMoves = new Move[Processor.MAX_MOVE_SLOTS];
            for (int i = 0; i < Processor.MAX_MOVE_SLOTS; i++)
            {
                BaseMoves[i] = new Move();
            }
        }
    }
}
