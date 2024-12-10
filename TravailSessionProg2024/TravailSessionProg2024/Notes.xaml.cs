using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using TravailSessionProg2024.Classes;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TravailSessionProg2024
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Notes : Page
    {
        ObservableCollection<Séance> listS;
        public Notes()
        {
            this.InitializeComponent();
            Singleton.getInstance().Ouverture();
            lvNotes.ItemsSource = Singleton.getInstance().Getliste_Activités();
            
        }

        private void lvNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Noter(object sender, RoutedEventArgs e)
        {

        }
    }
}
