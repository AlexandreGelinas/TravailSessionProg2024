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
using System.Threading.Tasks;

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
            ObservableCollection<Activité_Évaluation> list;
            list = Singleton.getInstance().BD_ActiviteUserParticipate();
            foreach (Activité_Évaluation obj in list)
            {
                obj.noteEtCommentaire();
            }
            lvNotes.ItemsSource = list;

        }

        private void lvNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void Button_Noter(object sender, RoutedEventArgs e)
        {
            if (Singleton.getInstance().getNiveauPermission() == 1)
            {
                if (lvNotes.SelectedIndex >= 0)
                {
                    Activité_Évaluation obj = (Activité_Évaluation)lvNotes.SelectedItem;
                    ContentDialog inscriptionDialog = new ContentDialog()
                    {
                        XamlRoot = this.XamlRoot,
                        Title = $"Noter l'activité: {obj.NomActivité}",
                        Content = new StackPanel
                        {
                            Children =
                            {
                            new TextBox
                            {
                                PlaceholderText = "Entrez une note",
                                Name = "TextBoxNote",
                                Margin = new Thickness(10)
                            },
                            new TextBox
                            {
                                PlaceholderText = "Ajoutez un commentaire",
                                Name = "TextBoxCommentaire",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10)
                            }
                            }
                        },
                        PrimaryButtonText = "Valider",
                        CloseButtonText = "Annuler"
                    };
                    var result = await inscriptionDialog.ShowAsync();
                    if (result + "" == "Primary")
                    {
                       if (obj.IdEvaluation == -1)
                        {
                            var stackPanel = (StackPanel)inscriptionDialog.Content;
                            Singleton.getInstance().BD_AjouterEvaluation(obj.IdActivité, double.Parse(((TextBox)stackPanel.Children[0]).Text.Replace('.', ',')), ((TextBox)stackPanel.Children[1]).Text);
                            TeachingTips.Content = "Évaluation ajoutée avec succès.";
                            TeachingTips.Title = "REDÉMARRAGE";
                            TeachingTips.IsOpen = true;
                            await Task.Delay(5000);
                            Frame.Navigate(typeof(Notes));
                            
                        }
                       else
                        {
                            var stackPanel = (StackPanel)inscriptionDialog.Content;
                            Singleton.getInstance().BD_ModifierEvaluation(obj.IdEvaluation, double.Parse(((TextBox)stackPanel.Children[0]).Text.Replace('.',',')), ((TextBox)stackPanel.Children[1]).Text);
                            TeachingTips.Content = "Évaluation changée avec succès.";
                            TeachingTips.Title = "REDÉMARRAGE";
                            TeachingTips.IsOpen = true;
                            await Task.Delay(5000);
                            Frame.Navigate(typeof(Notes));
                            
                        }
                    }
                }
                else
                {
                    TeachingTips.Content = "Vous devez être un adhérent pour noter une activité.";
                    TeachingTips.IsOpen = true;
                }
            }
        }
    }
}
