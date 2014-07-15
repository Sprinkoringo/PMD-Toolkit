using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Maps {
    public abstract class BasicDungeon {

        protected int seed;
        protected Random rand;
        public int Seed { get { return seed; } }
        
        protected Data.RDungeonEntry entry;

        //All dungeons have precisely one entry point
        public Loc3D Start { get; set; }

        protected Dictionary<int, RandomMap> maps;

        public int MapCount { get { return maps.Count; } }

        protected Dictionary<FloorLink, FloorLink> floorLinks;
        

        public RandomMap this[int i] {
            get {
                if (!maps.ContainsKey(i)) {
                    generateFloor(i);
                }
                if (!maps.ContainsKey(i))
                    return null;

                return maps[i];
            }
        }


		public BasicDungeon() {
            maps = new Dictionary<int, RandomMap>();
            floorLinks = new Dictionary<FloorLink, FloorLink>();
        }

        //dungeon structure is always decided upon before the details
        public abstract void Generate(int seed, Data.RDungeonEntry entry);

        protected abstract void generateFloor(int floor);

        //if given an exit from a floor, returns the entrance of the floor it will wind up in
        public FloorLink GetFloorLink(int floor, int exit)
        {
            FloorLink landing = floorLinks[new FloorLink(floor, exit)];
            return landing;
        }
    }
}
