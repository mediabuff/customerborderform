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
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Runtime.Serialization;

using Lizard.Drawing;

#endregion

namespace Lizard.Windows.Skin
{
#if !DEBUGFORM
	[DebuggerStepThrough]
#endif
    [XmlType("FormSkin")]
    public sealed class FormSkin : SkinObject
    {
        #region Variables

        private string _name;
        private Padding _clientAreaPadding;
        private Padding _iconPadding;
        private Padding _titlePadding;
        private Font _titleFont;
        private Color _titleColor;
        private Color _titleShadowColor;
        private int _sizingBorderWidth = 3;
        private int _sizingCornerOffset = 16;
        private SerializableImage _normalState;

        private List<CaptionButtonSkin> _captionButtonSkins = new List<CaptionButtonSkin>();

        #endregion

        #region Constructor

        public FormSkin(bool defaultButtons)
        {
            NormalState = new SerializableImage();

            if (!defaultButtons) return;

            AddCaptionButtonSkin(new CaptionButtonSkin("MinimizeButton"));
            AddCaptionButtonSkin(new CaptionButtonSkin("MaximizeButton"));
            AddCaptionButtonSkin(new CaptionButtonSkin("CloseButton"));
            AddCaptionButtonSkin(new CaptionButtonSkin("RestoreButton"));
            AddCaptionButtonSkin(new CaptionButtonSkin("HelpButton"));
        }

        public FormSkin():this(false)
        {
        }

        #endregion

        #region Properties

