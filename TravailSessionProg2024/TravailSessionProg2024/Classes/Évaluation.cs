using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravailSessionProg2024.Classes
{
    internal class Évaluation
    {
        public int ID { get; set; }
        public int IdAdherent { get; set; }
        public int IdActivite { get; set; }
        public decimal Note { get; set; } 
        public string Commentaire { get; set; } 

        
        public Évaluation(int id, int idAdherent, int idActivite, decimal note, string commentaire)
        {
            ID = id;
            IdAdherent = idAdherent;
            IdActivite = idActivite;
            Note = note;
            Commentaire = commentaire;
        }

        
        public Évaluation(int id, int idAdherent, int idActivite, decimal note)
        {
            ID = id;
            IdAdherent = idAdherent;
            IdActivite = idActivite;
            Note = note;
            Commentaire = string.Empty; 
        }
    }
}
