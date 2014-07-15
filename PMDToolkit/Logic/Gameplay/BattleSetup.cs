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
