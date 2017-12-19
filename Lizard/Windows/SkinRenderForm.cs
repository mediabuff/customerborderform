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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing.Imaging; // used for logging 

using Lizard.Drawing;
using Lizard.Windows.Skin;
using Lizard.Windows.Native;

#endregion

namespace Lizard.Windows
{

    [System.ComponentModel.DesignerCategory("code")]
    public class SkinRenderForm : NonClientBaseForm
    {
        #region Variables

        // Shortcuts to the standards buttons
        private CaptionButton _closeButton;
        private CaptionButton _restoreButton;
        private CaptionButton _maximizeButton;
        private CaptionButton _minimizeButton;
        private CaptionButton _helpButton;

        private CaptionButtonCollection _captionButtons = new CaptionButtonCollection();

        private FormSkin _formSkin;
        private bool _useFormSkinManager;
        private string _formSkinName;
        private FormSkin _activeformSkin;

        NativeMethods.TRACKMOUSEEVENT _trackMouseEvent;
        bool _trakingMouse = false;

        Icon _smallIcon;

        #endregion

        #region Constructor

        public SkinRenderForm()
        {
            InitSystemButtons();

            UpdateActiveFormSkin();

            // add handler for global stale changed event
            SkinManager.SkinChanged += new EventHandler(FormSkinManager_SkinChanged);
        }

        private void InitSystemButtons()
        {
            _closeButton = new CaptionButton();
            _closeButton.Key = "CloseButton";
            _closeButton.Visible = true;
            _closeButton.HitTestCode = (int)NativeMethods.NCHITTEST.HTCLOSE;
            _closeButton.SystemCommand = (int)NativeMethods.SystemCommands.SC_CLOSE;
            _captionButtons.Add(_closeButton);

            _restoreButton = new CaptionButton();
            _restoreButton.Key = "RestoreButton";
            _restoreButton.Enabled = this.MaximizeBox;
            _restoreButton.HitTestCode = (int)NativeMethods.NCHITTEST.HTMAXBUTTON;
            _restoreButton.SystemCommand = (int)NativeMethods.SystemCommands.SC_RESTORE;
            _captionButtons.Add(_restoreButton);

            _maximizeButton = new CaptionButton();
            _maximizeButton.Key = "MaximizeButton";
            _maximizeButton.Enabled = this.MaximizeBox;
            _maximizeButton.HitTestCode = (int)NativeMethods.NCHITTEST.HTMAXBUTTON;
            _maximizeButton.SystemCommand = (int)NativeMethods.SystemCommands.SC_MAXIMIZE;
            _captionButtons.Add(_maximizeButton);

            _minimizeButton = new CaptionButton();
            _minimizeButton.Key = "MinimizeButton";
            _minimizeButton.Enabled = this.MinimizeBox;
            _minimizeButton.HitTestCode = (int)NativeMethods.NCHITTEST.HTMINBUTTON;
            _minimizeButton.SystemCommand = (int)NativeMethods.SystemCommands.SC_MINIMIZE;
            _captionButtons.Add(_minimizeButton);

            _helpButton = new CaptionButton();
            _helpButton.Key = "HelpButton";
            _helpButton.Visible = this.HelpButton;
            _helpButton.HitTestCode = (int)NativeMethods.NCHITTEST.HTHELP;
            _helpButton.SystemCommand = (int)NativeMethods.SystemCommands.SC_CONTEXTHELP;
            _captionButtons.Add(_helpButton);

            OnUpdateWindowState();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (_smallIcon != null)
                    _smallIcon.Dispose();

                SkinManager.SkinChanged -= new EventHandler(FormSkinManager_SkinChanged);
                ActiveFormSkin = null;
            }
        }

        #endregion

        #region Properties

