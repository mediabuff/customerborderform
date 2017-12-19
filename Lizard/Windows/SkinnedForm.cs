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
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Lizard.Drawing;
using Lizard.Windows;
using Lizard.Windows.Skin;

#endregion

namespace Lizard.Windows
{
    /// <summary>
    /// A form able to detect transparent corners.
    /// </summary>
    public class SkinnedForm : SkinRenderForm
    {
        #region Variables

        private static bool _isWinNT = false;
        private Region _formBitmapRegion;

        #endregion

        #region Constructor

        static SkinnedForm()
        {
            OperatingSystem os = Environment.OSVersion;
            Version vs = os.Version;
            string platform = os.Platform.ToString();
            int majorVer = vs.Major;
            int minorVer = vs.Minor;
            // If Windows 2000 is currently running, then
            if ((platform == "Win32NT") && (majorVer == 5) && (minorVer == 0))
                _isWinNT = true;
        }

        public SkinnedForm()
        {
            // Set up events
            SkinManager.SkinChanged += new EventHandler(SkinChangedHandler);
            this.HandleCreated += new EventHandler(SkinnedForm_HandleCreated);
        }

        #endregion

        #region Events

        void SkinnedForm_HandleCreated(object sender, EventArgs e)
        {
            // Set up corners regions
            FormSkin = SkinManager.GetDefaultSkin();

            if (SkinManager.GetDefaultSkin() == null)
                return;

            CreateFormBitmapRegion(SkinManager.GetDefaultSkin().NormalState.Image);

            RecalculateResizableRegions();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RecalculateResizableRegions();
        }

        private void SkinChangedHandler(object sender, EventArgs args)
        {
            CreateFormBitmapRegion(SkinManager.GetDefaultSkin().NormalState.Image);
            RecalculateResizableRegions();
        }

        #endregion

        #region Properties

        public Color SkinBitmapTransparentColor
        {
            get
            {
                return Color.FromArgb(255, 0, 255);
            }
        }

        #endregion

        #region RecalculateRegionHandler

