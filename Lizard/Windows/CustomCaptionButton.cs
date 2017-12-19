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

namespace Lizard.Windows
{
    /// <summary>
    /// Used for the custom caption buttons. This kind of button is
    /// non-standard buttons.
    /// </summary>
    sealed public class CustomCaptionButton : CaptionButton
    {
        #region Variables

        static private int GlobalHitTestCounter = short.MaxValue + 1;

        // Summary:
        //     Occurs when the user clicks the CustomCaptionButton control.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public event EventHandler Click;

        #endregion

        #region Constructor

        public CustomCaptionButton()
        {
            Visible = true;
            Enabled = true;
            HitTestCode = GlobalHitTestCounter++; // Custom hit
        }

        #endregion

        #region OnClick

        public void OnClick()
        {
            if (Click != null)
                Click(this, null);
        }

        #endregion
    }
}
