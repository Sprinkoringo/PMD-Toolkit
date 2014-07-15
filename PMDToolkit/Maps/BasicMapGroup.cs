using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PMDToolkit.Maps {
    public class BasicMapGroup : MapGroup {


        public BasicMapGroup()
        {
        }

        public override void Initialize()
        {

        }

        protected override ActiveMap loadMap(string id)
        {
            ActiveMap map = new ActiveMap();
            if (File.Exists(id))
            {
                using (BinaryReader reader = new BinaryReader(File.OpenRead(id)))
                {
                    ((BasicMap)map).Load(reader);
                }
            }
            else
            {
                map.CreateBlank(10, 10);
            }
            return map;
        }
    }
}
