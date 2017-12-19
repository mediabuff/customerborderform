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
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Lizard.Drawing;
using Lizard.Windows.Skin;

#endregion

namespace Lizard.Windows.Skin
{
    public sealed class SerializableImage : SkinObject
    {
        #region Variables

        private ImageSizeMode _sizeMode;
        private Bitmap _image;
        private Padding _stretchMargins;

        #endregion

        #region Properties

        [XmlAttribute("sizeMode")]
        public ImageSizeMode SizeMode
        {
            get { return _sizeMode; }
            set
            {
                if (_sizeMode != value)
                {
                    _sizeMode = value;
                    OnPropertyChanged(SerializableImageProperty.SizeMode);
                }
            }
        }

        [XmlIgnore]
        public Padding StretchMargins
        {
            get { return _stretchMargins; }
            set
            {
                if (_stretchMargins != value)
                {
                    _stretchMargins = value;
                    OnPropertyChanged(SerializableImageProperty.StretchMargins);
                }
            }
        }

        [XmlAttribute("stretchMargins")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string StretchMargins_XmlSurrogate
        {
            get
            {
                if (StretchMargins != Padding.Empty)
                {
                    TypeConverter PaddingConverter = TypeDescriptor.GetConverter(typeof(Padding));
                    return PaddingConverter.ConvertToInvariantString(StretchMargins);
                }
                return null;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    TypeConverter PaddingConverter = TypeDescriptor.GetConverter(typeof(Padding));
                    StretchMargins = (Padding)PaddingConverter.ConvertFromInvariantString(value);
                }
                else
                    StretchMargins = Padding.Empty;
            }
        }

        [XmlIgnore]
        public Bitmap Image
        {
            get { return _image; }
            set
            {
                if (_image != value)
                {
                    _image = value;
                    OnPropertyChanged(SerializableImageProperty.Image);
                }
            }
        }

        [XmlElement("image")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] Image_XmlSurrogate
        {
            get
            {
                if (Image != null)
                {
                    TypeConverter BitmapConverter =
                        TypeDescriptor.GetConverter(Image.GetType());
                    return (byte[])BitmapConverter.ConvertTo(Image, typeof(byte[]));
                }
                else
                    return null;
            }
            set
            {
                if (value != null)
                    Image = new Bitmap(new MemoryStream(value));
                else
                    Image = null;
            }
        }

        #endregion

        #region DrawImage

        public void DrawImage(Graphics g, Rectangle destRect)
        {
            if (Image == null)
                return;

            DrawUtil.DrawImage(g, Image, destRect, SizeMode, StretchMargins);
        }

        #endregion
    }

}
