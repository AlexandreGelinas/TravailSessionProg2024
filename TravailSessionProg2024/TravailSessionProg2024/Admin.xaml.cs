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
            lvActivit�s.ItemsSource = Singleton.getInstance().Getliste_Activit�s();
            lvAdh�rents.ItemsSource = Singleton.getInstance().Getliste_Adh�rents();
            lvS�ances.ItemsSource   = Singleton.getInstance().Getliste_S�ances();
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
                Title = "Cr�ation de compte",
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
                                PlaceholderText = "Entrez votre pr�nom",
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
                                PlaceholderText = "Entrez votre �ge",
                                Name = "Age",
                                Minimum = 18,
                                Margin = new Thickness(10)
                            }
                            }
                },
                CloseButtonText = "Fermer",
                PrimaryButtonText = "Cr�er"
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
                        int i = Singleton.getInstance().BD_Ajouter(new Adh�rent(-1, "null", motdepasse, prenom, nom, adresse, DateOnly.Parse(datedenaissance.Substring(0, 10)), age));
                        if (i != -1)
                        {
                            ContentDialog contentDialog2 = new ContentDialog
                            {
                                XamlRoot = this.XamlRoot,
                                Title = "Succ�s!",
                                Content = $"Le compte a �t� cr�er, le code est: {Singleton.getInstance().getAdh�rentSelonID(i)}",
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
                        Content = $"Des champs �taient vides ou un mauvais format a �t� entr�",
                        CloseButtonText = "Fermer"
                    };

                    // Afficher le ContentDialog et attendre sa fermeture
                    await contentDialog3.ShowAsync();
                }

               
                }
            }

        private async void btn_supprimerAD_Click(object sender, RoutedEventArgs e)
        {
            if (lvAdh�rents.SelectedIndex >= 0)
            {
                int index = lvAdh�rents.SelectedIndex;
                Adh�rent obj = (Adh�rent)lvAdh�rents.SelectedItem;

                Singleton.getInstance().BD_Supprimer(obj);

                lvAdh�rents.ItemsSource = Singleton.getInstance().Getliste_Adh�rents();

                ContentDialog contentDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Succ�s!",
                    Content = $"L'utilisateur a �t� supprim�.",
                    CloseButtonText = "Fermer"
                };

                // Afficher le ContentDialog et attendre sa fermeture
                await contentDialog.ShowAsync();
            }
        }

        private async void btn_modifierAD_Click(object sender, RoutedEventArgs e)
        {
            Adh�rent obj = (Adh�rent)lvAdh�rents.SelectedItem;
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
                                PlaceholderText = "Entrez votre pr�nom",
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
                                PlaceholderText = "Entrez votre �ge",
                                Name = "Age",
                                Minimum = 18,
                                Margin = new Thickness(10),
                                Text = obj.Age+""
                            }
                            }
                },
                CloseButtonText = "Fermer",
                PrimaryButtonText = "Modifi�"
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
                            Title = "Succ�s!",
                            Content = $"Le compte a �t� modifi� avec succ�s",
                            CloseButtonText = "Fermer"
                        };

                        // Afficher le ContentDialog et attendre sa fermeture
                        await contentDialog2.ShowAsync();

                        lvAdh�rents.ItemsSource = Singleton.getInstance().Getliste_Adh�rents();
                    }
                }
                catch (Exception ex)
                {
                    ContentDialog contentDialog3 = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "ERREUR!",
                        Content = $"Des champs �taient vides ou un mauvais format a �t� entr�",
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
                Title = "Cr�ation d'activit�",
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
                PrimaryButtonText = "Cr�er"
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
                        Singleton.getInstance().BD_AjouterActivit�(nom, type, cout, prix);
                       
                            ContentDialog contentDialog2 = new ContentDialog
                            {
                                XamlRoot = this.XamlRoot,
                                Title = "Succ�s!",
                                Content = $"Activit� cr��e avec succ�s.",
                                CloseButtonText = "Fermer"
                            };

                            // Afficher le ContentDialog et attendre sa fermeture
                            await contentDialog2.ShowAsync();

                        lvActivit�s.ItemsSource = Singleton.getInstance().Getliste_Activit�s();
                    }
                }
                catch (Exception ex)
                {
                    ContentDialog contentDialog3 = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "ERREUR!",
                        Content = $"Des champs �taient vides ou un mauvais format a �t� entr�",
                        CloseButtonText = "Fermer"
                    };

                    // Afficher le ContentDialog et attendre sa fermeture
                    await contentDialog3.ShowAsync();
                }


            }
        }

        private async void btn_supprimerAC_Click(object sender, RoutedEventArgs e)
        {
            if (lvActivit�s.SelectedIndex >= 0)
            {
                int index = lvActivit�s.SelectedIndex;
                Activit� obj = (Activit�)lvActivit�s.SelectedItem;

                Singleton.getInstance().BD_Supprim�Activit�(obj.Nom);

                lvActivit�s.ItemsSource = Singleton.getInstance().Getliste_Activit�s();

                ContentDialog contentDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Succ�s!",
                    Content = $"L'activit� a �t� supprim�.",
                    CloseButtonText = "Fermer"
                };

                // Afficher le ContentDialog et attendre sa fermeture
                await contentDialog.ShowAsync();
            }
        }


        private async void btn_modifierAC_Click(object sender, RoutedEventArgs e)
        {
            Activit� obj = (Activit�)lvActivit�s.SelectedItem;
            ContentDialog contentDialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "Modification d'activit�",
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
                PrimaryButtonText = "Modifi�"
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
                        Singleton.getInstance().BD_ModifierActivit�(obj.Nom, nom, type, cout, prix);

                        ContentDialog contentDialog2 = new ContentDialog
                        {
                            XamlRoot = this.XamlRoot,
                            Title = "Succ�s!",
                            Content = $"L'activit� a �t� modifi� avec succ�s",
                            CloseButtonText = "Fermer"
                        };

                        // Afficher le ContentDialog et attendre sa fermeture
                        await contentDialog2.ShowAsync();

                        lvActivit�s.ItemsSource = Singleton.getInstance().Getliste_Activit�s();
                    }
                }
                catch (Exception ex)
                {
                    ContentDialog contentDialog3 = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "ERREUR!",
                        Content = $"Des champs �taient vides ou un mauvais format a �t� entr�",
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
                Title = "Cr�ation de s�ance",
                Content = new StackPanel
                {
                    Children =
                            {
                            new TextBox
                            {
                                PlaceholderText = "Entrez l'id d'activit�",
                                Name = "idActivit�",
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
                PrimaryButtonText = "Cr�er"
            };

            var result = await contentDialog.ShowAsync();

            if (result + "" == "Primary")
            {
                try
                {
                    var stackPanel = (StackPanel)contentDialog.Content;
                    int idActivit� = int.Parse(((TextBox)stackPanel.Children[0]).Text);
                    DateTime dt = ((DatePicker)stackPanel.Children[1]).Date.DateTime + ((TimePicker)stackPanel.Children[2]).Time;
                    int nombrePlaces = int.Parse(((TextBox)stackPanel.Children[3]).Text);


                    if (idActivit� > 0 && dt != null && nombrePlaces > 0)
                    {
                        Singleton.getInstance().BD_AjouterS�ance(idActivit�, dt, nombrePlaces);
                       
                            ContentDialog contentDialog2 = new ContentDialog
                            {
                                XamlRoot = this.XamlRoot,
                                Title = "Succ�s!",
                                Content = $"La s�ance a �t� cr��e.",
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
                        Content = $"Des champs �taient vides ou un mauvais format a �t� entr�",
                        CloseButtonText = "Fermer"
                    };

                    // Afficher le ContentDialog et attendre sa fermeture
                    await contentDialog3.ShowAsync();
                }


            }
        }

        private async void btn_supprimerSE_Click(object sender, RoutedEventArgs e)
        {
            if (lvS�ances.SelectedIndex >= 0)
            {
                int index = lvS�ances.SelectedIndex;
                S�ance obj = (S�ance)lvS�ances.SelectedItem;

                Singleton.getInstance().BD_Supprim�S�ance(obj.ID);

                lvS�ances.ItemsSource = Singleton.getInstance().Getliste_S�ances();

                ContentDialog contentDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Succ�s!",
                    Content = $"La s�ance a �t� supprim�.",
                    CloseButtonText = "Fermer"
                };

                // Afficher le ContentDialog et attendre sa fermeture
                await contentDialog.ShowAsync();
            }
        }

        private async void btn_modifierSE_Click(object sender, RoutedEventArgs e)
        {
            S�ance obj = (S�ance)lvS�ances.SelectedItem;
            ContentDialog contentDialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "Modification de s�ance",
                Content = new StackPanel
                {
                    Children =
                           {
                            new TextBox
                            {
                                PlaceholderText = "Entrez l'id d'activit�",
                                Name = "idActivit�",
                                AcceptsReturn = true,
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(10),
                                Width = 300,
                                Text = obj.IdActivit�+""
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
                PrimaryButtonText = "Modifi�"
            };

            var result = await contentDialog.ShowAsync();

            if (result + "" == "Primary")
            {
                try
                {
                    var stackPanel = (StackPanel)contentDialog.Content;
                    int idActivit� = int.Parse(((TextBox)stackPanel.Children[0]).Text);
                    DateTime dt = ((DatePicker)stackPanel.Children[1]).Date.DateTime + ((TimePicker)stackPanel.Children[2]).Time;
                    int nombrePlaces = int.Parse(((TextBox)stackPanel.Children[3]).Text);

                    if (idActivit� > 0 && dt != null && nombrePlaces > 0)
                    {
                        Singleton.getInstance().BD_Modifi�S�ance(obj.ID, dt, nombrePlaces);

                        ContentDialog contentDialog2 = new ContentDialog
                        {
                            XamlRoot = this.XamlRoot,
                            Title = "Succ�s!",
                            Content = $"La s�ance a �t� modifi� avec succ�s",
                            CloseButtonText = "Fermer"
                        };

                        // Afficher le ContentDialog et attendre sa fermeture
                        await contentDialog2.ShowAsync();

                        lvS�ances.ItemsSource = Singleton.getInstance().Getliste_S�ances();
                    }
                }
                catch (Exception ex)
                {
                    ContentDialog contentDialog3 = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "ERREUR!",
                        Content = $"Des champs �taient vides ou un mauvais format a �t� entr�",
                        CloseButtonText = "Fermer"
                    };

                    // Afficher le ContentDialog et attendre sa fermeture
                    await contentDialog3.ShowAsync();
                }


            }
        }
    }
    }

