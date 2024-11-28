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
    public sealed partial class Statistique : Page
    {

        private Singleton singleton;

        public Statistique()
        {
            this.InitializeComponent();
            singleton = Singleton.getInstance();
            ChargerStatistiques();
        }

        private void ChargerStatistiques()
        {
            try
            {
                // Nombre total d'adhérents
                int totalAdherents = singleton.GetTotalAdherents();
                tblAdherentsTotal.Text = $"Nombre total d'adhérents : {totalAdherents}";

                // Nombre total d'activités
                int totalActivites = singleton.GetTotalActivites();
                tblActivitesTotal.Text = $"Nombre total d'activités : {totalActivites}";

                // Adhérents par activité
                var adherentsParActivite = singleton.GetAdherentsParActivite();
                tblAdherentsParActivites.ItemsSource = adherentsParActivite
                    .Select(x => $"{x.Key} : {x.Value} adhérents")
                    .ToList();

                // Moyenne des notes par activité
                var moyenneNotes = singleton.GetMoyenneNotesParActivite();
                tblMoyenneNotesActivites.ItemsSource = moyenneNotes
                    .Select(x => $"{x.Key} : {x.Value:F2} / 10")
                    .ToList();

                // Moyenne des prix par activité
                var moyennePrix = singleton.GetMoyennePrixParActivite();
                tblPrixMoyenParActivites.ItemsSource = moyennePrix
                    .Select(x => $"{x.Key} : {x.Value:C}")
                    .ToList();

                // Revenu total pour toutes les activités
                decimal revenuTotal = singleton.GetRevenuTotalPourToutesLesActivites();
                tblRevenuTotalActivites.Text = $"Revenu total des activités : {revenuTotal:C}";

                // Total dépensé par chaque adhérent
                var totalDepenseParAdherent = singleton.GetTotalDepenseParAdherent();
                tblTotalDepenseParAdherents.ItemsSource = totalDepenseParAdherent
                    .Select(x => $"{x.Key} : {x.Value:C}")
                    .ToList();
            }
            catch (Exception ex)
            {
                // Gestion des erreurs
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Erreur",
                    Content = $"Une erreur est survenue lors du chargement des statistiques : {ex.Message}",
                    CloseButtonText = "OK"
                };
                errorDialog.ShowAsync();
            }
        }

    }
}
