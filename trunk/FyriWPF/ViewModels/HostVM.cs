using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fyri.Games;
using Fyri.Xna.Presentation;

namespace FyriWPF.ViewModels
{
    public class HostVM : ViewModelBase
    {
        private Game m_objGame = null;
        private GameHost m_objHost = null;
        private GameCanvas m_objCanvas = null;

        public Game Game
        {
            get
            {
                return m_objGame;
            }
            set
            {
                if (m_objGame != value)
                {
                    m_objGame = value;
                    RaisePropertyChanged("Game");
                }
            }
        }

        public GameHost Host
        {
            get
            {
                return m_objHost;
            }
            set
            {
                if (m_objHost != value)
                {
                    m_objHost = value;
                    if (m_objGame != null)
                    {
                        m_objHost.Game = m_objGame;
                    }
                    if (m_objCanvas != null)
                    {
                        m_objHost.GameCanvas = m_objCanvas;
                        m_objCanvas.Host = m_objHost;
                        if (m_objGame != null)
                            m_objGame.Canvas = m_objCanvas;
                    }
                    RaisePropertyChanged("Host");
                }
            }
        }

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
                    m_objCanvas = value;
                    if (m_objGame != null)
                        m_objCanvas.Game = m_objGame;
                    if (m_objHost != null)
                        m_objCanvas.Host = Host;
                    RaisePropertyChanged("Canvas");
                }
            }
        }

        protected override void OnEnsureProperties()
        {
            Game = new DesignGame();
            Game.IsActive = true;

            base.OnEnsureProperties();
        }

        
    }
}
