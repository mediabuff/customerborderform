#region Custom Border Forms - Copyright (C) 2005 Szymon Kobalczyk

// Custom Border Forms
// Copyright (C) 2005 Szymon Kobalczyk
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.

// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//
// Szymon Kobalczyk (http://www.geekswithblogs.com/kobush)

#endregion

#region using...

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Lizard.Drawing;
using Lizard.Windows;
using Lizard.Windows.Skin;

#endregion

namespace Samples.Demo
{
    public class LonghornForm : SkinnedForm
    {
        #region Constructor

        public LonghornForm()
        {
            this.FormSkin = CreateFormskin();
        }

        #endregion

        #region OnResize

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            int diam = 10;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, diam, diam, -90, -90);
            path.AddLines(new Point[] {new Point(0,diam), new Point(0, Height), 
                new Point(Width, Height), new Point(Width, diam)});
            path.AddArc(Width - diam, 0, diam, diam, 0, -90);
            path.CloseFigure();

            this.Region = new Region(path);
        }

        #endregion

        #region CreateFormskin

        FormSkin CreateFormskin()
        {
            FormSkin skin = new FormSkin(true);
            skin.NormalState.Image = Properties.Resources.Border;
            skin.NormalState.SizeMode = ImageSizeMode.Stretched;
            skin.NormalState.StretchMargins = new Padding(3, 23, 3, 3);

            skin.GetCaptionButtonSkin("CloseButton").Size = Properties.Resources.Close.Size;
            skin.GetCaptionButtonSkin("CloseButton").Margin = new Padding(1, 5, 5, 0);
            skin.GetCaptionButtonSkin("CloseButton").NormalState.Image = Properties.Resources.Close;
            skin.GetCaptionButtonSkin("CloseButton").DisabledState.Image = Properties.Resources.CloseDisabled;
            skin.GetCaptionButtonSkin("CloseButton").ActiveState.Image = Properties.Resources.ClosePressed;
            skin.GetCaptionButtonSkin("CloseButton").HoverState.Image = Properties.Resources.CloseHot;

            skin.GetCaptionButtonSkin("MaximizeButton").Size = Properties.Resources.Maximize.Size;
            skin.GetCaptionButtonSkin("MaximizeButton").Margin = new Padding(1, 5, 1, 0);
            skin.GetCaptionButtonSkin("MaximizeButton").NormalState.Image = Properties.Resources.Maximize;
            skin.GetCaptionButtonSkin("MaximizeButton").DisabledState.Image = Properties.Resources.MaximizeDisabled;
            skin.GetCaptionButtonSkin("MaximizeButton").ActiveState.Image = Properties.Resources.MaximizePressed;
            skin.GetCaptionButtonSkin("MaximizeButton").HoverState.Image = Properties.Resources.MaximizeHot;

            skin.GetCaptionButtonSkin("MinimizeButton").Size = Properties.Resources.Minimize.Size;
            skin.GetCaptionButtonSkin("MinimizeButton").Margin = new Padding(1, 5, 1, 0);
            skin.GetCaptionButtonSkin("MinimizeButton").NormalState.Image = Properties.Resources.Minimize;
            skin.GetCaptionButtonSkin("MinimizeButton").DisabledState.Image = Properties.Resources.MinimizeDisabled;
            skin.GetCaptionButtonSkin("MinimizeButton").ActiveState.Image = Properties.Resources.MinimizePressed;
            skin.GetCaptionButtonSkin("MinimizeButton").HoverState.Image = Properties.Resources.MinimizeHot;

            skin.GetCaptionButtonSkin("RestoreButton").Size = Properties.Resources.Restore.Size;
            skin.GetCaptionButtonSkin("RestoreButton").Margin = new Padding(1, 5, 1, 0);
            skin.GetCaptionButtonSkin("RestoreButton").NormalState.Image = Properties.Resources.Restore;
            skin.GetCaptionButtonSkin("RestoreButton").DisabledState.Image = Properties.Resources.RestoreDisabled;
            skin.GetCaptionButtonSkin("RestoreButton").ActiveState.Image = Properties.Resources.RestorePressed;
            skin.GetCaptionButtonSkin("RestoreButton").HoverState.Image = Properties.Resources.RestoreHot;

            skin.GetCaptionButtonSkin("HelpButton").Size = Properties.Resources.Restore.Size;
            skin.GetCaptionButtonSkin("HelpButton").Margin = new Padding(1, 5, 1, 0);
            skin.GetCaptionButtonSkin("HelpButton").NormalState.Image = Properties.Resources.Restore;
            skin.GetCaptionButtonSkin("HelpButton").DisabledState.Image = Properties.Resources.RestoreDisabled;
            skin.GetCaptionButtonSkin("HelpButton").ActiveState.Image = Properties.Resources.RestorePressed;
            skin.GetCaptionButtonSkin("HelpButton").HoverState.Image = Properties.Resources.RestoreHot;

            skin.TitleColor = Color.White;
            skin.TitleShadowColor = Color.DimGray;
            skin.TitleFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);

            skin.ClientAreaPadding = new Padding(3, 23, 3, 3);
            skin.IconPadding = new Padding(3, 3, 0, 0);

            this.NonClientAreaDoubleBuffering = true;

            return skin;
        }

        #endregion
    }
}
