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

#endregion

namespace Lizard.Windows.Skin
{
    public delegate void ChildPropertyChangedEventHandler(object sender, ChildPropertyChangedEventArgs args);

    public class ChildPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        #region Variables

        private SkinObject _subObject;
        
        #endregion

        #region Constructor

        public ChildPropertyChangedEventArgs(SkinObject subObject, string propertyName)
            : base(propertyName)
        {
            _subObject = subObject;
        }

        #endregion

        #region Properties

        public SkinObject SubObject
        {
            get { return _subObject; }
        }

        #endregion
    }
}
