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
using Fyri.Xna.Presentation;
using FyriWPF.ViewModels;

namespace FyriWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : GameHost
    {
        public HostVM m_hostVM = null;

        public MainWindow()
        {
            InitializeComponent();

            m_hostVM = new HostVM();
            m_hostVM.Canvas = myCanvas;
            m_hostVM.Host = this;
            //myCanvas.Host = this;
            
            this.DataContext = m_hostVM;
        }
    }
}
