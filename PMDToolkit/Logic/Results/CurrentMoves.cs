using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class CurrentMoves : IResult {

        //public ResultType Type { get { return ResultType.CurrentMoves; } }
        public RenderTime Delay { get { return RenderTime.Zero; } }

        int[] moves;

        public CurrentMoves(Gameplay.MoveState[] moves) {
            this.moves = new int[moves.Length];
            for (int i = 0; i < moves.Length; i++) {
                this.moves[i] = moves[i].MoveNum;
            }
        }

        public void Execute() {
            Display.Screen.CurrentCharMoves = moves;
        }
    }
}
