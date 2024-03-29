﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatriceHypothèque.Models
{
    public class Resultat
    {

        public static int? NbMois { get; set; }
        public int? Mois { get; set; }
        public double? Paiement { get; set; }
        public double? Capital { get; set; }
        public double? Interet { get; set; }
        public static double? TrackBalance { get; set; }
        public double? Balance { get; set; }

        public Resultat() { }

        public Resultat(double? paiement, double? capital, double? interet)
        {
            NbMois++;
            Mois = NbMois;
            Paiement = paiement;
            Capital = capital;
            Interet = interet;
            TrackBalance = TrackBalance - capital;
            Balance = (double)Math.Abs((decimal)TrackBalance);
        }
    }
}
