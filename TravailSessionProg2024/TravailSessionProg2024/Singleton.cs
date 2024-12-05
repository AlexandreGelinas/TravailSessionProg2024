using TravailSessionProg2024;
using Microsoft.UI.Xaml.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Protection.PlayReady;
using TravailSessionProg2024.Classes;
using WinRT;
using Windows.System;
using System.Data;

namespace TravailSessionProg2024
{
    internal class Singleton
    {
        ObservableCollection<Adhérent> liste_user;
        ObservableCollection<Administrateur> liste_admin;
        ObservableCollection<Activité> liste_activites;
        ObservableCollection<Séance> liste_seances;
        static Singleton instance = null;
        MySqlConnection con;
        int niveau_permission;
        Adhérent adhérent_connecter;
        Administrateur administrateur_connecter;

        //constructeur de la classe
        public Singleton()
        {
            liste_user = new ObservableCollection<Adhérent>();
            liste_admin = new ObservableCollection<Administrateur>();
            liste_activites = new ObservableCollection<Activité>();
            liste_seances = new ObservableCollection<Séance>();
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2024_420-345-ri_eq9;Uid=2375213;Pwd=2375213;");
        }

        public async void Ouverture()
        {
            liste_user.Clear();
            liste_admin.Clear();
            liste_activites.Clear();
            liste_seances.Clear();
            Ouverture_User();
            Ouverture_Admin();
            Ouverture_Activité();
            Ouverture_Séance();
        }

        public async void Ouverture_User()
        {
            try
            {
             MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = "select * from adherents";
            con.Open();
            MySqlDataReader r = commande.ExecuteReader();

            while (r.Read())
            {
                int id = int.Parse(r["ID"].ToString());
                string nom = r["Nom"].ToString();
                string prenom = r["Prenom"].ToString();
                string adresse = r["Adresse"].ToString();
                DateOnly dateNaissance = DateOnly.Parse(r["DateNaissance"].ToString().Substring(0,10));
                int age = int.Parse(r["Age"].ToString());
                string mdp = r["MotDePasse"].ToString();
                string codeAdherent = r["CodeAdherent"].ToString();

                ajouter(new Adhérent(id, codeAdherent, mdp, prenom, nom, adresse, dateNaissance, age));
            }

            r.Close();
            con.Close();
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                Debug.WriteLine(ex.Message);
            }

        }
        public async void Ouverture_Admin()
        {
            try
            {
            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = "select * from Administrateurs";
            con.Open();
            MySqlDataReader r = commande.ExecuteReader();

            while (r.Read())
            {
                string nom = r["Nom"].ToString();
                string prenom = r["Prenom"].ToString();
                string mdp = r["MotDePasse"].ToString();
                int id = int.Parse(r["ID"].ToString());

                ajouter(new Administrateur(id, prenom, nom, mdp));
            }

            r.Close();
            con.Close();
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                Debug.WriteLine(ex.Message);
            }

        }
        public async void Ouverture_Activité()
        {
            try
            {
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = "select * from activites";
                con.Open();
                MySqlDataReader r = commande.ExecuteReader();

                while (r.Read())
                {
                    string nom = r["Nom"].ToString();
                    string type = r["Type"].ToString();
                    double coutOrganisation = double.Parse(r["CoutOrganisation"].ToString());
                    double prixVenteParClient = double.Parse(r["PrixVenteParClient"].ToString());
                    int id = int.Parse(r["ID"].ToString());

                    ajouter(new Activité(id,nom, type, coutOrganisation, prixVenteParClient));
                }

                r.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                Debug.WriteLine(ex.Message);
            }
        }

        public async void Ouverture_Séance()
        {
            try
            {
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = "select * from seances";
                con.Open();
                MySqlDataReader r = commande.ExecuteReader();

                while (r.Read())
                {
                    int id = int.Parse(r["ID"].ToString());
                    int idA = int.Parse(r["idActivite"].ToString());
                    DateTime dateHeure = DateTime.Parse(r["DateHeure"].ToString());
                    int nbPlace = int.Parse(r["NombrePlaces"].ToString());

                    ajouter(new Séance(id,idA, dateHeure, nbPlace));
                }

                r.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                Debug.WriteLine(ex.Message);
            }
        }

