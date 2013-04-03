

using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;
using Fyri.Xna.Presentation.Properties;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace Fyri.Xna.Presentation
{
    public class GraphicsDeviceManager : IGraphicsDeviceService, IDisposable, IGraphicsDeviceManager
    {
        #region Fields

        public static readonly int m_iDefaultBackBufferHeight   = 600;
        public static readonly int m_iDefaultBackBufferWidth    = 800;

        public static readonly SurfaceFormat[] ValidAdapterFormats
            = new SurfaceFormat[] { 
                SurfaceFormat.Rg32, 
                SurfaceFormat.Bgr565, 
                SurfaceFormat.Bgr565, 
                SurfaceFormat.Rgba1010102 };

        public static readonly SurfaceFormat[] ValidBackBufferFormats 
            = new SurfaceFormat[] { 
                SurfaceFormat.Bgr565, 
                SurfaceFormat.Bgr565, 
                SurfaceFormat.Bgra5551, 
                SurfaceFormat.Rg32, 
                SurfaceFormat.Color, 
                SurfaceFormat.Rgba1010102 };

        private bool m_bAllowMultiSampling              = false;
        private bool m_bInDeviceTransition              = false;
        private bool m_bIsDeviceDirty                   = false;
        private bool m_bIsFullScreen                    = false;
        private bool m_bIsReallyFullScreen              = false;
        private bool m_bSynchronizeWithVerticalRetrace  = true;
        private bool m_bUseResizedBackBuffer            = false;
        private bool m_bBeginDrawOk                     = false;

        private DepthFormat m_objDepthStencilFormat = DepthFormat.Depth24;
        private SurfaceFormat m_objBackBufferFormat = SurfaceFormat.Color;

        private int m_iBackBufferHeight         = m_iDefaultBackBufferHeight;
        private int m_iBackBufferWidth          = m_iDefaultBackBufferWidth;
        private int m_iResizedBackBufferHeight  = 0;
        private int m_iResizedBackBufferWidth   = 0;
        
        private static DepthFormat[] m_objDepthFormatsWithoutStencil    = null;
        private static DepthFormat[] m_objDepthFormatsWithStencil       = null;

        private GraphicsDevice m_objDevice  = null;

        private Game m_objGame  = null;

        private static readonly TimeSpan m_tsDeviceLostSleepTime = TimeSpan.FromMilliseconds(50.0);
        
        #endregion

        #region Properties


        #endregion

        #region Events

        public event EventHandler<EventArgs> DeviceCreated;

        public event EventHandler<EventArgs> DeviceDisposing;

        public event EventHandler<EventArgs> DeviceReset;

        public event EventHandler<EventArgs> DeviceResetting;

        public event EventHandler<EventArgs> Disposed;

        public event EventHandler<PreparingDeviceSettingsEventArgs> PreparingDeviceSettings;

        #endregion

        // Methods
        static GraphicsDeviceManager()
        {
            m_objDepthFormatsWithStencil = new DepthFormat[] { DepthFormat.Depth24Stencil8, DepthFormat.Depth24, DepthFormat.Depth24, DepthFormat.Depth24Stencil8 };
            //depthFormatsWithoutStencil = new DepthFormat[] { DepthFormat.Depth24, DepthFormat.Depth24, DepthFormat.Depth16 };
        }

        public GraphicsDeviceManager(Game game)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game", Resources.GameCannotBeNull);
            }
            this.m_objGame = game;
            if (game.Services.GetService(typeof(IGraphicsDeviceManager)) != null)
            {
                throw new ArgumentException(Resources.GraphicsDeviceManagerAlreadyPresent);
            }
            game.Services.AddService(typeof(IGraphicsDeviceManager), this);
            game.Services.AddService(typeof(IGraphicsDeviceService), this);
            //game.Canvas.Window.SizeChanged += new System.Windows.SizeChangedEventHandler(this.GameWindowClientSizeChanged);
            //game.Window.ScreenDeviceNameChanged += new EventHandler(this.GameWindowScreenDeviceNameChanged);
        }


        private void AddDevices(bool anySuitableDevice, List<GraphicsDeviceInformation> foundDevices)
        {
            IntPtr handle = new System.Windows.Interop.WindowInteropHelper(this.m_objGame.Canvas.Window).Handle;
            foreach (GraphicsAdapter adapter in GraphicsAdapter.Adapters)
            {
                var newInfo = new GraphicsDeviceInformation();
                newInfo.Adapter = adapter;
                foundDevices.Add(newInfo);
            }
        }

        public void ApplyChanges()
        {
            if ((this.m_objDevice == null) || this.m_bIsDeviceDirty)
            {
                this.ChangeDevice(false);
            }
        }

        protected virtual bool CanResetDevice(GraphicsDeviceInformation newDeviceInfo)
        {
            return true;
        }

        private void ChangeDevice(bool forceCreate)
        {
            if (this.m_objGame == null)
            {
                throw new InvalidOperationException(Resources.GraphicsComponentNotAttachedToGame);
            }
            this.m_bInDeviceTransition = true;
            string screenDeviceName = string.Empty;
            int width = 0;
            int height = 0;
            if (this.m_objGame.Canvas != null && this.m_objGame.Canvas.Window != null)
            {
                screenDeviceName = this.m_objGame.Canvas.Window.Title;
                width = (int)this.m_objGame.Canvas.Window.ActualWidth;
                height = (int)this.m_objGame.Canvas.Window.ActualHeight;
            }
            bool flag = false;
            try
            {
                GraphicsDeviceInformation graphicsDeviceInformation = this.FindBestDevice(forceCreate);
                //this.game.Window.BeginScreenDeviceChange(graphicsDeviceInformation.PresentationParameters.IsFullScreen);
                flag = true;
                bool flag2 = true;
                if (!forceCreate && (this.m_objDevice != null))
                {
                    this.OnPreparingDeviceSettings(this, new PreparingDeviceSettingsEventArgs(graphicsDeviceInformation));
                    if (this.CanResetDevice(graphicsDeviceInformation))
                    {
                        try
                        {
                            GraphicsDeviceInformation information2 = graphicsDeviceInformation.Clone();
                            MassagePresentParameters(graphicsDeviceInformation.PresentationParameters);
                            ValidateGraphicsDeviceInformation(graphicsDeviceInformation);
                            m_objDevice.Reset(information2.PresentationParameters, information2.Adapter);
                            flag2 = false;
                        }
                        catch
                        {
                        }
                    }
                }
                if (flag2)
                {
                    this.CreateDevice(graphicsDeviceInformation);
                }
                PresentationParameters presentationParameters = this.m_objDevice.PresentationParameters;
                screenDeviceName = this.m_objDevice.Adapter.DeviceName;
                this.m_bIsReallyFullScreen = presentationParameters.IsFullScreen;
                if (presentationParameters.BackBufferWidth != 0)
                {
                    width = presentationParameters.BackBufferWidth;
                }
                if (presentationParameters.BackBufferHeight != 0)
                {
                    height = presentationParameters.BackBufferHeight;
                }
                this.m_bIsDeviceDirty = false;
            }
            finally
            {
                if (flag)
                {
                    //this.game.Window.EndScreenDeviceChange(screenDeviceName, width, height);
                }
                this.m_bInDeviceTransition = false;
            }
        }

        private void CreateDevice(GraphicsDeviceInformation newInfo)
        {
            if (this.m_objDevice != null)
            {
                this.m_objDevice.Dispose();
                this.m_objDevice = null;
            }
            this.OnPreparingDeviceSettings(this, new PreparingDeviceSettingsEventArgs(newInfo));
            this.MassagePresentParameters(newInfo.PresentationParameters);
            //try
            //{
            newInfo.PresentationParameters.DeviceWindowHandle = new System.Windows.Interop.WindowInteropHelper(this.m_objGame.Canvas.Window).Handle;
            //newInfo.PresentationParameters.BackBufferWidth = 2;
            //newInfo.PresentationParameters.BackBufferHeight = 2;
            newInfo.PresentationParameters.IsFullScreen = false;
            GraphicsDevice device = new GraphicsDevice(newInfo.Adapter, GraphicsProfile.Reach, newInfo.PresentationParameters);
            this.m_objDevice = device;
            this.m_objDevice.DeviceResetting += this.HandleDeviceResetting;
            this.m_objDevice.DeviceReset += this.HandleDeviceReset;
            this.m_objDevice.DeviceLost += this.HandleDeviceLost;
            this.m_objDevice.Disposing += this.HandleDisposing;
            //}
            this.OnDeviceCreated(this, EventArgs.Empty);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.m_objGame != null)
                {
                    if (this.m_objGame.Services.GetService(typeof(IGraphicsDeviceService)) == this)
                    {
                        this.m_objGame.Services.RemoveService(typeof(IGraphicsDeviceService));
                    }
                    this.m_objGame.Canvas.Window.SizeChanged -= new System.Windows.SizeChangedEventHandler(this.GameWindowClientSizeChanged);
                    // this.game.Window.ScreenDeviceNameChanged -= new EventHandler(this.GameWindowScreenDeviceNameChanged);
                }
                if (this.m_objDevice != null)
                {
                    this.m_objDevice.Dispose();
                    this.m_objDevice = null;
                }
                if (this.Disposed != null)
                {
                    this.Disposed(this, EventArgs.Empty);
                }
            }
        }

        private bool EnsureDevice()
        {
            if (this.m_objDevice == null)
            {
                return false;
            }
            return this.EnsureDevicePlatform();
        }

        private bool EnsureDevicePlatform()
        {
            if (this.m_bIsReallyFullScreen && !this.m_objGame.IsActive)
            {
                return false;
            }
            switch (this.m_objDevice.GraphicsDeviceStatus)
            {
                case GraphicsDeviceStatus.Lost:
                    Thread.Sleep((int)m_tsDeviceLostSleepTime.TotalMilliseconds);
                    return false;

                case GraphicsDeviceStatus.NotReset:
                    Thread.Sleep((int)m_tsDeviceLostSleepTime.TotalMilliseconds);
                    try
                    {
                        this.ChangeDevice(false);
                    }
                    catch (DeviceLostException)
                    {
                        return false;
                    }
                    catch
                    {
                        this.ChangeDevice(true);
                    }
                    break;
            }
            return true;
        }

        protected virtual GraphicsDeviceInformation FindBestDevice(bool anySuitableDevice)
        {
            return this.FindBestPlatformDevice(anySuitableDevice);
        }

        private GraphicsDeviceInformation FindBestPlatformDevice(bool anySuitableDevice)
        {
            List<GraphicsDeviceInformation> foundDevices = new List<GraphicsDeviceInformation>();
            this.AddDevices(anySuitableDevice, foundDevices);
            if ((foundDevices.Count == 0) && this.PreferMultiSampling)
            {
                this.PreferMultiSampling = false;
                this.AddDevices(anySuitableDevice, foundDevices);
            }
            return foundDevices[0];
        }

        public void GameWindowClientSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (m_objGame.IsActive)
            {
                if (!this.m_bInDeviceTransition && ((this.m_objGame.Canvas.Window.ActualHeight != 0) || (this.m_objGame.Canvas.Window.ActualWidth != 0)))
                {
                    this.m_iResizedBackBufferWidth = (int)this.m_objGame.Canvas.Window.ActualWidth;
                    this.m_iResizedBackBufferHeight = (int)this.m_objGame.Canvas.Window.ActualHeight;
                    this.m_bUseResizedBackBuffer = true;
                    this.ChangeDevice(false);
                }
            }
        }

        private void GameWindowScreenDeviceNameChanged(object sender, EventArgs e)
        {
            if (!this.m_bInDeviceTransition)
            {
                this.ChangeDevice(false);
            }
        }

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(uint smIndex);
        private void HandleDeviceLost(object sender, EventArgs e)
        {
        }

        private void HandleDeviceReset(object sender, EventArgs e)
        {
            this.OnDeviceReset(this, EventArgs.Empty);
        }

        private void HandleDeviceResetting(object sender, EventArgs e)
        {
            this.OnDeviceResetting(this, EventArgs.Empty);
        }

        private void HandleDisposing(object sender, EventArgs e)
        {
            this.OnDeviceDisposing(this, EventArgs.Empty);
        }

        private void MassagePresentParameters(PresentationParameters pp)
        {
            bool flag = pp.BackBufferWidth == 0;
            bool flag2 = pp.BackBufferHeight == 0;
            if (!pp.IsFullScreen)
            {
                NativeMethods.RECT rect;
                IntPtr deviceWindowHandle = pp.DeviceWindowHandle;
                if (deviceWindowHandle == IntPtr.Zero)
                {
                    if (this.m_objGame == null)
                    {
                        throw new InvalidOperationException(Resources.GraphicsComponentNotAttachedToGame);
                    }
                    deviceWindowHandle = new System.Windows.Interop.WindowInteropHelper(this.m_objGame.Canvas.Window).Handle;
                }
                NativeMethods.GetClientRect(deviceWindowHandle, out rect);
                if (flag && (rect.Right == 0))
                {
                    pp.BackBufferWidth = 1;
                }
                if (flag2 && (rect.Bottom == 0))
                {
                    pp.BackBufferHeight = 1;
                }
            }
        }

        bool IGraphicsDeviceManager.BeginDraw()
        {
            if (!this.EnsureDevice())
            {
                return false;
            }
            this.m_bBeginDrawOk = true;
            return true;
        }

        void IGraphicsDeviceManager.CreateDevice()
        {
            this.ChangeDevice(true);
        }

        void IGraphicsDeviceManager.EndDraw()
        {
            if (this.m_bBeginDrawOk && (this.m_objDevice != null))
            {
                try
                {
                    this.m_objDevice.Present();
                }
                catch (InvalidOperationException)
                {
                }
                catch (DeviceLostException)
                {
                }
                catch (DeviceNotResetException)
                {
                }
            }
        }

        protected virtual void OnDeviceCreated(object sender, EventArgs args)
        {
            if (this.DeviceCreated != null)
            {
                this.DeviceCreated(sender, args);
            }
        }

        protected virtual void OnDeviceDisposing(object sender, EventArgs args)
        {
            if (this.DeviceDisposing != null)
            {
                this.DeviceDisposing(sender, args);
            }
        }

        protected virtual void OnDeviceReset(object sender, EventArgs args)
        {
            if (this.DeviceReset != null)
            {
                this.DeviceReset(sender, args);
            }
        }

        protected virtual void OnDeviceResetting(object sender, EventArgs args)
        {
            if (this.DeviceResetting != null)
            {
                this.DeviceResetting(sender, args);
            }
        }

        protected virtual void OnPreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs args)
        {
            if (this.PreparingDeviceSettings != null)
            {
                this.PreparingDeviceSettings(sender, args);
            }
        }

        void IDisposable.Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void ToggleFullScreen()
        {
            this.IsFullScreen = !this.IsFullScreen;
            this.ChangeDevice(false);
        }

        private void ValidateGraphicsDeviceInformation(GraphicsDeviceInformation devInfo)
        {
            SurfaceFormat format = SurfaceFormat.Rg32;
            GraphicsAdapter adapter = devInfo.Adapter;
            PresentationParameters presentationParameters = devInfo.PresentationParameters;
            SurfaceFormat format4 = presentationParameters.BackBufferFormat;
            if (!presentationParameters.IsFullScreen)
            {
                format = adapter.CurrentDisplayMode.Format;
                format4 = format;
            }

            if (-1 == Array.IndexOf<SurfaceFormat>(ValidBackBufferFormats, format4))
            {
                //throw new ArgumentException(Resources.ValidateBackBufferFormatIsInvalid);
            }

            if (presentationParameters.IsFullScreen)
            {
                if ((presentationParameters.BackBufferWidth == 0) || (presentationParameters.BackBufferHeight == 0))
                {
                    //throw new ArgumentException(Resources.ValidateBackBufferDimsFullScreen);
                }
                bool flag2 = true;
                bool flag3 = false;
                DisplayMode currentDisplayMode = adapter.CurrentDisplayMode;
                if (((currentDisplayMode.Format != format) && (currentDisplayMode.Width != presentationParameters.BackBufferHeight)) && ((currentDisplayMode.Height != presentationParameters.BackBufferHeight)))
                {
                    flag2 = false;
                    foreach (DisplayMode mode2 in adapter.SupportedDisplayModes[format])
                    {
                        if ((mode2.Width == presentationParameters.BackBufferWidth) && (mode2.Height == presentationParameters.BackBufferHeight))
                        {
                            flag3 = true;
                        }
                    }
                }
                if (!flag2 && flag3)
                {
                    throw new ArgumentException(Resources.ValidateBackBufferDimsModeFullScreen);
                }
                if (!flag2)
                {
                    //throw new ArgumentException(Resources.ValidateBackBufferHzModeFullScreen);
                }
            }
        }

        // Properties
        public GraphicsDevice GraphicsDevice
        {
            get
            {
                return this.m_objDevice;
            }
        }

        public bool IsFullScreen
        {
            get
            {
                return this.m_bIsFullScreen;
            }
            set
            {
                this.m_bIsFullScreen = value;
                this.m_bIsDeviceDirty = true;
            }
        }

        public bool PreferMultiSampling
        {
            get
            {
                return this.m_bAllowMultiSampling;
            }
            set
            {
                this.m_bAllowMultiSampling = value;
                this.m_bIsDeviceDirty = true;
            }
        }

        public SurfaceFormat PreferredBackBufferFormat
        {
            get
            {
                return this.m_objBackBufferFormat;
            }
            set
            {
                if (Array.IndexOf<SurfaceFormat>(ValidBackBufferFormats, value) == -1)
                {
                    throw new ArgumentOutOfRangeException("value", Resources.ValidateBackBufferFormatIsInvalid);
                }
                this.m_objBackBufferFormat = value;
                this.m_bIsDeviceDirty = true;
            }
        }

        public int PreferredBackBufferHeight
        {
            get
            {
                return this.m_iBackBufferHeight;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", Resources.BackBufferDimMustBePositive);
                }
                this.m_iBackBufferHeight = value;
                this.m_bUseResizedBackBuffer = false;
                this.m_bIsDeviceDirty = true;
            }
        }

        public int PreferredBackBufferWidth
        {
            get
            {
                return this.m_iBackBufferWidth;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", Resources.BackBufferDimMustBePositive);
                }
                this.m_iBackBufferWidth = value;
                this.m_bUseResizedBackBuffer = false;
                this.m_bIsDeviceDirty = true;
            }
        }

        public DepthFormat PreferredDepthStencilFormat
        {
            get
            {
                return this.m_objDepthStencilFormat;
            }
            set
            {
                switch (value)
                {
                    case DepthFormat.Depth24Stencil8:
                    case DepthFormat.Depth24:
                    case DepthFormat.Depth16:
                        this.m_objDepthStencilFormat = value;
                        this.m_bIsDeviceDirty = true;
                        return;
                }
                throw new ArgumentOutOfRangeException("value", Resources.ValidateDepthStencilFormatIsInvalid);
            }
        }

        public bool SynchronizeWithVerticalRetrace
        {
            get
            {
                return this.m_bSynchronizeWithVerticalRetrace;
            }
            set
            {
                this.m_bSynchronizeWithVerticalRetrace = value;
                this.m_bIsDeviceDirty = true;
            }
        }
    }


}
