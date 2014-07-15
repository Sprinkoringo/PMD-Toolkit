using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Maps {
    public abstract class MapGroup {

        protected Dictionary<string, ActiveMap> maps;

        public int MapCount { get { return maps.Count; } }
        

        public ActiveMap this[string id] {
            get {
                if (!maps.ContainsKey(id)) {
                    maps.Add(id, loadMap(id));
                }
                if (!maps.ContainsKey(id))
                    return null;

                return maps[id];
            }
        }


        public MapGroup()
        {
            maps = new Dictionary<string, ActiveMap>();
        }

        public abstract void Initialize();

        protected abstract ActiveMap loadMap(string id);

        public void UnloadMap(string id)
        {
            if (maps.ContainsKey(id))
                maps.Remove(id);
        }

        public void ReloadMap(string id)
        {
            UnloadMap(id);
            loadMap(id);
        }

    }
}
