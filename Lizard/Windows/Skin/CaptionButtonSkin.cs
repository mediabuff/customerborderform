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
using System.Windows.Forms;
using System.Diagnostics;
using Lizard.Windows.Skin;

#endregion

namespace Lizard.Windows.Skin
{
#if !DEBUGFORM
    [DebuggerStepThrough]
#endif
    [XmlType("CaptionButtonSkin")]
    public class CaptionButtonSkin : SkinObject
    {
        #region Variables

        private string _key;
        private Size _size;
        private Padding _margin;
        private SerializableImage _disabledState;
        private SerializableImage _normalState;
        private SerializableImage _activeState;
        private SerializableImage _hoverState;

        #endregion

        #region Constructor

        public CaptionButtonSkin()
            : this("New caption button skin")
        {
        }

        public CaptionButtonSkin(string key)
        {
            _key = key;
            NormalState = new SerializableImage();
            ActiveState = new SerializableImage();
            HoverState = new SerializableImage();
            DisabledState = new SerializableImage();
        }

        #endregion

        #region Properties

        [XmlAttribute("key")]
        [Description("The key that identify the button to skin")]
        public string Key
        {
            get { return _key; }
            set
            {
                if (_key != value)
                {
                    _key = value;
                    OnPropertyChanged(CaptionButtonSkinProperty.Key);
                }
            }
        }

        [XmlIgnore]
        [Description("Size of button rectangle.")]
        public Size Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    OnPropertyChanged(CaptionButtonSkinProperty.Size);
                }
            }
        }

        [XmlAttribute("size")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Size_XmlSurrogate
        {
            get
            {
                if (Size != Size.Empty)
                {
                    TypeConverter SizeConverter = TypeDescriptor.GetConverter(typeof(Size));
                    return SizeConverter.ConvertToInvariantString(Size);
                }
                return null;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    TypeConverter SizeConverter = TypeDescriptor.GetConverter(typeof(Size));
                    Size = (Size)SizeConverter.ConvertFromInvariantString(value);
                }
                else
                    Size = Size.Empty;
            }
        }

        [XmlIgnore]
        [Description("Margin around button rectangle relative to title bar")]
        public Padding Margin
        {
            get { return _margin; }
            set
            {
                if (_margin != value)
                {
                    _margin = value;
                    OnPropertyChanged(CaptionButtonSkinProperty.Margin);
                }
            }
        }

        [XmlAttribute("margin")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Margin_XmlSurrogate
        {
            get
            {
                if (Margin != Padding.Empty)
                {
                    TypeConverter PaddingConverter = TypeDescriptor.GetConverter(typeof(Padding));
                    return PaddingConverter.ConvertToInvariantString(Margin);
                }
                return null;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    TypeConverter PaddingConverter = TypeDescriptor.GetConverter(typeof(Padding));
                    Margin = (Padding)PaddingConverter.ConvertFromInvariantString(value);
                }
                else
                    Margin = Padding.Empty;
            }
        }

        [Browsable(false)]
        [XmlElement("normalState")]
        [Description("Image used to paint button in normal state")]
        public SerializableImage NormalState
        {
            get { return _normalState; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(CaptionButtonSkinProperty.NormalState);

                if (_normalState != value)
                {
                    _normalState = value;
                    _normalState.Parent = this;
                    OnPropertyChanged(CaptionButtonSkinProperty.NormalState);
                }
            }
        }

        [Browsable(false)]
        [XmlElement("activeState")]
        [Description("Image used to paint button in active state")]
        public SerializableImage ActiveState
        {
            get { return _activeState; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(CaptionButtonSkinProperty.ActiveState);

                if (_activeState != value)
                {
                    _activeState = value;
                    _activeState.Parent = this;
                    OnPropertyChanged(CaptionButtonSkinProperty.ActiveState);
                }
            }
        }

        [Browsable(false)]
        [XmlElement("hoverState")]
        [Description("Image used to paint button in hover state")]
        public SerializableImage HoverState
        {
            get { return _hoverState; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(CaptionButtonSkinProperty.HoverState);

                if (_hoverState != value)
                {
                    _hoverState = value;
                    _hoverState.Parent = this;
                    OnPropertyChanged(CaptionButtonSkinProperty.HoverState);
                }
            }
        }

        [Browsable(false)]
        [XmlElement("disabledState")]
        [Description("Image used to paint button in disabled state")]
        public SerializableImage DisabledState
        {
            get { return _disabledState; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(CaptionButtonSkinProperty.DisabledState);

                if (_disabledState != value)
                {
                    _disabledState = value;
                    _disabledState.Parent = this;
                    OnPropertyChanged(CaptionButtonSkinProperty.DisabledState);
                }
            }
        }

        #endregion
    }
}
