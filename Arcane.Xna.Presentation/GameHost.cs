using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;

namespace Fyri.Xna.Presentation
{
    /// <summary>
    /// Logique d'interaction pour GameHost.xaml
    /// </summary>
    public partial class GameHost : Window
    {

        #region Fields

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool m_bDoneRun                 = false;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Game m_objGame                  = null;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Game m_objGameControl           = null;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Window m_objTopLevelControl     = null;
        //[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        //internal Window m_objFrontWindow        = null;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private GameCanvas m_objGameCanvas = null;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private ObservableCollection<GameCanvas> m_lstGameCanvas = new ObservableCollection<GameCanvas>();

        #endregion


        #region Properties


        //internal object WPFHost
        //{
        //    get
        //    {
        //        return this.m_objFrontWindow.Content;
        //    }
        //    set
        //    {
        //        this.m_objFrontWindow.Content = value;
        //    }
        //}

        /// <summary>
        /// <para>Gets the top level Window for the current Xna 3D scene hoster.</para>
        /// </summary>
        public Window TopLevelWindow
        {
            get
            {
                if (this.m_objTopLevelControl != Window.GetWindow(this.m_objGameCanvas))
                {
                    this.m_objTopLevelControl = Window.GetWindow(this.m_objGameCanvas);
                    this.Owner = this.m_objTopLevelControl;
                    this.m_objTopLevelControl.Closing -= new System.ComponentModel.CancelEventHandler(_topLevelControl_Closing);
                    this.m_objTopLevelControl.Closing += new System.ComponentModel.CancelEventHandler(_topLevelControl_Closing);
                    this.m_objTopLevelControl.Closed -= new EventHandler(_topLevelControl_Closed);
                    this.m_objTopLevelControl.Closed += new EventHandler(_topLevelControl_Closed);
                    this.m_objTopLevelControl.LocationChanged -= new EventHandler(MainWindow_LocationChanged);
                    this.m_objTopLevelControl.LocationChanged += new EventHandler(MainWindow_LocationChanged);
                    this.UpdateBounds();
                }
                return this.m_objTopLevelControl;
            }
        }


        public IntPtr Handle
        {
            get
            {
                return new System.Windows.Interop.WindowInteropHelper(this).Handle;
            }
        }

        public Game Game
        {
            get
            {
                return (Game)GetValue(GameProperty);
            }
            set
            {
                SetValue(GameProperty, value);
            }
        }


        public static readonly DependencyProperty GameProperty =
            DependencyProperty.Register("Game", typeof(Game), typeof(GameHost), new UIPropertyMetadata(null));

        public GameCanvas GameCanvas
        {
            get
            {
                return (GameCanvas)GetValue(GameCanvasProperty);
            }
            set
            {
                SetValue(GameCanvasProperty, value);

                if (this.m_objGameCanvas != value)
                {
                    if (this.m_objGameCanvas != null)
                        this.UnHookElementEvents();

                    this.m_objGameCanvas = value;

                    if (this.m_objGameCanvas != null)
                    {
                        this.HookElementEvents();
                        //this.LockThreadToProcessor();
                        //this.Width = this.Height = 1;
                        //ResizeMode = ResizeMode.NoResize;
                        //ShowInTaskbar = false;
                        //WindowState = WindowState.Normal;
                        //WindowStyle = WindowStyle.None;

                        //this.m_objFrontWindow = new Window();
                        //this.m_objFrontWindow.Width = this.m_objFrontWindow.Height = 1;
                        //this.m_objFrontWindow.ResizeMode = ResizeMode.NoResize;
                        //this.m_objFrontWindow.ShowInTaskbar = false;
                        //this.m_objFrontWindow.WindowState = WindowState.Normal;
                        //this.m_objFrontWindow.WindowStyle = WindowStyle.None;
                        //this.m_objFrontWindow.AllowsTransparency = true;
                        //this.m_objFrontWindow.Background = null;
                    }
                }
            }
        }


        public static readonly DependencyProperty GameCanvasProperty =
            DependencyProperty.Register("GameCanvas", typeof(GameCanvas), typeof(GameHost), new UIPropertyMetadata(null));
        
        #endregion


        #region Constructors

        public GameHost()
        {
            this.LockThreadToProcessor();
            //this.Width = this.Height = 1;
            //ResizeMode = ResizeMode.NoResize;
            //ShowInTaskbar = false;
            //WindowState = WindowState.Normal;
            //WindowStyle = WindowStyle.None;

            //this.m_objFrontWindow = this;
            //this.m_objFrontWindow.Width = this.m_objFrontWindow.Height = 1;
            //this.m_objFrontWindow.ResizeMode = ResizeMode.CanResizeWithGrip;
            //this.m_objFrontWindow.ShowInTaskbar = false;
            //this.m_objFrontWindow.WindowState = WindowState.Normal;
            //this.m_objFrontWindow.WindowStyle = WindowStyle.None;
            //this.m_objFrontWindow.AllowsTransparency = true;
            //this.m_objFrontWindow.Background = null;
        }

        //public GameHost(GameCanvas canvas)
        //{
        //    //this.game = canvas;
        //    m_objGameCanvas = canvas;
        //    this.HookElementEvents();
        //    this.LockThreadToProcessor();
        //    this.m_objGameControl = m_objGame;
        //    this.Width = this.Height = 1;
        //    ResizeMode = ResizeMode.NoResize;
        //    ShowInTaskbar = false;
        //    WindowState = WindowState.Normal;
        //    WindowStyle = WindowStyle.None;

