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
using System.IO;

#endregion

namespace Lizard.Windows.Skin
{
    /// <summary>
    /// Global manager of all the skins.
    /// </summary>
    public static class SkinManager
    {
        #region Variables

        static SkinLibrary globalSkinLibrary;
        public static EventHandler SkinChanged;

        #endregion

        #region Save

        public static void Save(string fileName)
        {
            using (Stream stream = File.Create(fileName))
            {
                Save(stream);
            }
        }

        public static void Save(Stream stream)
        {
            if (globalSkinLibrary != null)
                globalSkinLibrary.Save(stream);
        }

        #endregion

        #region Load

        public static void Load(string fileName)
        {
            using (Stream stream = File.OpenRead(fileName))
            {
                Load(stream);
            }
        }

        public static void Load(Stream stream)
        {
            SkinLibrary newLibrary = SkinLibrary.Load(stream);
            LoadHelper(newLibrary);
        }
        
        private static void LoadHelper(SkinLibrary newLibrary)
        {
            if (newLibrary != globalSkinLibrary)
            {
                SkinLibrary oldLibrary = globalSkinLibrary;
                globalSkinLibrary = newLibrary;
                OnSkinChanged();
            }
        }

        public static void Update(string fileName)
        {
            string tempFile = "";
            try
            {
                // update version
                tempFile = SkinLibrary.Update(fileName);
                
                // backup current version
                File.Replace(tempFile, fileName, Path.ChangeExtension(fileName, ".bak"));
            }
            finally
            {
                // remove the temp file
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }

            // load the current version
            Load(fileName);
        }

        #endregion

        #region Reset

        public static void Reset()
        {
            LoadHelper(null);
        }

        #endregion

        #region GetSkin

        public static FormSkin GetSkin(string styleName)
        {
            if (globalSkinLibrary != null)
            {
                foreach (FormSkin style in globalSkinLibrary.Skins)
                    if (styleName == style.Name)
                        return style;
            }

            return null;
        }

        #endregion

        #region GetDefaultSkin

        public static FormSkin GetDefaultSkin()
        {
            if (globalSkinLibrary != null)
                return GetSkin(globalSkinLibrary.DefaultSkinName);

            return null;
        }

        #endregion

        #region GetSkinNames

        public static string[] GetSkinNames()
        {
            List<string> styleNames = new List<string>();
            if (globalSkinLibrary != null)
            {
                foreach (FormSkin style in globalSkinLibrary.Skins)
                    styleNames.Add(style.Name);
            }
            return styleNames.ToArray();
        }

        #endregion

        #region AddNewSkin

        public static FormSkin AddNewSkin()
        {
            FormSkin skin = new FormSkin(true);

            if (globalSkinLibrary == null)
                globalSkinLibrary = new SkinLibrary();

            List<string> styleNames = new List<string>(SkinManager.GetSkinNames());
            skin.Name = "FormSkin";
            for (int i = 1; styleNames.Contains(skin.Name); i++)
                skin.Name = String.Format("FormStyle{0}", i);

            globalSkinLibrary.Skins.Add(skin);
            OnSkinChanged();

            return skin;
        }

        #endregion

        #region DeleteSkin

        public static void DeleteSkin(FormSkin style)
        {
            if (style == null)
                throw new ArgumentNullException("style");

            if (globalSkinLibrary == null)
                return;

            globalSkinLibrary.Skins.Remove(style);
            OnSkinChanged();
        }

        #endregion

        #region OnSkinChanged

        private static void OnSkinChanged()
        {
            if (SkinChanged != null)
                SkinChanged(null, EventArgs.Empty);
        }

        #endregion
    }
}
