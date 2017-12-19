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

namespace Samples.CustomCaption
{
    /// <summary>
    /// A simple Vista Form, used for Design time
    /// </summary>
    public class VistaForm : SkinnedForm
    {
        #region Constructor

        public VistaForm():base()
        {
            // Setup default style
            if (DesignMode)
                SkinManager.Load("Skins\\Vista.fsl");
            else
                SkinManager.Load(Application.StartupPath + "\\..\\..\\..\\..\\Skins\\Vista.fsl");
        }

        #endregion
    }
}
