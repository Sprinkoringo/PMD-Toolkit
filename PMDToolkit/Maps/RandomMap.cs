using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Logic.Gameplay;
using PMDToolkit.Logic.Display;
using PMDToolkit.Logic.Results;
using System.IO;

namespace PMDToolkit.Maps {
    public abstract class RandomMap : ActiveMap {

        protected int seed;
        protected Random rand;

        protected Data.RDungeonFloor entry;

        public List<FloorBorder> FloorBorders { get; set; }
        public Dictionary<int, List<int>> BorderLinks { get; set; }

        //includes all start points and end points
        public Loc2D[] BorderPoints { get; set; }

        public RandomMap()
        {

        }

        //recalculates wall/water/slippery textures after a change
        public void RecalculateArea(Loc2D start, Loc2D end) {
            for (int x = start.X; x <= end.X; x++) {
                for (int y = start.Y; y <= end.Y; y++) {
                    //recalculate water

                    //recalculate slippery (cover)

                    //recalculate wall
                }
            }
        }

        public abstract void Generate(int seed, Data.RDungeonFloor entry, List<FloorBorder> floorBorders, Dictionary<int, List<int>> borderLinks);



        public void Save(BinaryWriter writer)
        {
            base.Save(writer);
            //write to memory stream

        }

        public void Load(BinaryReader reader)
        {
            base.Load(reader);
            //read from memory stream
        }

    }
}
