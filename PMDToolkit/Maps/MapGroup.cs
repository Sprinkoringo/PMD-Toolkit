/*The MIT License (MIT)

Copyright (c) 2014 PMU Staff

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/


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
