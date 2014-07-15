using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using PMDToolkit.Core;

namespace PMDToolkit.Data {
    public class CharAnimation {
        public Logic.Display.CharSprite.ActionType ActionType { get; set; }

        public CharAnimation() {

        }

        public CharAnimation(CharAnimation copy) {
            ActionType = copy.ActionType;
        }

        public void Load(XmlReader reader) {
            while (reader.Read()) {
                if (reader.IsStartElement()) {
                    switch (reader.Name) {
                        case "ActionType": {
                                ActionType = reader.ReadString().ToEnum<Logic.Display.CharSprite.ActionType>();
                                break;
                            }
                    }
                }
            }
        }

        public void Save(XmlWriter writer) {
            writer.WriteElementString("ActionType", ActionType.ToString());
        }
    }
}
