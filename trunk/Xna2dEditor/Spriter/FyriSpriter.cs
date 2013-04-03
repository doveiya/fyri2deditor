using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using FuncWorks.XNA.XSpriter;

namespace Xna2dEditor
{
    [Serializable()]
    public class FyriSpriter : ISerializable
    {
        public string SpriterName { get; set; }
        public string FileName { get; set; }
        public string XnbFileLocation { get; set; }
        public string OriginalFileName { get; set; }
        public CharacterData CharacterData { get; set; }

        public FyriSpriter()
        {
            SpriterName = string.Empty;
            FileName = string.Empty;
            XnbFileLocation = string.Empty;
            OriginalFileName = string.Empty;

            CharacterData = null;
        }

        public FyriSpriter(SerializationInfo info, StreamingContext context)
        {
            SpriterName = (String)info.GetValue("SpriterName", typeof(string));
            FileName = (String)info.GetValue("FileName", typeof(string));
            XnbFileLocation = (String)info.GetValue("XnbFileLocation", typeof(string));
            OriginalFileName = (String)info.GetValue("OriginalFileName", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SpriterName", SpriterName);
            info.AddValue("FileName", FileName);
            info.AddValue("XnbFileLocation", XnbFileLocation);
            info.AddValue("OriginalFileName", OriginalFileName);
        }
    }
}
