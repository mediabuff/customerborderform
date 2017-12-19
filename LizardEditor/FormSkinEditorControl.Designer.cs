namespace LizardEditor
{
    partial class LizardSkinEditorControl
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
            if (disposing)
            {
                Lizard.Windows.Skin.SkinManager.SkinChanged -= new System.EventHandler(SkinManager_SkinChanged);
                
                if (components != null)
                 components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LizardSkinEditorControl));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.treeView = new System.Windows.Forms.TreeView();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.toolSkinLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolSkinList = new System.Windows.Forms.ToolStripComboBox();
            this.toolAddSkin = new System.Windows.Forms.ToolStripButton();
            this.toolDeleteSkin = new System.Windows.Forms.ToolStripButton();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolNew = new System.Windows.Forms.ToolStripButton();
            this.toolOpen = new System.Windows.Forms.ToolStripButton();
            this.toolSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolAddButton = new System.Windows.Forms.ToolStripButton();
            this.toolDeleteButton = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.msgEmptyLibrary = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 26);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.treeView);
            this.splitContainer.Panel1.Padding = new System.Windows.Forms.Padding(3, 3, 0, 3);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.propertyGrid);
            this.splitContainer.Panel2.Padding = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.splitContainer.Size = new System.Drawing.Size(705, 319);
            this.splitContainer.SplitterDistance = 257;
            this.splitContainer.TabIndex = 0;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.HideSelection = false;
            this.treeView.HotTracking = true;
            this.treeView.Location = new System.Drawing.Point(3, 3);
            this.treeView.Name = "treeView";
            this.treeView.ShowRootLines = false;
            this.treeView.Size = new System.Drawing.Size(254, 313);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(441, 313);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // toolSkinLabel
            // 
            this.toolSkinLabel.Name = "toolSkinLabel";
            this.toolSkinLabel.Size = new System.Drawing.Size(63, 22);
            this.toolSkinLabel.Text = "Active Skin:";
            // 
            // toolSkinList
            // 
            this.toolSkinList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolSkinList.Name = "toolSkinList";
            this.toolSkinList.Size = new System.Drawing.Size(100, 25);
            this.toolSkinList.SelectedIndexChanged += new System.EventHandler(this.toolSkinList_SelectedIndexChanged);
            // 
            // toolAddSkin
            // 
            this.toolAddSkin.Image = ((System.Drawing.Image)(resources.GetObject("toolAddSkin.Image")));
            this.toolAddSkin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolAddSkin.Name = "toolAddSkin";
            this.toolAddSkin.Size = new System.Drawing.Size(68, 22);
            this.toolAddSkin.Text = "Add Skin";
            this.toolAddSkin.Click += new System.EventHandler(this.toolAddSkin_Click);
            // 
            // toolDeleteSkin
            // 
            this.toolDeleteSkin.Image = ((System.Drawing.Image)(resources.GetObject("toolDeleteSkin.Image")));
            this.toolDeleteSkin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolDeleteSkin.Name = "toolDeleteSkin";
            this.toolDeleteSkin.Size = new System.Drawing.Size(80, 22);
            this.toolDeleteSkin.Text = "Delete Skin";
            this.toolDeleteSkin.Click += new System.EventHandler(this.toolDeleteSkin_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolNew,
            this.toolOpen,
            this.toolSave,
            this.toolStripButton1,
            this.toolSkinLabel,
            this.toolSkinList,
            this.toolAddSkin,
            this.toolDeleteSkin,
            this.toolStripSeparator1,
            this.toolAddButton,
            this.toolDeleteButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip.Size = new System.Drawing.Size(705, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolNew
            // 
            this.toolNew.Image = ((System.Drawing.Image)(resources.GetObject("toolNew.Image")));
            this.toolNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolNew.Name = "toolNew";
            this.toolNew.Size = new System.Drawing.Size(48, 22);
            this.toolNew.Text = "&New";
            this.toolNew.Click += new System.EventHandler(this.toolNew_Click);
            // 
            // toolOpen
            // 
            this.toolOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolOpen.Image")));
            this.toolOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolOpen.Name = "toolOpen";
            this.toolOpen.Size = new System.Drawing.Size(53, 22);
            this.toolOpen.Text = "&Open";
            this.toolOpen.Click += new System.EventHandler(this.toolOpen_Click);
            // 
            // toolSave
            // 
            this.toolSave.Image = ((System.Drawing.Image)(resources.GetObject("toolSave.Image")));
            this.toolSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSave.Name = "toolSave";
            this.toolSave.Size = new System.Drawing.Size(51, 22);
            this.toolSave.Text = "&Save";
            this.toolSave.Click += new System.EventHandler(this.toolSave_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolAddButton
            // 
            this.toolAddButton.Enabled = false;
            this.toolAddButton.Image = ((System.Drawing.Image)(resources.GetObject("toolAddButton.Image")));
            this.toolAddButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolAddButton.Name = "toolAddButton";
            this.toolAddButton.Size = new System.Drawing.Size(81, 22);
            this.toolAddButton.Text = "Add button";
            this.toolAddButton.Click += new System.EventHandler(this.toolAddButton_Click);
            // 
            // toolDeleteButton
            // 
            this.toolDeleteButton.Enabled = false;
            this.toolDeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("toolDeleteButton.Image")));
            this.toolDeleteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolDeleteButton.Name = "toolDeleteButton";
            this.toolDeleteButton.Size = new System.Drawing.Size(93, 22);
            this.toolDeleteButton.Text = "Delete button";
            this.toolDeleteButton.Click += new System.EventHandler(this.toolDeleteButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "*.fsl";
            this.openFileDialog.Filter = "Form Style Library (*.fsl)|*.fsl|All files (*.*)|*.*";
            this.openFileDialog.Title = "Open Form Style Library";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "*.fsl";
            this.saveFileDialog.FileName = "FormStyle.fsl";
            this.saveFileDialog.Filter = "Form Style Library (*.fsl)|*.fsl|All files (*.*)|*.*";
            this.saveFileDialog.Title = "Save Form Style Library";
            // 
            // msgEmptyLibrary
            // 
            this.msgEmptyLibrary.LinkArea = new System.Windows.Forms.LinkArea(72, 4);
            this.msgEmptyLibrary.Location = new System.Drawing.Point(59, 115);
            this.msgEmptyLibrary.Name = "msgEmptyLibrary";
            this.msgEmptyLibrary.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.msgEmptyLibrary.Size = new System.Drawing.Size(321, 69);
            this.msgEmptyLibrary.TabIndex = 1;
            this.msgEmptyLibrary.TabStop = true;
            this.msgEmptyLibrary.Text = "Currently there are no skins defined in SkinsManager. \r\nAdd new skin or open exis" +
                "ting skins library.";
            this.msgEmptyLibrary.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.msgEmptyLibrary.UseCompatibleTextRendering = true;
            this.msgEmptyLibrary.Visible = false;
            this.msgEmptyLibrary.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.msgEmptyLibrary_LinkClicked);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(705, 1);
            this.label1.TabIndex = 1;
            // 
            // LizardSkinEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.msgEmptyLibrary);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolStrip);
            this.Name = "LizardSkinEditorControl";
            this.Size = new System.Drawing.Size(705, 345);
            this.Load += new System.EventHandler(this.LizardSkinEditorControl_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolNew;
        private System.Windows.Forms.ToolStripButton toolOpen;
        private System.Windows.Forms.ToolStripButton toolSave;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripComboBox toolSkinList;
        private System.Windows.Forms.ToolStripButton toolAddSkin;
        private System.Windows.Forms.ToolStripButton toolDeleteSkin;
        private System.Windows.Forms.ToolStripLabel toolSkinLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripButton1;
        private System.Windows.Forms.LinkLabel msgEmptyLibrary;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolAddButton;
        private System.Windows.Forms.ToolStripButton toolDeleteButton;

    }
}
