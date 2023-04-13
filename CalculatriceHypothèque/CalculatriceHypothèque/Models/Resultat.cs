using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatriceHypothèque.Models
{
    public class Resultat
    {
        public int Mois { get; set; }
        public double Paiement { get; set; }
        public double Capital { get; set; }
        public double Interet { get; set; }
        public double Balance { get; set; }

        public Resultat(int mois, double paiement, double capital, double interet, double balance)
        {
            Mois = mois;
            Paiement = paiement;
            Capital = capital;
            Interet = interet;
            Balance = balance;
        }
    }
}
