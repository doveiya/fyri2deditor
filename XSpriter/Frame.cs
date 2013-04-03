using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FuncWorks.XNA.XSpriter
{
    public struct Frame
    {
        public Bone[] Bones;
        public FrameImage[] Objects; 

        public AnimationTransform ApplyTransform(AnimationTransform baseTransform, Vector2 scale, float angle,
                                                        Vector2 position)
        {
            Matrix m = Matrix.CreateScale(baseTransform.Scale.X, baseTransform.Scale.Y, 0) *
                        Matrix.CreateRotationZ(baseTransform.Angle) *
                        Matrix.CreateTranslation(baseTransform.Position.X, baseTransform.Position.Y, 0);

            AnimationTransform result = new AnimationTransform();
            result.Scale = baseTransform.Scale*scale;
            result.Angle = baseTransform.Angle + angle;
            result.Position = Vector2.Transform(position, m);
            return result;
        }

        public AnimationTransform ApplyBoneTransforms(FrameImage fimg, IList<RuntimeTransform> transforms)
        {

            // Apply transforms from self and/or parent
            AnimationTransform baseTransform = (fimg.Parent != -1)
                                                   ? ApplyBoneTransforms(Bones[fimg.Parent], transforms)
                                                   : new AnimationTransform(0, Vector2.Zero, Vector2.One);

            // Skip if hidden
            if (baseTransform.Hidden)
            {
                AnimationTransform hide = new AnimationTransform();
                hide.Hidden = true;
                return hide;
            }

            AnimationTransform result = ApplyTransform(baseTransform, fimg.Scale, fimg.Angle, fimg.Position);

            // Apply runtime transforms
            for (int i = 0; i < transforms.Count; i++)
            {
                if (transforms[i].TimelineId.HasValue && transforms[i].TimelineId.Value == fimg.TimelineId)
                {
                    result.Angle += transforms[i].Angle;
                    result.Scale *= transforms[i].Scale;
                    result.Position += transforms[i].Position;
                    if (transforms[i].Hidden)
                    {
                        AnimationTransform hide = new AnimationTransform();
                        hide.Hidden = true;
                        return hide;
                    }
                }
            }

            return result;
        }

        public AnimationTransform ApplyBoneTransforms(Bone bone, IList<RuntimeTransform> transforms)
        {
            AnimationTransform baseTransform = (bone.Parent != -1)
                                                   ? ApplyBoneTransforms(Bones[bone.Parent], transforms)
                                                   : new AnimationTransform(0, Vector2.Zero, Vector2.One);

            if (baseTransform.Hidden)
            {
                AnimationTransform hide = new AnimationTransform();
                hide.Hidden = true;
                return hide;
            }

            // Apply runtime transforms
            float runtimeAngle = 0;
            Vector2 runtimeScale = Vector2.One;
            Vector2 runtimePosition = Vector2.Zero;
            for (int i = 0; i < transforms.Count; i++)
            {
                if ((transforms[i].TimelineId.HasValue && transforms[i].TimelineId.Value == bone.TimelineId) ||
                    (!String.IsNullOrEmpty(transforms[i].BoneName) && transforms[i].BoneName == bone.Name))
                {
                    runtimeAngle += transforms[i].Angle;
                    runtimeScale *= transforms[i].Scale;
                    runtimePosition += transforms[i].Position;
                    if (transforms[i].Hidden)
                    {
                        AnimationTransform hide = new AnimationTransform();
                        hide.Hidden = true;
                        return hide;
                    }
                }
            }

            // Apply transforms from self and/or parent
            return ApplyTransform(baseTransform, bone.Scale*runtimeScale, bone.Angle+runtimeAngle, bone.Position+runtimePosition);
        }
    }
}
