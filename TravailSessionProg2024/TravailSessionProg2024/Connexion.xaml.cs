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
    public sealed partial class Connexion : Page
    {
        public Connexion()
        {
            this.InitializeComponent();
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            login.Visibility = Visibility.Visible;
            btn_register.Background = null;
            btn_login.Background = new SolidColorBrush(Colors.OrangeRed);
            register.Visibility = Visibility.Collapsed;
        }

        private void btn_register_Click(object sender, RoutedEventArgs e)
        {
            register.Visibility = Visibility.Visible;
            btn_login.Background = null;
            btn_register.Background = new SolidColorBrush(Colors.OrangeRed);
            login.Visibility = Visibility.Collapsed;
        }
    }
}
