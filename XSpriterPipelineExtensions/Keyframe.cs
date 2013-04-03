using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncWorks.XNA.XSpriter
{
    class Keyframe
    {
        public FrameImageContent[] Objects;
        public Bone[] Bones;
        public Int64 Time;

        public Keyframe Clone(long time)
        {
            Keyframe keyframe = new Keyframe();

            keyframe.Time = time;
            keyframe.Objects = new FrameImageContent[Objects.Length];
            for (int i = 0; i < Objects.Length; i++)
            {
                keyframe.Objects[i] = Objects[i].CloneFrameImage();
            }

            keyframe.Bones = new Bone[Bones.Length];
            for (int i = 0; i < Bones.Length; i++)
            {
                keyframe.Bones[i] = Bones[i].CloneBone();
            }

                return keyframe;
        }
    }
}
