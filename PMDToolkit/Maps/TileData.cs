using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Maps {
    public struct TileData {


        public Enums.TileType Type;
        public int Data1;
        public int Data2;
        public int Data3;

        public string String1;
        public string String2;
        public string String3;

        public TileData(Enums.TileType type, int data1, int data2, int data3, string String1, string String2, string String3) {
            Type = type;
            Data1 = data1;
            Data2 = data2;
            Data3 = data3;
            this.String1 = String1;
            this.String2 = String2;
            this.String3 = String3;
        }

    }
}
