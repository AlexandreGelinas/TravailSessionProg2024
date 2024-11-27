using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravailSessionProg2024.Classes
{
    internal class Séance
    {
        public DateTime DateHeure { get; set; }
        public int NombrePlaces { get; set; }

        public Séance(DateTime dateHeure, int nombrePlaces )
        {
            DateHeure = dateHeure;
            NombrePlaces = nombrePlaces;
        }

        public override string ToString() { 
            return $"Date: {DateHeure}, Nombre de places: {NombrePlaces}";
        }
    }
}
