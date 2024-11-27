//using Laboratoire2;
//using Microsoft.UI.Xaml.Controls;
//using MySql.Data.MySqlClient;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.Media.Protection.PlayReady;

//namespace Exercice_Multipages
//{
//    internal class Singleton
//    {
//        ObservableCollection<Produit> liste;
//        ObservableCollection<Produit> listeRecherche;
//        static Singleton instance = null;
//        MySqlConnection con;

//        //constructeur de la classe
//        public Singleton()
//        {
//            liste = new ObservableCollection<Produit>();
//            listeRecherche = new ObservableCollection<Produit>();
//            con = new MySqlConnection("Server=cours.cegep3r.info;Database=420345ri_gr00002_2375213-loucas-ferron;Uid=2375213;Pwd=2375213;");
//        }

//        public async void Ouverture()
//        {
//            liste.Clear();
//            MySqlCommand commande = new MySqlCommand();
//            commande.Connection = con;
//            commande.CommandText = "select * from Produit";
//            con.Open();
//            MySqlDataReader r = commande.ExecuteReader();

//            while (r.Read())
//            {
//                string nom = r["nom"].ToString();
//                string prix = r["prix"].ToString();
//                string categorie = r["categorie"].ToString();

//                ajouter(new Produit(nom, prix, categorie));
//            }

//            r.Close();
//            con.Close();
//        }

//        //retourne l’instance du singleton
//        public static Singleton getInstance()
//        {
//            if (instance == null)
//                instance = new Singleton();

//            return instance;
//        }

//        //retourne la liste des clients
//        public ObservableCollection<Produit> GetListeProduits()
//        {
//            Ouverture();
//            return liste;
//        }

//        public ObservableCollection<Produit> GetListeRecherche()
//        {
//            return listeRecherche;
//        }

//        //retourne un client à une position précise
//        public Produit getProduit(int position)
//        {
//            return liste[position];
//        }

//        public Produit getProduitRercherche(int position)
//        {
//            return listeRecherche[position];
//        }

//        //ajoute un client dans la liste
//        public void ajouter(Produit Produit)
//        {
//            liste.Add(Produit);
//        }

//        public void ajouterRecherche(Produit Produit)
//        {
//            listeRecherche.Add(Produit);
//        }

//        //modifie un client à une position précise
//        public void modifier(int position, Produit Produit)
//        {
//            liste[position] = Produit;
//        }

//        //supprime un client à une position précise
//        public void supprimer(int position)
//        {
//            BD_Supprimer(Singleton.getInstance().getProduit(position).Nom);
//            liste.RemoveAt(position);
//        }

//        public void supprimerRecherche(int position)
//        {
//            BD_Supprimer(Singleton.getInstance().getProduitRercherche(position).Nom);
//            listeRecherche.RemoveAt(position);
//        }

//        public bool verification(string nom)
//        {
//            Ouverture();
//            for (int ctr = 0; ctr < liste.Count; ctr++)
//            {
//                if (nom.Equals(getProduit(ctr).Nom))
//                {
//                    return true;
//                }
//            }

//            return false;
//        }

//        public void BD_Ajouter(Produit item)
//        {
//            try
//            {
//                MySqlCommand commande = new MySqlCommand();
//                commande.Connection = con;
//                commande.CommandText = $"insert into Produit values(\"{item.Nom}\",\"{item.Prix}\",\"{item.Categorie}\")";

//                con.Open();
//                commande.ExecuteNonQuery();

//                con.Close();
//                ajouter(item);
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine(ex.ToString());
//                if (con.State == System.Data.ConnectionState.Open)
//                {
//                    con.Close();
//                }
//            }
//        }

//        public void BD_Supprimer(string nom)
//        {
//            nom = nom.Replace("'", " ");
//            try
//            {
//                MySqlCommand commande = new MySqlCommand();
//                commande.Connection = con;
//                commande.CommandText = $"Delete from Produit WHERE REPLACE(nom, \"'\"  ,  \" \") = '{nom}'";

//                con.Open();
//                commande.ExecuteNonQuery();

//                con.Close();

//            }
//            catch (Exception ex)
//            {
//                if (con.State == System.Data.ConnectionState.Open)
//                {
//                    con.Close();
//                }
//            }
//        }


//        public void BD_Recherche(string rville)
//        {
//            listeRecherche.Clear();
//            MySqlCommand commande = new MySqlCommand();
//            commande.Connection = con;
//            commande.CommandText = $"select * from Produit where ville regexp \"{rville}\"";
//            con.Open();
//            MySqlDataReader r = commande.ExecuteReader();

//            while (r.Read())
//            {
//                string nom = r["nom"].ToString();
//                string ville = r["ville"].ToString();
//                string logo = r["logo"].ToString();

//                for (int i = 0; i < liste.Count(); i++)
//                {
//                    if (nom.Equals(liste[i].Nom))
//                    {
//                        ajouterRecherche(liste[i]);
//                    }
//                }


//            }

//            r.Close();
//            con.Close();
//        }

//    }
//}
