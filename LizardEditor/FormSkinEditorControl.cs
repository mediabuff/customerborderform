using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Lizard.Windows;
using Lizard.Windows.Skin;

namespace LizardEditor
{
    [ToolboxItem(false)]
    public partial class LizardSkinEditorControl : UserControl
    {
        #region Variables

        private string _fileName;
        private SkinnedForm _owningForm;
        private FormSkin _activeSkin;
        private bool _isDirty;

        #endregion

        #region Constructor

        public LizardSkinEditorControl()
        {
            InitializeComponent();

            UpdateSkinList();
            SkinManager.SkinChanged += new EventHandler(SkinManager_SkinChanged);
        }

        #endregion

        #region FormSkinEditorControl events

        private void LizardSkinEditorControl_Load(object sender, EventArgs e)
        {
            msgEmptyLibrary.Links.Clear();
            msgEmptyLibrary.Links.Add(new LinkLabel.Link(56, 12, "Add"));
            msgEmptyLibrary.Links.Add(new LinkLabel.Link(72, 27, "Open"));
            msgEmptyLibrary.Dock = DockStyle.Fill;
        }

        #endregion

        #region Tool buttons events...

        private void toolNew_Click(object sender, EventArgs e)
        {
            NewSkin();
        }

        private void toolOpen_Click(object sender, EventArgs e)
        {
            OpenSkinLibrary();
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            SaveSkinLibrary();
        }

        private void toolSkinList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActiveSkin = SkinManager.GetSkin((string)toolSkinList.SelectedItem);
        }

        private void toolAddSkin_Click(object sender, EventArgs e)
        {
            FormSkin style = SkinManager.AddNewSkin();
            toolSkinList.SelectedItem = style.Name;
        }

        private void toolDeleteSkin_Click(object sender, EventArgs e)
        {
            if (ActiveSkin != null)
                SkinManager.DeleteSkin(ActiveSkin);
        }

        #endregion

        #region SkinManager events

        internal void SkinManager_SkinChanged(object sender, EventArgs e)
        {
            UpdateSkinList();
        }

        #endregion

        #region ActiveSkin events

        void ActiveSkin_ChildPropertyChanged(object sender, ChildPropertyChangedEventArgs args)
        {
            _isDirty = true;

            if (args.SubObject == ActiveSkin && args.PropertyName == FormSkinProperty.Name)
                UpdateSkinList();
        }

        #endregion

