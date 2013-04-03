using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using Xna2dEditor;
using FuncWorks.XNA.XSpriter;

namespace Fyri2dEditor
{
    public class XnaSpriterManager : ISpriterManager
    {
        ContentBuilder contentBuilder;
        ContentManager contentManager;

        string ProjectOriginalContentFolder;

        public List<FyriSpriter> spriterList = new List<FyriSpriter>();

        public XnaSpriterManager(ContentBuilder builder, ContentManager manager, string originalContentFolder)
        {
            contentBuilder = builder;
            contentManager = manager;

            ProjectOriginalContentFolder = originalContentFolder;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public List<FyriSpriter> RefreshList(List<FyriSpriter> list)
        {
            contentBuilder.Clear();

            for (int i = 0; i < list.Count; i++)
            {
                contentBuilder.Add(list[i].OriginalFileName, list[i].SpriterName, null, "SCMLProcessor");
            }

            // Build this new model data.
            string buildError = contentBuilder.Build();

            if (string.IsNullOrEmpty(buildError))
            {
                // If the build succeeded, use the ContentManager to
                // load the temporary .xnb file that we just created.
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].CharacterData = contentManager.Load<CharacterData>(list[i].SpriterName);
                    if (spriterList.FirstOrDefault(p => p.OriginalFileName == list[i].OriginalFileName) == null)
                        spriterList.Add(list[i]);
                }

                return list;
            }
            else
            {
                // If the build failed, display an error message.
                MessageBox.Show(buildError, "Error");
                return null;
            }
        }

        public FyriSpriter LoadSpriter(string fileName)
        {
            if (spriterList.FirstOrDefault(p => p.OriginalFileName == fileName) != null)
                return spriterList.FirstOrDefault(p => p.OriginalFileName == fileName);

            FyriSpriter newlyAddedModel = new FyriSpriter();
            newlyAddedModel.OriginalFileName = fileName;

            string pathToMoveTo = ProjectOriginalContentFolder + "\\" + Path.GetFileName(fileName);
            if (!File.Exists(pathToMoveTo))
                File.Copy(fileName, pathToMoveTo);
            newlyAddedModel.FileName = pathToMoveTo;

            newlyAddedModel.XnbFileLocation = contentBuilder.OutputDirectory;

            string spriterName = Path.GetFileNameWithoutExtension(fileName);
            newlyAddedModel.SpriterName = spriterName;

            CharacterData loadedCharacterData = null;

            string xnbFileLoc = newlyAddedModel.XnbFileLocation + "\\" + spriterName + ".xnb";
            bool xnbFileExists = File.Exists(xnbFileLoc);
            if (!xnbFileExists)
            {
                // Unload any existing model.
                //xna2dEditorControl.Model = null;
                //contentManager.Unload();

                // Tell the ContentBuilder what to build.
                contentBuilder.Clear();

                contentBuilder.Add(fileName, spriterName, null, "SCMLProcesser");

                // Build this new model data.
                string buildError = contentBuilder.Build();

                if (string.IsNullOrEmpty(buildError))
                {
                    // If the build succeeded, use the ContentManager to
                    // load the temporary .xnb file that we just created.
                    loadedCharacterData = contentManager.Load<CharacterData>(spriterName);
                }
                else
                {
                    // If the build failed, display an error message.
                    MessageBox.Show(buildError, "Error");
                    return null;
                }
            }
            else
                loadedCharacterData = contentManager.Load<CharacterData>(spriterName);

            if (loadedCharacterData != null)
            {
                newlyAddedModel.CharacterData = loadedCharacterData;
            }
            else
                return null;

            spriterList.Add(newlyAddedModel);
            return newlyAddedModel;
        }

        public FyriSpriter GetSpriter(string modelName)
        {
            FyriSpriter spriterToReturn = spriterList.FirstOrDefault(p => p.SpriterName == modelName);
            return spriterToReturn;
        }
    }
}
