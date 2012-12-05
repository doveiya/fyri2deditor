using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace Fyri2dEditor
{
    [Serializable()]
    public class FyriTexture2d : ISerializable
    {
        public string TextureName { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string XnbFileLocation { get; set; }
        public Texture2D Texture { get; set; }

        public FyriTexture2d()
        {
            TextureName = string.Empty;
            FileName = string.Empty;
            OriginalFileName = string.Empty;
            XnbFileLocation = string.Empty;

            Texture = null;
        }

        public FyriTexture2d(SerializationInfo info, StreamingContext context)
        {
            TextureName = (String)info.GetValue("TextureName", typeof(string));
            FileName = (String)info.GetValue("FileName", typeof(string));
            XnbFileLocation = (String)info.GetValue("XnbFileLocation", typeof(string));
            OriginalFileName = (String)info.GetValue("OriginalFileName", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("TextureName", TextureName);
            info.AddValue("FileName", FileName);
            info.AddValue("XnbFileLocation", XnbFileLocation);
            info.AddValue("OriginalFileName", OriginalFileName);
        }
    }
}
