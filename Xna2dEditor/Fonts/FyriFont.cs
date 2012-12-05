using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace Fyri2dEditor
{
    [Serializable()]
    public class FyriFont : ISerializable
    {
        public string FontName { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string XnbFileLocation { get; set; }
        public SpriteFont Font { get; set; }

        public FyriFont()
        {
            FontName = string.Empty;
            FileName = string.Empty;
            OriginalFileName = string.Empty;
            XnbFileLocation = string.Empty;

            Font = null;
        }

        public FyriFont(SerializationInfo info, StreamingContext context)
        {
            FontName = (String)info.GetValue("FontName", typeof(string));
            FileName = (String)info.GetValue("FileName", typeof(string));
            XnbFileLocation = (String)info.GetValue("XnbFileLocation", typeof(string));
            OriginalFileName = (String)info.GetValue("OriginalFileName", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FontName", FontName);
            info.AddValue("FileName", FileName);
            info.AddValue("XnbFileLocation", XnbFileLocation);
            info.AddValue("OriginalFileName", OriginalFileName);
        }
    }
}