        public void Connexion() //FONCTION DE TEST
        {
            GestionWindow.mainWindow.admin_affichage();
            niveau_permission = 2;
        }

        public bool Connexion(string username, string mdp)
        {
            Administrateur admin;
            Adhérent adherent;
                for (int i = 0; i < liste_user.Count(); i++)
                {
                    adherent = liste_user[i];
                    if (string.Equals(adherent.CodeAdherent, username) && string.Equals(adherent.MotDePasse, mdp)){
                        adhérent_connecter = adherent;
                        niveau_permission = 1;
                        administrateur_connecter = null;
                        GestionWindow.mainWindow.admin_affichage();
                        return true;
                    }
                }
                for (int i = 0; i < liste_admin.Count(); i++)
                {
                    try
                    {
                        admin = liste_admin[i];
                        if (string.Equals(admin.ID, int.Parse(username)) && string.Equals(admin.MotDePasse, mdp))
                        {
                        adhérent_connecter = null;
                        niveau_permission = 2;
                        administrateur_connecter = admin;
                        GestionWindow.mainWindow.admin_affichage();
                        return true;
                        }
                    }
                    catch (System.FormatException ex) {
                        niveau_permission = 0;
                        break;
                    }
                    
                }

            if (adhérent_connecter==null && administrateur_connecter == null)
            {
                niveau_permission=0;
            }
            return false;
            
        }

        public void Déconnexion()
        {
            adhérent_connecter = null;
            administrateur_connecter = null;
            niveau_permission = 0 ;
            GestionWindow.mainWindow.admin_affichage();
        }

        //retourne l’instance du singleton
        public static Singleton getInstance()
        {
            if (instance == null)
                instance = new Singleton();

            return instance;
        }

