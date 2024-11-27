using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravailSessionProg2024.Classes
{
    internal class Administrateur
    {
        public int ID { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public string MotDePasse { get; set; }
       


        public Administrateur(int id, string prenom, string nom, string motDePasse)
        {
            
            ID = id;
            Prenom = prenom;
            Nom = nom;  
            MotDePasse = motDePasse;
            
        }

        public override string ToString()
        {
            return $"Nom: {Nom}, Prenom: {Prenom}, ID d'admin: {ID}";
        }
    }
}
