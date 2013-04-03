using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace FuncWorks.XNA.XSpriter
{
    [ContentImporter(".scml", DisplayName = "XSpriter - SCML Importer", DefaultProcessor = "SCMLProcessor")]
    public class SCMLImporter : ContentImporter<XDocument>
    {
        public override XDocument Import(string filename, ContentImporterContext context)
        {
            XDocument doc = XDocument.Load(filename);
            doc.Document.Root.Add(new XElement("File",
                new XAttribute("name", Path.GetFileName(filename)),
                new XAttribute("path", Path.GetDirectoryName(filename))));

            return doc;
        }
    }
}
