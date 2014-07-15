namespace PMDToolkit.Editors
{
    partial class MainPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPanel));
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkCoords = new System.Windows.Forms.CheckBox();
            this.chkGrid = new System.Windows.Forms.CheckBox();
            this.btnMapEditor = new System.Windows.Forms.Button();
            this.tabOptions = new System.Windows.Forms.TabControl();
            this.tabSprite = new System.Windows.Forms.TabPage();
            this.btnLoop = new System.Windows.Forms.Button();
            this.cbAnim = new System.Windows.Forms.ComboBox();
            this.cbGender = new System.Windows.Forms.ComboBox();
            this.cbShiny = new System.Windows.Forms.ComboBox();
            this.cbForme = new System.Windows.Forms.ComboBox();
            this.cbDexNum = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAnimate = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReloadSprites = new System.Windows.Forms.Button();
            this.chkShowSprites = new System.Windows.Forms.CheckBox();
            this.tabPath = new System.Windows.Forms.TabPage();
            this.btnDefaultPath = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSoundPath = new PMDToolkit.Editors.PathTextBox();
            this.txtMusicPath = new PMDToolkit.Editors.PathTextBox();
            this.txtItemPath = new PMDToolkit.Editors.PathTextBox();
            this.txtEffectPath = new PMDToolkit.Editors.PathTextBox();
            this.txtPortraitPath = new PMDToolkit.Editors.PathTextBox();
            this.txtSpritePath = new PMDToolkit.Editors.PathTextBox();
            this.txtTilePath = new PMDToolkit.Editors.PathTextBox();
            this.tabPage2.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.tabSprite.SuspendLayout();
            this.tabPath.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chkCoords);
            this.tabPage2.Controls.Add(this.chkGrid);
            this.tabPage2.Controls.Add(this.btnMapEditor);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(209, 257);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Map";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chkCoords
            // 
            this.chkCoords.AutoSize = true;
            this.chkCoords.Location = new System.Drawing.Point(6, 29);
            this.chkCoords.Name = "chkCoords";
            this.chkCoords.Size = new System.Drawing.Size(112, 17);
            this.chkCoords.TabIndex = 2;
            this.chkCoords.Text = "Show Coordinates";
            this.chkCoords.UseVisualStyleBackColor = true;
            this.chkCoords.CheckedChanged += new System.EventHandler(this.chkCoords_CheckedChanged);
            // 
            // chkGrid
            // 
            this.chkGrid.AutoSize = true;
            this.chkGrid.Location = new System.Drawing.Point(6, 6);
            this.chkGrid.Name = "chkGrid";
            this.chkGrid.Size = new System.Drawing.Size(75, 17);
            this.chkGrid.TabIndex = 1;
            this.chkGrid.Text = "Show Grid";
            this.chkGrid.UseVisualStyleBackColor = true;
            this.chkGrid.CheckedChanged += new System.EventHandler(this.chkGrid_CheckedChanged);
            // 
            // btnMapEditor
            // 
            this.btnMapEditor.Location = new System.Drawing.Point(6, 202);
            this.btnMapEditor.Name = "btnMapEditor";
            this.btnMapEditor.Size = new System.Drawing.Size(197, 49);
            this.btnMapEditor.TabIndex = 0;
            this.btnMapEditor.Text = "Open Map Editor";
            this.btnMapEditor.UseVisualStyleBackColor = true;
            this.btnMapEditor.Click += new System.EventHandler(this.btnMapEditor_Click);
            // 
            // tabOptions
            // 
            this.tabOptions.Controls.Add(this.tabPage2);
            this.tabOptions.Controls.Add(this.tabSprite);
            this.tabOptions.Controls.Add(this.tabPath);
            this.tabOptions.Location = new System.Drawing.Point(12, 13);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.SelectedIndex = 0;
            this.tabOptions.Size = new System.Drawing.Size(217, 283);
            this.tabOptions.TabIndex = 0;
            // 
            // tabSprite
            // 
            this.tabSprite.Controls.Add(this.btnLoop);
            this.tabSprite.Controls.Add(this.cbAnim);
            this.tabSprite.Controls.Add(this.cbGender);
            this.tabSprite.Controls.Add(this.cbShiny);
            this.tabSprite.Controls.Add(this.cbForme);
            this.tabSprite.Controls.Add(this.cbDexNum);
            this.tabSprite.Controls.Add(this.label5);
            this.tabSprite.Controls.Add(this.btnAnimate);
            this.tabSprite.Controls.Add(this.label4);
            this.tabSprite.Controls.Add(this.label3);
            this.tabSprite.Controls.Add(this.label2);
            this.tabSprite.Controls.Add(this.label1);
            this.tabSprite.Controls.Add(this.btnReloadSprites);
            this.tabSprite.Controls.Add(this.chkShowSprites);
            this.tabSprite.Location = new System.Drawing.Point(4, 22);
            this.tabSprite.Name = "tabSprite";
            this.tabSprite.Padding = new System.Windows.Forms.Padding(3);
            this.tabSprite.Size = new System.Drawing.Size(209, 257);
            this.tabSprite.TabIndex = 2;
            this.tabSprite.Text = "Sprite";
            this.tabSprite.UseVisualStyleBackColor = true;
            // 
            // btnLoop
            // 
            this.btnLoop.Location = new System.Drawing.Point(110, 225);
            this.btnLoop.Name = "btnLoop";
            this.btnLoop.Size = new System.Drawing.Size(88, 23);
            this.btnLoop.TabIndex = 13;
            this.btnLoop.Text = "Loop";
            this.btnLoop.UseVisualStyleBackColor = true;
            this.btnLoop.Click += new System.EventHandler(this.btnLoop_Click);
            // 
            // cbAnim
            // 
            this.cbAnim.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAnim.FormattingEnabled = true;
            this.cbAnim.Location = new System.Drawing.Point(10, 198);
            this.cbAnim.Name = "cbAnim";
            this.cbAnim.Size = new System.Drawing.Size(188, 21);
            this.cbAnim.TabIndex = 11;
            // 
            // cbGender
            // 
            this.cbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGender.FormattingEnabled = true;
            this.cbGender.Location = new System.Drawing.Point(110, 142);
            this.cbGender.Name = "cbGender";
            this.cbGender.Size = new System.Drawing.Size(88, 21);
            this.cbGender.TabIndex = 8;
            this.cbGender.SelectedValueChanged += new System.EventHandler(this.cbGender_SelectedIndexChanged);
            // 
            // cbShiny
            // 
            this.cbShiny.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbShiny.FormattingEnabled = true;
            this.cbShiny.Location = new System.Drawing.Point(10, 142);
            this.cbShiny.Name = "cbShiny";
            this.cbShiny.Size = new System.Drawing.Size(88, 21);
            this.cbShiny.TabIndex = 6;
            this.cbShiny.SelectedIndexChanged += new System.EventHandler(this.cbShiny_SelectedIndexChanged);
            // 
            // cbForme
            // 
            this.cbForme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbForme.FormattingEnabled = true;
            this.cbForme.Location = new System.Drawing.Point(10, 96);
            this.cbForme.Name = "cbForme";
            this.cbForme.Size = new System.Drawing.Size(188, 21);
            this.cbForme.TabIndex = 4;
            this.cbForme.SelectedIndexChanged += new System.EventHandler(this.cbForme_SelectedIndexChanged);
            // 
            // cbDexNum
            // 
            this.cbDexNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDexNum.FormattingEnabled = true;
            this.cbDexNum.Location = new System.Drawing.Point(10, 50);
            this.cbDexNum.Name = "cbDexNum";
            this.cbDexNum.Size = new System.Drawing.Size(188, 21);
            this.cbDexNum.TabIndex = 2;
            this.cbDexNum.SelectedIndexChanged += new System.EventHandler(this.cbDexNum_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 182);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Animation";
            // 
            // btnAnimate
            // 
            this.btnAnimate.Location = new System.Drawing.Point(9, 225);
            this.btnAnimate.Name = "btnAnimate";
            this.btnAnimate.Size = new System.Drawing.Size(89, 23);
            this.btnAnimate.TabIndex = 10;
            this.btnAnimate.Text = "Animate";
            this.btnAnimate.UseVisualStyleBackColor = true;
            this.btnAnimate.Click += new System.EventHandler(this.btnAnimate_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(110, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Gender";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Shiny";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Forme";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Pokedex Number";
            // 
            // btnReloadSprites
            // 
            this.btnReloadSprites.Location = new System.Drawing.Point(110, 4);
            this.btnReloadSprites.Name = "btnReloadSprites";
            this.btnReloadSprites.Size = new System.Drawing.Size(88, 23);
            this.btnReloadSprites.TabIndex = 1;
            this.btnReloadSprites.Text = "Reload Sprites";
            this.btnReloadSprites.UseVisualStyleBackColor = true;
            this.btnReloadSprites.Click += new System.EventHandler(this.btnReloadSprites_Click);
            // 
            // chkShowSprites
            // 
            this.chkShowSprites.AutoSize = true;
            this.chkShowSprites.Location = new System.Drawing.Point(10, 8);
            this.chkShowSprites.Name = "chkShowSprites";
            this.chkShowSprites.Size = new System.Drawing.Size(88, 17);
            this.chkShowSprites.TabIndex = 0;
            this.chkShowSprites.Text = "Show Sprites";
            this.chkShowSprites.UseVisualStyleBackColor = true;
            this.chkShowSprites.CheckedChanged += new System.EventHandler(this.chkShowSprites_CheckedChanged);
            // 
            // tabPath
            // 
            this.tabPath.Controls.Add(this.txtSoundPath);
            this.tabPath.Controls.Add(this.txtMusicPath);
            this.tabPath.Controls.Add(this.txtItemPath);
            this.tabPath.Controls.Add(this.txtEffectPath);
            this.tabPath.Controls.Add(this.txtPortraitPath);
            this.tabPath.Controls.Add(this.txtSpritePath);
            this.tabPath.Controls.Add(this.txtTilePath);
            this.tabPath.Controls.Add(this.btnDefaultPath);
            this.tabPath.Controls.Add(this.label12);
            this.tabPath.Controls.Add(this.label11);
            this.tabPath.Controls.Add(this.label10);
            this.tabPath.Controls.Add(this.label9);
            this.tabPath.Controls.Add(this.label8);
            this.tabPath.Controls.Add(this.label7);
            this.tabPath.Controls.Add(this.label6);
            this.tabPath.Location = new System.Drawing.Point(4, 22);
            this.tabPath.Name = "tabPath";
            this.tabPath.Padding = new System.Windows.Forms.Padding(3);
            this.tabPath.Size = new System.Drawing.Size(209, 257);
            this.tabPath.TabIndex = 3;
            this.tabPath.Text = "Folders";
            this.tabPath.UseVisualStyleBackColor = true;
            // 
            // btnDefaultPath
            // 
            this.btnDefaultPath.Location = new System.Drawing.Point(111, 228);
            this.btnDefaultPath.Name = "btnDefaultPath";
            this.btnDefaultPath.Size = new System.Drawing.Size(92, 23);
            this.btnDefaultPath.TabIndex = 20;
            this.btnDefaultPath.Text = "Reset Defaults";
            this.btnDefaultPath.UseVisualStyleBackColor = true;
            this.btnDefaultPath.Click += new System.EventHandler(this.btnDefaultPath_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 193);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(46, 13);
            this.label12.TabIndex = 18;
            this.label12.Text = "Sounds:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 163);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "Music:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 133);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Items:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 103);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Effects:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 73);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Portraits:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Sprites:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Tiles:";
            // 
            // txtSoundPath
            // 
            this.txtSoundPath.BackColor = System.Drawing.Color.White;
            this.txtSoundPath.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtSoundPath.Location = new System.Drawing.Point(60, 190);
            this.txtSoundPath.Name = "txtSoundPath";
            this.txtSoundPath.ReadOnly = true;
            this.txtSoundPath.Size = new System.Drawing.Size(143, 20);
            this.txtSoundPath.TabIndex = 27;
            this.txtSoundPath.Click += new System.EventHandler(this.txtSoundPath_Click);
            // 
            // txtMusicPath
            // 
            this.txtMusicPath.BackColor = System.Drawing.Color.White;
            this.txtMusicPath.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtMusicPath.Location = new System.Drawing.Point(60, 160);
            this.txtMusicPath.Name = "txtMusicPath";
            this.txtMusicPath.ReadOnly = true;
            this.txtMusicPath.Size = new System.Drawing.Size(143, 20);
            this.txtMusicPath.TabIndex = 26;
            this.txtMusicPath.Click += new System.EventHandler(this.txtMusicPath_Click);
            // 
            // txtItemPath
            // 
            this.txtItemPath.BackColor = System.Drawing.Color.White;
            this.txtItemPath.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtItemPath.Location = new System.Drawing.Point(60, 130);
            this.txtItemPath.Name = "txtItemPath";
            this.txtItemPath.ReadOnly = true;
            this.txtItemPath.Size = new System.Drawing.Size(143, 20);
            this.txtItemPath.TabIndex = 25;
            this.txtItemPath.Click += new System.EventHandler(this.txtItemPath_Click);
            // 
            // txtEffectPath
            // 
            this.txtEffectPath.BackColor = System.Drawing.Color.White;
            this.txtEffectPath.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtEffectPath.Location = new System.Drawing.Point(60, 100);
            this.txtEffectPath.Name = "txtEffectPath";
            this.txtEffectPath.ReadOnly = true;
            this.txtEffectPath.Size = new System.Drawing.Size(143, 20);
            this.txtEffectPath.TabIndex = 24;
            this.txtEffectPath.Click += new System.EventHandler(this.txtEffectPath_Click);
            // 
            // txtPortraitPath
            // 
            this.txtPortraitPath.BackColor = System.Drawing.Color.White;
            this.txtPortraitPath.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtPortraitPath.Location = new System.Drawing.Point(60, 70);
            this.txtPortraitPath.Name = "txtPortraitPath";
            this.txtPortraitPath.ReadOnly = true;
            this.txtPortraitPath.Size = new System.Drawing.Size(143, 20);
            this.txtPortraitPath.TabIndex = 23;
            this.txtPortraitPath.Click += new System.EventHandler(this.txtPortraitPath_Click);
            // 
            // txtSpritePath
            // 
            this.txtSpritePath.BackColor = System.Drawing.Color.White;
            this.txtSpritePath.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtSpritePath.Location = new System.Drawing.Point(60, 40);
            this.txtSpritePath.Name = "txtSpritePath";
            this.txtSpritePath.ReadOnly = true;
            this.txtSpritePath.Size = new System.Drawing.Size(143, 20);
            this.txtSpritePath.TabIndex = 22;
            this.txtSpritePath.Click += new System.EventHandler(this.txtSpritePath_Click);
            // 
            // txtTilePath
            // 
            this.txtTilePath.BackColor = System.Drawing.Color.White;
            this.txtTilePath.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtTilePath.Location = new System.Drawing.Point(60, 10);
            this.txtTilePath.Name = "txtTilePath";
            this.txtTilePath.ReadOnly = true;
            this.txtTilePath.Size = new System.Drawing.Size(143, 20);
            this.txtTilePath.TabIndex = 21;
            this.txtTilePath.Click += new System.EventHandler(this.txtTilePath_Click);
            // 
            // MainPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(241, 308);
            this.Controls.Add(this.tabOptions);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainPanel";
            this.Text = "Control Panel";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainPanel_FormClosed);
            this.Load += new System.EventHandler(this.MainPanel_Load);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabOptions.ResumeLayout(false);
            this.tabSprite.ResumeLayout(false);
            this.tabSprite.PerformLayout();
            this.tabPath.ResumeLayout(false);
            this.tabPath.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox chkCoords;
        private System.Windows.Forms.CheckBox chkGrid;
        private System.Windows.Forms.Button btnMapEditor;
        private System.Windows.Forms.TabControl tabOptions;
        private System.Windows.Forms.TabPage tabSprite;
        private System.Windows.Forms.ComboBox cbAnim;
        private System.Windows.Forms.ComboBox cbGender;
        private System.Windows.Forms.ComboBox cbShiny;
        private System.Windows.Forms.ComboBox cbForme;
        private System.Windows.Forms.ComboBox cbDexNum;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAnimate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReloadSprites;
        private System.Windows.Forms.CheckBox chkShowSprites;
        private System.Windows.Forms.Button btnLoop;
        private System.Windows.Forms.TabPage tabPath;
        private System.Windows.Forms.Button btnDefaultPath;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private PathTextBox txtSoundPath;
        private PathTextBox txtMusicPath;
        private PathTextBox txtItemPath;
        private PathTextBox txtEffectPath;
        private PathTextBox txtPortraitPath;
        private PathTextBox txtSpritePath;
        private PathTextBox txtTilePath;

    }
}