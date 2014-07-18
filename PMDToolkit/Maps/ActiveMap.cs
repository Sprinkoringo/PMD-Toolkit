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
using PMDToolkit.Logic.Gameplay;
using PMDToolkit.Logic.Display;
using PMDToolkit.Logic.Results;
using PMDToolkit.Core;
using System.IO;

namespace PMDToolkit.Maps {
    public class ActiveMap : BasicMap {
        
        public Item[] Items { get; set; }
        public Npc[] Npcs { get; set; }

        public ActiveMap()
        {
            Items = new Item[BasicMap.MAX_ITEM_SLOTS];
            for (int i = 0; i < BasicMap.MAX_ITEM_SLOTS; i++)
            {
                Items[i] = new Item();
            }
            Npcs = new Npc[BasicMap.MAX_NPC_SLOTS];
            for (int i = 0; i < BasicMap.MAX_NPC_SLOTS; i++)
            {
                Npcs[i] = new Npc();
            }
        }

        public int AddItem(Item item)
        {
            for (int i = 0; i < BasicMap.MAX_ITEM_SLOTS; i++)
            {
                if (Items[i].ItemIndex == -1)
                {
                    Items[i] = item;
                    return i;
                }
            }
            return -1;
        }

        public int GetItem(Loc2D loc)
        {
            for (int i = 0; i < BasicMap.MAX_ITEM_SLOTS; i++)
            {
                if (Items[i].ItemIndex != -1 && Items[i].ItemLoc == loc)
                {
                    return i;
                }
            }
            return -1;
        }

        public int AddNpc(Npc npc)
        {
            for (int i = 0; i < BasicMap.MAX_NPC_SLOTS; i++)
            {
                if (Npcs[i].dead)
                {
                    Npcs[i] = npc;
                    return i;
                }
            }
            return -1;
        }

        public virtual void Resize(int width, int height, Maps.Direction8 dir)
        {
            base.Resize(width, height, dir);


        }

        public void Save(BinaryWriter writer)
        {
            base.Save(writer);
            //write to memory stream

            writer.Write(Items.Length);
            for (int i = 0; i < Items.Length; i++)
            {
                writer.Write(Items[i].ItemLoc.X);
                writer.Write(Items[i].ItemLoc.Y);
                writer.Write(Items[i].ItemIndex);
                writer.Write(Items[i].Amount);
                writer.Write(Items[i].Tag);
                writer.Write(Items[i].Disabled);
            }

            writer.Write(Npcs.Length);
            for (int i = 0; i < Npcs.Length; i++)
            {
                writer.Write(Npcs[i].Name);

                writer.Write(Npcs[i].CharData.Species);
                writer.Write(Npcs[i].CharData.Form);
                writer.Write((int)Npcs[i].CharData.Gender);
                writer.Write((int)Npcs[i].CharData.Shiny);

                writer.Write((int)Npcs[i].Level);
                
                writer.Write(Npcs[i].MaxHPBonus);
                writer.Write(Npcs[i].AtkBonus);
                writer.Write(Npcs[i].DefBonus);
                writer.Write(Npcs[i].SpAtkBonus);
                writer.Write(Npcs[i].SpDefBonus);
                writer.Write(Npcs[i].SpeedBonus);

                for (int j = 0; j < Processor.MAX_MOVE_SLOTS; j++)
                {
                    writer.Write(Npcs[i].BaseMoves[j].MoveNum);
                    writer.Write(Npcs[i].BaseMoves[j].PPBoost);
                }
            }

        }

        public void Load(BinaryReader reader)
        {
            base.Load(reader);
            //read from memory stream
            int itemCount = reader.ReadInt32();
        }
    }
}
