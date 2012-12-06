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
    public class XnaTexture2dManager : Texture2dManagerBase
    {
        ContentBuilder contentBuilder;
        ContentManager contentManager;

        string ProjectOriginalContentFolder;

        public List<FyriTexture2d> textureList = new List<FyriTexture2d>();

        public XnaTexture2dManager(ContentBuilder builder, ContentManager manager, string originalContentFolder)
        {
            contentBuilder = builder;
            contentManager = manager;

            ProjectOriginalContentFolder = originalContentFolder;
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public override List<FyriTexture2d> RefreshList(List<FyriTexture2d> list)
        {
            contentBuilder.Clear();

            for (int i = 0; i < list.Count; i++)
            {
                contentBuilder.Add(list[i].FileName, list[i].TextureName, null, "TextureProcessor");
            }

            // Build this new model data.
            string buildError = contentBuilder.Build();

            if (string.IsNullOrEmpty(buildError))
            {
                // If the build succeeded, use the ContentManager to
                // load the temporary .xnb file that we just created.
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Texture = contentManager.Load<Texture2D>(list[i].TextureName);
                    if (textureList.FirstOrDefault(p => p.OriginalFileName == list[i].OriginalFileName) == null)
                        textureList.Add(list[i]);
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

        public override FyriTexture2d LoadTexture2d(string fileName)
        {
            if (textureList.FirstOrDefault(p => p.OriginalFileName == fileName) != null)
                return textureList.FirstOrDefault(p => p.OriginalFileName == fileName);

            FyriTexture2d newlyAddedTexture = new FyriTexture2d();
            newlyAddedTexture.OriginalFileName = fileName;

            string pathToMoveTo = ProjectOriginalContentFolder + "\\" + Path.GetFileName(fileName);
            if (!File.Exists(pathToMoveTo))
                File.Copy(fileName, pathToMoveTo);
            newlyAddedTexture.FileName = pathToMoveTo;

            newlyAddedTexture.XnbFileLocation = contentBuilder.OutputDirectory;

            string textureName = Path.GetFileNameWithoutExtension(fileName);
            newlyAddedTexture.TextureName = textureName;

            Texture2D loadedTexture = null;

            string xnbFileLoc = newlyAddedTexture.XnbFileLocation + "\\" + textureName + ".xnb";
            bool xnbFileExists = File.Exists(xnbFileLoc);
            if (!xnbFileExists)
            {
                // Unload any existing model.
                //xna2dEditorControl.Model = null;
                //contentManager.Unload();

                // Tell the ContentBuilder what to build.
                contentBuilder.Clear();

                contentBuilder.Add(fileName, textureName, null, "TextureProcessor");

                // Build this new model data.
                string buildError = contentBuilder.Build();

                if (string.IsNullOrEmpty(buildError))
                {
                    // If the build succeeded, use the ContentManager to
                    // load the temporary .xnb file that we just created.
                    loadedTexture = contentManager.Load<Texture2D>(textureName);
                }
                else
                {
                    // If the build failed, display an error message.
                    MessageBox.Show(buildError, "Error");
                    return null;
                }
            }
            else
                loadedTexture = contentManager.Load<Texture2D>(textureName);

            if (loadedTexture != null)
            {
                newlyAddedTexture.Texture = loadedTexture;
            }
            else
                return null;

            textureList.Add(newlyAddedTexture);
            return newlyAddedTexture;
        }

        public override FyriTexture2d GetTexture2d(string textureName)
        {
            FyriTexture2d textureToReturn = textureList.FirstOrDefault(p => p.TextureName == textureName);
            return textureToReturn;
        }
    }
}
