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

    }
}
