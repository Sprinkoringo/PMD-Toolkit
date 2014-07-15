using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Data {
    public class NpcPreset {


        public int SpawnX { get; set; }
        public int SpawnY { get; set; }


        public int NpcNum { get; set; }

        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }

        public NpcPreset() {
            
            NpcNum = 0;
            SpawnX = -1;
            SpawnY = -1;
            MinLevel = -1;
            MaxLevel = -1;
        }
    }
}
