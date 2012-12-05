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
    public class XnaEffectManager : EffectManagerBase
    {
        ContentBuilder contentBuilder;
        ContentManager contentManager;

        string ProjectOriginalContentFolder;

        public List<FyriEffect> effectList = new List<FyriEffect>();

        public XnaEffectManager(ContentBuilder builder, ContentManager manager, string originalContentFolder)
        {
            contentBuilder = builder;
            contentManager = manager;

            ProjectOriginalContentFolder = originalContentFolder;
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public override List<FyriEffect> RefreshList(List<FyriEffect> list)
        {
            contentBuilder.Clear();

            for (int i = 0; i < list.Count; i++)
            {
                contentBuilder.Add(list[i].FileName, list[i].EffectName, null, "EffectProcessor");
            }

            // Build this new model data.
            string buildError = contentBuilder.Build();

            if (string.IsNullOrEmpty(buildError))
            {
                // If the build succeeded, use the ContentManager to
                // load the temporary .xnb file that we just created.
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Effect = contentManager.Load<Effect>(list[i].EffectName);
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

        public override FyriEffect LoadEffect(string fileName)
        {
            if (effectList.FirstOrDefault(p => p.OriginalFileName == fileName) != null)
                return effectList.FirstOrDefault(p => p.OriginalFileName == fileName);

            FyriEffect newlyAddedEffect = new FyriEffect();
            newlyAddedEffect.OriginalFileName = fileName;

            string pathToMoveTo = ProjectOriginalContentFolder + "\\" + Path.GetFileName(fileName);
            if (!File.Exists(pathToMoveTo))
                File.Copy(fileName, pathToMoveTo);
            newlyAddedEffect.FileName = fileName;

            newlyAddedEffect.XnbFileLocation = contentBuilder.OutputDirectory;

            string effectName = Path.GetFileNameWithoutExtension(fileName);
            newlyAddedEffect.EffectName = effectName;

            Effect loadedEffect = null;

            string xnbFileLoc = newlyAddedEffect.XnbFileLocation + "\\" + effectName + ".xnb";
            bool xnbFileExists = File.Exists(xnbFileLoc);
            if (!xnbFileExists)
            {
                // Unload any existing model.
                //xna2dEditorControl.Model = null;
                //contentManager.Unload();

                // Tell the ContentBuilder what to build.
                contentBuilder.Clear();

                contentBuilder.Add(fileName, effectName, null, "EffectProcessor");

                // Build this new model data.
                string buildError = contentBuilder.Build();

                if (string.IsNullOrEmpty(buildError))
                {
                    // If the build succeeded, use the ContentManager to
                    // load the temporary .xnb file that we just created.
                    loadedEffect = contentManager.Load<Effect>(effectName);
                }
                else
                {
                    // If the build failed, display an error message.
                    MessageBox.Show(buildError, "Error");
                    return null;
                }
            }
            else
                loadedEffect = contentManager.Load<Effect>(effectName);

            if (loadedEffect != null)
            {
                newlyAddedEffect.Effect = loadedEffect;
            }
            else
                return null;

            effectList.Add(newlyAddedEffect);
            return newlyAddedEffect;
        }

        public override FyriEffect GetEffect(string textureName)
        {
            throw new NotImplementedException();
        }
    }
}
