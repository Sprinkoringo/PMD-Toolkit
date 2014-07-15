using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;

namespace PMDToolkit.Logic.Gameplay {

    public class Move {
	
		public int MoveNum { get; set; }
		public int PPBoost { get; set; }


        public Move()
        {
            MoveNum = -1;
        }
    }
}
