using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Threading;
using System.Windows;
using System.Diagnostics;
using System.Reflection;
using FyriDispatcher;

namespace FyriWPF.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region Variables 
        private Dispatcher m_objDispatcher;
        private bool m_bAsyncInit = true;
        private bool m_bDeferPropertyChanged = true;
        #endregion

        #region Constructors

        public ViewModelBase()
            : this(true)
        {
        }

        public ViewModelBase(bool asyncInitialize)
        {
            m_objDispatcher = CurrentDispatcher;

            m_bAsyncInit = asyncInitialize;

            Initialize();
        }

        

        #endregion

        #region Properties

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

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Event Handlers

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

        #region Methods

        private void Initialize()
        {
            if (m_objDispatcher.CheckAccess())
            {
                OnEnsureProperties();
            }
            else
            {
                if (m_bAsyncInit)
                    m_objDispatcher.BeginInvoke(new Dispatched(Initialize));
                else
                    m_objDispatcher.Invoke(new Dispatched(Initialize));
            }
        }

        protected virtual void OnEnsureProperties()
        {

        }

        #endregion

    }
}