        #region Property grid events

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label != "Key")
                return;

            //---- Change the skin key
            CaptionButtonSkin skin = treeView.SelectedNode.Tag as CaptionButtonSkin;
            skin.Key = e.ChangedItem.Value.ToString();
            treeView.SelectedNode.Text = skin.Key;
        }

        #endregion

        #region Properties

        public string FileName
        {
            get { return _fileName; }
            private set { _fileName = value; }
        }

        public SkinnedForm OwningForm
        {
            get { return _owningForm; }
            set
            {
                if (_owningForm != value)
                {
                    _owningForm = value;

                    //if (_owningForm != null)
                    //{
                    //    ActiveStyle = _owningForm.FormStyle;
                    //}
                }
            }
        }

        public FormSkin ActiveSkin
        {
            get { return _activeSkin; }
            set
            {
                if (_activeSkin != value)
                {
                    if (_activeSkin != null)
                        _activeSkin.ChildPropertyChanged -= new ChildPropertyChangedEventHandler(ActiveSkin_ChildPropertyChanged);

                    _activeSkin = value;

                    if (_activeSkin != null)
                        _activeSkin.ChildPropertyChanged += new ChildPropertyChangedEventHandler(ActiveSkin_ChildPropertyChanged);

                    ClearDirtyFlag();
                    UptadeTreeView();
                    EnableTools();

                    //if (OwningForm != null && OwningForm.FormStyle != _activeStyle)
                    //{
                    //    OwningForm.FormStyle = _activeStyle;
                    //}
                }
            }
        }

        public bool IsDirty
        {
            get { return _isDirty; }
        }

        #endregion

        #region ClearDirtyFlag

        public void ClearDirtyFlag()
        {
            _isDirty = false;
        }

        #endregion

        #region NewSkin

        private void NewSkin()
        {
            SkinManager.Reset();
        }

        #endregion

        #region UpdateSkinList

        private void UpdateSkinList()
        {
            object selected = toolSkinList.SelectedItem;
            toolSkinList.Items.Clear();

            string[] skins = SkinManager.GetSkinNames();
            toolSkinList.Items.AddRange(skins);

            if (toolSkinList.Items.Count > 0)
            {

                if (selected != null)
                    toolSkinList.SelectedItem = selected;

                if (toolSkinList.SelectedIndex < 0)
                    toolSkinList.SelectedIndex = 0;

            }
            EnableTools();
        }

        #endregion

        #region Load / Save SkinLibrary

        private void OpenSkinLibrary()
        {
            openFileDialog.FileName = FileName;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SkinManager.Load(openFileDialog.FileName);
                    FileName = openFileDialog.FileName;
                }
                catch (InvalidVersionException)
                {
                    if (MessageBox.Show(this.ParentForm, "The form skin library you are trying to load uses an older file format.\nDo you want to update it to the current version?",
                        "Invalid version", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        SkinManager.Update(openFileDialog.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void SaveSkinLibrary()
        {
            saveFileDialog.FileName = FileName;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SkinManager.Save(saveFileDialog.FileName);
                    ClearDirtyFlag();
                    FileName = saveFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        #endregion

        #region EnableTools

        private void EnableTools()
        {
            bool styleListEmpty = toolSkinList.Items.Count == 0;

            toolSave.Enabled = !styleListEmpty;
            toolSkinList.Enabled = !styleListEmpty;
            msgEmptyLibrary.Visible = styleListEmpty;
            splitContainer.Visible = !styleListEmpty;

            bool activeStyleSet = ActiveSkin != null;

            toolDeleteSkin.Enabled = activeStyleSet;
        }

        #endregion

        #region UptadeTreeView

        private void UptadeTreeView()
        {
            treeView.Nodes.Clear();

            if (ActiveSkin != null)
                treeView.Nodes.Add(MapFormSkinToTreeNode(ActiveSkin,
                    String.IsNullOrEmpty(ActiveSkin.Name) ? "Skin" : ActiveSkin.Name));
            //if (OwningForm != null)
            //    treeView.Nodes.Add(MapFormToTreeNode(OwningForm, "Form"));

            treeView.ExpandAll();

            if (treeView.Nodes.Count > 0)
                treeView.SelectedNode = treeView.Nodes[0];
        }

        #endregion

        #region Map...

        private TreeNode MapFormToTreeNode(SkinnedForm form, string propertyName)
        {
            TreeNode node = new TreeNode(propertyName);
            node.Tag = form;
            return node;
        }

        TreeNode MapFormSkinToTreeNode(FormSkin formSkin, string propertyName)
        {
            TreeNode node = new TreeNode(propertyName);
            node.Tag = formSkin;

            node.Nodes.Add(MapSerializableImageToTreeNode(formSkin.NormalState, FormSkinProperty.NormalState));

            //---- Caption skins
            foreach (CaptionButtonSkin skin in formSkin.CaptionButtonSkins)
                node.Nodes.Add(MapGenericCaptionButtonSkinToTreeNode(skin));

            return node;
        }

        private TreeNode MapSerializableImageToTreeNode(SerializableImage serializableImage, string propertyName)
        {
            TreeNode node = new TreeNode(propertyName);
            node.Tag = serializableImage;
            return node;
        }

        // OLD ONE -> replace with generic button
        private TreeNode MapCaptionButtonSkinToTreeNode(CaptionButtonSkin formButtonStyle, string propertyName)
        {
            TreeNode node = new TreeNode(propertyName);
            node.Tag = formButtonStyle;
            node.Nodes.Add(MapSerializableImageToTreeNode(formButtonStyle.NormalState, CaptionButtonSkinProperty.NormalState));
            node.Nodes.Add(MapSerializableImageToTreeNode(formButtonStyle.HoverState, CaptionButtonSkinProperty.HoverState));
            node.Nodes.Add(MapSerializableImageToTreeNode(formButtonStyle.ActiveState, CaptionButtonSkinProperty.ActiveState));
            node.Nodes.Add(MapSerializableImageToTreeNode(formButtonStyle.DisabledState, CaptionButtonSkinProperty.DisabledState));
            return node;
        }

        private TreeNode MapGenericCaptionButtonSkinToTreeNode(CaptionButtonSkin skin)
        {
            TreeNode node = new TreeNode(skin.Key);
            node.Tag = skin;
            node.Nodes.Add(MapSerializableImageToTreeNode(skin.NormalState, CaptionButtonSkinProperty.NormalState));
            node.Nodes.Add(MapSerializableImageToTreeNode(skin.HoverState, CaptionButtonSkinProperty.HoverState));
            node.Nodes.Add(MapSerializableImageToTreeNode(skin.ActiveState, CaptionButtonSkinProperty.ActiveState));
            node.Nodes.Add(MapSerializableImageToTreeNode(skin.DisabledState, CaptionButtonSkinProperty.DisabledState));
            return node;
        }

        #endregion

        #region Tree view events

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null)
            {
                propertyGrid.SelectedObject = null;
                return;
            }

            propertyGrid.SelectedObject = e.Node.Tag;

            // Default
            toolAddButton.Enabled = false;
            toolDeleteButton.Enabled = false;

            // Caption button
            if (e.Node.Tag is Lizard.Windows.Skin.CaptionButtonSkin)
            {
                toolAddButton.Enabled = false;
                toolDeleteButton.Enabled = true;

                Lizard.Windows.Skin.CaptionButtonSkin button = e.Node.Tag as Lizard.Windows.Skin.CaptionButtonSkin;
                if (button.Key == "MaximizeButton" ||
                    button.Key == "MinimizeButton" ||
                    button.Key == "RestoreButton" ||
                    button.Key == "CloseButton" ||
                    button.Key == "HelpButton")
                    toolDeleteButton.Enabled = false;
            }

            // Root
            if (e.Node.Tag is Lizard.Windows.Skin.FormSkin)
            {
                toolAddButton.Enabled = true;
                toolDeleteButton.Enabled = false;
            }

        }

        #endregion

        #region msgEmptyLibrary events

        private void msgEmptyLibrary_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if ((string)e.Link.LinkData == "Add")
                toolAddSkin.PerformClick();
            else if ((string)e.Link.LinkData == "Open")
                toolOpen.PerformClick();
        }

        #endregion

        #region Add/remove button's skin

        private void toolAddButton_Click(object sender, EventArgs e)
        {
            CaptionButtonSkin skin = new CaptionButtonSkin();
            ActiveSkin.AddCaptionButtonSkin(skin);

            treeView.Nodes[0].Nodes.Add(MapGenericCaptionButtonSkinToTreeNode(skin));
        }

        private void toolDeleteButton_Click(object sender, EventArgs e)
        {
            Lizard.Windows.Skin.CaptionButtonSkin skin = treeView.SelectedNode.Tag as Lizard.Windows.Skin.CaptionButtonSkin;
            ActiveSkin.RemoveCaptionButtonSkin(skin);
            treeView.Nodes[0].Nodes.Remove(treeView.SelectedNode);
            treeView.SelectedNode = treeView.Nodes[0];
        }

        #endregion
    }
}
