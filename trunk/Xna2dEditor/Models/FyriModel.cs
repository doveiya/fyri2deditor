using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace Fyri2dEditor
{
    [Serializable()]
    public class FyriModel : ISerializable
    {
        public string ModelName { get; set; }
        public string FileName { get; set; }
        public string XnbFileLocation {get; set;}
        public string OriginalFileName { get; set; }
        public Model Model { get; set; }

        public FyriModel()
        {
            ModelName = string.Empty;
            FileName = string.Empty;
            XnbFileLocation = string.Empty;
            OriginalFileName = string.Empty;

            Model = null;
        }

        public FyriModel(SerializationInfo info, StreamingContext context)
        {
            ModelName = (String)info.GetValue("ModelName", typeof(string));
            FileName = (String)info.GetValue("FileName", typeof(string));
            XnbFileLocation = (String)info.GetValue("XnbFileLocation", typeof(string));
            OriginalFileName = (String)info.GetValue("OriginalFileName", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ModelName", ModelName);
            info.AddValue("FileName", FileName);
            info.AddValue("XnbFileLocation", XnbFileLocation);
            info.AddValue("OriginalFileName", OriginalFileName);
        }
    }
}
