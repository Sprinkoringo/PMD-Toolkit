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
using System.Xml;
using System.IO;
using PMDToolkit.Core;
using PMDToolkit.Graphics;

namespace PMDToolkit.Data {
    public class MoveAnimation {
        public Logic.Display.MoveAnimationType AnimType { get; set; }
        public int AnimIndex { get; set; }
        public RenderTime FrameLength { get; set; }
        public int Anim1 { get; set; }
        public int Anim2 { get; set; }
        public int Anim3 { get; set; }

        public MoveAnimation() {
            AnimIndex = -1;
        }

        public MoveAnimation(MoveAnimation copy) {
            AnimType = copy.AnimType;
            AnimIndex = copy.AnimIndex;
            FrameLength = copy.FrameLength;
            Anim1 = copy.Anim1;
            Anim2 = copy.Anim2;
            Anim3 = copy.Anim3;
        }

        public void Load(XmlReader reader) {
            while (reader.Read()) {
                if (reader.IsStartElement()) {
                    switch (reader.Name) {
                        case "AnimType": {
                                AnimType = reader.ReadString().ToEnum<Logic.Display.MoveAnimationType>();
                                break;
                            }
                        case "AnimIndex": {
                                AnimIndex = reader.ReadString().ToInt();
                                break;
                            }
                        case "FrameLength": {
                                FrameLength = RenderTime.FromMillisecs(reader.ReadString().ToInt());
                                break;
                            }
                        case "Anim1": {
                                Anim1 = reader.ReadString().ToInt();
                                break;
                            }
                        case "Anim2": {
                                Anim2 = reader.ReadString().ToInt();
                                break;
                            }
                        case "Anim3": {
                                Anim3 = reader.ReadString().ToInt();
                                break;
                            }
                    }
                }
            }
        }

        public void Save(XmlWriter writer) {
            writer.WriteElementString("AnimType", AnimType.ToString());
            writer.WriteElementString("AnimIndex", AnimIndex.ToString());
            writer.WriteElementString("FrameLength", FrameLength.ToString());
            writer.WriteElementString("Anim1", Anim1.ToString());
            writer.WriteElementString("Anim2", Anim2.ToString());
            writer.WriteElementString("Anim3", Anim3.ToString());
        }

    }
}
