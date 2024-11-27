using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravailSessionProg2024.Classes
{
    internal class Adhérent
    {
        public string Prenom {  get; set; }
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public DateOnly DateNaissance { get; set; }
        public int Age { get; set; }

        public Adhérent(string prenom, string nom, string adresse, DateOnly dateNaissance, int age) {
            
            Prenom = prenom;
            Nom = nom;
            Adresse = adresse;
            DateNaissance = dateNaissance;
            Age = age;

        }

        public override string ToString() {

            return $"Prenom: {Prenom}, Nom: {Nom}, Age: {Age}, Adresse: {Adresse}, Date de naissance:" +
                $" {DateNaissance}";

        }
    }

}
