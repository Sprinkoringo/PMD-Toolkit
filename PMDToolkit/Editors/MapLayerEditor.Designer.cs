namespace PMDToolkit.Editors
{
    partial class MapLayerEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapLayerEditor));
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.chklbLayers = new PMDToolkit.Editors.MapLayerListBox();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(14, 142);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(61, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(81, 142);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(61, 23);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Visible = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // chklbLayers
            // 
            this.chklbLayers.FormattingEnabled = true;
            this.chklbLayers.Location = new System.Drawing.Point(12, 12);
            this.chklbLayers.Name = "chklbLayers";
            this.chklbLayers.Size = new System.Drawing.Size(225, 124);
            this.chklbLayers.TabIndex = 3;
            this.chklbLayers.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chklbLayers_ItemCheck);
            this.chklbLayers.SelectedIndexChanged += new System.EventHandler(this.chklbLayers_SelectedIndexChanged);
            // 
            // MapLayerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 173);
            this.Controls.Add(this.chklbLayers);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MapLayerEditor";
            this.Text = "MapLayerEditor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MapLayerEditor_FormClosed);
            this.Load += new System.EventHandler(this.MapLayerEditor_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private MapLayerListBox chklbLayers;
    }
}