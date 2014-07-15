using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Maps
{
    public struct FloorLink
    {
        public int FloorNum;
        public int EntranceIndex;

        public FloorLink(int floorNum, int entranceIndex)
        {
            FloorNum = floorNum;
            EntranceIndex = entranceIndex;
        }

        public static bool operator ==(FloorLink link1, FloorLink link2)
        {
            return ((link1.FloorNum == link2.FloorNum) && (link1.EntranceIndex == link2.EntranceIndex));
        }

        public static bool operator !=(FloorLink link1, FloorLink link2)
        {
            return !((link1.FloorNum == link2.FloorNum) && (link1.EntranceIndex == link2.EntranceIndex));
        }
    }
}
