﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravailSessionProg2024.Classes
{
    internal class Activité
    {
        public string Nom { get; set; }
        public string Type { get; set; }
        public double CoutOrganisation { get; set; }
        public double PrixVenteParClient { get; set; }


        public Activité(string nom, string type, double coutOrganisation, double prixVenteParClient)
        {
            Nom = nom;
            Type = type;
            CoutOrganisation = coutOrganisation;
            PrixVenteParClient = prixVenteParClient;
        }

        // Retourne une ligne de string pour CSV
        public override string ToString()
        {
            return $"{Nom};{Type};{CoutOrganisation};{PrixVenteParClient}";
        }

        

        public string PrixAffiche // Pour afficher un $ à coté de chaque prix dans la ListeView
        {
            get { return $"{PrixVenteParClient:F2}$"; } 
        }
    }
}
