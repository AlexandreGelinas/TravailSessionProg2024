using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public sealed partial class Activites : Page
    {
        ObservableCollection<Séance> listS;
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
            if (Singleton.getInstance().getNiveauPermission() != 2)
            {
                TeachingTips.Content = "Vous n'avez pas les permissions de supprimer \n une activité.";
                TeachingTips.IsOpen = true;
            }
            else
            {
                Activité act = (Activité)lvActivités.SelectedItem;
                Singleton.getInstance().BD_Supprimer(act, lvActivités.SelectedIndex);
            }
        }

        private async void Btn_inscription_Click(object sender, RoutedEventArgs e)
        {
            if (Singleton.getInstance().getNiveauPermission() == 1)
            {
                Activité act = (Activité)lvActivités.SelectedItem;
                if (act == null) {
                    return;
                }
                listS = Singleton.getInstance().getListeSéancesSelonActivité(act.Nom);
                if (Singleton.getInstance().BD_VerificationSiParticipation(act.ID) == true)
                {
                    TeachingTips.Content = "Vous êtes deja inscrit a cette activitée.";
                    TeachingTips.IsOpen = true;
                    return;
                }
                ObservableCollection<DateTime> listSHeure = new ObservableCollection<DateTime>();
                foreach (Séance o in listS)
                {
                    listSHeure.Add(o.DateHeure);
                }
                GridView GridView = new GridView();
                GridView.ItemsSource = listSHeure;
               

                ContentDialog inscriptionDialog = new ContentDialog()
                {
                    XamlRoot = this.XamlRoot,
                    Title = $"S'inscrire a l'activité: {act.Nom}",
                    Content = GridView,
                    PrimaryButtonText = "S'inscrire",
                    CloseButtonText = "Annuler"
                };
                var result = await inscriptionDialog.ShowAsync();
                if (result+"" == "Primary")
                {
                    if (GridView.SelectedIndex < 0)
                    {
                        return;
                    }
                    Séance obj;
                    try
                    {
                      obj = listS[GridView.SelectedIndex];
                    }
                    catch (Exception ex) {     
                        obj = null;
                    }
                    if (obj != null)
                    {
                    Singleton.getInstance().BD_AjouterParticipation(obj.ID);
                    TeachingTips.Content = "Vous avez été inscrit a la séance.";
                    TeachingTips.IsOpen = true;
                    }
                   
                }
            }
            else
            {
                TeachingTips.Content = "Vous devez être un adhérant pour vous inscrire a une activité";
                TeachingTips.IsOpen = true;
            }
        }
    }
}
