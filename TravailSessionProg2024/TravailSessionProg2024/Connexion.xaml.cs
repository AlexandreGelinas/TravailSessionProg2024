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
using TravailSessionProg2024.Classes;
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
            DateNaissance.MaxYear = DateTime.Now.AddYears(-18);
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

        private void submit_login_Click(object sender, RoutedEventArgs e)
        {
            if (Singleton.getInstance().Connexion(txtbox_login_username.Text, txtbox_login_mdp.Password))
            {
            login.Visibility=Visibility.Collapsed;
            register.Visibility = Visibility.Collapsed;
            stckpnl_btn_formulaire.Visibility = Visibility.Collapsed;
            deconnect.Visibility = Visibility.Visible;
                txtbox_login_mdp.Password = "";
                txtbox_login_username.Text = "";
            }
            else
            {
                ToggleThemeTeachingTip1.Content = "Impossible de vous connecter. Veuillez réessayer ou contacter l'administrateur.";
                ToggleThemeTeachingTip1.IsOpen = true;
                txtbox_login_mdp.Password = "";
            }
            
        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (Singleton.getInstance().getNiveauPermission()==1 || Singleton.getInstance().getNiveauPermission() == 2)
            {
                login.Visibility = Visibility.Collapsed;
                register.Visibility = Visibility.Collapsed;
                stckpnl_btn_formulaire.Visibility = Visibility.Collapsed;
                deconnect.Visibility = Visibility.Visible;
            }
        }

        private void disconnect_Click(object sender, RoutedEventArgs e)
        {
            Singleton.getInstance().Déconnexion();
            login.Visibility = Visibility.Visible;
            stckpnl_btn_formulaire.Visibility = Visibility.Visible;
            deconnect.Visibility = Visibility.Collapsed;
            btn_register.Background = null;
            btn_login.Background = new SolidColorBrush(Colors.OrangeRed);
        }

        private async void submit_register_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MotDePasse.Password) && !string.IsNullOrEmpty(Prenom.Text) && !string.IsNullOrEmpty(Nom.Text) && !string.IsNullOrEmpty(Adresse.Text) && !string.IsNullOrEmpty(Age.Text) && !string.IsNullOrEmpty(DateNaissance.Date.ToString()))
            {
              int i = Singleton.getInstance().BD_Ajouter(new Adhérent(-1, "null", MotDePasse.Password, Prenom.Text, Nom.Text, Adresse.Text, DateOnly.Parse(DateNaissance.Date.ToString().Substring(0,10)),int.Parse(Age.Text)));
              if (i != -1)
                {
                    ContentDialog contentDialog = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "Succès!",
                        Content = $"Votre compte a été créer, retenez votre code: {Singleton.getInstance().getAdhérentSelonID(i)}",
                        CloseButtonText = "Fermer"
                    };

                    // Afficher le ContentDialog et attendre sa fermeture
                    await contentDialog.ShowAsync();
                    login.Visibility = Visibility.Visible;
                    btn_register.Background = null;
                    btn_login.Background = new SolidColorBrush(Colors.OrangeRed);
                    register.Visibility = Visibility.Collapsed;
                    MotDePasse.Password = "";
                    Prenom.Text = "";
                    Nom.Text = "";
                    Age.Text = "";
                    Adresse.Text = "";
                }
            }
            else {
                ToggleThemeTeachingTip1.Content = "Des champs sont vides.";
                ToggleThemeTeachingTip1.IsOpen = true;
            }


        }
    }
}