        private void RecalculateResizableRegions()
        {
            if (ActiveFormSkin == null || SkinManager.GetDefaultSkin() == null || _formBitmapRegion == null)
                return;

            //-- Prepare
            Bitmap bitmap = SkinManager.GetDefaultSkin().NormalState.Image;
            Matrix scaleMatrix = new Matrix();

            Padding padding = ActiveFormSkin.ClientAreaPadding;

            //-- Prepare some calculs
            float factorX = (float)(this.Width - padding.Left - padding.Right) / (bitmap.Width - padding.Left - padding.Right);
            float factorY = (float)(this.Height - padding.Top - padding.Bottom) / (bitmap.Height - padding.Top - padding.Bottom);
            int bitmapInnerWidth = bitmap.Width - padding.Left - padding.Right;
            int bitmapInnerHeight = bitmap.Height - padding.Top - padding.Bottom;

            //-- Region 1
            Region resizedRegion = new Region(new Rectangle(0, 0, padding.Left, padding.Top));
            resizedRegion.Intersect(_formBitmapRegion);

            //-- Region 2
            Region region2 = new Region(new Rectangle(padding.Left, 0, bitmapInnerWidth, padding.Top));
            region2.Intersect(_formBitmapRegion);

            region2.Translate(-padding.Left, 0);
            scaleMatrix = new Matrix();
            scaleMatrix.Scale(factorX, 1);
            region2.Transform(scaleMatrix);
            region2.Translate(padding.Left, 0);

            resizedRegion.Union(region2);

            //-- Region 3
            Region region3 = new Region(new Rectangle(bitmap.Width - padding.Right, 0, padding.Right, padding.Top));
            region3.Intersect(_formBitmapRegion);
            region3.Translate(factorX * bitmapInnerWidth - bitmapInnerWidth, 0);

            resizedRegion.Union(region3);

            //-- Region 4
            Region region4 = _formBitmapRegion.Clone();
            region4.Intersect(new Rectangle(0, padding.Top, padding.Left, bitmapInnerHeight));

            region4.Translate(0, -padding.Top);
            scaleMatrix = new Matrix();
            scaleMatrix.Scale(1, factorY);
            region4.Transform(scaleMatrix);
            region4.Translate(0, padding.Top);

            resizedRegion.Union(region4);

            //-- Region 5
            Region region5 = _formBitmapRegion.Clone();
            region5.Intersect(new Rectangle(padding.Left, padding.Top, bitmap.Width - padding.Left - padding.Right, bitmap.Height - padding.Top - padding.Bottom));

            region5.Translate(-padding.Left, -padding.Top);
            scaleMatrix = new Matrix();
            scaleMatrix.Scale(factorX, factorY);
            region5.Transform(scaleMatrix);
            region5.Translate(padding.Left, padding.Top);

            resizedRegion.Union(region5);

            //-- Region 6
            Region region6 = new Region(new Rectangle(bitmap.Width - padding.Right, padding.Top, padding.Right, bitmapInnerHeight));
            region6.Intersect(_formBitmapRegion);

            region6.Translate(0, -padding.Top);
            scaleMatrix = new Matrix();
            scaleMatrix.Scale(1, factorY);
            region6.Transform(scaleMatrix);
            region6.Translate(factorX * bitmapInnerWidth - bitmapInnerWidth, padding.Top);

            resizedRegion.Union(region6);

            //-- Region 7
            Region region7 = new Region(new Rectangle(0, bitmap.Height - padding.Bottom, padding.Left, padding.Bottom));
            region7.Intersect(_formBitmapRegion);
            region7.Translate(0, factorY * bitmapInnerHeight - bitmapInnerHeight);

            resizedRegion.Union(region7);

            //-- Region 8
            Region region8 = new Region(new Rectangle(padding.Left, bitmap.Height - padding.Bottom, bitmapInnerWidth, padding.Bottom));
            region8.Intersect(_formBitmapRegion);

            region8.Translate(-padding.Left, 0);
            scaleMatrix = new Matrix();
            scaleMatrix.Scale(factorX, 1);
            region8.Transform(scaleMatrix);
            region8.Translate(padding.Left, factorY * bitmapInnerHeight - bitmapInnerHeight);

            resizedRegion.Union(region8);

            //-- Region 9
            Region region9 = new Region(new Rectangle(bitmap.Width - padding.Right, bitmap.Height - padding.Bottom, padding.Right, padding.Bottom));
            region9.Intersect(_formBitmapRegion);
            region9.Translate(factorX * bitmapInnerWidth - bitmapInnerWidth, factorY * bitmapInnerHeight - bitmapInnerHeight);

            resizedRegion.Union(region9);

            //---- For Win2000
            if (_isWinNT)
                resizedRegion.Translate(SystemInformation.FrameBorderSize.Width - 1, SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height - 1);

            //---- Set region
            this.Region = resizedRegion;
        }

        #endregion

        #region CreateFormBitmapRegion

        /// <summary>
        /// Create a region for the form's bitmap used for background.
        /// </summary>
        /// <param name="bitmap"></param>
        private void CreateFormBitmapRegion(Bitmap bitmap)
        {
            Color TransparentColor = this.SkinBitmapTransparentColor;

            GraphicsPath graphicsPath = new GraphicsPath();
            int StartRegionArea = -1;
            Color PixelColor = Color.Empty;
            BitmapData bData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
            IntPtr ScanL = bData.Scan0;
            int YOffset = bData.Stride - bitmap.Width * 4;
            unsafe
            {
                int width = bitmap.Width;
                int height = bitmap.Height;
                unsafe
                {
                    byte* p = (byte*)(void*)ScanL;
                    for (int y = 0; y <= height - 1; y++)
                    {
                        for (int x = 0; x <= width - 1; x++)
                        {
                            int B = (int)p[0];
                            int G = (int)p[1];
                            int R = (int)p[2];
                            int A = (int)p[3];
                            PixelColor = Color.FromArgb(R, G, B);
                            if (PixelColor == TransparentColor && StartRegionArea != -1)
                            {
                                graphicsPath.AddRectangle(new Rectangle(StartRegionArea, y, (x - 1) - StartRegionArea, 1));
                                StartRegionArea = -1;
                            }
                            if (PixelColor != TransparentColor && StartRegionArea == -1)
                                StartRegionArea = x;
                            p += 4;
                        }
                        if (StartRegionArea != -1)
                        {
                            graphicsPath.AddRectangle(new Rectangle(StartRegionArea, y, bitmap.Width - StartRegionArea, 1));
                            StartRegionArea = -1;
                        }
                        p += YOffset;
                    }
                }
            }
            bitmap.UnlockBits(bData);

            _formBitmapRegion = new Region(graphicsPath);
        }

        #endregion
    }
}
