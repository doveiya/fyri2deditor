﻿

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace Fyri.Xna.Presentation
{
    internal class GraphicsDeviceInformationComparer : System.Collections.Generic.IComparer<GraphicsDeviceInformation>
    {
        private GraphicsDeviceManager graphics;

        public GraphicsDeviceInformationComparer(GraphicsDeviceManager graphicsComponent)
        {
            this.graphics = graphicsComponent;
        }

        public int Compare(GraphicsDeviceInformation d1, GraphicsDeviceInformation d2)
        {
            float num5;
            if (d1.Adapter.DeviceId != d2.Adapter.DeviceId)
            {
                if (d1.Adapter.DeviceId >= d2.Adapter.DeviceId)
                {
                    return 1;
                }
                return -1;
            }
            PresentationParameters presentationParameters = d1.PresentationParameters;
            PresentationParameters parameters2 = d2.PresentationParameters;
            if (presentationParameters.IsFullScreen != parameters2.IsFullScreen)
            {
                if (this.graphics.IsFullScreen != presentationParameters.IsFullScreen)
                {
                    return 1;
                }
                return -1;
            }
            int num = this.RankFormat(presentationParameters.BackBufferFormat);
            int num2 = this.RankFormat(parameters2.BackBufferFormat);
            if (num != num2)
            {
                if (num >= num2)
                {
                    return 1;
                }
                return -1;
            }
            if (presentationParameters.MultiSampleCount != parameters2.MultiSampleCount)
            {
                if (presentationParameters.MultiSampleCount <= parameters2.MultiSampleCount)
                {
                    return 1;
                }
                return -1;
            }
            if ((this.graphics.PreferredBackBufferWidth == 0) || (this.graphics.PreferredBackBufferHeight == 0))
            {
                num5 = ((float)GraphicsDeviceManager.m_iDefaultBackBufferWidth) / ((float)GraphicsDeviceManager.m_iDefaultBackBufferHeight);
            }
            else
            {
                num5 = ((float)this.graphics.PreferredBackBufferWidth) / ((float)this.graphics.PreferredBackBufferHeight);
            }
            float num6 = ((float)presentationParameters.BackBufferWidth) / ((float)presentationParameters.BackBufferHeight);
            float num7 = ((float)parameters2.BackBufferWidth) / ((float)parameters2.BackBufferHeight);
            float num8 = Math.Abs((float)(num6 - num5));
            float num9 = Math.Abs((float)(num7 - num5));
            if (Math.Abs((float)(num8 - num9)) > 0.2f)
            {
                if (num8 >= num9)
                {
                    return 1;
                }
                return -1;
            }
            int num10 = 0;
            int num11 = 0;
            if (this.graphics.IsFullScreen)
            {
                if ((this.graphics.PreferredBackBufferWidth == 0) || (this.graphics.PreferredBackBufferHeight == 0))
                {
                    GraphicsAdapter adapter = d1.Adapter;
                    num10 = adapter.CurrentDisplayMode.Width * adapter.CurrentDisplayMode.Height;
                    GraphicsAdapter adapter2 = d2.Adapter;
                    num11 = adapter2.CurrentDisplayMode.Width * adapter2.CurrentDisplayMode.Height;
                }
                else
                {
                    num10 = num11 = this.graphics.PreferredBackBufferWidth * this.graphics.PreferredBackBufferHeight;
                }
            }
            else if ((this.graphics.PreferredBackBufferWidth == 0) || (this.graphics.PreferredBackBufferHeight == 0))
            {
                num10 = num11 = GraphicsDeviceManager.m_iDefaultBackBufferWidth * GraphicsDeviceManager.m_iDefaultBackBufferHeight;
            }
            else
            {
                num10 = num11 = this.graphics.PreferredBackBufferWidth * this.graphics.PreferredBackBufferHeight;
            }
            int num12 = Math.Abs((int)((presentationParameters.BackBufferWidth * presentationParameters.BackBufferHeight) - num10));
            int num13 = Math.Abs((int)((parameters2.BackBufferWidth * parameters2.BackBufferHeight) - num11));
            if (num12 != num13)
            {
                if (num12 >= num13)
                {
                    return 1;
                }
                return -1;
            }
            if (this.graphics.IsFullScreen && (presentationParameters.IsFullScreen))
            {

                    return 1;
                
                return -1;
            }
            if (d1.Adapter != d2.Adapter)
            {
                if (d1.Adapter.IsDefaultAdapter)
                {
                    return -1;
                }
                if (d2.Adapter.IsDefaultAdapter)
                {
                    return 1;
                }
            }
            return 0;
        }

        private int RankFormat(SurfaceFormat format)
        {
            int index = Array.IndexOf<SurfaceFormat>(GraphicsDeviceManager.ValidBackBufferFormats, format);
            if (index != -1)
            {
                int num2 = Array.IndexOf<SurfaceFormat>(GraphicsDeviceManager.ValidBackBufferFormats, this.graphics.PreferredBackBufferFormat);
                if (num2 == -1)
                {
                    return (GraphicsDeviceManager.ValidBackBufferFormats.Length - index);
                }
                if (index >= num2)
                {
                    return (index - num2);
                }
            }
            return 0x7fffffff;
        }
    }
}
