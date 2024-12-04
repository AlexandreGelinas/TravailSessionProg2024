using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravailSessionProg2024.Classes
{
    internal class Séance
    {
        public int IdActivité { get; set; }
        public DateTime DateHeure { get; set; }
        public int NombrePlaces { get; set; }

        public Séance(int idActivité, DateTime dateHeure, int nombrePlaces )
        {
            IdActivité = idActivité;
            DateHeure = dateHeure;
            NombrePlaces = nombrePlaces;
        }

        public override string ToString() { 
            return $"Id Activité: {IdActivité}, Date: {DateHeure}, Nombre de places: {NombrePlaces}";
        }
    }
}
