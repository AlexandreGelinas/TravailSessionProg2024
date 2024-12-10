using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TravailSessionProg2024
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Admin : Page
    {
        public Admin()
        {
            this.InitializeComponent();
        }

        private void btn_Adherent_Click(object sender, RoutedEventArgs e)
        {
            stckpnl_ad.Visibility = Visibility.Visible;
            stckpnl_ac.Visibility = Visibility.Collapsed;
            stckpnl_se.Visibility = Visibility.Collapsed;
            btn_Adherent.Background = new SolidColorBrush(Colors.OrangeRed);
            btn_Activite.Background = new SolidColorBrush(Colors.Transparent); 
            btn_Seance.Background = new SolidColorBrush(Colors.Transparent); 
        }

        private void btn_Activite_Click(object sender, RoutedEventArgs e)
        {
            stckpnl_ad.Visibility = Visibility.Collapsed;
            stckpnl_ac.Visibility = Visibility.Visible;
            stckpnl_se.Visibility = Visibility.Collapsed;
            btn_Adherent.Background = new SolidColorBrush(Colors.Transparent);
            btn_Activite.Background = new SolidColorBrush(Colors.OrangeRed);
            btn_Seance.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void btn_Seance_Click(object sender, RoutedEventArgs e)
        {
            stckpnl_ad.Visibility = Visibility.Collapsed;
            stckpnl_ac.Visibility = Visibility.Collapsed;
            stckpnl_se.Visibility = Visibility.Visible;
            btn_Adherent.Background = new SolidColorBrush(Colors.Transparent);
            btn_Activite.Background = new SolidColorBrush(Colors.Transparent);
            btn_Seance.Background = new SolidColorBrush(Colors.OrangeRed);
        }
    }
}
