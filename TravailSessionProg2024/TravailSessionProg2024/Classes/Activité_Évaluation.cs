using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravailSessionProg2024.Classes
{
    internal class Activité_Évaluation
    {
        public int IdActivité { get; set; }
        public string NomActivité { get; set; }
        public string TypeActivité { get; set; }
        public double Note {  get; set; }
        public string commentaire { get; set; }

        public Activité_Évaluation(int idActivité, string nomActivité, string typeActivité, double note, string commentaire)
        {
            IdActivité = idActivité;
            NomActivité = nomActivité;
            TypeActivité = typeActivité;
            Note = note;
            this.commentaire = commentaire;
        }
    }
}
