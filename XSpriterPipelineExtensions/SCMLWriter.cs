using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace FuncWorks.XNA.XSpriter
{
    [ContentTypeWriter]
    public class SCMLWriter : ContentTypeWriter<CharacterContent>
    {
        protected override void Write(ContentWriter output, CharacterContent value)
        {
            output.Write(value.FramesPerSecond);

            output.Write(value.Textures.Length);
            foreach (Image[] folder in value.Textures)
            {
                output.Write(folder.Length);
                foreach (Image img in folder)
                {
                    output.Write(img.Pivot);
                }
            }

            output.Write(value.Animations.Count);
            foreach (AnimationContent anim in value.Animations)
            {
                output.Write(anim.Name);
                output.Write(anim.Looping);
                output.Write(anim.Length);
                output.Write(anim.Frames.Length);
                foreach (FrameContent frame in anim.Frames)
                {
                    output.Write(frame.Objects.Length);
                    foreach (FrameImage img in frame.Objects)
                    {
                        output.Write(img.Angle);
                        output.Write(img.Clockwise);
                        output.Write(img.Pivot);
                        output.Write(img.Position);
                        output.Write(img.TextureFolder);
                        output.Write(img.TextureFile);
                        output.Write(img.TimelineId);
                        output.Write(img.Parent);
                        output.Write(img.Scale);
                    }

                    output.Write(frame.Bones.Length);
                    foreach (Bone bone in frame.Bones)
                    {
                        output.Write(bone.Angle);
                        output.Write(bone.Clockwise);
                        output.Write(bone.Id);
                        output.Write(bone.Parent);
                        output.Write(bone.Position);
                        output.Write(bone.Scale);
                        output.Write(bone.TimelineId);
                        output.Write(!String.IsNullOrEmpty(bone.Name) ? bone.Name : String.Empty);
                    }
                }

                output.Write(anim.TextureTimelines.Count);
                foreach (string key in anim.TextureTimelines.Keys)
                {
                    output.Write(key);
                    output.Write(anim.TextureTimelines[key]);
                }

                output.Write(anim.BoneTimelines.Count);
                foreach (string key in anim.BoneTimelines.Keys)
                {
                    output.Write(key);
                    output.Write(anim.BoneTimelines[key]);
                }
            }
        }

        public override string GetRuntimeReader(Microsoft.Xna.Framework.Content.Pipeline.TargetPlatform targetPlatform)
        {
            return "FuncWorks.XNA.XSpriter.CharacterReader, FuncWorks.XNA.XSpriter";
        }

        public override string GetRuntimeType(Microsoft.Xna.Framework.Content.Pipeline.TargetPlatform targetPlatform)
        {
            return "FuncWorks.XNA.XSpriter.CharacterData, FuncWorks.XNA.XSpriter";
        }
    }
}
