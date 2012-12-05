using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace Fyri2dEditor
{
    [Serializable()]
    public class FyriEffect : ISerializable
    {
        public string EffectName { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string XnbFileLocation { get; set; }
        public Effect Effect { get; set; }

        public FyriEffect()
        {
            EffectName = string.Empty;
            FileName = string.Empty;
            OriginalFileName = string.Empty;
            XnbFileLocation = string.Empty;

            Effect = null;
        }

        public FyriEffect(SerializationInfo info, StreamingContext context)
        {
            EffectName = (String)info.GetValue("FontName", typeof(string));
            FileName = (String)info.GetValue("FileName", typeof(string));
            XnbFileLocation = (String)info.GetValue("XnbFileLocation", typeof(string));
            OriginalFileName = (String)info.GetValue("OriginalFileName", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FontName", EffectName);
            info.AddValue("FileName", FileName);
            info.AddValue("XnbFileLocation", XnbFileLocation);
            info.AddValue("OriginalFileName", OriginalFileName);
        }
    }
}
