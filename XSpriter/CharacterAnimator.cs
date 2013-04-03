using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FuncWorks.XNA.XSpriter
{
    public class CharacterAnimator
    {
        public CharacterData Data;

        public Boolean Playing;
        public Vector2 Position;
        public Double Timer;
        public Int32 CurrentAnimation;
        public Int32 FrameIndex;
        public Boolean FlipX;
        public Boolean FlipY;

        public List<List<RuntimeTransform>> Transforms;

        public float LayerStart = 0;
        public float LayerStep = 0.01f;

        public CharacterAnimator(CharacterData data)
        {
            Data = data;
            Transforms = new List<List<RuntimeTransform>>();
            for (int i = 0; i < data.Animations.Count; i++)
            {
                Transforms.Add(new List<RuntimeTransform>());
            }
        }

        public void ResetAnimation()
        {
            Timer = 0;
            FrameIndex = 0;
        }

        public void SetAnimation(int animationIndex)
        {
            if (animationIndex != CurrentAnimation)
            {
                CurrentAnimation = animationIndex;
                ResetAnimation();
            }
        }

        public void SetAnimation(string animationName)
        {
            int? id = Data.GetAnimationIdByName(animationName);
            if (id.HasValue)
            {
                SetAnimation(id.Value);
            }
        }

        public Animation GetCurrentAnimation()
        {
            return Data.Animations[CurrentAnimation];
        }

        public float? GetCurrentLayerForTimeline(int timelineId)
        {
            float result = LayerStart;
            for (int i = 0; i < Data.Animations[CurrentAnimation].Frames[FrameIndex].Objects.Length; i++)
            {
                if (Data.Animations[CurrentAnimation].Frames[FrameIndex].Objects[i].TimelineId.Equals(timelineId))
                {
                    return result;
                }
                result += LayerStep;
            }
            return null;
        }

        public RenderedPosition? GetCurrentRenderedPositionForTimeline(int timelineId)
        {
            // Get the frame image
            FrameImage fimg = null;
            Frame frame = Data.Animations[CurrentAnimation].Frames[FrameIndex];
            float layer = LayerStart;
            foreach (FrameImage fi in frame.Objects)
            {
                if (fi.TimelineId.Equals(timelineId))
                {
                    fimg = fi;
                    break;
                }
                layer += LayerStep;
            }

            if (fimg == null)
            {
                return null;
            }

            RenderedPosition result = GetRenderedPosition(frame, fimg);
            result.Layer = layer;
            return result;
        }

        public RenderedPosition GetRenderedPosition(Frame frame, FrameImage fimg)
        {
            // Apply transforms
            AnimationTransform transform = frame.ApplyBoneTransforms(fimg, Transforms[CurrentAnimation]);
            RenderedPosition result = new RenderedPosition();

            result.Positon = Vector2.Zero;
            result.Positon.Y = Position.Y + (transform.Position.Y * (FlipY ? -1 : 1));
            result.Positon.X = Position.X + (transform.Position.X * (FlipX ? -1 : 1));

            bool flipX = FlipX;
            bool flipY = FlipY;

            result.Angle = transform.Angle;
            if (flipX != flipY) { result.Angle *= -1; }

            result.Pivot = fimg.Pivot;
            if (flipX) { result.Pivot.X = Data.Textures[fimg.TextureFolder][fimg.TextureFile].Texture.Width - result.Pivot.X; }
            if (flipY) { result.Pivot.Y = Data.Textures[fimg.TextureFolder][fimg.TextureFile].Texture.Height - result.Pivot.Y; }

            result.Scale = transform.Scale;
            if (result.Scale.X < 0)
            {
                result.Scale.X = -result.Scale.X;
                flipX = !flipX;
            }
            if (result.Scale.Y < 0)
            {
                result.Scale.Y = -result.Scale.Y;
                flipY = !flipY;
            }

            result.Effects = SpriteEffects.None;
            if (flipX) { result.Effects |= SpriteEffects.FlipHorizontally; }
            if (flipY) { result.Effects |= SpriteEffects.FlipVertically; }

            return result;
        }

        public void Update(GameTime gameTime)
        {
            if (Playing)
            {
                Timer += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (Timer > Data.Animations[CurrentAnimation].Length)
                {
                    if (Data.Animations[CurrentAnimation].Looping)
                    {
                        Timer %= Data.Animations[CurrentAnimation].Length;
                    }
                    else
                    {
                        Timer = Data.Animations[CurrentAnimation].Length;
                    }
                }
                FrameIndex =
                    (int)MathHelper.Clamp(
                    (int)Math.Ceiling((Timer / 1000.0) * Data.FramesPerSecond),
                    0, Data.Animations[CurrentAnimation].Frames.Length - 1);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Frame frame = Data.Animations[CurrentAnimation].Frames[FrameIndex];

            float layer = LayerStart;
            foreach (FrameImage ki in frame.Objects)
            {
                RenderedPosition pos = GetRenderedPosition(frame, ki);

                spriteBatch.Draw(Data.Textures[ki.TextureFolder][ki.TextureFile].Texture, pos.Positon, null, Color.White,
                                 pos.Angle, pos.Pivot, pos.Scale, pos.Effects, layer);

                layer += LayerStep;
            }
        }
    }
}
