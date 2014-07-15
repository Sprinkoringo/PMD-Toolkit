namespace PMDToolkit.Editors
{
    partial class MapEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapEditor));
            this.mnuMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resizeMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.picTileset = new System.Windows.Forms.PictureBox();
            this.hScroll = new System.Windows.Forms.HScrollBar();
            this.vScroll = new System.Windows.Forms.VScrollBar();
            this.chkTexEyeDropper = new System.Windows.Forms.CheckBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tabMapOptions = new System.Windows.Forms.TabControl();
            this.tabTextures = new System.Windows.Forms.TabPage();
            this.lbxFrames = new PMDToolkit.Editors.TileAnimListBox();
            this.btnRemoveFrame = new System.Windows.Forms.Button();
            this.btnAddFrame = new System.Windows.Forms.Button();
            this.nudFrameLength = new PMDToolkit.Editors.IntNumericUpDown();
            this.lblTileInfo = new System.Windows.Forms.Label();
            this.picTile = new System.Windows.Forms.PictureBox();
            this.chkAnimationMode = new System.Windows.Forms.CheckBox();
            this.chkFill = new System.Windows.Forms.CheckBox();
            this.btnReloadTiles = new System.Windows.Forms.Button();
            this.lblTileset = new System.Windows.Forms.Label();
            this.tbTileset = new System.Windows.Forms.TrackBar();
            this.lblFrames = new System.Windows.Forms.Label();
            this.lblFrameLength = new System.Windows.Forms.Label();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.chkIndoors = new System.Windows.Forms.CheckBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.cbWeather = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMapName = new System.Windows.Forms.TextBox();
            this.btnReloadSongs = new System.Windows.Forms.Button();
            this.lblMapName = new System.Windows.Forms.Label();
            this.nudTimeLimit = new PMDToolkit.Editors.IntNumericUpDown();
            this.nudDarkness = new PMDToolkit.Editors.IntNumericUpDown();
            this.lbxMusic = new System.Windows.Forms.ListBox();
            this.lblMusic = new System.Windows.Forms.Label();
            this.lblDarkness = new System.Windows.Forms.Label();
            this.mnuMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTileset)).BeginInit();
            this.tabMapOptions.SuspendLayout();
            this.tabTextures.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFrameLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTileset)).BeginInit();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimeLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDarkness)).BeginInit();
            this.SuspendLayout();
            // 
            // mnuMenu
            // 
            this.mnuMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.mnuMenu.Location = new System.Drawing.Point(0, 0);
            this.mnuMenu.Name = "mnuMenu";
            this.mnuMenu.Size = new System.Drawing.Size(681, 24);
            this.mnuMenu.TabIndex = 0;
            this.mnuMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resizeMapToolStripMenuItem,
            this.clearLayerToolStripMenuItem,
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // resizeMapToolStripMenuItem
            // 
            this.resizeMapToolStripMenuItem.Name = "resizeMapToolStripMenuItem";
            this.resizeMapToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.resizeMapToolStripMenuItem.Text = "Resize Map";
            this.resizeMapToolStripMenuItem.Click += new System.EventHandler(this.resizeMapToolStripMenuItem_Click);
            // 
            // clearLayerToolStripMenuItem
            // 
            this.clearLayerToolStripMenuItem.Name = "clearLayerToolStripMenuItem";
            this.clearLayerToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.clearLayerToolStripMenuItem.Text = "Clear Layer";
            this.clearLayerToolStripMenuItem.Click += new System.EventHandler(this.clearLayerToolStripMenuItem_Click);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.layersToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // layersToolStripMenuItem
            // 
            this.layersToolStripMenuItem.Name = "layersToolStripMenuItem";
            this.layersToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.layersToolStripMenuItem.Text = "Layers";
            this.layersToolStripMenuItem.Click += new System.EventHandler(this.layersToolStripMenuItem_Click);
            // 
            // picTileset
            // 
            this.picTileset.Location = new System.Drawing.Point(6, 62);
            this.picTileset.Name = "picTileset";
            this.picTileset.Size = new System.Drawing.Size(480, 416);
            this.picTileset.TabIndex = 1;
            this.picTileset.TabStop = false;
            this.picTileset.Click += new System.EventHandler(this.picTileset_Click);
            // 
            // hScroll
            // 
            this.hScroll.Location = new System.Drawing.Point(6, 478);
            this.hScroll.Name = "hScroll";
            this.hScroll.Size = new System.Drawing.Size(480, 17);
            this.hScroll.TabIndex = 2;
            this.hScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScroll_Scroll);
            // 
            // vScroll
            // 
            this.vScroll.Location = new System.Drawing.Point(485, 62);
            this.vScroll.Name = "vScroll";
            this.vScroll.Size = new System.Drawing.Size(17, 416);
            this.vScroll.TabIndex = 1;
            this.vScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScroll_Scroll);
            // 
            // chkTexEyeDropper
            // 
            this.chkTexEyeDropper.AutoSize = true;
            this.chkTexEyeDropper.Location = new System.Drawing.Point(505, 478);
            this.chkTexEyeDropper.Name = "chkTexEyeDropper";
            this.chkTexEyeDropper.Size = new System.Drawing.Size(85, 17);
            this.chkTexEyeDropper.TabIndex = 4;
            this.chkTexEyeDropper.Text = "Eye Dropper";
            this.chkTexEyeDropper.UseVisualStyleBackColor = true;
            this.chkTexEyeDropper.CheckedChanged += new System.EventHandler(this.chkTexEyeDropper_CheckedChanged);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "map files (*.pmdmap)|*.pmdmap";
            this.openFileDialog.RestoreDirectory = true;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "map files (*.pmdmap)|*.pmdmap";
            this.saveFileDialog.RestoreDirectory = true;
            // 
            // tabMapOptions
            // 
            this.tabMapOptions.Controls.Add(this.tabTextures);
            this.tabMapOptions.Controls.Add(this.tabProperties);
            this.tabMapOptions.Location = new System.Drawing.Point(12, 27);
            this.tabMapOptions.Name = "tabMapOptions";
            this.tabMapOptions.SelectedIndex = 0;
            this.tabMapOptions.Size = new System.Drawing.Size(657, 525);
            this.tabMapOptions.TabIndex = 16;
            // 
            // tabTextures
            // 
            this.tabTextures.Controls.Add(this.lbxFrames);
            this.tabTextures.Controls.Add(this.btnRemoveFrame);
            this.tabTextures.Controls.Add(this.btnAddFrame);
            this.tabTextures.Controls.Add(this.nudFrameLength);
            this.tabTextures.Controls.Add(this.lblTileInfo);
            this.tabTextures.Controls.Add(this.picTile);
            this.tabTextures.Controls.Add(this.chkAnimationMode);
            this.tabTextures.Controls.Add(this.chkFill);
            this.tabTextures.Controls.Add(this.btnReloadTiles);
            this.tabTextures.Controls.Add(this.lblTileset);
            this.tabTextures.Controls.Add(this.picTileset);
            this.tabTextures.Controls.Add(this.hScroll);
            this.tabTextures.Controls.Add(this.chkTexEyeDropper);
            this.tabTextures.Controls.Add(this.vScroll);
            this.tabTextures.Controls.Add(this.tbTileset);
            this.tabTextures.Controls.Add(this.lblFrames);
            this.tabTextures.Controls.Add(this.lblFrameLength);
            this.tabTextures.Location = new System.Drawing.Point(4, 22);
            this.tabTextures.Name = "tabTextures";
            this.tabTextures.Padding = new System.Windows.Forms.Padding(3);
            this.tabTextures.Size = new System.Drawing.Size(649, 499);
            this.tabTextures.TabIndex = 0;
            this.tabTextures.Text = "Textures";
            this.tabTextures.UseVisualStyleBackColor = true;
            // 
            // lbxFrames
            // 
            this.lbxFrames.FormattingEnabled = true;
            this.lbxFrames.Location = new System.Drawing.Point(513, 139);
            this.lbxFrames.Name = "lbxFrames";
            this.lbxFrames.Size = new System.Drawing.Size(120, 95);
            this.lbxFrames.TabIndex = 29;
            this.lbxFrames.SelectedIndexChanged += new System.EventHandler(this.lbxFrames_SelectedIndexChanged);
            // 
            // btnRemoveFrame
            // 
            this.btnRemoveFrame.Location = new System.Drawing.Point(575, 240);
            this.btnRemoveFrame.Name = "btnRemoveFrame";
            this.btnRemoveFrame.Size = new System.Drawing.Size(58, 23);
            this.btnRemoveFrame.TabIndex = 28;
            this.btnRemoveFrame.Text = "Remove";
            this.btnRemoveFrame.UseVisualStyleBackColor = true;
            this.btnRemoveFrame.Click += new System.EventHandler(this.btnRemoveFrame_Click);
            // 
            // btnAddFrame
            // 
            this.btnAddFrame.Location = new System.Drawing.Point(513, 240);
            this.btnAddFrame.Name = "btnAddFrame";
            this.btnAddFrame.Size = new System.Drawing.Size(56, 23);
            this.btnAddFrame.TabIndex = 27;
            this.btnAddFrame.Text = "Add";
            this.btnAddFrame.UseVisualStyleBackColor = true;
            this.btnAddFrame.Click += new System.EventHandler(this.btnAddFrame_Click);
            // 
            // nudFrameLength
            // 
            this.nudFrameLength.Location = new System.Drawing.Point(513, 92);
            this.nudFrameLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFrameLength.Name = "nudFrameLength";
            this.nudFrameLength.Size = new System.Drawing.Size(120, 20);
            this.nudFrameLength.TabIndex = 23;
            this.nudFrameLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFrameLength.TextChanged += new System.EventHandler(this.nudFrameLength_TextChanged);
            // 
            // lblTileInfo
            // 
            this.lblTileInfo.AutoSize = true;
            this.lblTileInfo.Location = new System.Drawing.Point(552, 41);
            this.lblTileInfo.Name = "lblTileInfo";
            this.lblTileInfo.Size = new System.Drawing.Size(45, 13);
            this.lblTileInfo.TabIndex = 22;
            this.lblTileInfo.Text = "Tile Info";
            // 
            // picTile
            // 
            this.picTile.Location = new System.Drawing.Point(513, 32);
            this.picTile.Name = "picTile";
            this.picTile.Size = new System.Drawing.Size(32, 32);
            this.picTile.TabIndex = 21;
            this.picTile.TabStop = false;
            // 
            // chkAnimationMode
            // 
            this.chkAnimationMode.AutoSize = true;
            this.chkAnimationMode.Location = new System.Drawing.Point(513, 9);
            this.chkAnimationMode.Name = "chkAnimationMode";
            this.chkAnimationMode.Size = new System.Drawing.Size(77, 17);
            this.chkAnimationMode.TabIndex = 20;
            this.chkAnimationMode.Text = "Animations";
            this.chkAnimationMode.UseVisualStyleBackColor = true;
            this.chkAnimationMode.CheckedChanged += new System.EventHandler(this.chkAnimationMode_CheckedChanged);
            // 
            // chkFill
            // 
            this.chkFill.AutoSize = true;
            this.chkFill.Location = new System.Drawing.Point(505, 455);
            this.chkFill.Name = "chkFill";
            this.chkFill.Size = new System.Drawing.Size(38, 17);
            this.chkFill.TabIndex = 19;
            this.chkFill.Text = "Fill";
            this.chkFill.UseVisualStyleBackColor = true;
            this.chkFill.CheckedChanged += new System.EventHandler(this.chkFill_CheckedChanged);
            // 
            // btnReloadTiles
            // 
            this.btnReloadTiles.Location = new System.Drawing.Point(427, 3);
            this.btnReloadTiles.Name = "btnReloadTiles";
            this.btnReloadTiles.Size = new System.Drawing.Size(75, 23);
            this.btnReloadTiles.TabIndex = 18;
            this.btnReloadTiles.Text = "Reload Tiles";
            this.btnReloadTiles.UseVisualStyleBackColor = true;
            this.btnReloadTiles.Click += new System.EventHandler(this.btnReloadTiles_Click);
            // 
            // lblTileset
            // 
            this.lblTileset.AutoSize = true;
            this.lblTileset.Location = new System.Drawing.Point(8, 8);
            this.lblTileset.Name = "lblTileset";
            this.lblTileset.Size = new System.Drawing.Size(0, 13);
            this.lblTileset.TabIndex = 17;
            // 
            // tbTileset
            // 
            this.tbTileset.BackColor = System.Drawing.SystemColors.Window;
            this.tbTileset.Location = new System.Drawing.Point(6, 26);
            this.tbTileset.Margin = new System.Windows.Forms.Padding(0);
            this.tbTileset.Maximum = 9;
            this.tbTileset.Name = "tbTileset";
            this.tbTileset.Size = new System.Drawing.Size(480, 45);
            this.tbTileset.TabIndex = 16;
            this.tbTileset.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.tbTileset.Scroll += new System.EventHandler(this.tbTileset_Scroll);
            // 
            // lblFrames
            // 
            this.lblFrames.AutoSize = true;
            this.lblFrames.Location = new System.Drawing.Point(510, 123);
            this.lblFrames.Name = "lblFrames";
            this.lblFrames.Size = new System.Drawing.Size(41, 13);
            this.lblFrames.TabIndex = 26;
            this.lblFrames.Text = "Frames";
            // 
            // lblFrameLength
            // 
            this.lblFrameLength.AutoSize = true;
            this.lblFrameLength.Location = new System.Drawing.Point(510, 76);
            this.lblFrameLength.Name = "lblFrameLength";
            this.lblFrameLength.Size = new System.Drawing.Size(72, 13);
            this.lblFrameLength.TabIndex = 24;
            this.lblFrameLength.Text = "Frame Length";
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.chkIndoors);
            this.tabProperties.Controls.Add(this.lblTime);
            this.tabProperties.Controls.Add(this.cbWeather);
            this.tabProperties.Controls.Add(this.label1);
            this.tabProperties.Controls.Add(this.txtMapName);
            this.tabProperties.Controls.Add(this.btnReloadSongs);
            this.tabProperties.Controls.Add(this.lblMapName);
            this.tabProperties.Controls.Add(this.nudTimeLimit);
            this.tabProperties.Controls.Add(this.nudDarkness);
            this.tabProperties.Controls.Add(this.lbxMusic);
            this.tabProperties.Controls.Add(this.lblMusic);
            this.tabProperties.Controls.Add(this.lblDarkness);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Padding = new System.Windows.Forms.Padding(3);
            this.tabProperties.Size = new System.Drawing.Size(649, 499);
            this.tabProperties.TabIndex = 1;
            this.tabProperties.Text = "Properties";
            this.tabProperties.UseVisualStyleBackColor = true;
            // 
            // chkIndoors
            // 
            this.chkIndoors.AutoSize = true;
            this.chkIndoors.Location = new System.Drawing.Point(9, 196);
            this.chkIndoors.Name = "chkIndoors";
            this.chkIndoors.Size = new System.Drawing.Size(61, 17);
            this.chkIndoors.TabIndex = 13;
            this.chkIndoors.Text = "Indoors";
            this.chkIndoors.UseVisualStyleBackColor = true;
            this.chkIndoors.CheckedChanged += new System.EventHandler(this.chkIndoors_CheckedChanged);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(6, 56);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(54, 13);
            this.lblTime.TabIndex = 12;
            this.lblTime.Text = "Time Limit";
            // 
            // cbWeather
            // 
            this.cbWeather.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWeather.FormattingEnabled = true;
            this.cbWeather.Location = new System.Drawing.Point(9, 116);
            this.cbWeather.Name = "cbWeather";
            this.cbWeather.Size = new System.Drawing.Size(324, 21);
            this.cbWeather.TabIndex = 8;
            this.cbWeather.SelectedIndexChanged += new System.EventHandler(this.cbWeather_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Weather";
            // 
            // txtMapName
            // 
            this.txtMapName.Location = new System.Drawing.Point(9, 30);
            this.txtMapName.Name = "txtMapName";
            this.txtMapName.Size = new System.Drawing.Size(324, 20);
            this.txtMapName.TabIndex = 6;
            this.txtMapName.TextChanged += new System.EventHandler(this.txtMapName_TextChanged);
            // 
            // btnReloadSongs
            // 
            this.btnReloadSongs.Location = new System.Drawing.Point(361, 285);
            this.btnReloadSongs.Name = "btnReloadSongs";
            this.btnReloadSongs.Size = new System.Drawing.Size(237, 23);
            this.btnReloadSongs.TabIndex = 5;
            this.btnReloadSongs.Text = "Reload Music Folder";
            this.btnReloadSongs.UseVisualStyleBackColor = true;
            this.btnReloadSongs.Click += new System.EventHandler(this.btnReloadSongs_Click);
            // 
            // lblMapName
            // 
            this.lblMapName.AutoSize = true;
            this.lblMapName.Location = new System.Drawing.Point(6, 12);
            this.lblMapName.Name = "lblMapName";
            this.lblMapName.Size = new System.Drawing.Size(59, 13);
            this.lblMapName.TabIndex = 1;
            this.lblMapName.Text = "Map Name";
            // 
            // nudTimeLimit
            // 
            this.nudTimeLimit.Location = new System.Drawing.Point(9, 74);
            this.nudTimeLimit.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudTimeLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudTimeLimit.Name = "nudTimeLimit";
            this.nudTimeLimit.Size = new System.Drawing.Size(324, 20);
            this.nudTimeLimit.TabIndex = 11;
            this.nudTimeLimit.TextChanged += new System.EventHandler(this.nudTimeLimit_TextChanged);
            // 
            // nudDarkness
            // 
            this.nudDarkness.Location = new System.Drawing.Point(9, 160);
            this.nudDarkness.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudDarkness.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudDarkness.Name = "nudDarkness";
            this.nudDarkness.Size = new System.Drawing.Size(324, 20);
            this.nudDarkness.TabIndex = 10;
            this.nudDarkness.TextChanged += new System.EventHandler(this.nudDarkness_TextChanged);
            // 
            // lbxMusic
            // 
            this.lbxMusic.FormattingEnabled = true;
            this.lbxMusic.Location = new System.Drawing.Point(361, 28);
            this.lbxMusic.Name = "lbxMusic";
            this.lbxMusic.Size = new System.Drawing.Size(237, 251);
            this.lbxMusic.TabIndex = 0;
            this.lbxMusic.SelectedIndexChanged += new System.EventHandler(this.lbxMusic_SelectedIndexChanged);
            // 
            // lblMusic
            // 
            this.lblMusic.AutoSize = true;
            this.lblMusic.Location = new System.Drawing.Point(358, 12);
            this.lblMusic.Name = "lblMusic";
            this.lblMusic.Size = new System.Drawing.Size(35, 13);
            this.lblMusic.TabIndex = 2;
            this.lblMusic.Text = "Music";
            // 
            // lblDarkness
            // 
            this.lblDarkness.AutoSize = true;
            this.lblDarkness.Location = new System.Drawing.Point(6, 144);
            this.lblDarkness.Name = "lblDarkness";
            this.lblDarkness.Size = new System.Drawing.Size(52, 13);
            this.lblDarkness.TabIndex = 9;
            this.lblDarkness.Text = "Darkness";
            // 
            // MapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 562);
            this.Controls.Add(this.tabMapOptions);
            this.Controls.Add(this.mnuMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnuMenu;
            this.Name = "MapEditor";
            this.Text = "Map Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MapEditor_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MapEditor_FormClosed);
            this.Load += new System.EventHandler(this.MapEditor_Load);
            this.mnuMenu.ResumeLayout(false);
            this.mnuMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTileset)).EndInit();
            this.tabMapOptions.ResumeLayout(false);
            this.tabTextures.ResumeLayout(false);
            this.tabTextures.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFrameLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTileset)).EndInit();
            this.tabProperties.ResumeLayout(false);
            this.tabProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimeLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDarkness)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.PictureBox picTileset;
        private System.Windows.Forms.HScrollBar hScroll;
        private System.Windows.Forms.VScrollBar vScroll;
        private System.Windows.Forms.CheckBox chkTexEyeDropper;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.TabControl tabMapOptions;
        private System.Windows.Forms.TabPage tabTextures;
        private System.Windows.Forms.Label lblTileset;
        private System.Windows.Forms.TrackBar tbTileset;
        private System.Windows.Forms.TabPage tabProperties;
        private System.Windows.Forms.Button btnReloadTiles;
        private System.Windows.Forms.Button btnReloadSongs;
        private System.Windows.Forms.Label lblMusic;
        private System.Windows.Forms.Label lblMapName;
        private System.Windows.Forms.ListBox lbxMusic;
        private System.Windows.Forms.ComboBox cbWeather;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMapName;
        private System.Windows.Forms.CheckBox chkIndoors;
        private System.Windows.Forms.Label lblTime;
        private IntNumericUpDown nudTimeLimit;
        private IntNumericUpDown nudDarkness;
        private System.Windows.Forms.Label lblDarkness;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resizeMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layersToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkFill;
        private System.Windows.Forms.Button btnRemoveFrame;
        private System.Windows.Forms.Button btnAddFrame;
        private System.Windows.Forms.Label lblFrames;
        private System.Windows.Forms.Label lblFrameLength;
        private IntNumericUpDown nudFrameLength;
        private System.Windows.Forms.Label lblTileInfo;
        private System.Windows.Forms.PictureBox picTile;
        private System.Windows.Forms.CheckBox chkAnimationMode;
        private TileAnimListBox lbxFrames;
        private System.Windows.Forms.ToolStripMenuItem clearLayerToolStripMenuItem;
    }
}