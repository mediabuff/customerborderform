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
using System.Xml.Serialization;

#endregion

namespace Lizard.Windows.Skin
{
    public abstract class SkinObject : INotifyPropertyChanged
    {
        #region Variables

        private SkinObject _parent;
        public event PropertyChangedEventHandler PropertyChanged;
        public event ChildPropertyChangedEventHandler ChildPropertyChanged;

        #endregion

        #region Properties

        [XmlIgnore]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SkinObject Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        #endregion

        #region On...

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            OnChildPropertyChanged(this, propertyName);
        }

        protected virtual void OnChildPropertyChanged(SkinObject subObject, string propertyName)
        {
            if (ChildPropertyChanged != null)
                ChildPropertyChanged(this, new ChildPropertyChangedEventArgs(subObject, propertyName));

            if (Parent != null)
                Parent.OnChildPropertyChanged(subObject, propertyName);
        }

        #endregion
    }
}
