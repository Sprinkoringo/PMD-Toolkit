using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class BattleMsg : IResult {

        //public ResultType Type { get { return ResultType.BattleMsg; } }
        public RenderTime Delay { get { return RenderTime.Zero; } }

        string msg;

        public BattleMsg(string msg) {
            this.msg = msg;
        }

        public void Execute() {
            Logs.Logger.LogBattle(msg);
        }

    }
}