        public CaptionButtonCollection CaptionButtons
        {
            get { return _captionButtons; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FormSkin FormSkin
        {
            get { return _formSkin; }
            set
            {
                if (_formSkin != value)
                {
                    _formSkin = value;
                    UpdateActiveFormSkin();
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        public bool UseFormSkinManager
        {
            get { return _useFormSkinManager; }
            set
            {
                if (_useFormSkinManager != value)
                {
                    _useFormSkinManager = value;
                    UpdateActiveFormSkin();
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(null)]
        public string FormSkinName
        {
            get { return _formSkinName; }
            set
            {
                if (_formSkinName != value)
                {
                    _formSkinName = value;
                    UpdateActiveFormSkin();
                }
            }
        }

        protected FormSkin ActiveFormSkin
        {
            get { return _activeformSkin; }
            set
            {
                if (_activeformSkin != value)
                {
                    if (_activeformSkin != null)
                        _activeformSkin.ChildPropertyChanged -= new ChildPropertyChangedEventHandler(FormSkin_ChildPropertyChanged);

                    _activeformSkin = value;

                    if (_activeformSkin != null)
                        _activeformSkin.ChildPropertyChanged += new ChildPropertyChangedEventHandler(FormSkin_ChildPropertyChanged);

                    UpdateButtonSkin();
                    InvalidateWindow();
                }
            }
        }

        public new bool MaximizeBox
        {
            get { return base.MaximizeBox; }
            set
            {
                this._maximizeButton.Enabled = value;
                this._minimizeButton.Visible = this._maximizeButton.Visible
                    = this._maximizeButton.Enabled | this._minimizeButton.Enabled;

                base.MaximizeBox = value;
            }
        }

        public new bool MinimizeBox
        {
            get { return base.MinimizeBox; }
            set
            {
                this._minimizeButton.Enabled = value;
                this._minimizeButton.Visible = this._maximizeButton.Visible
                    = this._maximizeButton.Enabled | this._minimizeButton.Enabled;

                base.MinimizeBox = value;
            }
        }

        public new bool ControlBox
        {
            get { return base.ControlBox; }
            set
            {
                this._closeButton.Enabled = value;
                base.ControlBox = value;

            }
        }

        public new bool HelpButton
        {
            get { return base.HelpButton; }
            set
            {
                this._helpButton.Visible = value;
                base.HelpButton = value;
            }
        }

        public new Icon Icon
        {
            get { return base.Icon; }
            set
            {
                if (value != Icon)
                {
                    if (this._smallIcon != null)
                    {
                        _smallIcon.Dispose();
                        _smallIcon = null;
                    }
                    try
                    {
                        _smallIcon = new Icon(value, SystemInformation.SmallIconSize);

                        // [2414]: When new icon wasn't scaled to requested size then discard
                        // (this indicates that .ico file doesn't have proper sized version).
                        if (_smallIcon.Size != SystemInformation.SmallIconSize)
                            _smallIcon = null;
                    }
                    catch
                    { }
                }
                base.Icon = value;
            }
        }

        #endregion

        #region FormSkinManager events...

        void FormSkinManager_SkinChanged(object sender, EventArgs args)
        {
            UpdateActiveFormSkin();
        }

        #endregion

        #region FormSkin events...

        void FormSkin_ChildPropertyChanged(object sender, ChildPropertyChangedEventArgs args)
        {
            InvalidateWindow();
        }

        #endregion

        #region On...

        protected override void OnNonClientAreaCalcSize(ref Rectangle bounds, bool update)
        {
            if (ActiveFormSkin == null)
                return;

            if (update)
                UpdateCaptionButtonBounds(bounds);

            Padding clientPadding = ActiveFormSkin.ClientAreaPadding;
            bounds = new Rectangle(bounds.Left + clientPadding.Left, bounds.Top + clientPadding.Top,
                bounds.Width - clientPadding.Horizontal, bounds.Height - clientPadding.Vertical);
        }

        protected override void OnNonClientMouseLeave(EventArgs args)
        {
            if (!_trakingMouse)
                return;

            foreach (CaptionButton button in this.CaptionButtons)
            {
                if (button.State != CaptionButtonState.Normal)
                {
                    button.State = CaptionButtonState.Normal;
                    DrawButton(button);
                    UnhookMouseEvent();
                }
            }
        }

        protected override void OnNonClientMouseDown(NonClientMouseEventArgs args)
        {
            if (args.Button != MouseButtons.Left)
                return;

            // custom button
            foreach (CaptionButton button in this.CaptionButtons)
                if (args.HitTest > short.MaxValue && args.HitTest == button.HitTestCode && button.Visible && button.Enabled)
                {
                    ((CustomCaptionButton)button).OnClick();
                    args.Handled = true;
                    return;
                }

            // find appropriate button
            foreach (CaptionButton button in this.CaptionButtons)
            {
                // [1530]: Don't execute any action when button is disabled or not visible.
                if (args.HitTest == button.HitTestCode && button.Visible && button.Enabled)
                {
                    Log(MethodInfo.GetCurrentMethod(), "MouseDown: button = {0}", button);

                    if (DepressButton(button))
                    {
                        if (button.SystemCommand >= 0)
                        {
                            int sc = button.SystemCommand;

                            if (button == _maximizeButton)
                                sc = (WindowState == FormWindowState.Maximized) ?
                                    (int)NativeMethods.SystemCommands.SC_RESTORE : (int)NativeMethods.SystemCommands.SC_MAXIMIZE;

                            NativeMethods.SendMessage(this.Handle,
                                (int)NativeMethods.WindowMessages.WM_SYSCOMMAND,
                                (IntPtr)sc, IntPtr.Zero);
                        }

                        //TODO: fire event for custom button

                        args.Handled = true;
                    }
                    return;
                }
            }
        }

        protected override void OnUpdateWindowState()
        {
            this._minimizeButton.Visible = this._maximizeButton.Enabled | this._minimizeButton.Enabled;
            this._maximizeButton.Visible = this._minimizeButton.Visible && this.WindowState != FormWindowState.Maximized;
            this._restoreButton.Visible = this._minimizeButton.Visible && this.WindowState == FormWindowState.Maximized;
        }

        protected override int OnNonClientAreaHitTest(Point p)
        {
            if (ActiveFormSkin == null)
                return (int)NativeMethods.NCHITTEST.HTCLIENT;

            foreach (CaptionButton button in this.CaptionButtons)
            {
                if (button.Visible && button.Bounds.Contains(p) && (button.HitTestCode > 0 || button.HitTestCode < -1))
                    return button.HitTestCode;
            }

            if (FormBorderStyle != FormBorderStyle.FixedToolWindow &&
                FormBorderStyle != FormBorderStyle.SizableToolWindow)
            {
                if (GetIconRectangle().Contains(p))
                    return (int)NativeMethods.NCHITTEST.HTSYSMENU;
            }

            if (this.FormBorderStyle == FormBorderStyle.Sizable
                || this.FormBorderStyle == FormBorderStyle.SizableToolWindow)
            {
                #region Handle sizable window borders
                if (p.X <= ActiveFormSkin.SizingBorderWidth) // left border
                {
                    if (p.Y <= ActiveFormSkin.SizingCornerOffset)
                        return (int)NativeMethods.NCHITTEST.HTTOPLEFT;
                    else if (p.Y >= this.Height - ActiveFormSkin.SizingCornerOffset)
                        return (int)NativeMethods.NCHITTEST.HTBOTTOMLEFT;
                    else
                        return (int)NativeMethods.NCHITTEST.HTLEFT;
                }
                else if (p.X >= this.Width - ActiveFormSkin.SizingBorderWidth) // right border
                {
                    if (p.Y <= ActiveFormSkin.SizingCornerOffset)
                        return (int)NativeMethods.NCHITTEST.HTTOPRIGHT;
                    else if (p.Y >= this.Height - ActiveFormSkin.SizingCornerOffset)
                        return (int)NativeMethods.NCHITTEST.HTBOTTOMRIGHT;
                    else
                        return (int)NativeMethods.NCHITTEST.HTRIGHT;
                }
                else if (p.Y <= ActiveFormSkin.SizingBorderWidth) // top border
                {
                    if (p.X <= ActiveFormSkin.SizingCornerOffset)
                        return (int)NativeMethods.NCHITTEST.HTTOPLEFT;
                    if (p.X >= this.Width - ActiveFormSkin.SizingCornerOffset)
                        return (int)NativeMethods.NCHITTEST.HTTOPRIGHT;
                    else
                        return (int)NativeMethods.NCHITTEST.HTTOP;
                }
                else if (p.Y >= this.Height - ActiveFormSkin.SizingBorderWidth) // bottom border
                {
                    if (p.X <= ActiveFormSkin.SizingCornerOffset)
                        return (int)NativeMethods.NCHITTEST.HTBOTTOMLEFT;
                    if (p.X >= this.Width - ActiveFormSkin.SizingCornerOffset)
                        return (int)NativeMethods.NCHITTEST.HTBOTTOMRIGHT;
                    else
                        return (int)NativeMethods.NCHITTEST.HTBOTTOM;
                }
                #endregion
            }

            // title bar
            if (p.Y <= ActiveFormSkin.ClientAreaPadding.Top)
                return (int)NativeMethods.NCHITTEST.HTCAPTION;

            // rest of non client area
            if (p.X <= this.ActiveFormSkin.ClientAreaPadding.Left || p.X >= this.ActiveFormSkin.ClientAreaPadding.Right
                || p.Y >= this.ActiveFormSkin.ClientAreaPadding.Bottom)
                return (int)NativeMethods.NCHITTEST.HTBORDER;

            return (int)NativeMethods.NCHITTEST.HTCLIENT;
        }

        protected override void OnNonClientMouseMove(MouseEventArgs mouseEventArgs)
        {
            foreach (CaptionButton button in this.CaptionButtons)
            {
                if (button.Visible && button.Bounds.Contains(mouseEventArgs.X, mouseEventArgs.Y) && button.HitTestCode > 0)
                {
                    if (button.State != CaptionButtonState.Over)
                    {
                        button.State = CaptionButtonState.Over;
                        DrawButton(button);
                        HookMouseEvent();
                    }
                }
                else
                {
                    if (button.State != CaptionButtonState.Normal)
                    {
                        button.State = CaptionButtonState.Normal;
                        DrawButton(button);
                        UnhookMouseEvent();
                    }
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (ActiveFormSkin == null || ActiveFormSkin.NormalState.Image == null)
                return;

            Rectangle srcRect =
                DrawUtil.ExcludePadding(new Rectangle(Point.Empty, ActiveFormSkin.NormalState.Image.Size),
                    ActiveFormSkin.ClientAreaPadding);

            Padding margins =
                DrawUtil.SubstractPadding(ActiveFormSkin.NormalState.StretchMargins, ActiveFormSkin.ClientAreaPadding);

            DrawUtil.DrawImage(e.Graphics, ActiveFormSkin.NormalState.Image, srcRect, ClientRectangle, null, margins);
        }

        protected override void OnNonClientAreaPaint(NonClientPaintEventArgs e)
        {
            if (ActiveFormSkin == null)
                return;

            // assign clip region to exclude client area
            Region clipRegion = new Region(e.Bounds);
            clipRegion.Exclude(DrawUtil.ExcludePadding(e.Bounds, ActiveFormSkin.ClientAreaPadding));
            e.Graphics.Clip = clipRegion;

            // paint borders
            ActiveFormSkin.NormalState.DrawImage(e.Graphics, e.Bounds);

            int textOffset = 0;

            // paint icon
            if (ShowIcon &&
                FormBorderStyle != FormBorderStyle.FixedToolWindow &&
                FormBorderStyle != FormBorderStyle.SizableToolWindow)
            {
                Rectangle iconRect = GetIconRectangle();
                textOffset += iconRect.Right;

                if (_smallIcon != null)
                    e.Graphics.DrawIconUnstretched(_smallIcon, iconRect);
                else
                    e.Graphics.DrawIcon(Icon, iconRect);
            }


            // paint caption
            string text = this.Text;
            if (!String.IsNullOrEmpty(text))
            {
                // disable text wrapping and request elipsis characters on overflow
                using (StringFormat sf = new StringFormat())
                {
                    sf.Trimming = StringTrimming.EllipsisCharacter;
                    sf.FormatFlags = StringFormatFlags.NoWrap;
                    sf.LineAlignment = StringAlignment.Center;

                    // find position of the first button from left
                    int firstButton = e.Bounds.Width;
                    foreach (CaptionButton button in this.CaptionButtons)
                        if (button.Visible)
                            firstButton = Math.Min(firstButton, button.Bounds.X);

                    Padding padding = ActiveFormSkin.TitlePadding;
                    Rectangle textRect = new Rectangle(textOffset + padding.Left,
                        padding.Top, firstButton - textOffset - padding.Horizontal,
                        ActiveFormSkin.ClientAreaPadding.Top - padding.Vertical);

                    Font textFont = this.Font;
                    if (ActiveFormSkin.TitleFont != null)
                        textFont = ActiveFormSkin.TitleFont;

                    if (!ActiveFormSkin.TitleShadowColor.IsEmpty)
                    {
                        Rectangle shadowRect = textRect;
                        shadowRect.Offset(1, 1);

                        // draw drop shadow
                        using (Brush b = new SolidBrush(ActiveFormSkin.TitleShadowColor))
                        {
                            e.Graphics.DrawString(text, textFont, b, shadowRect, sf);
                        }
                    }

                    if (!ActiveFormSkin.TitleColor.IsEmpty)
                    {
                        // draw text
                        using (Brush b = new SolidBrush(ActiveFormSkin.TitleColor))
                        {
                            e.Graphics.DrawString(text, textFont, b, textRect, sf);
                        }
                    }
                }
            }

            // Translate for the frame border
            //if (this.WindowState == FormWindowState.Maximized)
            //    e.Graphics.TranslateTransform(0, SystemInformation.FrameBorderSize.Height);

            // [2415] Because mouse actions over a button might cause it to repaint we need 
            // to buffer the background under the button in order to repaint it correctly.
            // This is important when partially transparent images are used for some button states.
            foreach (CaptionButton button in this.CaptionButtons)
                 button.UpdateBackground(e.Graphics, this.Location);

            // Paint all visible buttons (in this case the background doesn't need to be repainted)
            foreach (CaptionButton button in this.CaptionButtons)
                button.DrawButton(e.Graphics, false);
        }

        #endregion

        #region Mouse events hook...

        private void HookMouseEvent()
        {
            if (!_trakingMouse)
            {
                _trakingMouse = true;
                if (this._trackMouseEvent == null)
                {
                    this._trackMouseEvent = new NativeMethods.TRACKMOUSEEVENT();
                    this._trackMouseEvent.dwFlags =
                        (int)(NativeMethods.TrackMouseEventFalgs.TME_HOVER |
                              NativeMethods.TrackMouseEventFalgs.TME_LEAVE |
                              NativeMethods.TrackMouseEventFalgs.TME_NONCLIENT);

                    this._trackMouseEvent.hwndTrack = this.Handle;
                }

                if (NativeMethods.TrackMouseEvent(this._trackMouseEvent) == false)
                    // use getlasterror to see whats wrong
                    Log(MethodInfo.GetCurrentMethod(), "Failed enabling TrackMouseEvent: error {0}",
                        NativeMethods.GetLastError());
            }
        }

        private void UnhookMouseEvent()
        {
            _trakingMouse = false;
        }

        #endregion

        #region Update...

        private void UpdateActiveFormSkin()
        {
            FormSkin skin = null;
            if (UseFormSkinManager)
            {
                // try to load specified skin
                if (!String.IsNullOrEmpty(FormSkinName))
                    skin = SkinManager.GetSkin(FormSkinName);

                // if it wasn't found try to load default style
                if (skin == null)
                    skin = SkinManager.GetDefaultSkin();
            }
            else
            {
                skin = FormSkin;
            }
            ActiveFormSkin = skin;
        }

        private void UpdateButtonSkin()
        {
            if (ActiveFormSkin != null)
            {
                foreach (CaptionButton button in _captionButtons)
                {
                    if (button.Key == "MaximizeButton")
                    {
                        if (this.WindowState == FormWindowState.Maximized)
                            button.Skin = ActiveFormSkin.GetCaptionButtonSkin("RestoreButton");
                        else
                            button.Skin = ActiveFormSkin.GetCaptionButtonSkin("MaximizeButton");
                    }
                    else
                        button.Skin = ActiveFormSkin.GetCaptionButtonSkin(button.Key);
                }
            }
            else
            {
                foreach (CaptionButton button in _captionButtons)
                    button.Skin = null;
            }
        }

        private void UpdateCaptionButtonBounds(Rectangle windowRect)
        {
            // start from top-right corner
            int x = windowRect.Width;

            foreach (CaptionButton button in this.CaptionButtons)
            {
                if (button.Visible && button.Skin != null)
                {
                    int y = button.Skin.Margin.Top;
                    x -= (button.Skin.Size.Width + button.Skin.Margin.Right);
                    button.Bounds = new Rectangle(x, y,
                        button.Skin.Size.Width, button.Skin.Size.Height);
                    x -= button.Skin.Margin.Left;
                }
            }

            // Should I move this where this actually changes WM_GETMINMAXINFO ??
            //maximizeButton.Appearance = (this.WindowState == FormWindowState.Maximized) ?
            //    _borderAppearance.RestoreButton : _borderAppearance.MaximizeButton;
        }



        #endregion

        #region Misc...

        /// <summary>
        /// This method handles depressing the titlebar button. It captures the mouse and creates a message loop
        /// filtring only the mouse buttons until a WM_MOUSEMOVE or WM_LBUTTONUP message is received.
        /// </summary>
        /// <param name="currentButton">The button that was pressed</param>
        /// <returns>true if WM_LBUTTONUP occured over this button; false when mouse was moved away from this button.</returns>
        private bool DepressButton(CaptionButton currentButton)
        {
            try
            {
                // callect all mouse events (should do the same as SetCapture())
                this.Capture = true;

                // draw button in pressed mode
                currentButton.State = CaptionButtonState.Pressed;
                DrawButton(currentButton);

                // loop until button is released
                bool result = false;
                bool done = false;
                while (!done)
                {
                    // NOTE: This struct needs to be here. We had strange error (starting from Beta 2).
                    // when this was defined at begining of this method. also check if types are correct (no overlap). 
                    Message m = new Message();

                    if (NativeMethods.PeekMessage(ref m, IntPtr.Zero,
                        (int)NativeMethods.WindowMessages.WM_MOUSEFIRST, (int)NativeMethods.WindowMessages.WM_MOUSELAST,
                        (int)NativeMethods.PeekMessageOptions.PM_REMOVE))
                    {
                        Log(MethodInfo.GetCurrentMethod(), "Message = {0}, Button = {1}", (NativeMethods.WindowMessages)m.Msg, currentButton);
                        switch (m.Msg)
                        {
                            case (int)NativeMethods.WindowMessages.WM_LBUTTONUP:
                                {
                                    if (currentButton.State == CaptionButtonState.Pressed)
                                    {
                                        currentButton.State = CaptionButtonState.Normal;
                                        DrawButton(currentButton);
                                    }
                                    Point pt = PointToWindow(PointToScreen(new Point(m.LParam.ToInt32())));
                                    Log(MethodInfo.GetCurrentMethod(), "### Point = ({0}, {1})", pt.X, pt.Y);

                                    result = currentButton.Bounds.Contains(pt);
                                    done = true;
                                }
                                break;
                            case (int)NativeMethods.WindowMessages.WM_MOUSEMOVE:
                                {
                                    Point clientPoint = PointToWindow(PointToScreen(new Point(m.LParam.ToInt32())));
                                    if (currentButton.Bounds.Contains(clientPoint))
                                    {
                                        if (currentButton.State == CaptionButtonState.Normal)
                                        {
                                            currentButton.State = CaptionButtonState.Pressed;
                                            DrawButton(currentButton);
                                        }
                                    }
                                    else
                                    {
                                        if (currentButton.State == CaptionButtonState.Pressed)
                                        {
                                            currentButton.State = CaptionButtonState.Normal;
                                            DrawButton(currentButton);
                                        }
                                    }

                                    // [1531]: These variables need to be reset here although thay aren't changed 
                                    // before reaching this point
                                    result = false;
                                    done = false;
                                }
                                break;
                        }
                    }
                }
                return result;
            }
            finally
            {
                this.Capture = false;
            }
        }

        private void DrawButton(CaptionButton button)
        {
            if (IsHandleCreated)
            {
                // MSDN states that only WINDOW and INTERSECTRGN are needed,
                // but other sources confirm that CACHE is required on Win9x
                // and you need CLIPSIBLINGS to prevent painting on overlapping windows.
                IntPtr hDC = NativeMethods.GetDCEx(this.Handle, (IntPtr)1,
                    (int)(NativeMethods.DCX.DCX_WINDOW | NativeMethods.DCX.DCX_INTERSECTRGN
                        | NativeMethods.DCX.DCX_CACHE | NativeMethods.DCX.DCX_CLIPSIBLINGS));

                if (hDC != IntPtr.Zero)
                {
                    using (Graphics g = Graphics.FromHdc(hDC))
                    {
                        button.DrawButton(g, true);
                    }
                }

                NativeMethods.ReleaseDC(this.Handle, hDC);
            }
        }

        private Rectangle GetIconRectangle()
        {
            return new Rectangle(ActiveFormSkin.IconPadding.Left, ActiveFormSkin.IconPadding.Top, 16, 16);
        }

        #endregion
    }

}
