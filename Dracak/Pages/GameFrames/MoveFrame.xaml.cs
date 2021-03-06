﻿using Dracak.Classes.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dracak.Pages
{
    /// <summary>
    /// Interakční logika pro Actions.xaml
    /// </summary>
    public partial class MoveFrame : Page
    {
        public MoveFrame()
        {
            InitializeComponent();
        }
        private void Click_Walk(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MoveFrameSelect(MoveOptions.Walk));
        }

        private void Click_March(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MoveFrameSelect(MoveOptions.March));
        }

        private void Click_Run(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MoveFrameSelect(MoveOptions.Run));
        }

        private void Click_GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();

        }
    }
}
