using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using PMDToolkit.Data;
using PMDToolkit.Graphics;
using System.IO;

namespace PMDToolkit.Editors
{
    public partial class MainPanel : Form
    {

        public static bool EditorLoaded;

        public static bool GameNeedWait;
        public static bool GameWaiting;

        public static MapEditor CurrentMapEditor;
        public static MapLayerEditor CurrentMapLayerEditor;

        public MainPanel()
        {
            while (Game.GameLoaded < Game.GameLoadState.Loaded)
                Thread.Sleep(100);

            InitializeComponent();



            for (int i = 0; i <= GameData.MAX_DEX; i++)
            {
                cbDexNum.Items.Add(i + " - " + GameData.Dex[i].Name);
            }
            cbDexNum.SelectedIndex = 1;


            for (int i = 0; i < 2; i++)
            {
                cbShiny.Items.Add(((Enums.Coloration)i).ToString());
            }
            cbShiny.SelectedIndex = 0;

            for (int i = 0; i < 3; i++)
            {
                cbGender.Items.Add(((Enums.Gender)i).ToString());
            }
            cbGender.SelectedIndex = 0;

            chkShowSprites.Checked = Logic.Display.Screen.ShowSprites;

            for (int i = 0; i < (int)Logic.Display.CharSprite.ActionType.Throw; i++)
            {
                cbAnim.Items.Add(((Logic.Display.CharSprite.ActionType)i).ToString());
            }
            cbAnim.SelectedIndex = 0;

            txtTilePath.Text = Paths.TilesPath;
            txtSpritePath.Text = Paths.SpritesPath;
            txtPortraitPath.Text = Paths.PortraitsPath;
            txtEffectPath.Text = Paths.EffectsPath;
            txtItemPath.Text = Paths.ItemsPath;
            txtMusicPath.Text = Paths.MusicPath;
            txtSoundPath.Text = Paths.SoundsPath;

            chkGrid.Checked = Logic.Display.Screen.ShowGrid;
            chkCoords.Checked = Logic.Display.Screen.ShowCoords;

        }

        private void chkShowSprites_CheckedChanged(object sender, EventArgs e)
        {
            Logic.Display.Screen.ShowSprites = chkShowSprites.Checked;
        }

        private void cbDexNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbForme.Items.Clear();
            for (int i = 0; i < GameData.Dex[cbDexNum.SelectedIndex].Forms.Count; i++)
            {
                cbForme.Items.Add(i + " - " + GameData.Dex[cbDexNum.SelectedIndex].Forms[i].FormName);
            }
            cbForme.SelectedIndex = 0;
            UpdateSprite();
        }

