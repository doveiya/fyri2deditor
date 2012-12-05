using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;

namespace Fyri2dEditor
{
    public class XnaModelManager : ModelManagerBase
    {
        ContentBuilder contentBuilder;
        ContentManager contentManager;

        string ProjectOriginalContentFolder;

        public List<FyriModel> modelList = new List<FyriModel>();

        public XnaModelManager(ContentBuilder builder, ContentManager manager, string originalContentFolder)
        {
            contentBuilder = builder;
            contentManager = manager;

            ProjectOriginalContentFolder = originalContentFolder;
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public override List<FyriModel> RefreshList(List<FyriModel> list)
        {
            contentBuilder.Clear();

            for (int i = 0; i < list.Count; i++)
            {
                contentBuilder.Add(list[i].OriginalFileName, list[i].ModelName, null, "ModelProcessor");
            }

            // Build this new model data.
            string buildError = contentBuilder.Build();

            if (string.IsNullOrEmpty(buildError))
            {
                // If the build succeeded, use the ContentManager to
                // load the temporary .xnb file that we just created.
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Model = contentManager.Load<Model>(list[i].ModelName);
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

        public override FyriModel LoadModel(string fileName)
        {
            if (modelList.FirstOrDefault(p => p.OriginalFileName == fileName) != null)
                return modelList.FirstOrDefault(p => p.OriginalFileName == fileName);

            FyriModel newlyAddedModel = new FyriModel();
            newlyAddedModel.OriginalFileName = fileName;

            string pathToMoveTo = ProjectOriginalContentFolder + "\\" + Path.GetFileName(fileName);
            if (!File.Exists(pathToMoveTo))
                File.Copy(fileName, pathToMoveTo);
            newlyAddedModel.FileName = pathToMoveTo;

            newlyAddedModel.XnbFileLocation = contentBuilder.OutputDirectory;

            string modelName = Path.GetFileNameWithoutExtension(fileName);
            newlyAddedModel.ModelName = modelName;

            Model loadedModel = null;

            string xnbFileLoc = newlyAddedModel.XnbFileLocation + "\\" + modelName + ".xnb";
            bool xnbFileExists = File.Exists(xnbFileLoc);
            if (!xnbFileExists)
            {
                // Unload any existing model.
                //xna2dEditorControl.Model = null;
                //contentManager.Unload();

                // Tell the ContentBuilder what to build.
                contentBuilder.Clear();

                contentBuilder.Add(fileName, modelName, null, "ModelProcessor");

                // Build this new model data.
                string buildError = contentBuilder.Build();

                if (string.IsNullOrEmpty(buildError))
                {
                    // If the build succeeded, use the ContentManager to
                    // load the temporary .xnb file that we just created.
                    loadedModel = contentManager.Load<Model>(modelName);
                }
                else
                {
                    // If the build failed, display an error message.
                    MessageBox.Show(buildError, "Error");
                    return null;
                }
            }
            else
                loadedModel = contentManager.Load<Model>(modelName);

            if (loadedModel != null)
            {
                newlyAddedModel.Model = loadedModel;
            }
            else
                return null;

            modelList.Add(newlyAddedModel);
            return newlyAddedModel;
        }

        public override FyriModel GetModel(string modelName)
        {
            throw new NotImplementedException();
        }
    }
}
