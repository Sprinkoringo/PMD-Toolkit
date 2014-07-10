using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PMDToolkit.Logic.Gameplay;

namespace PMDToolkit.Editors
{
    public partial class MapLayerEditor : Form
    {

        public MapLayerEditor()
        {
            InitializeComponent();

            
            LoadLayers();
        }

        public void LoadLayers()
        {
            chklbLayers.Items.Clear();
            for (int i = Logic.Gameplay.Processor.CurrentMap.FringeLayers.Count - 1; i >= 0; i--)
            {
                chklbLayers.Items.Add("[Fringe] " + Logic.Gameplay.Processor.CurrentMap.FringeLayers[i].Name, MapEditor.showFringeLayer[i]);
            }

            for (int i = Logic.Gameplay.Processor.CurrentMap.PropFrontLayers.Count - 1; i >= 0; i--)
            {
                chklbLayers.Items.Add("[Front] " + Logic.Gameplay.Processor.CurrentMap.PropFrontLayers[i].Name, MapEditor.showPropFrontLayer[i]);
            }

            for (int i = Logic.Gameplay.Processor.CurrentMap.PropBackLayers.Count - 1; i >= 0; i--)
            {
                chklbLayers.Items.Add("[Back] " + Logic.Gameplay.Processor.CurrentMap.PropBackLayers[i].Name, MapEditor.showPropBackLayer[i]);
            }

            for (int i = Logic.Gameplay.Processor.CurrentMap.GroundLayers.Count - 1; i >= 0; i--)
            {
                chklbLayers.Items.Add("[Ground] " + Logic.Gameplay.Processor.CurrentMap.GroundLayers[i].Name, MapEditor.showGroundLayer[i]);
            }

            //chklbLayers.Items.Add("[Data]", MapEditor.showDataLayer);

            chklbLayers.SelectedIndex = 0;
        }

        private void MapLayerEditor_Load(object sender, EventArgs e)
        {

        }

        private void MapLayerEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainPanel.CurrentMapLayerEditor = null;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

        }

        private void chklbLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetLayerInfoFromIndex(chklbLayers.SelectedIndex, ref MapEditor.chosenEditLayer, ref MapEditor.chosenLayer);
        }

        void GetLayerInfoFromIndex(int index, ref MapEditor.EditLayer layerType, ref int layer)
        {
            if (Logic.Gameplay.Processor.CurrentMap.FringeLayers.Count > index)
            {
                layerType = MapEditor.EditLayer.Fringe;
                layer = Logic.Gameplay.Processor.CurrentMap.FringeLayers.Count - index - 1;
                return;
            }
            index -= Logic.Gameplay.Processor.CurrentMap.FringeLayers.Count;

            if (Logic.Gameplay.Processor.CurrentMap.PropFrontLayers.Count > index)
            {
                layerType = MapEditor.EditLayer.PropFront;
                layer = Logic.Gameplay.Processor.CurrentMap.PropFrontLayers.Count - index - 1;
                return;
            }
            index -= Logic.Gameplay.Processor.CurrentMap.PropFrontLayers.Count;

            if (Logic.Gameplay.Processor.CurrentMap.PropBackLayers.Count > index)
            {
                layerType = MapEditor.EditLayer.PropBack;
                layer = Logic.Gameplay.Processor.CurrentMap.PropBackLayers.Count - index - 1;
                return;
            }
            index -= Logic.Gameplay.Processor.CurrentMap.PropBackLayers.Count;

            if (Logic.Gameplay.Processor.CurrentMap.GroundLayers.Count > index)
            {
                layerType = MapEditor.EditLayer.Ground;
                layer = Logic.Gameplay.Processor.CurrentMap.GroundLayers.Count - index - 1;
                return;
            }

            layerType = MapEditor.EditLayer.Data;
            layer = 0;
        }


        int GetIndexFromLayerInfo(MapEditor.EditLayer layerType, int layer)
        {
            int layerIndex = 0;
            if (layerType == MapEditor.EditLayer.Fringe)
                return layerIndex + Processor.CurrentMap.FringeLayers.Count - 1 - layer;

            layerIndex += Logic.Gameplay.Processor.CurrentMap.FringeLayers.Count;
            if (layerType == MapEditor.EditLayer.PropFront)
                return layerIndex + Processor.CurrentMap.PropFrontLayers.Count - 1 - layer;

            layerIndex += Logic.Gameplay.Processor.CurrentMap.PropFrontLayers.Count;
            if (layerType == MapEditor.EditLayer.PropBack)
                return layerIndex + Processor.CurrentMap.PropBackLayers.Count - 1 - layer;

            layerIndex += Logic.Gameplay.Processor.CurrentMap.PropBackLayers.Count;
            if (layerType == MapEditor.EditLayer.Ground)
                return layerIndex + Logic.Gameplay.Processor.CurrentMap.GroundLayers.Count - 1 - layer;

            layerIndex += Logic.Gameplay.Processor.CurrentMap.GroundLayers.Count;
            if (layerType == MapEditor.EditLayer.Data)
                return layerIndex;

            
            return -1;
        }

        private void chklbLayers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            MapEditor.EditLayer layerType = MapEditor.EditLayer.Data;
            int layerNum = -1;
            GetLayerInfoFromIndex(e.Index, ref layerType, ref layerNum);

            switch (layerType)
            {
                case MapEditor.EditLayer.Data:
                    {
                        MapEditor.showDataLayer = (e.NewValue == CheckState.Checked);
                        break;
                    }
                case MapEditor.EditLayer.Ground:
                    {
                        MapEditor.showGroundLayer[layerNum] = (e.NewValue == CheckState.Checked);
                        break;
                    }
                case MapEditor.EditLayer.PropBack:
                    {
                        MapEditor.showPropBackLayer[layerNum] = (e.NewValue == CheckState.Checked);
                        break;
                    }
                case MapEditor.EditLayer.PropFront:
                    {
                        MapEditor.showPropFrontLayer[layerNum] = (e.NewValue == CheckState.Checked);
                        break;
                    }
                case MapEditor.EditLayer.Fringe:
                    {
                        MapEditor.showFringeLayer[layerNum] = (e.NewValue == CheckState.Checked);
                        break;
                    }
            }
        }
    }
}
