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
    public sealed partial class Activites : Page
    {
        public Activites()
        {
            this.InitializeComponent();
            Singleton.getInstance().Ouverture();
            lvActivités.ItemsSource = Singleton.getInstance().Getliste_Activités();
        }

        private void lvActivités_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Pour pouvoir selectionner quelle activité s'inscrire??
        }

        private void Btn_supprimer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_inscription_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
