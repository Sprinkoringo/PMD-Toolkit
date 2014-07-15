using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PMDToolkit.Data;
using System.IO;

namespace PMDToolkit.Editors {
    public partial class ItemEditor : Form {

        const int TOTAL_PICS = 5;
        int itemNum;
        List<Image> items;
        int chosenPic;

        public ItemEditor() {
            InitializeComponent();

            Array arr = Enum.GetValues(typeof(Enums.ItemType));
            for (int i = 0; i < arr.Length; i++) {
                cbItemType.Items.Add(arr.GetValue(i));
            }
            cbItemType.SelectedIndex = 0;

            nudRarity.Minimum = 0;
            nudRarity.Maximum = 10;
            nudPrice.Minimum = 0;
            nudPrice.Maximum = Int32.MaxValue;

            picSprite.BackColor = Color.Black;

            int count = 0;
            items = new List<Image>();
            while(File.Exists(Paths.ItemsPath+count+".png")) {
                items.Add(new Bitmap(Paths.ItemsPath + count + ".png"));
                count++;
            }
            if (items.Count > TOTAL_PICS) {
                vsItemScroll.Maximum = items.Count - TOTAL_PICS;
            } else {
                vsItemScroll.Enabled = false;
            }

            nudRequirement.Minimum = -1;
            nudRequirement.Maximum = Int32.MaxValue;

            nudReqData1.Minimum = Int32.MinValue;
            nudReqData1.Maximum = Int32.MaxValue;
            nudReqData2.Minimum = Int32.MinValue;
            nudReqData2.Maximum = Int32.MaxValue;
            nudReqData3.Minimum = Int32.MinValue;
            nudReqData3.Maximum = Int32.MaxValue;
            nudReqData4.Minimum = Int32.MinValue;
            nudReqData4.Maximum = Int32.MaxValue;
            nudReqData5.Minimum = Int32.MinValue;
            nudReqData5.Maximum = Int32.MaxValue;

            nudEffect.Minimum = -1;
            nudEffect.Maximum = Int32.MaxValue;

            nudEffectData1.Minimum = Int32.MinValue;
            nudEffectData1.Maximum = Int32.MaxValue;
            nudEffectData2.Minimum = Int32.MinValue;
            nudEffectData2.Maximum = Int32.MaxValue;
            nudEffectData3.Minimum = Int32.MinValue;
            nudEffectData3.Maximum = Int32.MaxValue;


            nudThrowEffect.Minimum = -1;
            nudThrowEffect.Maximum = Int32.MaxValue;

            nudThrowData1.Minimum = Int32.MinValue;
            nudThrowData1.Maximum = Int32.MaxValue;
            nudThrowData2.Minimum = Int32.MinValue;
            nudThrowData2.Maximum = Int32.MaxValue;
            nudThrowData3.Minimum = Int32.MinValue;
            nudThrowData3.Maximum = Int32.MaxValue;
        }

        private void ItemEditor_Load(object sender, EventArgs e) {

        }

        public void LoadItem(int index) {
            itemNum = index;
            ItemEntry entry = GameData.ItemDex[index];

            txtName.Text = entry.Name;

            cbItemType.SelectedIndex = (int)entry.Type;
            nudRarity.Value = entry.Rarity;
            nudPrice.Value = entry.Price;
            txtDescription.Text = entry.Desc;

            chosenPic = entry.Sprite;
            RefreshPic();

            nudRequirement.Value = entry.Req;
            nudReqData1.Value = entry.Req1;
            nudReqData2.Value = entry.Req2;
            nudReqData3.Value = entry.Req3;
            nudReqData4.Value = entry.Req4;
            nudReqData5.Value = entry.Req5;

            nudEffect.Value = entry.Effect;
            nudEffectData1.Value = entry.Effect1;
            nudEffectData2.Value = entry.Effect2;
            nudEffectData3.Value = entry.Effect3;

            nudThrowEffect.Value = entry.ThrowEffect;
            nudThrowData1.Value = entry.Throw1;
            nudThrowData2.Value = entry.Throw2;
            nudThrowData3.Value = entry.Throw3;

        }

        public void SaveItem() {

            ItemEntry entry = new ItemEntry();
            entry.Name = txtName.Text;

            entry.Type = (Enums.ItemType)cbItemType.SelectedIndex;
            entry.Rarity = (int)nudRarity.Value;
            entry.Price = (int)nudPrice.Value;
            entry.Desc = txtDescription.Text;

            entry.Sprite = chosenPic;

            entry.Req = (int)nudRequirement.Value;
            entry.Req1 = (int)nudReqData1.Value;
            entry.Req2 = (int)nudReqData2.Value;
            entry.Req3 = (int)nudReqData3.Value;
            entry.Req4 = (int)nudReqData4.Value;
            entry.Req5 = (int)nudReqData5.Value;

            entry.Effect = (int)nudEffect.Value;
            entry.Effect1 = (int)nudEffectData1.Value;
            entry.Effect2 = (int)nudEffectData2.Value;
            entry.Effect3 = (int)nudEffectData3.Value;

            entry.ThrowEffect = (int)nudThrowEffect.Value;
            entry.Throw1 = (int)nudThrowData1.Value;
            entry.Throw2 = (int)nudThrowData2.Value;
            entry.Throw3 = (int)nudThrowData3.Value;

            GameData.ItemDex[itemNum] = entry;
            GameData.ItemDex[itemNum].Save(itemNum);
        }

        void RefreshPic() {
            Image endImage = new Bitmap(picSprite.Width, picSprite.Height);
            using (var graphics = System.Drawing.Graphics.FromImage(endImage)) {
                for (int i = 0; i <= TOTAL_PICS; i++) {
                    int picIndex = i + vsItemScroll.Value;
                    if (picIndex < items.Count) {
                        //blit at given location
                        graphics.DrawImage(items[picIndex], new Point((picSprite.Width - items[picIndex].Width) / 2, i * Graphics.TextureManager.TILE_SIZE));

                        //draw red square
                        if (chosenPic == picIndex) {
                            graphics.DrawRectangle(new Pen(Color.Red, 2), new Rectangle(0 + 1, i * Graphics.TextureManager.TILE_SIZE + 1,
                                Graphics.TextureManager.TILE_SIZE - 2, Graphics.TextureManager.TILE_SIZE - 2));
                        }
                    }
                }
            }
            picSprite.Image = endImage;
        }

        private void picSprite_Click(object sender, EventArgs e)
        {
            int picIndex = ((MouseEventArgs)e).Y / Graphics.TextureManager.TILE_SIZE + vsItemScroll.Value;
            if (picIndex < items.Count)
                chosenPic = picIndex;
            RefreshPic();
        }

        private void vsItemScroll_ValueChanged(object sender, EventArgs e) {
            RefreshPic();

        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e) {
            SaveItem();
            this.Close();
        }
    }
}
