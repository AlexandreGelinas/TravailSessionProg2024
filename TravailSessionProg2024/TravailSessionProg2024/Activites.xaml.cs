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
        ObservableCollection<S�ance> listS;
        public Activites()
        {
            this.InitializeComponent();
            Singleton.getInstance().Ouverture();
            lvActivit�s.ItemsSource = Singleton.getInstance().Getliste_Activit�s();
        }

        private void lvActivit�s_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Pour pouvoir selectionner quelle activit� s'inscrire??
        }

        private void Btn_supprimer_Click(object sender, RoutedEventArgs e)
        {
            if (Singleton.getInstance().getNiveauPermission() != 2)
            {
                TeachingTips.Content = "Vous n'avez pas les permissions de supprimer \n une activit�.";
                TeachingTips.IsOpen = true;
            }
            else
            {
                Activit� act = (Activit�)lvActivit�s.SelectedItem;
                Singleton.getInstance().BD_Supprimer(act, lvActivit�s.SelectedIndex);
            }
        }

        private async void Btn_inscription_Click(object sender, RoutedEventArgs e)
        {
            if (Singleton.getInstance().getNiveauPermission() == 1)
            {
                Activit� act = (Activit�)lvActivit�s.SelectedItem;
                if (act == null) {
                    return;
                }
                listS = Singleton.getInstance().getListeS�ancesSelonActivit�(act.Nom);
                if (Singleton.getInstance().BD_VerificationSiParticipation(act.ID) == true)
                {
                    TeachingTips.Content = "Vous �tes deja inscrit a cette activit�e.";
                    TeachingTips.IsOpen = true;
                    return;
                }
                ObservableCollection<DateTime> listSHeure = new ObservableCollection<DateTime>();
                foreach (S�ance o in listS)
                {
                    listSHeure.Add(o.DateHeure);
                }
                GridView GridView = new GridView();
                GridView.ItemsSource = listSHeure;
               

                ContentDialog inscriptionDialog = new ContentDialog()
                {
                    XamlRoot = this.XamlRoot,
                    Title = $"S'inscrire a l'activit�: {act.Nom}",
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
                    S�ance obj;
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
                    TeachingTips.Content = "Vous avez �t� inscrit a la s�ance.";
                    TeachingTips.IsOpen = true;
                    }
                   
                }
            }
            else
            {
                TeachingTips.Content = "Vous devez �tre un adh�rant pour vous inscrire a une activit�";
                TeachingTips.IsOpen = true;
            }
        }
    }
}