        //GET DES LISTES
        public ObservableCollection<Adhérent> Getliste_Adhérents()
        {
            Ouverture();
            return liste_user;
        }
        public ObservableCollection<Administrateur> Getliste_Admin()
        {
            Ouverture();
            return liste_admin;
        }
        public ObservableCollection<Activité> Getliste_Activités()
        {
            Ouverture();
            return liste_activites;
        }
        public ObservableCollection<Séance> getListeSéancesSelonActivité(string nomActivité)
        {
            ObservableCollection<Séance> listS = new ObservableCollection<Séance>();
            Ouverture();

            try
            {
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = $"select * from seances where idActivite = (select ID from activites where Nom like \"{nomActivité}\")"; 
                con.Open();
                MySqlDataReader r = commande.ExecuteReader();

                while (r.Read())
                {
                    int id = int.Parse(r["ID"].ToString());
                    int idA = int.Parse(r["idActivite"].ToString());
                    DateTime dateHeure = DateTime.Parse(r["DateHeure"].ToString());
                    int nbPlace = int.Parse(r["NombrePlaces"].ToString());

                    if (nbPlace > 0)
                    {
                        listS.Add(new Séance(id, idA, dateHeure, nbPlace));
                    }
                   
                }

                r.Close();
                con.Close();


            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return listS;
        }


        //GET OBJECTS
        public Adhérent getAdhérent(int position)
        {
            return liste_user[position];
        }
        public Séance getSéance(int position)
        {
            return liste_seances[position];
        }
        public Administrateur GetAdministrateur(int position)
        {
            return liste_admin[position];
        }
        public Activité GetActivites(int position)
        {
            return liste_activites[position];
        }
        public int getNiveauPermission()
        {
            return niveau_permission;
        }
        public Administrateur getAdminConnecter()
        {
            return administrateur_connecter;
        }
        public Adhérent getAdhérentConnecter()
        {
            return adhérent_connecter;
        }


        //AJOUTER
        public void ajouter(Adhérent Adhérent)
        {
            liste_user.Add(Adhérent);
        }

        public void ajouter(Administrateur admin)
        {
            liste_admin.Add(admin);
        }
        public void ajouter(Activité activité)
        {
            liste_activites.Add(activité);
        }
        public void ajouter(Séance séance)
        {
            liste_seances.Add(séance);
        }


        //SUPPRIMER
        public void supprimerAdhérent(int position)
        {
            liste_user.RemoveAt(position);
        }
        public void supprimerActivité(int position)
        {
            liste_activites.RemoveAt(position);
        }


        //VERIFICATION DE CODE
        public bool verification(string codeAdherent)
        {
            Ouverture();
            for (int ctr = 0; ctr < liste_user.Count; ctr++)
            {
                if (codeAdherent.Equals(getAdhérent(ctr).CodeAdherent))
                {
                    return true;
                }
            }

            return false;
        }

        public void BD_Ajouter(string nom, string prenom, string adresse, DateOnly dateNaissance, string motdepasse)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand("CreerAdherent");
                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;
                commande.Parameters.AddWithValue("p_Nom", $"{nom}");
                commande.Parameters.AddWithValue("p_Prenom", $"{prenom}");
                commande.Parameters.AddWithValue("p_Adresse", $"{adresse}");
                commande.Parameters.AddWithValue("p_DateNaissance", $"{dateNaissance}");
                commande.Parameters.AddWithValue("p_MotDePasse", $"{motdepasse}");

                con.Open();
                commande.Prepare();
                int i = commande.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        public void BD_Supprimer(Adhérent user)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = $"Delete from adherents WHERE CodeAdherent = '{user.CodeAdherent}'";

                con.Open();
                commande.ExecuteNonQuery();

                con.Close();


            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        public void BD_Supprimer(Activité activité, int index)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand("SupprimerActivite");
                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;
                commande.Parameters.AddWithValue("nomActivite", $"{activité.Nom}");

                con.Open();
                commande.Prepare();
                int i = commande.ExecuteNonQuery();

                con.Close();

                liste_activites.RemoveAt(index);

            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        // Fonctions pour la page de statistiques ////////////////////////////////////

        public int GetTotalAdherents()
        {
            try
            {
                MySqlCommand commande = new MySqlCommand("SELECT COUNT(*) FROM Adherents", con);
                con.Open();
                int count = Convert.ToInt32(commande.ExecuteScalar());
                con.Close();
                return count;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                if (con.State == System.Data.ConnectionState.Open) con.Close();
                return 0;
            }
        }

        public int GetTotalActivites()
        {
            try
            {
                MySqlCommand commande = new MySqlCommand("SELECT COUNT(*) FROM Activites", con);
                con.Open();
                int count = Convert.ToInt32(commande.ExecuteScalar());
                con.Close();
                return count;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                if (con.State == System.Data.ConnectionState.Open) con.Close();
                return 0;
            }
        }

        public Dictionary<string, int> GetAdherentsParActivite()
        {
            Dictionary<string, int> adherentsParActivite = new Dictionary<string, int>();
            try
            {
                MySqlCommand commande = new MySqlCommand(@"
                SELECT a.Nom, COUNT(p.idAdherent) AS NombreAdherents
                FROM Activites a
                JOIN Seances s ON a.ID = s.idActivite
                JOIN Participations p ON s.ID = p.idSeance
                GROUP BY a.Nom", con);

                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    string nomActivite = reader["Nom"].ToString();
                    int count = Convert.ToInt32(reader["NombreAdherents"]);
                    adherentsParActivite[nomActivite] = count;
                }
                reader.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                if (con.State == System.Data.ConnectionState.Open) con.Close();
            }
            return adherentsParActivite;
        }

        public Dictionary<string, double> GetMoyenneNotesParActivite()
        {
            Dictionary<string, double> moyenneNotes = new Dictionary<string, double>();
            try
            {
                MySqlCommand commande = new MySqlCommand(@"
                SELECT a.Nom, AVG(e.Note) AS MoyenneNote
                FROM Activites a
                JOIN Evaluations e ON a.ID = e.idActivite
                GROUP BY a.Nom", con);

                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    string nomActivite = reader["Nom"].ToString();
                    double moyenne = Convert.ToDouble(reader["MoyenneNote"]);
                    moyenneNotes[nomActivite] = moyenne;
                }
                reader.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                if (con.State == System.Data.ConnectionState.Open) con.Close();
            }
            return moyenneNotes;
        }

        public Dictionary<string, decimal> GetRevenuTotalParActivite()
        {
            Dictionary<string, decimal> revenuParActivite = new Dictionary<string, decimal>();
            try
            {
                MySqlCommand commande = new MySqlCommand(@"
                SELECT a.Nom, SUM(p.NombrePlaces * a.PrixVenteParClient) AS RevenuTotal
                FROM Activites a
                JOIN Seances s ON a.ID = s.idActivite
                JOIN Participations p ON s.ID = p.idSeance
                GROUP BY a.Nom", con);

                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    string nomActivite = reader["Nom"].ToString();
                    decimal revenu = Convert.ToDecimal(reader["RevenuTotal"]);
                    revenuParActivite[nomActivite] = revenu;
                }
                reader.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                if (con.State == System.Data.ConnectionState.Open) con.Close();
            }
            return revenuParActivite;
        }

        public Dictionary<string, decimal> GetMoyennePrixParActivite()
        {
            Dictionary<string, decimal> moyennePrix = new Dictionary<string, decimal>();
            try
            {
                MySqlCommand commande = new MySqlCommand(@"
                SELECT Nom, AVG(PrixVenteParClient) AS PrixMoyen
                FROM Activites
                GROUP BY Nom", con);

                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    string nomActivite = reader["Nom"].ToString();
                    decimal prixMoyen = Convert.ToDecimal(reader["PrixMoyen"]);
                    moyennePrix[nomActivite] = prixMoyen;
                }
                reader.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                if (con.State == System.Data.ConnectionState.Open) con.Close();
            }
            return moyennePrix;
        }

        public decimal GetRevenuTotalPourToutesLesActivites()
        {
            try
            {
                MySqlCommand commande = new MySqlCommand(@"
                SELECT SUM(s.NombrePlaces * a.PrixVenteParClient) AS RevenuTotal
                FROM Seances s
                JOIN Activites a ON s.idActivite = a.ID", con);

                con.Open();
                object result = commande.ExecuteScalar();
                con.Close();

                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur dans GetRevenuTotalPourToutesLesActivites: {ex.Message}");
                if (con.State == System.Data.ConnectionState.Open) con.Close();
                return 0;
            }
        }




        public Dictionary<string, decimal> GetTotalDepenseParAdherent()
        {
            Dictionary<string, decimal> depenseParAdherent = new Dictionary<string, decimal>();
            try
            {
                MySqlCommand commande = new MySqlCommand(@"
                SELECT CONCAT(a.Nom, ' ', a.Prenom) AS NomComplet, SUM(act.PrixVenteParClient) AS TotalDepense
                FROM Participations p
                JOIN Seances s ON p.idSeance = s.ID
                JOIN Activites act ON s.idActivite = act.ID
                JOIN Adherents a ON p.idAdherent = a.ID
                GROUP BY p.idAdherent", con);

                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    string nomAdherent = reader["NomComplet"].ToString();
                    decimal totalDepense = Convert.ToDecimal(reader["TotalDepense"]);
                    depenseParAdherent[nomAdherent] = totalDepense;
                }
                reader.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                if (con.State == System.Data.ConnectionState.Open) con.Close();
            }
            return depenseParAdherent;
        }


        //FONCTION POUR LA PAGE ACTIVITÉ
        public void BD_AjouterParticipation(int idSéance)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand("AjouterParticipant");
                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;
                commande.Parameters.AddWithValue("p_idAdherent", $"{adhérent_connecter.ID}");
                commande.Parameters.AddWithValue("p_idSeance", $"{idSéance}");

                con.Open();
                commande.Prepare();
                int i = commande.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        public bool BD_VerificationSiParticipation(int idActivite)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = $"Select COUNT(*) as count from participations where idAdherent = {adhérent_connecter.ID}  AND idSeance in (select ID from seances where idActivite = {idActivite})";

                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    int count = Convert.ToInt32(reader["count"]);
                    if (count > 0)
                    {
                        return true;
                    }
                }
                reader.Close();
                con.Close();


            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                    return true;
                }
            }

            return false;
        }







    }
}
