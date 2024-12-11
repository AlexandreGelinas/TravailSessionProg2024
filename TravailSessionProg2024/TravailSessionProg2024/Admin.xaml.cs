using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.VisualBasic;
using Microsoft.Win32;
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
    public sealed partial class Admin : Page
    {
        public Admin()
        {
            this.InitializeComponent();
            lvActivités.ItemsSource = Singleton.getInstance().Getliste_Activités();
            lvAdhérents.ItemsSource = Singleton.getInstance().Getliste_Adhérents();
            lvSéances.ItemsSource   = Singleton.getInstance().Getliste_Séances();
        }

        private void btn_Adherent_Click(object sender, RoutedEventArgs e)
        {
            stckpnl_ad.Visibility = Visibility.Visible;
            stckpnl_ac.Visibility = Visibility.Collapsed;
            stckpnl_se.Visibility = Visibility.Collapsed;
            btn_Adherent.Background = new SolidColorBrush(Colors.OrangeRed);
            btn_Activite.Background = new SolidColorBrush(Colors.Transparent); 
            btn_Seance.Background = new SolidColorBrush(Colors.Transparent); 
        }

        private void btn_Activite_Click(object sender, RoutedEventArgs e)
        {
            stckpnl_ad.Visibility = Visibility.Collapsed;
            stckpnl_ac.Visibility = Visibility.Visible;
            stckpnl_se.Visibility = Visibility.Collapsed;
            btn_Adherent.Background = new SolidColorBrush(Colors.Transparent);
            btn_Activite.Background = new SolidColorBrush(Colors.OrangeRed);
            btn_Seance.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void btn_Seance_Click(object sender, RoutedEventArgs e)
        {
            stckpnl_ad.Visibility = Visibility.Collapsed;
            stckpnl_ac.Visibility = Visibility.Collapsed;
            stckpnl_se.Visibility = Visibility.Visible;
            btn_Adherent.Background = new SolidColorBrush(Colors.Transparent);
            btn_Activite.Background = new SolidColorBrush(Colors.Transparent);
            btn_Seance.Background = new SolidColorBrush(Colors.OrangeRed);
        }

        private async void btn_ajouterAD_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "Création de compte",
                Content = new StackPanel
                {
                    Children =
                            {
                            new PasswordBox
                            {
                                PlaceholderText = "Entrez votre mot de passe",
                                Name = "MotDePasse",
                                Margin = new Thickness(10)
                            },
                            new TextBox
                            {
                                PlaceholderText = "Entrez votre prénom",
                                Name = "Prenom",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10)
                            },
                            new TextBox
                            {
                                PlaceholderText = "Entrez votre nom",
                                Name = "Nom",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10)
                            },
                            new TextBox
                            {
                                PlaceholderText = "Entrez votre adresse",
                                Name = "Adresse",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10)
                            },
                            new DatePicker
                            {
                                Name = "DateNaissance",
                                MaxYear = DateTime.Now.AddYears(-18),
                                Margin = new Thickness(10)
                            },
                            new NumberBox
                            {
                                PlaceholderText = "Entrez votre âge",
                                Name = "Age",
                                Minimum = 18,
                                Margin = new Thickness(10)
                            }
                            }
                },
                CloseButtonText = "Fermer",
                PrimaryButtonText = "Créer"
            };

           var result = await contentDialog.ShowAsync();

            if (result + "" == "Primary")
            {
                try
                {
                    var stackPanel = (StackPanel)contentDialog.Content;
                    string motdepasse = ((PasswordBox)stackPanel.Children[0]).Password;
                    string prenom = ((TextBox)stackPanel.Children[1]).Text;
                    string nom = ((TextBox)stackPanel.Children[2]).Text;
                    string adresse = ((TextBox)stackPanel.Children[3]).Text;
                    string datedenaissance = ((DatePicker)stackPanel.Children[4]).Date.ToString();
                    int age = int.Parse(((NumberBox)stackPanel.Children[5]).Text);

                    if (!string.IsNullOrEmpty(motdepasse) && !string.IsNullOrEmpty(prenom) && !string.IsNullOrEmpty(nom) && !string.IsNullOrEmpty(adresse) && age != 0 && !string.IsNullOrEmpty(datedenaissance))
                    {
                        int i = Singleton.getInstance().BD_Ajouter(new Adhérent(-1, "null", motdepasse, prenom, nom, adresse, DateOnly.Parse(datedenaissance.Substring(0, 10)), age));
                        if (i != -1)
                        {
                            ContentDialog contentDialog2 = new ContentDialog
                            {
                                XamlRoot = this.XamlRoot,
                                Title = "Succès!",
                                Content = $"Le compte a été créer, le code est: {Singleton.getInstance().getAdhérentSelonID(i)}",
                                CloseButtonText = "Fermer"
                            };

                            // Afficher le ContentDialog et attendre sa fermeture
                            await contentDialog2.ShowAsync();

                        }

                    }
                }
                catch (Exception ex)
                {
                    ContentDialog contentDialog3 = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "ERREUR!",
                        Content = $"Des champs étaient vides ou un mauvais format a été entré",
                        CloseButtonText = "Fermer"
                    };

                    // Afficher le ContentDialog et attendre sa fermeture
                    await contentDialog3.ShowAsync();
                }

               
                }
            }

        private async void btn_supprimerAD_Click(object sender, RoutedEventArgs e)
        {
            if (lvAdhérents.SelectedIndex >= 0)
            {
                int index = lvAdhérents.SelectedIndex;
                Adhérent obj = (Adhérent)lvAdhérents.SelectedItem;

                Singleton.getInstance().BD_Supprimer(obj);

                lvAdhérents.ItemsSource = Singleton.getInstance().Getliste_Adhérents();

                ContentDialog contentDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Succès!",
                    Content = $"L'utilisateur a été supprimé.",
                    CloseButtonText = "Fermer"
                };

                // Afficher le ContentDialog et attendre sa fermeture
                await contentDialog.ShowAsync();
            }
        }

        private async void btn_modifierAD_Click(object sender, RoutedEventArgs e)
        {
            Adhérent obj = (Adhérent)lvAdhérents.SelectedItem;
            ContentDialog contentDialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "Modification de compte",
                Content = new StackPanel
                {
                    Children =
                            {
                            new PasswordBox
                            {
                                PlaceholderText = "Entrez votre mot de passe",
                                Name = "MotDePasse",
                                Margin = new Thickness(10),
                                Password = obj.MotDePasse
                            },
                            new TextBox
                            {
                                PlaceholderText = "Entrez votre prénom",
                                Name = "Prenom",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10),
                                Text = obj.Prenom
                            },
                            new TextBox
                            {
                                PlaceholderText = "Entrez votre nom",
                                Name = "Nom",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10),
                                Text = obj.Nom
                            },
                            new TextBox
                            {
                                PlaceholderText = "Entrez votre adresse",
                                Name = "Adresse",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10),
                                Text = obj.Adresse
                            },
                            new DatePicker
                            {
                                Name = "DateNaissance",
                                MaxYear = DateTime.Now.AddYears(-18),
                                Margin = new Thickness(10),
                                SelectedDate = obj.DateNaissance.ToDateTime(TimeOnly.MinValue)
                            },
                            new NumberBox
                            {
                                PlaceholderText = "Entrez votre âge",
                                Name = "Age",
                                Minimum = 18,
                                Margin = new Thickness(10),
                                Text = obj.Age+""
                            }
                            }
                },
                CloseButtonText = "Fermer",
                PrimaryButtonText = "Modifié"
            };

            var result = await contentDialog.ShowAsync();

            if (result + "" == "Primary")
            {
                try
                {
                    var stackPanel = (StackPanel)contentDialog.Content;
                    string motdepasse = ((PasswordBox)stackPanel.Children[0]).Password;
                    string prenom = ((TextBox)stackPanel.Children[1]).Text;
                    string nom = ((TextBox)stackPanel.Children[2]).Text;
                    string adresse = ((TextBox)stackPanel.Children[3]).Text;
                    string datedenaissance = ((DatePicker)stackPanel.Children[4]).Date.ToString();
                    int age = int.Parse(((NumberBox)stackPanel.Children[5]).Text);

                    if (!string.IsNullOrEmpty(motdepasse) && !string.IsNullOrEmpty(prenom) && !string.IsNullOrEmpty(nom) && !string.IsNullOrEmpty(adresse) && age != 0 && !string.IsNullOrEmpty(datedenaissance))
                    {
                        Singleton.getInstance().BD_ModifierAdherent(obj.CodeAdherent, nom, prenom, adresse, DateOnly.Parse(datedenaissance.Substring(0, 10)), motdepasse);

                        ContentDialog contentDialog2 = new ContentDialog
                        {
                            XamlRoot = this.XamlRoot,
                            Title = "Succès!",
                            Content = $"Le compte a été modifié avec succès",
                            CloseButtonText = "Fermer"
                        };

                        // Afficher le ContentDialog et attendre sa fermeture
                        await contentDialog2.ShowAsync();

                        lvAdhérents.ItemsSource = Singleton.getInstance().Getliste_Adhérents();
                    }
                }
                catch (Exception ex)
                {
                    ContentDialog contentDialog3 = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "ERREUR!",
                        Content = $"Des champs étaient vides ou un mauvais format a été entré",
                        CloseButtonText = "Fermer"
                    };

                    // Afficher le ContentDialog et attendre sa fermeture
                    await contentDialog3.ShowAsync();
                }


            }
        }

        private async void btn_ajouterAC_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "Création d'activité",
                Content = new StackPanel
                {
                    Children =
                            {
                            new TextBox
                            {
                                PlaceholderText = "Entrez le nom",
                                Name = "nom",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10)
                            },
                            new TextBox
                            {
                                PlaceholderText = "Entrez le type",
                                Name = "type",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10)
                            },
                            new TextBox
                            {
                                PlaceholderText = "Entrez le cout",
                                Name = "cout",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10)
                            },
                            new TextBox
                            {
                                PlaceholderText = "Entrez le prix",
                                Name = "prix",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10)
                            }
                            }
                },
                CloseButtonText = "Fermer",
                PrimaryButtonText = "Créer"
            };

            var result = await contentDialog.ShowAsync();

            if (result + "" == "Primary")
            {
                try
                {
                    var stackPanel = (StackPanel)contentDialog.Content;
                    string nom = ((TextBox)stackPanel.Children[0]).Text;
                    string type = ((TextBox)stackPanel.Children[1]).Text;
                    double cout = Double.Parse(((TextBox)stackPanel.Children[2]).Text);
                    double prix = Double.Parse (((TextBox)stackPanel.Children[3]).Text);


                    if (!string.IsNullOrEmpty(nom) && !string.IsNullOrEmpty(type) && cout >= 0 && prix >=0)
                    {
                        Singleton.getInstance().BD_AjouterActivité(nom, type, cout, prix);
                       
                            ContentDialog contentDialog2 = new ContentDialog
                            {
                                XamlRoot = this.XamlRoot,
                                Title = "Succès!",
                                Content = $"Activité créée avec succès.",
                                CloseButtonText = "Fermer"
                            };

                            // Afficher le ContentDialog et attendre sa fermeture
                            await contentDialog2.ShowAsync();

                        lvActivités.ItemsSource = Singleton.getInstance().Getliste_Activités();
                    }
                }
                catch (Exception ex)
                {
                    ContentDialog contentDialog3 = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "ERREUR!",
                        Content = $"Des champs étaient vides ou un mauvais format a été entré",
                        CloseButtonText = "Fermer"
                    };

                    // Afficher le ContentDialog et attendre sa fermeture
                    await contentDialog3.ShowAsync();
                }


            }
        }

        private async void btn_supprimerAC_Click(object sender, RoutedEventArgs e)
        {
            if (lvActivités.SelectedIndex >= 0)
            {
                int index = lvActivités.SelectedIndex;
                Activité obj = (Activité)lvActivités.SelectedItem;

                Singleton.getInstance().BD_SuppriméActivité(obj.Nom);

                lvActivités.ItemsSource = Singleton.getInstance().Getliste_Activités();

                ContentDialog contentDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Succès!",
                    Content = $"L'activité a été supprimé.",
                    CloseButtonText = "Fermer"
                };

                // Afficher le ContentDialog et attendre sa fermeture
                await contentDialog.ShowAsync();
            }
        }


        private async void btn_modifierAC_Click(object sender, RoutedEventArgs e)
        {
            Activité obj = (Activité)lvActivités.SelectedItem;
            ContentDialog contentDialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "Modification d'activité",
                Content = new StackPanel
                {
                    Children =
                            {
                            new TextBox
                            {
                                PlaceholderText = "Entrez le nom",
                                Name = "nom",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10),
                                Text = obj.Nom
                            },
                            new TextBox
                            {
                                PlaceholderText = "Entrez le type",
                                Name = "type",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10),
                                Text = obj.Type
                            },
                            new TextBox
                            {
                                PlaceholderText = "Entrez le cout",
                                Name = "cout",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10),
                                Text = obj.CoutOrganisation+""
                            },
                            new TextBox
                            {
                                PlaceholderText = "Entrez le prix",
                                Name = "prix",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10),
                                Text = obj.PrixVenteParClient+""
                            }
                            }
                },
                CloseButtonText = "Fermer",
                PrimaryButtonText = "Modifié"
            };

            var result = await contentDialog.ShowAsync();

            if (result + "" == "Primary")
            {
                try
                {
                    var stackPanel = (StackPanel)contentDialog.Content;
                    string nom = ((TextBox)stackPanel.Children[0]).Text;
                    string type = ((TextBox)stackPanel.Children[1]).Text;
                    double cout = Double.Parse(((TextBox)stackPanel.Children[2]).Text);
                    double prix = Double.Parse(((TextBox)stackPanel.Children[3]).Text);

                    if (!string.IsNullOrEmpty(nom) && !string.IsNullOrEmpty(type) && cout >= 0 && prix >= 0)
                    {
                        Singleton.getInstance().BD_ModifierActivité(obj.Nom, nom, type, cout, prix);

                        ContentDialog contentDialog2 = new ContentDialog
                        {
                            XamlRoot = this.XamlRoot,
                            Title = "Succès!",
                            Content = $"L'activité a été modifié avec succès",
                            CloseButtonText = "Fermer"
                        };

                        // Afficher le ContentDialog et attendre sa fermeture
                        await contentDialog2.ShowAsync();

                        lvActivités.ItemsSource = Singleton.getInstance().Getliste_Activités();
                    }
                }
                catch (Exception ex)
                {
                    ContentDialog contentDialog3 = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "ERREUR!",
                        Content = $"Des champs étaient vides ou un mauvais format a été entré",
                        CloseButtonText = "Fermer"
                    };

                    // Afficher le ContentDialog et attendre sa fermeture
                    await contentDialog3.ShowAsync();
                }


            }
        }

        private async void btn_ajouterSE_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "Création de séance",
                Content = new StackPanel
                {
                    Children =
                            {
                            new TextBox
                            {
                                PlaceholderText = "Entrez l'id d'activité",
                                Name = "idActivité",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10),
                                Width = 300
                            },
                            new DatePicker
                            {
                                Name = "Date",
                                Margin = new Thickness(10),
                                Width = 300
                            },
                            new TimePicker
                            {
                                Name = "Temps",
                                Margin = new Thickness(10),
                                Width = 300
                            },
                            new TextBox
                            {
                                PlaceholderText = "Entrez le nombre de places",
                                Name = "Adresse",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10),
                                Width = 300
                            }
                            }
                },
                CloseButtonText = "Fermer",
                PrimaryButtonText = "Créer"
            };

            var result = await contentDialog.ShowAsync();

            if (result + "" == "Primary")
            {
                try
                {
                    var stackPanel = (StackPanel)contentDialog.Content;
                    int idActivité = int.Parse(((TextBox)stackPanel.Children[0]).Text);
                    DateTime dt = ((DatePicker)stackPanel.Children[1]).Date.DateTime + ((TimePicker)stackPanel.Children[2]).Time;
                    int nombrePlaces = int.Parse(((TextBox)stackPanel.Children[3]).Text);


                    if (idActivité > 0 && dt != null && nombrePlaces > 0)
                    {
                        Singleton.getInstance().BD_AjouterSéance(idActivité, dt, nombrePlaces);
                       
                            ContentDialog contentDialog2 = new ContentDialog
                            {
                                XamlRoot = this.XamlRoot,
                                Title = "Succès!",
                                Content = $"La séance a été créée.",
                                CloseButtonText = "Fermer"
                            };

                            // Afficher le ContentDialog et attendre sa fermeture
                            await contentDialog2.ShowAsync();

                        }

                    
                }
                catch (Exception ex)
                {
                    ContentDialog contentDialog3 = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "ERREUR!",
                        Content = $"Des champs étaient vides ou un mauvais format a été entré",
                        CloseButtonText = "Fermer"
                    };

                    // Afficher le ContentDialog et attendre sa fermeture
                    await contentDialog3.ShowAsync();
                }


            }
        }

        private async void btn_supprimerSE_Click(object sender, RoutedEventArgs e)
        {
            if (lvSéances.SelectedIndex >= 0)
            {
                int index = lvSéances.SelectedIndex;
                Séance obj = (Séance)lvSéances.SelectedItem;

                Singleton.getInstance().BD_SuppriméSéance(obj.ID);

                lvSéances.ItemsSource = Singleton.getInstance().Getliste_Séances();

                ContentDialog contentDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Succès!",
                    Content = $"La séance a été supprimé.",
                    CloseButtonText = "Fermer"
                };

                // Afficher le ContentDialog et attendre sa fermeture
                await contentDialog.ShowAsync();
            }
        }

        private async void btn_modifierSE_Click(object sender, RoutedEventArgs e)
        {
            Séance obj = (Séance)lvSéances.SelectedItem;
            ContentDialog contentDialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "Modification de séance",
                Content = new StackPanel
                {
                    Children =
                           {
                            new TextBox
                            {
                                PlaceholderText = "Entrez l'id d'activité",
                                Name = "idActivité",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10),
                                Width = 300,
                                Text = obj.IdActivité+""
                            },
                            new DatePicker
                            {
                                Name = "Date",
                                Margin = new Thickness(10),
                                Width = 300,
                                Date = obj.DateHeure.Date
                            },
                            new TimePicker
                            {
                                Name = "Temps",
                                Margin = new Thickness(10),
                                Width = 300,
                                SelectedTime = TimeSpan.Parse($"{obj.DateHeure.Hour}:{obj.DateHeure.Minute}:{obj.DateHeure.Second}")
                            },
                            new TextBox
                            {
                                PlaceholderText = "Entrez le nombre de places",
                                Name = "Adresse",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10),
                                Width = 300,
                                Text = obj.NombrePlaces+""
                            }
                            }
                },
                CloseButtonText = "Fermer",
                PrimaryButtonText = "Modifié"
            };

            var result = await contentDialog.ShowAsync();

            if (result + "" == "Primary")
            {
                try
                {
                    var stackPanel = (StackPanel)contentDialog.Content;
                    int idActivité = int.Parse(((TextBox)stackPanel.Children[0]).Text);
                    DateTime dt = ((DatePicker)stackPanel.Children[1]).Date.DateTime + ((TimePicker)stackPanel.Children[2]).Time;
                    int nombrePlaces = int.Parse(((TextBox)stackPanel.Children[3]).Text);

                    if (idActivité > 0 && dt != null && nombrePlaces > 0)
                    {
                        Singleton.getInstance().BD_ModifiéSéance(obj.ID, dt, nombrePlaces);

                        ContentDialog contentDialog2 = new ContentDialog
                        {
                            XamlRoot = this.XamlRoot,
                            Title = "Succès!",
                            Content = $"La séance a été modifié avec succès",
                            CloseButtonText = "Fermer"
                        };

                        // Afficher le ContentDialog et attendre sa fermeture
                        await contentDialog2.ShowAsync();

                        lvSéances.ItemsSource = Singleton.getInstance().Getliste_Séances();
                    }
                }
                catch (Exception ex)
                {
                    ContentDialog contentDialog3 = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "ERREUR!",
                        Content = $"Des champs étaient vides ou un mauvais format a été entré",
                        CloseButtonText = "Fermer"
                    };

                    // Afficher le ContentDialog et attendre sa fermeture
                    await contentDialog3.ShowAsync();
                }


            }
        }
    }
    }

