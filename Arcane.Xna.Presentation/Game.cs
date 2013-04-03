using System.Windows;
using System.Windows.Controls;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Windows.Threading;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;
using Microsoft.Xna.Framework.Content;
using System.Xaml;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Linq.Expressions;
using FyriDispatcher;

namespace Fyri.Xna.Presentation
{
    /// <summary>
    /// Logique d'interaction pour GameCanvas.xaml
    /// </summary>
    [System.Windows.Markup.ContentProperty("WPFHost")]
    public partial class Game : IDisposable, INotifyPropertyChanged
    {

        #region Fields

        private bool m_bInRun                   = false;
        private bool m_bIsMouseVisible          = false;
        private bool m_bDoneFirstUpdate         = false;
        private bool m_bDrawRunningSlowly       = false;
        private bool m_bExitRequested           = false;
        private bool m_bIsActivated             = false;
        private bool m_bAsyncInit               = true;
        private bool m_bDeferPropertyChanged    = true;
        private bool m_bIsFixedTimeStep         = true;
        private bool m_bIsRunning               = false;
        private bool m_bIsVisible               = false;
            
        
        private GameClock m_objClock            = null;
        private GameTime m_objGameTime          = new GameTime();

        private GameServiceContainer m_objGameServices          = new GameServiceContainer();
        private GameServiceContainer m_objServices              = new GameServiceContainer();
        private GameComponentCollection m_objComponents         = null;
        private List<IUpdateable> m_lstUpdateableComponents     = new List<IUpdateable>();
        private List<IDrawable> m_lstDrawableComponents         = new List<IDrawable>();
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private DispatcherTimer m_objTickGenerator  = null;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private TimeSpan m_tsInactiveSleepTime              = TimeSpan.Zero;
        private TimeSpan m_tsLastFrameElapsedGameTime       = TimeSpan.Zero;
        private TimeSpan m_tsTargetElapsedTime              = TimeSpan.Zero;
        private TimeSpan m_tsTotalGameTime                  = TimeSpan.Zero;
        private TimeSpan m_tsAccumulatedElapsedGameTime     = TimeSpan.Zero;
        private TimeSpan m_tsElapsedRealTime                = TimeSpan.Zero;
        private readonly TimeSpan maximumElapsedTime        = TimeSpan.FromMilliseconds(500);

        private IGraphicsDeviceManager m_objGraphicsDeviceManager   = null;
        private IGraphicsDeviceService m_objGraphicsDeviceService   = null;
        
        private ContentManager m_objContentManager                  = null;

        private Dispatcher m_objDispatcher                          = null;

        private GameCanvas m_objCanvas = null;

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler Activated;
        public event EventHandler Deactivated;
        public event EventHandler Disposed;
        public event EventHandler Exiting;

        private UIElementCollection Children
        {
            get;
            set;
        }

        //public object WPFHost
        //{
        //    get
        //    {
        //        if (!(System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)))
        //            return this.Window.WPFHost;
        //        else
        //            return (base.Children[0] as ContentControl).Content;
        //    }
        //    set
        //    {
        //        if (!(System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)))
        //            this.Window.WPFHost = value;
        //        else
        //            (base.Children[0] as ContentControl).Content = value;
        //    }
        //}

        protected virtual void OnActivated(object sender, EventArgs args)
        {
            if (!this.IsRunning)
                this.Run();
            if (this.Activated != null)
            {
                this.Activated(this, args);
            }
            if (this.IsRunning)
            {
                this.m_objTickGenerator.Start();
            }
        }

        protected virtual void OnDeactivated(object sender, EventArgs args)
        {
            if (this.Deactivated != null)
            {
                this.Deactivated(this, args);
            }
            this.m_objTickGenerator.Stop();
        }

        //protected virtual void OnExiting(object sender, EventArgs args)
        //{
        //    this.UnhookDeviceEvents();
        //    if (this.Exiting != null)
        //    {
        //        this.Exiting(null, args);
        //    }
        //}

