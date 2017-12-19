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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Lizard.Drawing;
using Lizard.Windows.Skin;

#endregion

namespace Lizard.Windows
{
    /// <summary>
    /// A button that can appear in the window caption bar
    /// </summary>
    public class CaptionButton
    {
        #region Variables

        private Rectangle _bounds;
        private CaptionButtonState _state;
        private CaptionButtonSkin _skin;
        private string _key;
        private bool _visible = true;
        private int _hitTestCode = -1;
        private int _systemCommand = -1;
        private bool _enabled = true;
        private Bitmap backgroundImage;

        #endregion

        #region ToString

        public override string ToString()
        {
            return this.Key;
        }

        #endregion

        #region Properties

        public CaptionButtonState State
        {
            get { return _state; }
            set { _state = value; }
        }

        public Rectangle Bounds
        {
            get { return _bounds; }
            set
            {
                _bounds = value;
                backgroundImage = null;
            }
        }

        public CaptionButtonSkin Skin
        {
            get { return _skin; }
            set { _skin = value; }
        }

        public string Key
        {
            get { return _key; }
            set
            {
                // Map the skin to the button
                if (_key == null &&
                    value != "MaximizeButton" &&
                    value != "MinimizeButton" &&
                    value != "RestoreButton" &&
                    value != "CloseButton" &&
                    value != "HelpButton")
                {
                    Skin = SkinManager.GetSkin("Default").GetCaptionButtonSkin(value);
                    if (Skin == null)
                        throw new InvalidOperationException("Unable to find the skin for the Caption button : " + value);
                }

                _key = value;
            }
        }

        public int HitTestCode
        {
            get { return _hitTestCode; }
            set { _hitTestCode = value; }
        }

        public int SystemCommand
        {
            get { return _systemCommand; }
            set { _systemCommand = value; }
        }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        #endregion

        #region UpdateBackground

        public void UpdateBackground(Graphics g, Point windowLocation)
        {
            if (!Visible)
                return;

            Bitmap bmp = new Bitmap(Bounds.Width, Bounds.Height, g);
            DrawUtil.CopyFromGraphics(g, bmp, Bounds.Location, Point.Empty, Bounds.Size);
            backgroundImage = bmp;
        }

        #endregion

        #region DrawButton

        public void DrawButton(Graphics g, bool paintBackground)
        {
            if (!Visible || Skin == null)
                return;

            // paint buffered background image
            if (paintBackground && backgroundImage != null)
                g.DrawImage(backgroundImage, Bounds);

            if (this.Enabled)
            {
                switch (this.State)
                {
                    case CaptionButtonState.Normal:
                        if (Skin.NormalState != null)
                            Skin.NormalState.DrawImage(g, Bounds);
                        break;
                    case CaptionButtonState.Pressed:
                        if (Skin.ActiveState != null)
                            Skin.ActiveState.DrawImage(g, Bounds);
                        break;
                    case CaptionButtonState.Over:
                        if (Skin.HoverState != null)
                            Skin.HoverState.DrawImage(g, Bounds);
                        break;
                }
            }
            else
            {
                Skin.DisabledState.DrawImage(g, Bounds);
            }
        }

        #endregion
    }

    #region Enum: CaptionButtonState

    public enum CaptionButtonState
    {
        Normal,
        Pressed,
        Over
    }

    #endregion

    #region Class: CaptionButtonCollection

    public class CaptionButtonCollection : CollectionBase
    {
        public void Add(CaptionButton button)
        {
            this.List.Add(button);
        }
    }

    #endregion
}