        //    this.m_objFrontWindow = new Window();
        //    this.m_objFrontWindow.Width = this.m_objFrontWindow.Height = 1;
        //    this.m_objFrontWindow.ResizeMode = ResizeMode.NoResize;
        //    this.m_objFrontWindow.ShowInTaskbar = false;
        //    this.m_objFrontWindow.WindowState = WindowState.Normal;
        //    this.m_objFrontWindow.WindowStyle = WindowStyle.None;
        //    this.m_objFrontWindow.AllowsTransparency = true;
        //    this.m_objFrontWindow.Background = null;
        //}

        //public GameHost(Game game, GameCanvas gameCanvas)
        //{
        //    // TODO: Complete member initialization
        //    this.m_objGame = game;
        //    this.m_objGameCanvas = gameCanvas;
        //    this.HookElementEvents();
        //    this.LockThreadToProcessor();
        //    this.m_objGameControl = game;
        //    this.Width = this.Height = 1;
        //    ResizeMode = ResizeMode.NoResize;
        //    ShowInTaskbar = false;
        //    WindowState = WindowState.Normal;
        //    WindowStyle = WindowStyle.None;

        //    this.m_objFrontWindow = new Window();
        //    this.m_objFrontWindow.Width = this.m_objFrontWindow.Height = 1;
        //    this.m_objFrontWindow.ResizeMode = ResizeMode.NoResize;
        //    this.m_objFrontWindow.ShowInTaskbar = false;
        //    this.m_objFrontWindow.WindowState = WindowState.Normal;
        //    this.m_objFrontWindow.WindowStyle = WindowStyle.None;
        //    this.m_objFrontWindow.AllowsTransparency = true;
        //    this.m_objFrontWindow.Background = null;
        //}

        #endregion


        #region Public methods

        public void UpdateBounds()
        {
            if (this.IsVisible)
            {
                GeneralTransform gt = this.m_objGameCanvas.TransformToVisual(this.TopLevelWindow);

                // this.m_objFrontWindow.Width = 
                // this.m_objFrontWindow.Height = 
                //this.Width = this.m_objGameCanvas.ActualWidth;
                //this.Height = this.m_objGameCanvas.ActualHeight;

                // this.m_objFrontWindow.Left = 
                // this.m_objFrontWindow.Top = 
                //this.Left = this.TopLevelWindow.Left + gt.Transform(new Point(0, 0)).X + 8;
                //this.Top = this.TopLevelWindow.Top + gt.Transform(new Point(0, 0)).Y + 28;
            }
        }

        #endregion


        #region Life methods

        [DllImport("kernel32.dll")]
        private static extern UIntPtr SetThreadAffinityMask(IntPtr hThread, UIntPtr dwThreadAffinityMask);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentProcess();
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentThread();
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetProcessAffinityMask(IntPtr hProcess, out UIntPtr lpProcessAffinityMask, out UIntPtr lpSystemAffinityMask);
        private void LockThreadToProcessor()
        {
            UIntPtr lpProcessAffinityMask = UIntPtr.Zero;
            UIntPtr lpSystemAffinityMask = UIntPtr.Zero;
            if (GetProcessAffinityMask(GetCurrentProcess(), out lpProcessAffinityMask, out lpSystemAffinityMask) && (lpProcessAffinityMask != UIntPtr.Zero))
            {
                UIntPtr dwThreadAffinityMask = (UIntPtr)(lpProcessAffinityMask.ToUInt64() & (~lpProcessAffinityMask.ToUInt64() + 1));
                SetThreadAffinityMask(GetCurrentThread(), dwThreadAffinityMask);
            }
        }

        internal void Exit()
        {
            //this.exitRequested = true;
        }

        internal void PreRun()
        {
            if (this.m_bDoneRun)
            {
                throw new InvalidOperationException(Properties.Resources.NoMultipleRuns);
            }
            try
            {
            }
            catch (Exception)
            {
                PostRun();
                throw;
            }
        }

        internal void PostRun()
        {

            this.m_bDoneRun = true;

        }

        #endregion


        #region Events

        private void UnHookElementEvents()
        {
            this.m_objGameCanvas.SizeChanged -= new SizeChangedEventHandler(this.Game_SizeChanged);
            this.m_objGameCanvas.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.Game_IsVisibleChanged);
        }

        private void HookElementEvents()
        {
            this.m_objGameCanvas.SizeChanged += new SizeChangedEventHandler(this.Game_SizeChanged);
            this.m_objGameCanvas.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.Game_IsVisibleChanged);
        }

        void _topLevelControl_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner = null;
        }

        void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            this.UpdateBounds();
        }

        void Game_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateBounds();
        }

        void _topLevelControl_Closed(object sender, EventArgs e)
        {
         }

        void Game_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                if (!this.IsActive)
                {
                    this.Show();
                    //if (this.m_objFrontWindow.Owner != this)
                    //    this.m_objFrontWindow.Owner = this;
                    //this.m_objFrontWindow.Show();
                }
             }
            else
            {
                this.Hide();
                //this.m_objFrontWindow.Hide();
            }
        }

        #endregion

    }
}
