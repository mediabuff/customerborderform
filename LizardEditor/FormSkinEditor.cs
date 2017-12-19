using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

using Lizard.Windows;

namespace LizardEditor
{
    public partial class LizardSkinEditor : Component
    {
        #region Variables
        
        Form editorForm;

        #endregion

        #region Constructor

        public LizardSkinEditor()
        {
            InitializeComponent();
        }

        public LizardSkinEditor(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region ShowFormEditor

        public void ShowSkinEditor(SkinnedForm owningForm)
        {
            LizardSkinEditorControl editor;
            if (editorForm == null || editorForm.IsDisposed)
            {
                editorForm = new Form();
                components.Add(editorForm);
                editorForm.Text = "Form Style Editor";
               // editorForm.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                editorForm.Size = new System.Drawing.Size(700, 400);
                editor = new LizardSkinEditorControl();
                editor.Dock = DockStyle.Fill;
                editorForm.Controls.Add(editor);
                editorForm.FormClosing += new FormClosingEventHandler(editorForm_FormClosing);
                editorForm.FormClosed += new FormClosedEventHandler(editorForm_FormClosed);
            }
            else
            {
                editor = (LizardSkinEditorControl)editorForm.Controls[0];
            }
            editor.OwningForm = owningForm;
            editorForm.Show();
        }

        #endregion

        #region Editor form's events

        void editorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            editorForm.FormClosing -= new FormClosingEventHandler(editorForm_FormClosing);
            editorForm.FormClosed -= new FormClosedEventHandler(editorForm_FormClosed);
            editorForm.Dispose();
            editorForm = null;
        }

        void editorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            LizardSkinEditorControl editor = (LizardSkinEditorControl)editorForm.Controls[0];
            if (editor.IsDirty)
            {
                DialogResult result = MessageBox.Show(editorForm,
                    "Current style contains unsaved changed. Do you want to close anyway?",
                    editorForm.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                e.Cancel = (result == DialogResult.No);
            }
        }

        #endregion
    }
}