        //protected virtual void OnDisposed(object sender, EventArgs args)
        //{
        //    if (this.Disposed != null)
        //    {
        //        this.Disposed(null, args);
        //    }
        //}

        #endregion

        #region ViewModel Event Handlers

        /// <summary>
        /// Get Name of Property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<T>> e)
        {
            var member = (MemberExpression)e.Body;
            return member.Member.Name;
        }

        /// <summary>
        /// Raise when Property Value PropertyChanged or override PropertyChanged
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            RaisePropertyChanged(GetPropertyName(propertyExpression));
        }

        /// <summary>
        /// Raise when property value propertychanged
        /// </summary>
        /// <param name="propertyName"></param>
        protected void RaisePropertyChanged(String propertyName)
        {
            this.VerifyPropertyName(propertyName);

            if (!DeferPropertyChanged)
            {
                if (m_objDispatcher.CheckAccess())
                {
                    OnPropertyChanged(propertyName);
                }
                else
                {
                    m_objDispatcher.BeginInvoke(new DispatchedString(RaisePropertyChanged), DispatcherPriority.Background, propertyName);
                }
            }

        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler dHandler;

            if (m_objDispatcher.CheckAccess())
            {
                dHandler = PropertyChanged;

                if (dHandler != null)
                    dHandler(this, new PropertyChangedEventArgs(propertyName));
            }
            else
            {
                throw new InvalidOperationException("OnPropertyChanged should be called from dispatcher thread");
            }
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private void VerifyPropertyName(string propertyName)
        {
            PropertyInfo[] aProperties = null;

            if (string.IsNullOrEmpty(propertyName))
                return;

            aProperties = this.GetType().GetProperties(System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | BindingFlags.NonPublic);

            for (int i = 0; i < aProperties.Length; i++)
            {
                if (aProperties[i].Name.EndsWith(propertyName))
                    return;
            }

            Debug.Fail(string.Format("Invalid property name [{0}] on type [{1}]", propertyName, this.GetType()));
        }

        #endregion

        #region Properties

        public event EventHandler FirstActivation;

        public GameCanvas Canvas
        {
            get
            {
                return m_objCanvas;
            }
            set
            {
                if (m_objCanvas != value)
                {
                    if (m_objCanvas != null)
                    {
                        m_objCanvas.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(GameCanvas_IsVisibleChanged);
                        if (m_objGraphicsDeviceManager is GraphicsDeviceManager)
                        {
                            m_objCanvas.Window.SizeChanged += new System.Windows.SizeChangedEventHandler(((GraphicsDeviceManager)m_objGraphicsDeviceManager).GameWindowClientSizeChanged);
                            
                            //if((m_objCanvas.Window)
                            //((Window)m_objCanvas.Window).ScreenDeviceNameChanged += new EventHandler(this.GameWindowScreenDeviceNameChanged);
                        }
                        this.m_objGameServices.RemoveService(typeof(IInputPublisherService));
                    }

                    m_objCanvas = value;

                    if (m_objCanvas != null)
                    {
                        m_objCanvas.IsVisibleChanged += new DependencyPropertyChangedEventHandler(GameCanvas_IsVisibleChanged);
                        this.m_objGameServices.AddService(typeof(IInputPublisherService), new ControlInputPublisher(m_objCanvas));
                    }

                    RaisePropertyChanged("Canvas");
                }
            }
        }

        public GraphicsDevice GraphicsDevice
        {
            get
            {
                IGraphicsDeviceService graphicsDeviceService = this.m_objGraphicsDeviceService;
                if (graphicsDeviceService == null)
                {
                    graphicsDeviceService = this.Services.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
                    if (graphicsDeviceService == null)
                    {
                        throw new InvalidOperationException(Properties.Resources.NoGraphicsDeviceService);
                    }
                }
                return graphicsDeviceService.GraphicsDevice;
            }
        }

        public bool IsVisible
        {
            get
            {
                return m_bIsVisible;
            }
            private set
            {
                if (m_bIsVisible != value)
                {
                    m_bIsVisible = value;
                    RaisePropertyChanged("IsVisible");
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return m_bIsRunning;
            }
            private set
            {
                if (m_bIsRunning != value)
                {
                    m_bIsRunning = value;
                    RaisePropertyChanged("IsRunning");
                }
            }
        }

        public ContentManager ContentManager
        {
            get
            {
                return this.m_objContentManager;
            }
            set
            {
                if (this.m_objContentManager != value)
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException();
                    }
                    this.m_objContentManager = value;
                    RaisePropertyChanged("ContentManager");
                }
            }
        }

