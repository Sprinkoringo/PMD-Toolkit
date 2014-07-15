using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit;
using PMDToolkit.Maps;
using PMDToolkit.Data;


namespace PMDToolkit.Maps.Dungeons {
    public class FloorByFloor : BasicDungeon {

        public override void Generate(int seed, RDungeonEntry entry) {
            this.seed = seed;
            this.entry = entry;

            rand = new Random(seed);

            for (int i = 0; i < entry.Floors.Count - 1; i++) {
                floorLinks.Add(new FloorLink(i, 1), new FloorLink(i + 1, 0));
            }
            floorLinks.Add(new FloorLink(entry.Floors.Count - 1, 1), new FloorLink(-1, 0));
            
            Start = new Loc3D(this[0].BorderPoints[0].X, this[0].BorderPoints[0].Y, 0);
        }

        //re-call-able floor getting methods, caching where needed
        protected override void generateFloor(int floor) {
            //borders are orders to the floor generator for entrance/exit data: requirements of where they need to be, and which direction they are
            List<FloorBorder> borders = new List<FloorBorder>();
            borders.Add(new FloorBorder(null, Direction3D.Forth));
            borders.Add(new FloorBorder(null, Direction3D.Back));

            //borderLinks are orders to the floor generator for entrance/exit data: requirements of which nodes must connect to each other
            Dictionary<int, List<int>> borderLinks = new Dictionary<int, List<int>>();
            List<int> ends = new List<int>();
            ends.Add(1);
            borderLinks.Add(0, ends);

            RandomMap map = GameData.FloorAlgorithmDex[entry.Floors[floor].Algorithm].CreateFloor();
            map.Generate(rand.Next(), entry.Floors[floor], borders, borderLinks);
            maps.Add(floor, map);
        }
    }
}
