using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public string Commentaire { get; set; }
        public int IdEvaluation {  get; set; }

        public Activité_Évaluation(int idActivité, string nomActivité, string typeActivité)
        {
            IdActivité = idActivité;
            NomActivité = nomActivité;
            TypeActivité = typeActivité;
        }

        public void noteEtCommentaire()
        {
           ObservableCollection<string> list = Singleton.getInstance().BD_NoteParActivitéUser(IdActivité);
            if (list.Count() > 0) { 
                IdEvaluation = int.Parse(list[0]);
                Note = double.Parse(list[1]);
                Commentaire = list[2];
            }
            else
            {
                IdEvaluation = -1;
                Note = 0.0;
                Commentaire = "";
            }

            
        }
    }
}
