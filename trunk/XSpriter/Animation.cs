using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncWorks.XNA.XSpriter
{
    public struct Animation
    {
        public String Name;
        public Int64 Length;
        public Boolean Looping;
        public Frame[] Frames;
        public Dictionary<String, Int32> TextureTimelines;
        public Dictionary<String, Int32> BoneTimelines;

        public int? GetTimelineIdByTextureName(string textureName)
        {
            return TextureTimelines.ContainsKey(textureName) ? (int?)TextureTimelines[textureName] : null;
        }

        public int? GetTimelineIdByBoneName(string boneName)
        {
            return BoneTimelines.ContainsKey(boneName) ? (int?)BoneTimelines[boneName] : null;
        }
    }
}
