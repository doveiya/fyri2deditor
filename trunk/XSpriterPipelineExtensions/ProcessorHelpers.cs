using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FuncWorks.XNA.XSpriter
{
    static class ProcessorHelpers
    {

        static public FrameImageContent CloneFrameImage(this FrameImageContent fimg)
        {
            return fimg.CloneFrameImage(null, null, null);
        }

        static public FrameImageContent CloneFrameImage(this FrameImageContent fimg, int? zindex, int? timelineId, int? parent)
        {
            return new FrameImageContent()
                       {
                           Angle = fimg.Angle,
                           Clockwise = fimg.Clockwise,
                           Pivot = fimg.Pivot,
                           Position = fimg.Position,
                           TextureFile = fimg.TextureFile,
                           TextureFolder = fimg.TextureFolder,
                           TextureName = fimg.TextureName,
                           Tweened = fimg.Tweened,
                           ZIndex = zindex.HasValue ? zindex.Value : fimg.ZIndex,
                           TimelineId = timelineId.HasValue ? timelineId.Value : fimg.TimelineId,
                           Parent = parent.HasValue ? parent.Value : fimg.Parent,
                           Scale = fimg.Scale
                       };
        }

        static public Bone CloneBone(this Bone bone)
        {
            return bone.CloneBone(null, null);
        }

        static public Bone CloneBone(this Bone bone, int? timelineId, int? parentId)
        {
            return new Bone()
                       {
                           Id = bone.Id,
                           Parent = parentId.HasValue ? parentId.Value : bone.Parent,
                           Angle = bone.Angle,
                           Position = bone.Position,
                           TimelineId = timelineId.HasValue ? timelineId.Value : bone.TimelineId,
                           Name = bone.Name,
                           Clockwise = bone.Clockwise,
                           Scale = bone.Scale
                       };
        }

        static public Vector2 Rotate(this Vector2 position, Vector2 pivot, float angle)
        {
            Quaternion rot = new Quaternion(pivot.X, pivot.Y, 0, angle);
            return Vector2.Transform(position, rot);

        }

        static public FrameImageContent Tween(this FrameImageContent prev, FrameImageContent next, float pct)
        {
            FrameImageContent result = new FrameImageContent();

            result.TextureFolder = prev.TextureFolder;
            result.TextureFile = prev.TextureFile;
            result.Clockwise = prev.Clockwise;
            result.Pivot = prev.Pivot;
            result.Parent = prev.Parent;
            result.TimelineId = prev.TimelineId;

            result.Scale.X = MathHelper.Lerp(prev.Scale.X, next.Scale.X, pct);
            result.Scale.Y = MathHelper.Lerp(prev.Scale.Y, next.Scale.Y, pct);

            result.Position.X = MathHelper.Lerp(prev.Position.X, next.Position.X, pct);
            result.Position.Y = MathHelper.Lerp(prev.Position.Y, next.Position.Y, pct);
            
            double angleB = prev.Clockwise
                ? ((next.Angle - prev.Angle < 0) ? (next.Angle + MathHelper.TwoPi) : next.Angle)
                : ((next.Angle - prev.Angle > 0) ? (next.Angle - MathHelper.TwoPi) : next.Angle);

            result.Angle = MathHelper.Lerp(prev.Angle, (float)angleB, pct);

            return result;
        }

        static public Bone Tween(this Bone prev, Bone next, float pct)
        {
            Bone result = new Bone();

            result.Clockwise = prev.Clockwise;
            result.Id = prev.Id;
            result.Parent = prev.Parent;
            result.TimelineId = prev.TimelineId;
            result.Name = prev.Name;

            result.Position.X = MathHelper.Lerp(prev.Position.X, next.Position.X, pct);
            result.Position.Y = MathHelper.Lerp(prev.Position.Y, next.Position.Y, pct);

            result.Scale.X = MathHelper.Lerp(prev.Scale.X, next.Scale.X, pct);
            result.Scale.Y = MathHelper.Lerp(prev.Scale.Y, next.Scale.Y, pct);

            double angleB = prev.Clockwise
                ? ((next.Angle - prev.Angle < 0) ? (next.Angle + MathHelper.TwoPi) : next.Angle)
                : ((next.Angle - prev.Angle > 0) ? (next.Angle - MathHelper.TwoPi) : next.Angle);

            result.Angle = MathHelper.Lerp(prev.Angle, (float)angleB, pct);

            return result;
        }
    }
}