        public GameComponentCollection Components
        {
            get
            {
                return m_objComponents;
            }
            private set
            {
                if (m_objComponents != value)
                {
                    m_objComponents = value;
                    RaisePropertyChanged("Components");
                }
            }
        }

        public TimeSpan InactiveSleepTime
        {
            get
            {
                return this.m_tsInactiveSleepTime;
            }
            set
            {
                if (m_tsInactiveSleepTime != value)
                {
                    if (value < TimeSpan.Zero)
                    {
                        throw new ArgumentOutOfRangeException("Resources.InactiveSleepTimeCannotBeZero", "value");
                    }
                    this.m_tsInactiveSleepTime = value;

                    RaisePropertyChanged("InactiveSleepTime");
                }
            }
        }

        public bool IsFixedTimeStep
        {
            get
            {
                return this.m_bIsFixedTimeStep;
            }
            set
            {
                if (this.m_bIsFixedTimeStep != value)
                {
                    this.m_bIsFixedTimeStep = value;
                    this.RecomputeStepSpan();

                    RaisePropertyChanged("IsFixedTimeStep");
                }
            }
        }

        public GameServiceContainer Services
        {
            get
            {
                return m_objServices;
            }
            private set
            {
                if (m_objServices != value)
                {
                    m_objServices = value;
                    RaisePropertyChanged("Services");
                }
            }
        }

        public bool IsActive
        {
            get
            {
                return this.m_bIsActivated;
            }
            set
            {
                this.m_bIsActivated = value;
                if (value)
                    OnActivated(this, EventArgs.Empty);
                else
                    OnDeactivated(this, EventArgs.Empty);

                RaisePropertyChanged("IsActive");
            }
        }

        //public GameHost Window
        //{
        //    get
        //    {
        //        return this.m_objWindow;
        //    }
        //}

        public bool IsMouseVisible
        {
            get
            {
                return this.m_bIsMouseVisible;
            }
            set
            {
                this.m_bIsMouseVisible = value;
                Canvas.IsMouseVisible = this.m_bIsMouseVisible;
                
                RaisePropertyChanged("IsMouseVisible");
            }
        }

        #endregion

        #region View Model Properties

        protected Dispatcher Dispatcher
        {
            get
            {
                return m_objDispatcher;
            }
        }

        public static Dispatcher CurrentDispatcher
        {
            get
            {
                if (Application.Current == null)
                    return Dispatcher.CurrentDispatcher;
                return Application.Current.Dispatcher;
            }
        }

        protected bool DeferPropertyChanged
        {
            get
            {
                return m_bDeferPropertyChanged;
            }
            set
            {
                if (m_bDeferPropertyChanged != value)
                {
                    m_bDeferPropertyChanged = value;

                    if (!value)
                        RaisePropertyChanged(string.Empty);
                }
            }
        }

        #endregion

        #region Constructors

        public Game()
            : this(true)
        {
        }

        public Game(bool asyncInitialize)
        {
            // InitializeComponent();
            //if (!(System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)))
            //{
                m_objDispatcher = CurrentDispatcher;

                m_bAsyncInit = asyncInitialize;

                InitializeViewModel();
            //}
            //else
            //    base.Children.Add(new ContentControl());
        }