        private void cbForme_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSprite();
        }

        private void cbShiny_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSprite();
        }

        private void cbGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSprite();
        }

        private void UpdateSprite()
        {
            Logic.Gameplay.Processor.ChangeAppearance(Logic.Gameplay.Processor.Players[0], new Logic.Gameplay.FormData(cbDexNum.SelectedIndex, cbForme.SelectedIndex, (Enums.Gender)cbGender.SelectedIndex, (Enums.Coloration)cbShiny.SelectedIndex));
        }

        private void btnReloadSprites_Click(object sender, EventArgs e)
        {
            Game.UpdateLoadMsg("Reloading Sprites...");
            EnterLoadPhase(Game.GameLoadState.Loading);

            Conversion.CompileAllSprites(Paths.SpritesPath, Paths.CachedGFXPath + "Sprite");
            TextureManager.NeedSpriteReload = true;

            EnterLoadPhase(Game.GameLoadState.Loaded);
        }

        private void btnAnimate_Click(object sender, EventArgs e)
        {
            Logic.Gameplay.Processor.MockAnim((Logic.Display.CharSprite.ActionType)cbAnim.SelectedIndex, false, false);
        }

        private void btnLoop_Click(object sender, EventArgs e)
        {
            Logic.Display.CharSprite.ActionType action = (Logic.Display.CharSprite.ActionType)cbAnim.SelectedIndex;
            Logic.Gameplay.Processor.MockAnim(action, true, action == Logic.Display.CharSprite.ActionType.Walk);
        }

        private void btnMapEditor_Click(object sender, EventArgs e)
        {
            if (CurrentMapEditor == null)
            {
                CurrentMapEditor = new MapEditor();
            }
            if (CurrentMapLayerEditor == null)
            {
                CurrentMapLayerEditor = new MapLayerEditor();
            }
            
            CurrentMapEditor.Show();
            CurrentMapLayerEditor.Show();
        }

        private void chkGrid_CheckedChanged(object sender, EventArgs e)
        {
            Logic.Display.Screen.ShowGrid = chkGrid.Checked;
        }

        private void chkCoords_CheckedChanged(object sender, EventArgs e)
        {
            Logic.Display.Screen.ShowCoords = chkCoords.Checked;
        }

        private void MainPanel_Load(object sender, EventArgs e)
        {
            EditorLoaded = true;
        }

        private void MainPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            Game.UpdateLoadMsg("Closing...");
            EnterLoadPhase(Game.GameLoadState.Closing);
        }

        public static void EnterLoadPhase(Game.GameLoadState loadState)
        {
            MainPanel.GameNeedWait = true;
            while (!MainPanel.GameWaiting)
                Thread.Sleep(100);

            Game.GameLoaded = loadState;

            MainPanel.GameNeedWait = false;
            while (MainPanel.GameWaiting)
                Thread.Sleep(100);
        }


        private void txtTilePath_Click(object sender, EventArgs e)
        {
            if (Path.GetFullPath(Paths.TilesPath) != Path.GetFullPath(txtTilePath.Text))
            {
                Game.UpdateLoadMsg("Switching Paths...");
                EnterLoadPhase(Game.GameLoadState.Loading);

                Paths.TilesPath = txtTilePath.Text;
                Directory.Delete(Paths.CachedGFXPath + "Tile", true);
                Conversion.CompileAllTiles(Paths.TilesPath, Paths.CachedGFXPath + "Tile");
                TextureManager.NeedTileReload = true;
                Paths.SavePaths();

                EnterLoadPhase(Game.GameLoadState.Loaded);
            }
        }

        private void txtSpritePath_Click(object sender, EventArgs e)
        {
            if (Path.GetFullPath(Paths.SpritesPath) != Path.GetFullPath(txtSpritePath.Text))
            {
                Game.UpdateLoadMsg("Switching Paths...");
                EnterLoadPhase(Game.GameLoadState.Loading);

                Paths.SpritesPath = txtSpritePath.Text;
                Directory.Delete(Paths.CachedGFXPath + "Sprite", true);
                Conversion.CompileAllSprites(Paths.SpritesPath, Paths.CachedGFXPath + "Sprite");
                TextureManager.NeedSpriteReload = true;
                Paths.SavePaths();

                EnterLoadPhase(Game.GameLoadState.Loaded);
            }
        }

        private void txtPortraitPath_Click(object sender, EventArgs e)
        {
            if (Path.GetFullPath(Paths.PortraitsPath) != Path.GetFullPath(txtPortraitPath.Text))
            {
                Game.UpdateLoadMsg("Switching Paths...");
                EnterLoadPhase(Game.GameLoadState.Loading);

                Paths.PortraitsPath = txtPortraitPath.Text;
                Directory.Delete(Paths.CachedGFXPath + "Portrait", true);
                Conversion.CompileAllPortraits(Paths.PortraitsPath, Paths.CachedGFXPath + "Portrait");
                TextureManager.NeedPortraitReload = true;
                Paths.SavePaths();

                EnterLoadPhase(Game.GameLoadState.Loaded);
            }
        }

        private void txtEffectPath_Click(object sender, EventArgs e)
        {
            if (Path.GetFullPath(Paths.EffectsPath) != Path.GetFullPath(txtEffectPath.Text))
            {
                Game.UpdateLoadMsg("Switching Paths...");
                EnterLoadPhase(Game.GameLoadState.Loading);

                Paths.EffectsPath = txtEffectPath.Text;
                TextureManager.NeedEffectReload = true;
                Paths.SavePaths();

                EnterLoadPhase(Game.GameLoadState.Loaded);
            }
        }

        private void txtItemPath_Click(object sender, EventArgs e)
        {
            if (Path.GetFullPath(Paths.ItemsPath) != Path.GetFullPath(txtItemPath.Text))
            {
                Game.UpdateLoadMsg("Switching Paths...");
                EnterLoadPhase(Game.GameLoadState.Loading);

                Paths.ItemsPath = txtItemPath.Text;
                TextureManager.NeedItemReload = true;
                Paths.SavePaths();

                EnterLoadPhase(Game.GameLoadState.Loaded);
            }
        }

        private void txtMusicPath_Click(object sender, EventArgs e)
        {
            if (Path.GetFullPath(Paths.MusicPath) != Path.GetFullPath(txtMusicPath.Text))
            {
                Game.UpdateLoadMsg("Switching Paths...");
                EnterLoadPhase(Game.GameLoadState.Loading);

                Paths.MusicPath = txtMusicPath.Text;
                //TextureManager.NeedMusicReload = true;
                Paths.SavePaths();

                EnterLoadPhase(Game.GameLoadState.Loaded);
            }
        }

        private void txtSoundPath_Click(object sender, EventArgs e)
        {
            if (Path.GetFullPath(Paths.SoundsPath) != Path.GetFullPath(txtSoundPath.Text))
            {
                Game.UpdateLoadMsg("Switching Paths...");
                EnterLoadPhase(Game.GameLoadState.Loading);

                Paths.SoundsPath = txtSoundPath.Text;
                //TextureManager.NeedSoundReload = true;
                Paths.SavePaths();

                EnterLoadPhase(Game.GameLoadState.Loaded);
            }
        }

        private void btnDefaultPath_Click(object sender, EventArgs e)
        {
            
            Game.UpdateLoadMsg("Resetting all paths to default...");
            EnterLoadPhase(Game.GameLoadState.Loading);

            Paths.CreateDefaultPaths();

            if (Path.GetFullPath(txtTilePath.Text) != Path.GetFullPath(Paths.TilesPath))
            {
                Directory.Delete(Paths.CachedGFXPath + "Tile", true);
                Conversion.CompileAllTiles(Paths.TilesPath, Paths.CachedGFXPath + "Tile");
            }
            txtTilePath.Text = Paths.TilesPath;

            if (Path.GetFullPath(txtSpritePath.Text) != Path.GetFullPath(Paths.SpritesPath))
            {
                Directory.Delete(Paths.CachedGFXPath + "Sprite", true);
                Conversion.CompileAllSprites(Paths.SpritesPath, Paths.CachedGFXPath + "Sprite");
            }
            txtSpritePath.Text = Paths.SpritesPath;

            if (Path.GetFullPath(txtPortraitPath.Text) != Path.GetFullPath(Paths.PortraitsPath))
            {
                Directory.Delete(Paths.CachedGFXPath + "Portrait", true);
                Conversion.CompileAllPortraits(Paths.PortraitsPath, Paths.CachedGFXPath + "Portrait");
            }
            txtPortraitPath.Text = Paths.PortraitsPath;

            txtEffectPath.Text = Paths.EffectsPath;
            txtItemPath.Text = Paths.ItemsPath;
            txtMusicPath.Text = Paths.MusicPath;
            txtSoundPath.Text = Paths.SoundsPath;

            TextureManager.NeedTileReload = true;
            TextureManager.NeedSpriteReload = true;
            TextureManager.NeedPortraitReload = true;
            TextureManager.NeedEffectReload = true;
            TextureManager.NeedItemReload = true;

            EnterLoadPhase(Game.GameLoadState.Loaded);
        }
    }
}