        [XmlAttribute("name")]
        [Description("Name used to indentify this style in the library")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentNullException(FormSkinProperty.Name);

                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(FormSkinProperty.Name);
                }
            }
        }

        [XmlIgnore]
        [Description("Padding of the client rectangle relative to window bounds")]
        public Padding ClientAreaPadding
        {
            get { return _clientAreaPadding; }
            set
            {
                if (_clientAreaPadding != value)
                {
                    _clientAreaPadding = value;
                    OnPropertyChanged(FormSkinProperty.ClientAreaPadding);
                }
            }
        }

        [XmlAttribute("clientAreaPadding")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string ClientAreaPadding_XmlSurrogate
        {
            get
            {
                if (ClientAreaPadding != Padding.Empty)
                {
                    TypeConverter PaddingConverter = TypeDescriptor.GetConverter(typeof(Padding));
                    return PaddingConverter.ConvertToInvariantString(ClientAreaPadding);
                }
                return null;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    TypeConverter PaddingConverter = TypeDescriptor.GetConverter(typeof(Padding));
                    ClientAreaPadding = (Padding)PaddingConverter.ConvertFromInvariantString(value);
                }
                else
                    ClientAreaPadding = Padding.Empty;
            }
        }

        [XmlIgnore]
        [Description("Padding of icon rectangle relative to top-left corner of title bar")]
        public Padding IconPadding
        {
            get { return _iconPadding; }
            set
            {
                if (_iconPadding != value)
                {
                    _iconPadding = value;
                    OnPropertyChanged(FormSkinProperty.IconPadding);
                }
            }
        }

        [XmlAttribute("iconPadding")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string IconPadding_XmlSurrogate
        {
            get
            {
                if (IconPadding != Padding.Empty)
                {
                    TypeConverter PaddingConverter = TypeDescriptor.GetConverter(typeof(Padding));
                    return PaddingConverter.ConvertToInvariantString(IconPadding);
                }
                return null;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    TypeConverter PaddingConverter = TypeDescriptor.GetConverter(typeof(Padding));
                    IconPadding = (Padding)PaddingConverter.ConvertFromInvariantString(value);
                }
                else
                    IconPadding = Padding.Empty;
            }
        }

        [XmlIgnore]
        [Description("Padding of the title rectangle relative to rectangle of title bar")]
        public Padding TitlePadding
        {
            get { return _titlePadding; }
            set
            {
                if (_titlePadding != value)
                {
                    _titlePadding = value;
                    OnPropertyChanged(FormSkinProperty.TitlePadding);
                }
            }
        }

        [XmlAttribute("titlePadding")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string TitlePadding_XmlSurrogate
        {
            get
            {
                if (TitlePadding != Padding.Empty)
                {
                    TypeConverter PaddingConverter = TypeDescriptor.GetConverter(typeof(Padding));
                    return PaddingConverter.ConvertToInvariantString(TitlePadding);
                }
                return null;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    TypeConverter PaddingConverter = TypeDescriptor.GetConverter(typeof(Padding));
                    TitlePadding = (Padding)PaddingConverter.ConvertFromInvariantString(value);
                }
                else
                    TitlePadding = Padding.Empty;
            }
        }

        [XmlIgnore]
        [Description("Font used to paint window title")]
        public Font TitleFont
        {
            get { return _titleFont; }
            set
            {
                if (_titleFont != value)
                {
                    _titleFont = value;
                    OnPropertyChanged(FormSkinProperty.TitleFont);
                }
            }
        }

        [XmlAttribute("titleFont")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string TitleFont_XmlSurrogate
        {
            get
            {
                if (TitleFont != null)
                {
                    TypeConverter FontConverter = TypeDescriptor.GetConverter(typeof(Font));
                    return FontConverter.ConvertToInvariantString(TitleFont);
                }
                else
                    return null;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    TypeConverter FontConverter = TypeDescriptor.GetConverter(typeof(Font));
                    TitleFont = (Font)FontConverter.ConvertFromInvariantString(value);
                }
                else
                    TitleFont = null;
            }
        }

        [XmlIgnore]
        [Description("Color used to paint window title")]
        public Color TitleColor
        {
            get { return _titleColor; }
            set
            {
                if (_titleColor != value)
                {
                    _titleColor = value;
                    OnPropertyChanged(FormSkinProperty.TitleColor);
                }
            }
        }

        [XmlAttribute("titleColor")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string TitleColor_XmlSurrogate
        {
            get
            {
                if (TitleColor != Color.Empty)
                {
                    TypeConverter ColorConverter = TypeDescriptor.GetConverter(typeof(Color));
                    return ColorConverter.ConvertToInvariantString(TitleColor);
                }
                else
                    return null;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    TypeConverter ColorConverter = TypeDescriptor.GetConverter(typeof(Color));
                    TitleColor = (Color)ColorConverter.ConvertFromInvariantString(value);
                }
                else
                    TitleColor = Color.Empty;
            }
        }

        [XmlIgnore]
        [Description("Color used to draw drop shadow behind window title")]
        public Color TitleShadowColor
        {
            get { return _titleShadowColor; }
            set
            {
                if (_titleShadowColor != value)
                {
                    _titleShadowColor = value;
                    OnPropertyChanged(FormSkinProperty.TitleShadowColor);
                }
            }
        }

        [XmlAttribute("titleShadowColor")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string TitleShadowColor_XmlSurrogate
        {
            get
            {
                if (TitleShadowColor != Color.Empty)
                {
                    TypeConverter ColorConverter = TypeDescriptor.GetConverter(typeof(Color));
                    return ColorConverter.ConvertToInvariantString(TitleShadowColor);
                }
                else
                    return null;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    TypeConverter ColorConverter = TypeDescriptor.GetConverter(typeof(Color));
                    TitleShadowColor = (Color)ColorConverter.ConvertFromInvariantString(value);
                }
                else
                    TitleShadowColor = Color.Empty;
            }
        }

        [XmlAttribute("sizingBorderWidth")]
        [Description("Offset from window border where window can be sized using a mouse horizontaly or verically")]
        public int SizingBorderWidth
        {
            get { return _sizingBorderWidth; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("SizingBorderWidth", "Sizing offset must be greater then zero.");

                if (_sizingBorderWidth != value)
                {
                    _sizingBorderWidth = value;
                    OnPropertyChanged(FormSkinProperty.SizingBorderWidth);
                }
            }
        }

        [XmlAttribute("sizingBorderOffset")]
        [Description("Offset from window corner where form can be sized in both directions")]
        public int SizingCornerOffset
        {
            get { return _sizingCornerOffset; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("SizingCornerOffset", "Sizing offset must be greater then zero.");

                if (_sizingCornerOffset != value)
                {
                    _sizingCornerOffset = value;
                    OnPropertyChanged(FormSkinProperty.SizingCornerOffset);
                }
            }
        }

        [XmlElement("normalState")]
        [Browsable(false)]
        [Description("Image used to paint background on forms non client area")]
        public SerializableImage NormalState
        {
            get { return _normalState; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("NormalState");

                if (_normalState != value)
                {
                    _normalState = value;
                    _normalState.Parent = this;
                    OnPropertyChanged(FormSkinProperty.NormalState);
                }
            }
        }

        [XmlArray("CaptionButtonSkins")]
        [Browsable(false)]
        [Description("The caption buttons")]
        public List<CaptionButtonSkin> CaptionButtonSkins
        {
            get { return _captionButtonSkins; }
        }

        #endregion

        #region CaptionButtonSkins management

        public void AddCaptionButtonSkin(CaptionButtonSkin skin)
        {
            skin.Parent = this;
            _captionButtonSkins.Add(skin);

            OnPropertyChanged(FormSkinProperty.CaptionButton);
        }

        public void RemoveCaptionButtonSkin(CaptionButtonSkin skin)
        {
            skin.Parent = null;
            _captionButtonSkins.Remove(skin);

            OnPropertyChanged(FormSkinProperty.CaptionButton);
        }

        public CaptionButtonSkin GetCaptionButtonSkin(string key)
        {
            foreach (CaptionButtonSkin skin in _captionButtonSkins)
                if (key == skin.Key)
                    return skin;

            return null;
        }

        #endregion
    }
}
