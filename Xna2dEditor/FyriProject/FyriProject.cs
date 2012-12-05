using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Fyri2dEditor
{
    [Serializable()]
    public class FyriProject : ISerializable
    {
        public int ProjectID;
        public string ProjectName;
        public string ProjectFileName;
        public string ProjectContentFolder;
        public string OriginalContentFolder;

        public bool IsDirty { get; set; }

        public List<FyriModel> LoadedModels = new List<FyriModel>();
        public List<FyriTexture2d> LoadedTexture2ds = new List<FyriTexture2d>();
        public List<FyriFont> LoadedFonts = new List<FyriFont>();
        public List<FyriEffect> LoadedEffects = new List<FyriEffect>();

        public FyriProject()
        {
            ProjectID = 0;
            ProjectName = null;
            ProjectFileName = null;
            ProjectContentFolder = null;
            OriginalContentFolder = null;

            IsDirty = false;
        }

        //Deserialization constructor.
        public FyriProject(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            ProjectID = (int)info.GetValue("ProjectID", typeof(int));
            ProjectName = (String)info.GetValue("ProjectName", typeof(string));
            ProjectFileName = (String)info.GetValue("ProjectFileName", typeof(string));
            ProjectContentFolder = (String)info.GetValue("ProjectContentFolder", typeof(string));

            OriginalContentFolder = ProjectContentFolder + "\\Original";
            if (!Directory.Exists(OriginalContentFolder))
                Directory.CreateDirectory(OriginalContentFolder);

            LoadedModels = (List<FyriModel>)info.GetValue("ModelList", typeof(List<FyriModel>));
            LoadedTexture2ds = (List<FyriTexture2d>)info.GetValue("Texture2dList", typeof(List<FyriTexture2d>));
            LoadedFonts = (List<FyriFont>)info.GetValue("FontList", typeof(List<FyriFont>));
            LoadedEffects = (List<FyriEffect>)info.GetValue("EffectList", typeof(List<FyriEffect>));


            IsDirty = false;
        }
        
        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            //You can use any custom name for your name-value pair. But make sure you
            // read the values with the same name. For ex:- If you write EmpId as "EmployeeId"
            // then you should read the same with "EmployeeId"
            info.AddValue("ProjectID", ProjectID);
            info.AddValue("ProjectName", ProjectName);
            info.AddValue("ProjectFileName", ProjectFileName);
            info.AddValue("ProjectContentFolder", ProjectContentFolder);
            info.AddValue("ModelList", LoadedModels);
            info.AddValue("Texture2dList", LoadedTexture2ds);
            info.AddValue("FontList", LoadedFonts);
            info.AddValue("EffectList", LoadedEffects);
        }
    }
}
