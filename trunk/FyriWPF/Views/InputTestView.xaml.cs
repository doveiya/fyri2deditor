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
using FyriWPF.ViewModels;

namespace FyriWPF.Views
{
    /// <summary>
    /// Interaction logic for InputTestView.xaml
    /// </summary>
    public partial class InputTestView : UserControl
    {
        public InputTestView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //this.KeyDown += ((DesignGameVM)(this.DataContext)).KeyDownHandler;
            this.KeyDown += myDesignGame.KeyDownHandler;
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            string aKeyIsDown = string.Empty;
        }
    }
}
