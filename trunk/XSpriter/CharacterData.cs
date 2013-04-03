using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FuncWorks.XNA.XSpriter
{
    public class CharacterData
    {
        public Image[][] Textures;
        public AnimationList Animations;
        public Int32 FramesPerSecond;

        public int? GetAnimationIdByName(string animationName)
        {
            for (int i = 0; i < Animations.Count; i++)
            {
                if (Animations[i].Name.Equals(animationName))
                {
                    return i;
                }
            }
            return null;
        }

        public CharacterAnimator GetCharacterAnimator()
        {
            return new CharacterAnimator(this);
        }
    }
}
