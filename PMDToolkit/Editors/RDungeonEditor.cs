using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PMDToolkit.Data;

namespace PMDToolkit.Editors {
    public partial class RDungeonEditor : Form {

        int dungeonNum;

        public RDungeonEditor() {
            InitializeComponent();
        }


        public void LoadRDungeon(int index) {
            dungeonNum = index;
            RDungeonEntry entry = GameData.RDungeonDex[index];
        }


        public void SaveRDungeon() {

        }
    }
}
