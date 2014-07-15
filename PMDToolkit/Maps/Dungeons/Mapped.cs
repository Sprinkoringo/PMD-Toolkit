using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit;
using PMDToolkit.Maps;
using PMDToolkit.Data;


namespace PMDToolkit.Maps.Dungeons {
    public class Mapped : BasicDungeon {

        string fileName;

        public override void Generate(int seed, RDungeonEntry entry)
        {
            Generate(seed, "");
        }

        public void Generate(int seed, string fileName )
        {
            this.seed = seed;
            this.entry = null;

            this.fileName = fileName;
            rand = new Random(seed);

            Start = new Loc3D(this[0].BorderPoints[0].X, this[0].BorderPoints[0].Y, 0);
        }

        //re-call-able floor getting methods, caching where needed
        protected override void generateFloor(int floor)
        {
            if (floor == 0)
            {
                Floors.StandardMap map = new Floors.StandardMap();
                map.Generate(rand.Next(), fileName);
                maps.Add(floor, map);
            }
        }
    }
}
