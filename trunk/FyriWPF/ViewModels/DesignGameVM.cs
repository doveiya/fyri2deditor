using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fyri.Games;
using System.Windows.Input;
using Fyri.Xna.Presentation;

namespace FyriWPF.ViewModels
{
    class DesignGameVM : ViewModelBase
    {
        private DesignGame m_objDesignGame = null;
        private GameCanvas m_objCanvas = null;

        private string m_strKeysPressed = string.Empty;

        public bool m_bIsActive = false;
        public bool m_bIsUpdating = false;
        public bool m_bIsDrawing = false;

        private KeyEventHandler m_objKeyDown = null;

        public DesignGame DesignGame
        {
            get 
            {
                return m_objDesignGame;
            }
            set
            {
                if (m_objDesignGame != value)
                {
                    m_objDesignGame = value;
                    if(m_objCanvas != null)
                        m_objDesignGame.Canvas = m_objCanvas;
                    RaisePropertyChanged("Canvas");
                    RaisePropertyChanged("DesignGame");
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
                    if (m_objDesignGame != null)
                        m_objDesignGame.Canvas = m_objCanvas;
                    RaisePropertyChanged("Canvas");
                    RaisePropertyChanged("DesignGame");
                }
            }
        }

        public bool IsActive
        {
            get
            {
                if(m_objDesignGame != null)
                    return m_objDesignGame.IsActive;
                return false;
            }
            set
            {
                if (m_bIsActive != value)
                {
                    m_bIsActive = value;
                    RaisePropertyChanged("IsActive");
                }
            }
        }

        public bool IsUpdating
        {
            get
            {
                return m_bIsUpdating;
            }
            set
            {
                if (m_bIsUpdating != value)
                {
                    m_bIsUpdating = value;
                    RaisePropertyChanged("IsUpdating");
                }
            }
        }

        public bool IsDrawing
        {
            get
            {
                return m_bIsDrawing;
            }
            set
            {
                if (m_bIsDrawing != value)
                {
                    m_bIsDrawing = value;
                    RaisePropertyChanged("IsDrawing");
                }
            }
        }

        public string KeysPressed
        {
            get
            {
                if(m_objDesignGame != null)
                    return m_objDesignGame.KeysPressed;
                return string.Empty;
            }
        }

        public KeyEventHandler KeyDownHandler
        {
            get
            {
                return m_objKeyDown;
            }
            private set
            {
                if (m_objKeyDown != value)
                {
                    m_objKeyDown = value;
                    RaisePropertyChanged("KeyDownHandler");
                }
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            string madeit = string.Empty;
            RaisePropertyChanged("KeysPressed");
        }

        protected override void OnEnsureProperties()
        {
            DesignGame = new DesignGame();
            KeyDownHandler = new KeyEventHandler(OnKeyDown);

            base.OnEnsureProperties();
        }
    }
}
