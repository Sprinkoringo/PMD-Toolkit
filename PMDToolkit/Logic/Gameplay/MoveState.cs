using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;

namespace PMDToolkit.Logic.Gameplay {

    public class MoveState {

        public int MoveNum { get; set; }
		public int CurrentPP { get; set; }
		public int MaxPP { get; set; }
		public bool Disabled { get; set; }


        public MoveState()
        {
            MoveNum = -1;
        }
    }
}
