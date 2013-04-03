using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FuncWorks.XNA.XSpriter
{
    public class CharacterReader : ContentTypeReader<CharacterData>
    {
        protected override CharacterData Read(ContentReader input, CharacterData existingInstance)
        {
            CharacterData character = new CharacterData();
            character.FramesPerSecond = input.ReadInt32();

            int count, count2, count3;

            // Get textures
            count = input.ReadInt32();
            character.Textures = new Image[count][];
            for (int i = 0; i < count; i++)
            {
                count2 = input.ReadInt32();
                character.Textures[i] = new Image[count2];
                for (int j = 0; j < count2; j++)
                {
                    Image img = new Image();
                    img.Pivot = input.ReadVector2();
                    character.Textures[i][j] = img;
                }
            }

            // Get animations
            count = input.ReadInt32();
            character.Animations = new AnimationList();
            for (int i = 0; i < count; i++)
            {
                Animation anim = new Animation();
                anim.Name = input.ReadString();
                anim.Looping = input.ReadBoolean();
                anim.Length = input.ReadInt64();
                
                count2 = input.ReadInt32();
                anim.Frames = new Frame[count2];
                for (int j = 0; j < count2; j++)
                {
                    Frame key = new Frame();
                    count3 = input.ReadInt32();
                    key.Objects = new FrameImage[count3];
                    for (int k = 0; k < count3; k++)
                    {
                        FrameImage img = new FrameImage();
                        img.Angle = input.ReadSingle();
                        img.Clockwise = input.ReadBoolean();
                        img.Pivot = input.ReadVector2();
                        img.Position = input.ReadVector2();
                        img.TextureFolder = input.ReadInt32();
                        img.TextureFile = input.ReadInt32();
                        img.TimelineId = input.ReadInt32();
                        img.Parent = input.ReadInt32();
                        img.Scale = input.ReadVector2();
                        
                        key.Objects[k] = img;
                    }

                    count3 = input.ReadInt32();
                    key.Bones = new Bone[count3];
                    for (int k = 0; k < count3; k++)
                    {
                        Bone bone = new Bone();
                        bone.Angle = input.ReadSingle();
                        bone.Clockwise = input.ReadBoolean();
                        bone.Id = input.ReadInt32();
                        bone.Parent = input.ReadInt32();
                        bone.Position = input.ReadVector2();
                        bone.Scale = input.ReadVector2();
                        bone.TimelineId = input.ReadInt32();
                        bone.Name = input.ReadString();

                        key.Bones[k] = bone;
                    }
                    anim.Frames[j] = key;
                }

                // Read timeline/texture lookups
                count2 = input.ReadInt32();
                anim.TextureTimelines = new Dictionary<string, int>();
                for (int k = 0; k < count2; k++)
                {
                    anim.TextureTimelines[input.ReadString()] = input.ReadInt32();
                }

                // Read timeline/bone lookups
                count2 = input.ReadInt32();
                anim.BoneTimelines = new Dictionary<string, int>();
                for (int k = 0; k < count2; k++)
                {
                    anim.BoneTimelines[input.ReadString()] = input.ReadInt32();
                }

                character.Animations.Add(anim);
            }

            // Load textures
            for (int i = 0; i < character.Textures.Length; i++)
            {
                for (int j = 0; j < character.Textures[i].Length; j++)
                {
                    character.Textures[i][j].Texture =
                        input.ContentManager.Load<Texture2D>(String.Format("{0}/{1}/{2}", input.AssetName,
                                                                            i.ToString("00"), j.ToString("00")));
                }
            }

            return character;
        }
    }
}
