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
    //[System.Windows.Markup.ContentProperty("WPFHost")]
    public class GameCanvas : Canvas
    {
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private GameHost m_objWindow = null;
        private Game m_objGame = null;
        private bool m_bIsMouseVisible = false;
        private bool m_bIsShowing = false;

        public GameCanvas()
        {
            if (!(System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)))
            {
                //this.m_objWindow = new GameHost(this);
                //this.m_objWindow.Closed += new EventHandler(_window_Closed);

                //this.m_objWindow.Title = string.Empty;
            }
            else
                base.Children.Add(new ContentControl());
        }

        void _window_Closed(object sender, EventArgs e)
        {
            this.OnExiting(this, EventArgs.Empty);
            this.OnDisposed(this, EventArgs.Empty);
        }

        protected virtual void OnExiting(object sender, EventArgs args)
        {
        //    this.UnhookDeviceEvents();
        //    if (this.Exiting != null)
        //    {
        //        this.Exiting(null, args);
        //    }
        }

        protected virtual void OnDisposed(object sender, EventArgs args)
        {
            //if (this.Disposed != null)
            //{
            //    this.Disposed(null, args);
            //}
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

                //this.m_objWindow.Title = string.Empty;

                //if (value != null)
                //{
                //    this.m_objWindow = new GameHost(value, this);
                //    this.m_objWindow.Closed += new EventHandler(_window_Closed);
                //}
            }
        }

        
        public static readonly DependencyProperty GameProperty =
            DependencyProperty.Register("Game", typeof(Game), typeof(GameCanvas), new UIPropertyMetadata(null));

        public GameHost Host
        {
            get
            {
                return (GameHost)GetValue(HostProperty);
            }
            set
            {
                SetValue(HostProperty, value);

                this.m_objWindow = value;
                this.m_objWindow.Closed += new EventHandler(_window_Closed);
                //this._tickGenerator = new DispatcherTimer();
                //this._tickGenerator.Tick += new EventHandler(_tickGenerator_Tick);
                this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(GameCanvas_IsVisibleChanged);

                //this.m_objWindow.Title = string.Empty;

                //if (value != null)
                //{
                //    this.m_objWindow = new GameHost(value, this);
                //    this.m_objWindow.Closed += new EventHandler(_window_Closed);
                //}
            }
        }


        public static readonly DependencyProperty HostProperty =
            DependencyProperty.Register("Host", typeof(GameHost), typeof(GameCanvas), new UIPropertyMetadata(null));

        public GameHost Window
        {
            get
            {
                return this.m_objWindow;
            }
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

        void GameCanvas_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.IsShowing = (bool)e.NewValue;
        }

        public bool IsMouseVisible 
        {
            get
            {
                return m_bIsMouseVisible;
            }
            set
            {
                if (value)
                    this.Window.Cursor = System.Windows.Input.Cursors.None;
                else
                    this.Window.Cursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        public bool IsShowing
        {
            get
            {
                return m_bIsShowing;
            }
            set
            {
                if (m_bIsShowing = value)
                {
                    m_bIsShowing = value;
                }
            }
        }

        internal void Exit()
        {
            this.OnExiting(this, EventArgs.Empty);
            this.Window.Hide();
            this.Window.Close();
            this.Window.Exit();
        }

        public void Dispose()
        {
        }
    }
}