        //void _window_Closed(object sender, EventArgs e)
        //{
        //    this.OnExiting(this, EventArgs.Empty);
        //    this.OnDisposed(this, EventArgs.Empty);
        //}

        private void RecomputeStepSpan()
        {
            this.m_objTickGenerator.Interval = new TimeSpan(0, 0, 0, 0, (this.IsFixedTimeStep ? 33 : 1));
        }

        void GameCanvas_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.IsActive = (bool)e.NewValue;
            this.IsVisible = (bool)e.NewValue;
        }

        void _tickGenerator_Tick(object sender, EventArgs e)
        {
            this.Tick();
        }

        public void Run()
        {
            // Upper part of Run() method
            this.m_objGraphicsDeviceManager = this.Services.GetService(typeof(IGraphicsDeviceManager)) as IGraphicsDeviceManager;
            if (this.m_objGraphicsDeviceManager != null && this.Canvas != null && this.Canvas.Window != null)
            {
                this.m_objGraphicsDeviceManager.CreateDevice();
            }
            if (this.Canvas != null && this.Canvas.Window != null)
            {
                this.Initialize();
                this.m_bInRun = true;
                try
                {
                    this.BeginRun();
                    this.m_objGameTime = new GameTime(this.m_tsTotalGameTime, this.m_objClock.CurrentTime,
                      false
                    );
                    this.Update(this.m_objGameTime);
                    this.m_bDoneFirstUpdate = true;
                    if (this.Canvas != null && this.Canvas.Window != null)
                    {
                        //this.Canvas.Window.Owner = Application.Current.MainWindow;
                        this.Canvas.Window.PreRun();
                    }
                    this.IsRunning = true;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    this.m_bInRun = false;
                    Microsoft.Xna.Framework.Input.Mouse.WindowHandle = this.Canvas.Window.Handle;
                }
            }
        }


        protected virtual bool BeginDraw()
        {
            if ((this.m_objGraphicsDeviceManager != null) && !this.m_objGraphicsDeviceManager.BeginDraw())
            {
                return false;
            }
            return true;
        }

        protected virtual void BeginRun()
        {
        }

        private void DrawFrame()
        {
            if ((this.m_bDoneFirstUpdate /*&& !this.Window.IsMinimized*/) && this.BeginDraw())
            {
                this.m_objGameTime = new GameTime(
                  this.m_tsTotalGameTime, this.m_tsLastFrameElapsedGameTime,
                  this.m_bDrawRunningSlowly
                );

                this.Draw(this.m_objGameTime);
                this.EndDraw();
            }
        }

        protected virtual void EndDraw()
        {
            if (this.m_objGraphicsDeviceManager != null)
            {
                this.m_objGraphicsDeviceManager.EndDraw();
            }
        }

        protected virtual void EndRun()
        {
        }

        public void Tick()
        {
            if (!this.m_bExitRequested)
            {
                if (!this.IsActive)
                {
                    Thread.Sleep((int)this.m_tsInactiveSleepTime.TotalMilliseconds);
                }
                else
                    this.m_objClock.Step();
                this.m_tsElapsedRealTime = this.m_objClock.ElapsedTime;
                if (this.m_tsElapsedRealTime < TimeSpan.Zero)
                {
                    this.m_tsElapsedRealTime = TimeSpan.Zero;
                }
                if (this.m_tsElapsedRealTime > this.maximumElapsedTime)
                {
                    this.m_tsElapsedRealTime = this.maximumElapsedTime;
                }
                this.m_objGameTime = new GameTime(
                    this.m_objGameTime.TotalGameTime, this.m_objGameTime.ElapsedGameTime,
                    this.m_objGameTime.IsRunningSlowly
                );
                this.m_bDrawRunningSlowly = false;
                if (this.m_bIsFixedTimeStep)
                {
                    this.m_tsAccumulatedElapsedGameTime += this.m_tsElapsedRealTime;
                    long num = this.m_tsAccumulatedElapsedGameTime.Ticks / this.m_tsTargetElapsedTime.Ticks;
                    this.m_tsAccumulatedElapsedGameTime = TimeSpan.FromTicks(this.m_tsAccumulatedElapsedGameTime.Ticks % this.m_tsTargetElapsedTime.Ticks);
                    this.m_tsLastFrameElapsedGameTime = TimeSpan.Zero;
                    TimeSpan targetElapsedTime = this.m_tsTargetElapsedTime;
                    if (num > 0)
                    {
                        while (num > 1)
                        {
                            this.m_bDrawRunningSlowly = true;
                            this.m_objGameTime = new GameTime(
                              this.m_objGameTime.TotalGameTime, this.m_objGameTime.ElapsedGameTime,
                              true
                            );
                            num--;
                            try
                            {
                                this.m_objGameTime = new GameTime(
                                  this.m_tsTotalGameTime, targetElapsedTime,
                                  this.m_objGameTime.IsRunningSlowly
                                );
                                this.Update(this.m_objGameTime);
                                continue;
                            }
                            finally
                            {
                                this.m_tsLastFrameElapsedGameTime += targetElapsedTime;
                                this.m_tsTotalGameTime += targetElapsedTime;
                            }
                        }
                        this.m_objGameTime = new GameTime(
                          this.m_objGameTime.TotalGameTime, this.m_objGameTime.ElapsedGameTime,
                          false
                        );
                        try
                        {
                            this.m_objGameTime = new GameTime(
                              this.m_tsTotalGameTime, targetElapsedTime,
                              this.m_objGameTime.IsRunningSlowly
                            );
                            this.Update(this.m_objGameTime);
                        }
                        finally
                        {
                            this.m_tsLastFrameElapsedGameTime += targetElapsedTime;
                            this.m_tsTotalGameTime += targetElapsedTime;
                        }
                    }
                }
                else
                {
                    TimeSpan elapsedRealTime = this.m_tsElapsedRealTime;
                    try
                    {
                        this.m_objGameTime = new GameTime(
                          this.m_tsTotalGameTime, this.m_tsLastFrameElapsedGameTime = elapsedRealTime,
                          this.m_objGameTime.IsRunningSlowly
                        );
                        this.Update(this.m_objGameTime);
                    }
                    finally
                    {
                        this.m_tsTotalGameTime += elapsedRealTime;
                    }
                }
                if (!this.m_bExitRequested)
                {
                    this.DrawFrame();
                }
            }
        }

        public void Exit()
        {
            this.m_bExitRequested = true;
            this.IsRunning = false;
            this.m_objTickGenerator.Stop();

            this.UnhookDeviceEvents();

            if (Canvas != null)
                Canvas.Exit();

            this.Dispose();
        }

        #endregion

        #region Device life


        private void HookDeviceEvents()
        {

            this.m_objGraphicsDeviceService = this.Services.GetService(typeof (IGraphicsDeviceService)) as IGraphicsDeviceService;
            if (this.m_objGraphicsDeviceService != null)
            {
                this.m_objGraphicsDeviceService.DeviceCreated += DeviceCreated;
                this.m_objGraphicsDeviceService.DeviceResetting += DeviceResetting;
                this.m_objGraphicsDeviceService.DeviceReset += DeviceReset;
                this.m_objGraphicsDeviceService.DeviceDisposing += DeviceDisposing;
            }
        }

        private void UnhookDeviceEvents()
        {
            if (this.m_objGraphicsDeviceService != null)
            {
                this.m_objGraphicsDeviceService.DeviceCreated -= DeviceCreated;
                this.m_objGraphicsDeviceService.DeviceResetting -= DeviceResetting;
                this.m_objGraphicsDeviceService.DeviceReset -= DeviceReset;
                this.m_objGraphicsDeviceService.DeviceDisposing -= DeviceDisposing;
            }
        }

        private void DeviceCreated(object sender, EventArgs e)
        {
            this.LoadContent();
        }

        private void DeviceDisposing(object sender, EventArgs e)
        {
            this.UnloadContent();
        }

        private void DeviceReset(object sender, EventArgs e)
        {
            
        }

        private void DeviceResetting(object sender, EventArgs e)
        {
            var vertices = new VertexPositionColor[8];
            vertices[0].Position = new Vector3(-10f, -10f, 10f);
            vertices[0].Color = Color.Yellow;
            vertices[1].Position = new Vector3(-10f, 10f, 10f);
            vertices[1].Color = Color.Green;
            vertices[2].Position = new Vector3(10f, 10f, 10f);
            vertices[2].Color = Color.Blue;
            vertices[3].Position = new Vector3(10f, -10f, 10f);
            vertices[3].Color = Color.Black;
            vertices[4].Position = new Vector3(10f, 10f, -10f);
            vertices[4].Color = Color.Red;
            vertices[5].Position = new Vector3(10f, -10f, -10f);
            vertices[5].Color = Color.Violet;
            vertices[6].Position = new Vector3(-10f, -10f, -10f);
            vertices[6].Color = Color.Orange;
            vertices[7].Position = new Vector3(-10f, 10f, -10f);
            vertices[7].Color = Color.Gray;
            var vertexBuffer = new VertexBuffer(this.GraphicsDevice, typeof(VertexPositionColor), 8, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);
            this.GraphicsDevice.SetVertexBuffer(vertexBuffer);
        }

        #endregion

        #region Components life

        private void GameComponentAdded(object sender, GameComponentCollectionEventArgs e)
        {
            if (this.m_bInRun)
            {
                e.GameComponent.Initialize();
            }
            IUpdateable item = e.GameComponent as IUpdateable;
            if (item != null)
            {
                int num = this.m_lstUpdateableComponents.BinarySearch(item, UpdateOrderComparer.Default);
                if (num < 0)
                {
                    this.m_lstUpdateableComponents.Insert(~num, item);
                    item.UpdateOrderChanged += this.UpdateableUpdateOrderChanged;
                }
            }
            IDrawable drawable = e.GameComponent as IDrawable;
            if (drawable != null)
            {
                int num2 = this.m_lstDrawableComponents.BinarySearch(drawable, DrawOrderComparer.Default);
                if (num2 < 0)
                {
                    this.m_lstDrawableComponents.Insert(~num2, drawable);
                    drawable.DrawOrderChanged += this.DrawableDrawOrderChanged;
                }
            }
        }

        private void GameComponentRemoved(object sender, GameComponentCollectionEventArgs e)
        {
            IUpdateable item = e.GameComponent as IUpdateable;
            if (item != null)
            {
                this.m_lstUpdateableComponents.Remove(item);
                item.UpdateOrderChanged -= this.UpdateableUpdateOrderChanged;
            }
            IDrawable drawable = e.GameComponent as IDrawable;
            if (drawable != null)
            {
                this.m_lstDrawableComponents.Remove(drawable);
                drawable.DrawOrderChanged -= this.DrawableDrawOrderChanged;
            }
        }

        private void UpdateableUpdateOrderChanged(object sender, EventArgs e)
        {
            IUpdateable item = sender as IUpdateable;
            this.m_lstUpdateableComponents.Remove(item);
            int num = this.m_lstUpdateableComponents.BinarySearch(item, UpdateOrderComparer.Default);
            if (num < 0)
            {
                this.m_lstUpdateableComponents.Insert(~num, item);
            }
        }

        private void DrawableDrawOrderChanged(object sender, EventArgs e)
        {
            IDrawable item = sender as IDrawable;
            this.m_lstDrawableComponents.Remove(item);
            int num = this.m_lstDrawableComponents.BinarySearch(item, DrawOrderComparer.Default);
            if (num < 0)
            {
                this.m_lstDrawableComponents.Insert(~num, item);
            }
        }

        #endregion

        #region Game Specific Methods


        /// <summary>
        /// <para>Called when graphics resources need to be loaded.  Override this method to load any game-specific graphics resources.</para>
        /// </summary>
        protected virtual void LoadContent()
        {
        }

        protected virtual void UnloadContent()
        {
        }

        protected virtual void Initialize()
        {
            for (int i = 0; i < this.Components.Count; i++)
            {
                this.Components[i].Initialize();
            }
            this.HookDeviceEvents();
            if ((this.m_objGraphicsDeviceService != null) && (this.m_objGraphicsDeviceService.GraphicsDevice != null))
            {
                this.LoadContent();
            }
        }

        protected virtual void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.m_lstUpdateableComponents.Count; i++)
            {
                IUpdateable updateable = this.m_lstUpdateableComponents[i];
                if (updateable.Enabled)
                {
                    updateable.Update(gameTime);
                }
            }
        }

        protected virtual void Draw(GameTime gameTime)
        {
            for (int i = 0; i < this.m_lstDrawableComponents.Count; i++)
            {
                IDrawable drawable = this.m_lstDrawableComponents[i];
                if (drawable.Visible)
                {
                    drawable.Draw(gameTime);
                }
            }
        }

        #endregion

        #region ViewModel Methods

        private void InitializeViewModel()
        {
            if (m_objDispatcher.CheckAccess())
            {
                OnEnsureProperties();
            }
            else
            {
                if (m_bAsyncInit)
                    m_objDispatcher.BeginInvoke(new Dispatched(InitializeViewModel));
                else
                    m_objDispatcher.Invoke(new Dispatched(InitializeViewModel));
            }
        }

        protected virtual void OnEnsureProperties()
        {
            //this.m_objWindow = new GameHost(this);
            //this.m_objWindow.Closed += new EventHandler(_window_Closed);
            this.m_objTickGenerator = new DispatcherTimer();
            this.m_objTickGenerator.Tick += new EventHandler(_tickGenerator_Tick);                

            this.Components = new GameComponentCollection();
            this.Components.ComponentAdded += new EventHandler<GameComponentCollectionEventArgs>(this.GameComponentAdded);
            this.Components.ComponentRemoved += new EventHandler<GameComponentCollectionEventArgs>(this.GameComponentRemoved);

            this.Services = new GameServiceContainer();
            this.m_objContentManager = new ContentManager(this.Services);

            this.IsFixedTimeStep = true;

            this.m_objClock = new GameClock();
            this.m_tsTotalGameTime = TimeSpan.Zero;
            this.m_tsAccumulatedElapsedGameTime = TimeSpan.Zero;
            this.m_tsLastFrameElapsedGameTime = TimeSpan.Zero;
            this.m_tsTargetElapsedTime = TimeSpan.FromTicks((long)0x28b0a);
            this.m_tsInactiveSleepTime = TimeSpan.FromMilliseconds(20);

            //if (this.Canvas != null)
            //{
            //    this.Canvas.IsVisibleChanged += new DependencyPropertyChangedEventHandler(GameCanvas_IsVisibleChanged);
            //    this.m_objGameServices.AddService(typeof(IInputPublisherService), new ControlInputPublisher(this.Canvas));
            //}
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (this)
            {
                IGameComponent[] array = new IGameComponent[this.Components.Count];
                this.Components.CopyTo(array, 0);
                for (int i = 0; i < array.Length; i++)
                {
                    IDisposable disposable = array[i] as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                IDisposable disposable2 = this.m_objGraphicsDeviceManager as IDisposable;
                if (disposable2 != null)
                {
                    disposable2.Dispose();
                }
                this.UnhookDeviceEvents();

                if (this.Disposed != null)
                {
                    this.Disposed(this, EventArgs.Empty);
                }
            }
        }

        #endregion
    }
}
