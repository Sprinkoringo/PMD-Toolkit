using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Logic.Gameplay;
using PMDToolkit.Logic.Display;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class SetStats : IResult {

        //public ResultType Type { get { return ResultType.SetStats; } }
        public RenderTime Delay { get { return RenderTime.Zero; } }

        int hp;
        int hpMax;
        int pp;
        int ppMax;
        int gold;

        public SetStats(int hp, int hpMax, int pp, int ppMax, int gold) {
            this.hp = hp;
            this.hpMax = hpMax;
            this.pp = pp;
            this.ppMax = ppMax;
            this.gold = gold;
        }

        public void Execute() {
            Screen.HP = hp;
            Screen.PPMax = hpMax;
            Screen.PP = pp;
            Screen.PPMax = ppMax;
            Screen.Gold = gold;
        }

    }
}
