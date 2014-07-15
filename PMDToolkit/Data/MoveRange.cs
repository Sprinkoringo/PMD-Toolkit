using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Data {
    public class MoveRange {


        public Enums.RangeType RangeType { get; set; }
        public int Mobility { get; set; }
        public bool CutsCorners { get; set; }
        public int Distance { get; set; }

        public bool HitsSelf { get; set; }
        public bool HitsFriend { get; set; }
        public bool HitsFoe { get; set; }

        public MoveRange() {

        }

        public MoveRange(Enums.RangeType rangeType, int mobility, bool cutsCorners, int distance, bool hitsSelf, bool hitsFriend, bool hitsFoe) {
            RangeType = rangeType;
            Mobility = mobility;
            CutsCorners = cutsCorners;
            Distance = distance;
            HitsSelf = hitsSelf;
            HitsFriend = hitsFriend;
            HitsFoe = hitsFoe;
        }

        public MoveRange(MoveRange copy) {
            RangeType = copy.RangeType;
            Mobility = copy.Mobility;
            CutsCorners = copy.CutsCorners;
            Distance = copy.Distance;


            HitsSelf = copy.HitsSelf;
            HitsFriend = copy.HitsFriend;
            HitsFoe = copy.HitsFoe;
        }

    }
}
