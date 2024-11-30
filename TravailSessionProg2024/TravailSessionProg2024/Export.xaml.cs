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
    public sealed partial class Export : Page
    {
        public Export()
        {
            this.InitializeComponent();
        }

        //Bouton d'exportation des activités
        private async void btn_export_activite(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileSavePicker();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

            picker.SuggestedFileName = "activites";
            picker.FileTypeChoices.Add("Fichier texte", new List<string>() { ".csv" });

            List<Activité> liste = new List<Activité>();
            foreach (Activité act in Singleton.getInstance().Getliste_Activités())
            {
                liste.Add(act);
            }

            if (liste.Count == 0) //Vérification avant, car sinon on créer un fichier vide
            {
                teachtip.Content = "La liste des adhérents est vide. Rien à exporter.";
                teachtip.IsOpen = true;
                return;
            }

            // Crée le fichier
            Windows.Storage.StorageFile monFichier = await picker.PickSaveFileAsync();

            

            // Vérification et écriture
            try
            {
                if (monFichier != null)
                {

                    var lignes = new List<string> { "Nom;Type;CoutOrganisation;PrixVenteParClient" }; // En-tête CSV
                    lignes.AddRange(liste.ConvertAll(x => x.ToString()));

                    await Windows.Storage.FileIO.WriteLinesAsync(monFichier, lignes, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                    teachtip.Content = "Fichier exporté avec succès.";
                    teachtip.IsOpen = true;
                }
                else
                {
                    teachtip.Content = "Exportation annulée.";
                    teachtip.IsOpen = true;
                }
            }
            catch (Exception ex)
            { 
                teachtip.Content = $"Erreur lors de l'exportation : {ex.Message}";
                teachtip.IsOpen = true;
            }
        }

        //Bouton d'exportation des adhérents
        private async void btn_export_adherent(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileSavePicker();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(GestionWindow.mainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

            picker.SuggestedFileName = "adhérent";
            picker.FileTypeChoices.Add("Fichier texte", new List<string>() { ".csv" });

            List<Adhérent> liste = new List<Adhérent>();
            foreach (Adhérent adh in Singleton.getInstance().Getliste_Adhérents())
            {
                liste.Add(adh);
            }

            if (liste.Count == 0) //Vérification avant, car sinon on créer un fichier vide
            {
                teachtip.Content = "La liste des adhérents est vide. Rien à exporter.";
                teachtip.IsOpen = true;
                return;
            }

            // Crée le fichier
            Windows.Storage.StorageFile monFichier = await picker.PickSaveFileAsync();

            // Vérification et écriture
            try
            {
                if (monFichier != null)
                {

                    var lignes = new List<string> { "Prenom;Nom;Adresse;DateNaissance;Age;CodeAdherent;MotDePasse" }; // En-tête CSV
                    lignes.AddRange(liste.ConvertAll(x => x.ToString()));

                    await Windows.Storage.FileIO.WriteLinesAsync(monFichier, lignes, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                    teachtip.Content = "Fichier exporté avec succès.";
                    teachtip.IsOpen = true;
                }
                else
                {
                    teachtip.Content = "Exportation annulée sans erreur.";
                    teachtip.IsOpen = true;
                }
            }
            catch (Exception ex)
            {  
                teachtip.Content = $"Erreur lors de l'exportation : {ex.Message}";
                teachtip.IsOpen = true;
            }

        }
    }
}
