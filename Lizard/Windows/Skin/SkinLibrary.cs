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
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using Lizard.Windows.Skin;
using System.Xml.Xsl;
using System.Resources;

#endregion

namespace Lizard.Windows.Skin
{
    [System.Xml.Serialization.XmlRoot("styleLibrary")]
    public class SkinLibrary : SkinObject
    {
        #region Variables

        const string CurrentSchemaVersion = "1.1";

        private List<FormSkin> _skins = new List<FormSkin>();
        private string _version = CurrentSchemaVersion;
        private string _defaultSkinName = "Default";

        #endregion

        #region Properties

        [XmlArray("styles")]
        [Description("List of styles contained in this library")]
        public List<FormSkin> Skins
        {
            get { return _skins; }
        }

        [XmlAttribute("schemaVersion")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Description("Version of Xml Schema the style library was saved")]
        public string SchemaVersion
        {
            get { return _version; }
            set { _version = value; }
        }

        [XmlAttribute("defaultStyleName")]
        [Description("Name of default style in this library")]
        public string DefaultSkinName
        {
            get { return _defaultSkinName; }
            set { _defaultSkinName = value; }
        }

        #endregion

        #region Load / Save

        internal static SkinLibrary Load(Stream stream)
        {
            CheckSchemaVersion(stream);

            using (XmlReader reader = XmlReader.Create(stream))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SkinLibrary));
                SkinLibrary library = (SkinLibrary)serializer.Deserialize(reader);
                return library;
            }
        }

        internal void Save(Stream stream)
        {
            XmlWriterSettings set = new XmlWriterSettings();
            set.Indent = true;

            using (XmlWriter wr = XmlWriter.Create(stream, set))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SkinLibrary));
                serializer.Serialize(wr, this);
            }
        }

        internal static void CheckSchemaVersion(Stream stream)
        {
            string version = GetSchemaVersion(stream);
            if (stream.CanSeek)
                stream.Seek(0, SeekOrigin.Begin);

            if (version != CurrentSchemaVersion)
                throw new InvalidVersionException(version, CurrentSchemaVersion);
        }

        /// <summary>
        /// Reads the schemaVersion attribute from the root element in input stream.
        /// </summary>
        private static string GetSchemaVersion(Stream stream)
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                        return reader.GetAttribute("schemaVersion");
                }
                return "";
            }
        }

        #endregion

        internal static string Update(string fileName)
        {
            string version;
            using (Stream stream = File.OpenRead(fileName))
            {
                version = GetSchemaVersion(stream);
            }

            if (version == CurrentSchemaVersion)
                return fileName;

            switch (version)
            {
                case "1.0":
                    string tempFile = ApplyUpdateTransform(fileName, "Update_1_0_to_1_1.xslt");
                    return Update(tempFile);
                default:
                    throw new InvalidOperationException(
                        string.Format("Skin libary in version {0} can't be updated to current version.", version));
            }
        }

        private static string ApplyUpdateTransform(string fileName, string templateName)
        {
            Type refType = typeof(SkinLibrary);
            Stream templateStream = refType.Assembly.GetManifestResourceStream(refType, templateName);

            XslCompiledTransform xslt = new XslCompiledTransform();
           
            // load transformation
            using (XmlReader xsltReader = XmlReader.Create(templateStream))
            {
                xslt.Load(xsltReader);
            }

            // create temporary file
            string tempFile = Path.GetTempFileName();
            
            // apply transformation
            xslt.Transform(fileName, tempFile);

            return tempFile;
        }
    }
}
