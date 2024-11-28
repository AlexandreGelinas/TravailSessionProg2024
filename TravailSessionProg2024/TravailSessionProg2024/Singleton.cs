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

namespace TravailSessionProg2024
{
    internal class Singleton
    {
        ObservableCollection<Adhérent> liste;
        static Singleton instance = null;
        MySqlConnection con;
        int niveau_permission;
        Adhérent adhérent_connecter;
        Administrateur administrateur_connecter;

        //constructeur de la classe
        public Singleton()
        {
            liste = new ObservableCollection<Adhérent>();
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2024_420-345-ri_eq9;Uid=2375213;Pwd=2375213;");
        }

        public async void Ouverture()
        {
            liste.Clear();
            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = "select * from Adhérent";
            con.Open();
            MySqlDataReader r = commande.ExecuteReader();

            while (r.Read())
            {
                string nom = r["nom"].ToString();
                string prix = r["prix"].ToString();
                string categorie = r["categorie"].ToString();

                //ajouter(new Adhérent(nom, prix, categorie));
            }

            r.Close();
            con.Close();
        }

        public void Connexion() //FONCTION DE TEST
        {
            GestionWindow.mainWindow.admin_affichage();
        }

        public void Connexion(Administrateur user)
        {
            GestionWindow.mainWindow.admin_affichage();
            administrateur_connecter = user;
        }

        public void Connexion(Adhérent user)
        {
            adhérent_connecter = user;
        }

        //retourne l’instance du singleton
        public static Singleton getInstance()
        {
            if (instance == null)
                instance = new Singleton();

            return instance;
        }

        //retourne la liste des clients
        public ObservableCollection<Adhérent> GetListeAdhérents()
        {
            Ouverture();
            return liste;
        }


        //retourne un client à une position précise
        public Adhérent getAdhérent(int position)
        {
            return liste[position];
        }


        //ajoute un client dans la liste
        public void ajouter(Adhérent Adhérent)
        {
            liste.Add(Adhérent);
        }


        //modifie un client à une position précise
        public void modifier(int position, Adhérent Adhérent)
        {
            liste[position] = Adhérent;
        }

        //supprime un client à une position précise
        //public void supprimer(int position)
        //{
        //    BD_Supprimer(Singleton.getInstance().getAdhérent(position).Nom);
        //    liste.RemoveAt(position);
        //}


        //public bool verification(string nom)
        //{
        //    Ouverture();
        //    for (int ctr = 0; ctr < liste.Count; ctr++)
        //    {
        //        if (nom.Equals(getAdhérent(ctr).Nom))
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //public void BD_Ajouter(Adhérent item)
        //{
        //    try
        //    {
        //        MySqlCommand commande = new MySqlCommand();
        //        commande.Connection = con;
        //        commande.CommandText = $"insert into Adhérent values(\"{item.Nom}\",\"{item.Prix}\",\"{item.Categorie}\")";

        //        con.Open();
        //        commande.ExecuteNonQuery();

        //        con.Close();
        //        ajouter(item);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.ToString());
        //        if (con.State == System.Data.ConnectionState.Open)
        //        {
        //            con.Close();
        //        }
        //    }
        //}

        //public void BD_Supprimer(string nom)
        //{
        //    nom = nom.Replace("'", " ");
        //    try
        //    {
        //        MySqlCommand commande = new MySqlCommand();
        //        commande.Connection = con;
        //        commande.CommandText = $"Delete from Adhérent WHERE REPLACE(nom, \"'\"  ,  \" \") = '{nom}'";

        //        con.Open();
        //        commande.ExecuteNonQuery();

        //        con.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State == System.Data.ConnectionState.Open)
        //        {
        //            con.Close();
        //        }
        //    }
        //}

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












    }
}
