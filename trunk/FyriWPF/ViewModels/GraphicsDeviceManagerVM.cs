using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Fyri.Xna.Presentation;

namespace FyriWPF.ViewModels
{
    class GraphicsDeviceManagerVM : ViewModelBase
    {
        private GraphicsDeviceManager m_objGraphicsDeviceManager = null;
        public GraphicsDeviceManager Manager
        {
            get
            {
                return m_objGraphicsDeviceManager;
            }
            set
            {
                if (m_objGraphicsDeviceManager != value)
                {
                    m_objGraphicsDeviceManager = value;

                    if (m_objGraphicsDeviceManager != null)
                    {
                        //DoesAllowMultiSampling = m_objGraphicsDeviceManager.Doe
                    }
                    RaisePropertyChanged("Manager");
                }
            }
        }
        private bool m_bDoesAllowMultiSampling;
        private SurfaceFormat CurrentBackBufferFormat = SurfaceFormat.Color;

        public bool DoesAllowMultiSampling
        {
            get
            {
                return m_bDoesAllowMultiSampling;
            }
            set
            {
                RaisePropertyChanged("DoesAlloMultiSampling");
            }
        }

        private int backBufferHeight = DefaultBackBufferHeight;
        private int backBufferWidth = DefaultBackBufferWidth;
        private bool beginDrawOk;
        public static readonly int DefaultBackBufferHeight = 600;
        public static readonly int DefaultBackBufferWidth = 800;
        private static DepthFormat[] depthFormatsWithoutStencil;
        private static DepthFormat[] depthFormatsWithStencil;
        private DepthFormat depthStencilFormat = DepthFormat.Depth24;
        private GraphicsDevice device;
        private static readonly TimeSpan deviceLostSleepTime = TimeSpan.FromMilliseconds(50.0);
        private Game game;
        private bool inDeviceTransition;
        private bool isDeviceDirty;
        private bool isFullScreen;
        private bool isReallyFullScreen;
        private int resizedBackBufferHeight;
        private int resizedBackBufferWidth;
        private bool synchronizeWithVerticalRetrace = true;
        private bool useResizedBackBuffer;
        public static readonly SurfaceFormat[] ValidAdapterFormats = new SurfaceFormat[] { SurfaceFormat.Rg32, SurfaceFormat.Bgr565, SurfaceFormat.Bgr565, SurfaceFormat.Rgba1010102 };
        public static readonly SurfaceFormat[] ValidBackBufferFormats = new SurfaceFormat[] { SurfaceFormat.Bgr565, SurfaceFormat.Bgr565, SurfaceFormat.Bgra5551, SurfaceFormat.Rg32, SurfaceFormat.Color, SurfaceFormat.Rgba1010102 };
    }
}
