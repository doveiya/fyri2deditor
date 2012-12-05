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
    public class XnaFontManager : FontManagerBase
    {
        ContentBuilder contentBuilder;
        ContentManager contentManager;

        string ProjectOriginalContentFolder;

        public List<FyriFont> fontList = new List<FyriFont>();

        public XnaFontManager(ContentBuilder builder, ContentManager manager, string originalContentFolder)
        {
            contentBuilder = builder;
            contentManager = manager;

            ProjectOriginalContentFolder = originalContentFolder;
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public override List<FyriFont> RefreshList(List<FyriFont> list)
        {
            contentBuilder.Clear();

            for (int i = 0; i < list.Count; i++)
            {
                contentBuilder.Add(list[i].FileName, list[i].FontName, null, "FontTextureProcessor");
            }

            // Build this new model data.
            string buildError = contentBuilder.Build();

            if (string.IsNullOrEmpty(buildError))
            {
                // If the build succeeded, use the ContentManager to
                // load the temporary .xnb file that we just created.
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Font = contentManager.Load<SpriteFont>(list[i].FontName);
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

        public override FyriFont LoadFont(string fileName)
        {
            if (fontList.FirstOrDefault(p => p.OriginalFileName == fileName) != null)
                return fontList.FirstOrDefault(p => p.OriginalFileName == fileName);

            FyriFont newlyAddedFont = new FyriFont();
            newlyAddedFont.OriginalFileName = fileName;

            //string pathToMoveTo = ProjectOriginalContentFolder + Path.GetFileName(fileName);
            //if (!File.Exists(pathToMoveTo))
            //    File.Copy(fileName, pathToMoveTo);
            newlyAddedFont.FileName = fileName;

            newlyAddedFont.XnbFileLocation = contentBuilder.OutputDirectory;

            string fontName = Path.GetFileNameWithoutExtension(fileName);
            newlyAddedFont.FontName = fontName;

            SpriteFont loadedFont = null;

            string xnbFileLoc = newlyAddedFont.XnbFileLocation + "\\" + fontName + ".xnb";
            bool xnbFileExists = File.Exists(xnbFileLoc);
            if (!xnbFileExists)
            {
                // Unload any existing model.
                //xna2dEditorControl.Model = null;
                //contentManager.Unload();

                // Tell the ContentBuilder what to build.
                contentBuilder.Clear();

                string ext = Path.GetExtension(fileName).ToLower();

                if (ext == ".spritefont")
                    contentBuilder.Add(fileName, fontName, null, "FontDescriptionProcessor");
                else
                    contentBuilder.Add(fileName, fontName, null, "FontTextureProcessor");

                // Build this new model data.
                string buildError = contentBuilder.Build();

                if (string.IsNullOrEmpty(buildError))
                {
                    // If the build succeeded, use the ContentManager to
                    // load the temporary .xnb file that we just created.
                    loadedFont = contentManager.Load<SpriteFont>(fontName);
                }
                else
                {
                    // If the build failed, display an error message.
                    MessageBox.Show(buildError, "Error");
                    return null;
                }
            }
            else
                loadedFont = contentManager.Load<SpriteFont>(fontName);

            if (loadedFont != null)
            {
                newlyAddedFont.Font = loadedFont;
            }
            else
                return null;

            fontList.Add(newlyAddedFont);
            return newlyAddedFont;
        }

        public override FyriFont GetFont(string fontName)
        {
            FyriFont fontToReturn = fontList.FirstOrDefault(p => p.FontName == fontName);
            return fontToReturn;
        }
    }
}
