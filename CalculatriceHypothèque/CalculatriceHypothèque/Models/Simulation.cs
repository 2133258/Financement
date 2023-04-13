using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatriceHypothèque.Models
{
    public class Simulation
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Description { get; set; }
        public double Capital { get; set; }
        public double TauxAnnuel { get; set; }
        public int Période { get; set; }
        public int Frequence { get; set; }

        public List<Resultat> Resultats = new List<Resultat>();

        public Simulation(string nom, string prenom, string description, double capital, double tauxAnnuel, int période, int frequence)
        {
            Nom = nom;
            Prenom = prenom;
            Description = description;
            Capital = capital;
            TauxAnnuel = tauxAnnuel;
            Période = période;
            Frequence = frequence;
        }
    }
}
