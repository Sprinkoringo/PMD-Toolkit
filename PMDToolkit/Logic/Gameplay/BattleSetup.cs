/*The MIT License (MIT)

Copyright (c) 2014 Sprinkoringo

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Data;
using PMDToolkit.Maps;

namespace PMDToolkit.Logic.Gameplay {

    public class IntReference
    {
        public int Value { get; set; }
    }

    public class BattleSetup {

        public ActiveChar Attacker { get; set; }
        public ActiveChar Defender { get; set; }
        public Loc2D? DefenderTile { get; set; }
        public TargetCollection AllTargets { get; set; }

        //public int AttackStat { get; set; }
        //public int DefenseStat { get; set; }

        //AttackerLevel (may not be needed)

        public int moveSlot { get; set; }
        public int moveIndex { get; set; }
        public MoveEntry Move { get; set; }

        public bool Cancel { get; set; }
        //public int AttackerMultiplier { get; set; }
        public int Multiplier { get; set; }
        public IntReference TimeForHit { get; set; }
        public int TotalWaveTime { get; set; }
        public int TotalWaves { get; set; }
        //public bool Hit { get; set; }
        //public int Damage { get; set; }
        //public bool KnockedOut { get; set; }
        //public ulong ExpGained { get; set; }

        public Dictionary<String, object> BattleTags { get; set; }


        public BattleSetup() {
            Attacker = null;
            Defender = null;
            DefenderTile = null;
            AllTargets = new TargetCollection();
            moveSlot = -1;
            moveIndex = -1;
            Move = null;
            Multiplier = 1000;
            BattleTags = new Dictionary<String, object>();
            TimeForHit = new IntReference();
        }

        public BattleSetup(BattleSetup copy) {
            Attacker = copy.Attacker;
            Defender = copy.Defender;
            DefenderTile = copy.DefenderTile;
            AllTargets = copy.AllTargets;
            moveSlot = copy.moveSlot;
            moveIndex = copy.moveIndex;
            Move = new MoveEntry(copy.Move);
            Cancel = copy.Cancel;
            Multiplier = copy.Multiplier;
            BattleTags = copy.BattleTags;
            TimeForHit = copy.TimeForHit;
            TotalWaveTime = copy.TotalWaveTime;
            TotalWaves = copy.TotalWaves;
        }

        public object GetBattleTag(string tag) {
            if (BattleTags.ContainsKey(tag)) {
                return BattleTags[tag];
            } else {
                return null;
            }
        }

        public void SetBattleTag(string tag, object value) {
            if (BattleTags.ContainsKey(tag)) {
                BattleTags[tag] = value;
            } else {
                BattleTags.Add(tag, value);
            }
        }
    }
}
