#region using...

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using Lizard.Windows;
using Lizard.Windows.Skin;

#endregion

namespace Samples.CustomCaption
{
    [System.ComponentModel.DesignerCategory("form")]
    partial class DemoForm : VistaForm
    {
        #region Constructor

        public DemoForm()
        {
            InitializeComponent();

            chkEnableNCPaint.Checked = this.EnableNonClientAreaPaint;
            chkUseStyleManager.Checked = this.UseFormSkinManager;

            AddCustomCaption();
        }

        #endregion

        #region AddCustomCaption

        private void AddCustomCaption()
        {
            // Setup tray button
            CustomCaptionButton customButton = new CustomCaptionButton();
            customButton.Key = "TrayButton";

            customButton.Click += new EventHandler(trayButton_Click);

            CaptionButtons.Add(customButton);
        }

        #endregion

        #region trayButton_Click

        void trayButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tray button has been pressed");
        }

        #endregion

        #region Events...

        private void chkEnableNCPaint_CheckedChanged(object sender, EventArgs e)
        {
            this.EnableNonClientAreaPaint = chkEnableNCPaint.Checked;
        }

        private void chkUseStyleManager_CheckedChanged(object sender, EventArgs e)
        {
            this.UseFormSkinManager = chkUseStyleManager.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void linkStyleEditor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            formStyleEditor1.ShowSkinEditor(this);
        }

        #endregion
    }
}