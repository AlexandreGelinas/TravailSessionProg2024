using TravailSessionProg2024;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            GestionWindow.mainWindow = this;
        }

        private void nvSample_Loaded(object sender, RoutedEventArgs e)
        {
            nvSample.SelectedItem = nvSample.MenuItems[0];
        }

        private void nvSample_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            ObservableCollection<NavigationViewItem> liste = new ObservableCollection<NavigationViewItem>();
            NavigationViewItem nvi = nvSample.SelectedItem as NavigationViewItem;
            int index = -1;
           
            foreach(NavigationViewItem nvItem in nvSample.MenuItems)
            {
                liste.Add(nvItem);
            }
            foreach (NavigationViewItem nvItem in nvSample.FooterMenuItems)
            {
                liste.Add(nvItem);
            }

            for (int ctr = 0; ctr < liste.Count(); ctr++)
            {
                if (liste[ctr] == nvi)
                {
                    index= ctr;
                }
            }

            switch (index)
            {
                case 0: contentFrame.Navigate(typeof(Activites));
                    break;

                case 1: contentFrame.Navigate(typeof(Connexion));
                    break;

                case 2: contentFrame.Navigate(typeof(Admin));
                    break;

                case 3: contentFrame.Navigate(typeof(Export));
                    break;

                case 4: contentFrame.Navigate(typeof(Statistique));
                    break;
            }
        }

        public void admin_affichage()
        {
            ObservableCollection<NavigationViewItem> liste = new ObservableCollection<NavigationViewItem>();

            foreach (NavigationViewItem nvItem in nvSample.MenuItems)
            {
                liste.Add(nvItem);
            }
            foreach (NavigationViewItem nvItem in nvSample.FooterMenuItems)
            {
                liste.Add(nvItem);
            }

            if (Singleton.getInstance().getNiveauPermission() == 2)
            {
                foreach (NavigationViewItem nvi in liste)
                {
                    nvi.Visibility = Visibility.Visible;
                }
            }
            if (Singleton.getInstance().getNiveauPermission() == 1 || Singleton.getInstance().getNiveauPermission() == 0)
            {
                foreach (NavigationViewItem nvi in liste)
                {
                    if (string.Equals(nvi.Tag, "Admin")){
                        nvi.Visibility = Visibility.Collapsed;
                    }
                    
                }
            }
            changementNomConnexion();
        }

        public void changementNomConnexion()
        {
             NavigationViewItem nvi = (NavigationViewItem)nvSample.FooterMenuItems[0];
            if (Singleton.getInstance().getNiveauPermission() == 0)
            {
                nvi.Content = "Connexion"; 
            }
            else if (Singleton.getInstance().getNiveauPermission() == 1)
            {
                nvi.Content = Singleton.getInstance().getAdhérentConnecter().Prenom;
            }
            else if (Singleton.getInstance().getNiveauPermission() == 2)
            {
                nvi.Content = Singleton.getInstance().getAdminConnecter().Prenom;
            }
        }

        internal class MessageDialog
        {
            private string v;

            public MessageDialog(string v)
            {
                this.v = v;
            }
        }
    }
}
