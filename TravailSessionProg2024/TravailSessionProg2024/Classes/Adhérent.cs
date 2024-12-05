using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravailSessionProg2024.Classes
{
    internal class Adhérent
    {

        public int ID { get; set; }
        public string CodeAdherent { get; set; }
        public string MotDePasse { get; set; }
        public string Prenom {  get; set; }
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public DateOnly DateNaissance { get; set; }
        public int Age { get; set; }

        public Adhérent(int id, string codeAdherent, string motDePasse, string prenom, string nom, string adresse, DateOnly dateNaissance, int age)
        {
            ID = id;
            CodeAdherent = codeAdherent;
            MotDePasse = motDePasse;
            Prenom = prenom;
            Nom = nom;
            Adresse = adresse;
            DateNaissance = dateNaissance;
            Age = age;
        }


        //Pour importation CSV
        public override string ToString() {

            return $"{ID};{Prenom};{Nom};{Adresse};{DateNaissance};{Age};{CodeAdherent};{MotDePasse}";
                

        }
    }

}
