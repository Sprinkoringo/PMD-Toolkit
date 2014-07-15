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
    public class BasicMap {

        public const int MAX_NPC_SLOTS = 30;
        public const int MAX_ITEM_SLOTS = 30;

        public string Title { get; set; }
        public string Music { get; set; }
        public bool Indoors { get; set; }
        public int Darkness;
        public Enums.Weather Weather { get; set; }
        public int TimeLimit;
        public int MinNpcs { get; set; }
        public int MaxNpcs { get; set; }
        public int NpcSpawnTime { get; set; }

        public List<NpcSpawnData> NpcSpawns { get; set; }

        public List<MapLayer> GroundLayers { get; set; }
        public List<MapLayer> PropBackLayers { get; set; }
        public List<MapLayer> PropFrontLayers { get; set; }
        public List<MapLayer> FringeLayers { get; set; }
        public Tile[,] MapArray;

        public int Width { get { return MapArray.GetLength(0); } }
        public int Height { get { return MapArray.GetLength(1); } }

        public BasicMap()
        {
            Title = "";
            Music = "";

            TimeLimit = -1;
            Darkness = -1;

            NpcSpawns = new List<Maps.NpcSpawnData>();

            GroundLayers = new List<MapLayer>();
            PropBackLayers = new List<MapLayer>();
            PropFrontLayers = new List<MapLayer>();
            FringeLayers = new List<MapLayer>();
        }

        public void CreateBlank(int width, int height)
        {
            MapArray = new Tile[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    MapArray[i, j] = new Tile();
                }
            }

            MapLayer layer = new MapLayer("Ground", Width, Height);
            GroundLayers.Add(layer);
            layer = new MapLayer("Mask1", Width, Height);
            GroundLayers.Add(layer);
            layer = new MapLayer("Mask2", Width, Height);
            GroundLayers.Add(layer);
            layer = new MapLayer("Fringe1", Width, Height);
            FringeLayers.Add(layer);
            layer = new MapLayer("Fringe2", Width, Height);
            FringeLayers.Add(layer);
            
        }

        public virtual void Resize(int width, int height, Maps.Direction8 dir)
        {
            Loc2D diff = Operations.GetResizeOffset(MapArray.GetLength(0), MapArray.GetLength(1), width, height, dir);
            Operations.ResizeArray<Tile>(ref MapArray, width, height, dir, true);
            for (int i = 0; i < GroundLayers.Count; i++)
            {
                Game.UpdateLoadMsg("Resizing Map... (Layer " + GroundLayers[i].Name + ")");
                GroundLayers[i].Resize(width, height, dir);
            }
            for (int i = 0; i < PropBackLayers.Count; i++)
            {
                Game.UpdateLoadMsg("Resizing Map... (Layer " + PropBackLayers[i].Name + ")");
                PropBackLayers[i].Resize(width, height, dir);
            }
            for (int i = 0; i < PropFrontLayers.Count; i++)
            {
                Game.UpdateLoadMsg("Resizing Map... (Layer " + PropFrontLayers[i].Name + ")");
                PropFrontLayers[i].Resize(width, height, dir);
            }
            for (int i = 0; i < FringeLayers.Count; i++)
            {
                Game.UpdateLoadMsg("Resizing Map... (Layer " + FringeLayers[i].Name + ")");
                FringeLayers[i].Resize(width, height, dir);
            }
            for (int i = 0; i < NpcSpawns.Count; i++)
            {
                if (NpcSpawns[i].Loc.X > -1 && NpcSpawns[i].Loc.Y > -1)
                {
                    NpcSpawns[i].Loc += diff;
                    if (NpcSpawns[i].Loc.X < 0)
                        NpcSpawns[i].Loc.X = 0;
                    if (NpcSpawns[i].Loc.Y < 0)
                        NpcSpawns[i].Loc.Y = 0;
                }
            }
        }

        public void Save(BinaryWriter writer)
        {
            //write to memory stream

            writer.Write(Title);
            writer.Write(Music);
            writer.Write(Indoors);
            writer.Write(Darkness);
            writer.Write((int)Weather);
            writer.Write(TimeLimit);
            writer.Write(MinNpcs);
            writer.Write(MaxNpcs);
            writer.Write(NpcSpawnTime);

            writer.Write(NpcSpawns.Count);
            for (int i = 0; i < NpcSpawns.Count; i++)
            {
                writer.Write(NpcSpawns[i].Data.Species);
                writer.Write(NpcSpawns[i].Data.Form);
                writer.Write((int)NpcSpawns[i].Data.Gender);
                writer.Write((int)NpcSpawns[i].Data.Shiny);

                writer.Write(NpcSpawns[i].Loc.X);
                writer.Write(NpcSpawns[i].Loc.Y);

                writer.Write(NpcSpawns[i].MinLevel);
                writer.Write(NpcSpawns[i].MaxLevel);

                for (int j = 0; j < NpcSpawns[i].Moves.Length; i++)
                {
                    writer.Write(NpcSpawns[i].Moves[j]);
                }

                writer.Write(NpcSpawns[i].AppearanceRate);
            }

            writer.Write(Width);
            writer.Write(Height);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    writer.Write((int)MapArray[x, y].Data.Type);
                    writer.Write(MapArray[x, y].Data.Data1);
                    writer.Write(MapArray[x, y].Data.Data2);
                    writer.Write(MapArray[x, y].Data.Data3);
                    writer.Write(MapArray[x, y].Data.String1);
                    writer.Write(MapArray[x, y].Data.String2);
                    writer.Write(MapArray[x, y].Data.String3);
                }
            }

            writer.Write(GroundLayers.Count);
            for (int i = 0; i < GroundLayers.Count; i++)
            {
                writer.Write(GroundLayers[i].Name);
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        GroundLayers[i].Tiles[x, y].Save(writer);
                    }
                }
            }

            writer.Write(PropBackLayers.Count);
            for (int i = 0; i < PropBackLayers.Count; i++)
            {
                writer.Write(PropBackLayers[i].Name);
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        PropBackLayers[i].Tiles[x, y].Save(writer);
                    }
                }
            }

            writer.Write(PropFrontLayers.Count);
            for (int i = 0; i < PropFrontLayers.Count; i++)
            {
                writer.Write(PropFrontLayers[i].Name);
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        PropFrontLayers[i].Tiles[x, y].Save(writer);
                    }
                }
            }

            writer.Write(FringeLayers.Count);
            for (int i = 0; i < FringeLayers.Count; i++)
            {
                writer.Write(FringeLayers[i].Name);
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        FringeLayers[i].Tiles[x, y].Save(writer);
                    }
                }
            }
        }

        public void Load(BinaryReader reader)
        {
            //read from memory stream
            Title = reader.ReadString();
            Music = reader.ReadString();
            Indoors = reader.ReadBoolean();
            Darkness = reader.ReadInt32();
            Weather = (Enums.Weather)reader.ReadInt32();
            TimeLimit = reader.ReadInt32();
            MinNpcs = reader.ReadInt32();
            MaxNpcs = reader.ReadInt32();
            NpcSpawnTime = reader.ReadInt32();

            int npcSpawnCount = reader.ReadInt32();
            for (int i = 0; i < NpcSpawns.Count; i++)
            {
                NpcSpawnData spawnData = new NpcSpawnData();
                spawnData.Data.Species = reader.ReadInt32();
                spawnData.Data.Form = reader.ReadInt32();
                spawnData.Data.Gender = (Enums.Gender)reader.ReadInt32();
                spawnData.Data.Shiny = (Enums.Coloration)reader.ReadInt32();

                spawnData.Loc.X = reader.ReadInt32();
                spawnData.Loc.Y = reader.ReadInt32();

                spawnData.MinLevel = reader.ReadInt32();
                spawnData.MaxLevel = reader.ReadInt32();

                for (int j = 0; j < spawnData.Moves.Length; i++)
                {
                    spawnData.Moves[j] = reader.ReadInt32();
                }

                spawnData.AppearanceRate = reader.ReadInt32();

                NpcSpawns.Add(spawnData);
            }

            int width = reader.ReadInt32();
            int height = reader.ReadInt32();

            MapArray = new Tile[width, height];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    MapArray[x, y] = new Tile();
                    MapArray[x, y].Data.Type = (Enums.TileType)reader.ReadInt32();
                    MapArray[x, y].Data.Data1 = reader.ReadInt32();
                    MapArray[x, y].Data.Data2 = reader.ReadInt32();
                    MapArray[x, y].Data.Data3 = reader.ReadInt32();
                    MapArray[x, y].Data.String1 = reader.ReadString();
                    MapArray[x, y].Data.String2 = reader.ReadString();
                    MapArray[x, y].Data.String3 = reader.ReadString();
                }
            }

            int groundLayerCount = reader.ReadInt32();
            for (int i = 0; i < groundLayerCount; i++)
            {
                string name = reader.ReadString();
                MapLayer layerAnim = new MapLayer(name, width, height);
                Game.UpdateLoadMsg("Loading Map... (Layer " + layerAnim.Name + ")");
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        layerAnim.Tiles[x, y] = new TileAnim();
                        layerAnim.Tiles[x, y].Load(reader);
                    }
                }
                GroundLayers.Add(layerAnim);
            }

            int propBackLayerCount = reader.ReadInt32();
            for (int i = 0; i < propBackLayerCount; i++)
            {
                string name = reader.ReadString();
                MapLayer layerAnim = new MapLayer(name, width, height);
                Game.UpdateLoadMsg("Loading Map... (Layer " + layerAnim.Name + ")");
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        layerAnim.Tiles[x, y] = new TileAnim();
                        layerAnim.Tiles[x, y].Load(reader);
                    }
                }
                PropBackLayers.Add(layerAnim);
            }

            int propFrontLayerCount = reader.ReadInt32();
            for (int i = 0; i < propFrontLayerCount; i++)
            {
                string name = reader.ReadString();
                MapLayer layerAnim = new MapLayer(name, width, height);
                Game.UpdateLoadMsg("Loading Map... (Layer " + layerAnim.Name + ")");
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        layerAnim.Tiles[x, y] = new TileAnim();
                        layerAnim.Tiles[x, y].Load(reader);
                    }
                }
                PropFrontLayers.Add(layerAnim);
            }

            int fringeLayerCount = reader.ReadInt32();
            for (int i = 0; i < fringeLayerCount; i++)
            {
                string name = reader.ReadString();
                MapLayer layerAnim = new MapLayer(name, width, height);
                Game.UpdateLoadMsg("Loading Map... (Layer " + layerAnim.Name + ")");
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        layerAnim.Tiles[x, y] = new TileAnim();
                        layerAnim.Tiles[x, y].Load(reader);
                    }
                }
                FringeLayers.Add(layerAnim);
            }
        }
    }
}
