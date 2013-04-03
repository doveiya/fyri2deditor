using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FuncWorks.XNA.XSpriter
{
    public class ImageContent : Image
    {
        public String TextureName;
    }

    public class FrameImageContent : FrameImage
    {
        public bool Tweened;
        public Int32 ZIndex;
        public String TextureName;
    }

    public struct FrameContent
    {
        public Bone[] Bones;
        public FrameImageContent[] Objects;  
    }

    public struct AnimationContent
    {
        public String Name;
        public Int64 Length;
        public Boolean Looping;
        public FrameContent[] Frames;
        public Dictionary<String, Int32> TextureTimelines;
        public Dictionary<String, Int32> BoneTimelines;
        public List<RuntimeTransform> Transforms;
    }

    public class CharacterContent
    {
        public ImageContent[][] Textures;
        public List<AnimationContent> Animations;
        public Vector2 Position;
        public Int32 FramesPerSecond;
    }
}
