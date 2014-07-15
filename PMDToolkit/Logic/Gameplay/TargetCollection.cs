using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using PMDToolkit.Maps;

namespace PMDToolkit.Logic.Gameplay {
    public class TargetCollection {

        List<Target> targets;
        List<TileTarget> tiles;

        int self;
        public int Self { get { return self; } }
        int friends;
        public int Friends { get { return friends; } }
        int foes;
        public int Foes { get { return foes; } }

        public List<Target> Targets { get { return targets; } }
        public List<TileTarget> Tiles { get { return tiles; } }

        public TargetCollection() {
            targets = new List<Target>();
            tiles = new List<TileTarget>();
        }

        public void Add(Target target) {
            if (target.TargetAlignment == Enums.Alignment.Self) {
                self++;
            } else if (target.TargetAlignment == Enums.Alignment.Friend) {
                friends++;
            } else if (target.TargetAlignment == Enums.Alignment.Foe) {
                foes++;
            }
            targets.Add(target);
        }

        public void Add(TileTarget tile) {
            tiles.Add(tile);
        }

    }
}
