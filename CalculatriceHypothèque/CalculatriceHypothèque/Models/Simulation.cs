using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatriceHypothèque.Models
{
    public class Simulation
    {
        public static int NbSimulations = 0;
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Description { get; set; }
        public double? Capital { get; set; }
        public double? TauxAnnuel { get; set; }
        public int? Periode { get; set; }
        public Frequence Frequence { get; set; }
        private double? Mensualite { get; set; }

        public List<Resultat> Resultats = new List<Resultat>();

        public Simulation() { }

        public Simulation(string? nom, string? prenom)
        {
            NbSimulations++;
            Nom = nom;
            Prenom = prenom;
            Description= NbSimulations.ToString();
        }

        public Simulation(string? nom, string? prenom, string? description, double? capital, double? tauxAnnuel, int? période, Frequence frequence)
        {
            Nom = nom;
            Prenom = prenom;
            Description = description;
            Capital = capital;
            TauxAnnuel = tauxAnnuel;
            Periode = période;
            Frequence = frequence;
        }

        public void GenererResultat()
        {
            SetMensualite();
            Resultats.Clear();
            Resultat.TrackBalance = Capital;
            Resultat.NbMois = 0;
            for (int i = 1; i <= Periode; i++)
            {
                Resultats.Add(new Resultat(Mensualite, GetCapitalMensuelle(), GetMontantInteretMensuelle()));
            }
        }

        private double? GetCapitalMensuelle()
        {
            return Mensualite - GetMontantInteretMensuelle();
        }

        private double? GetMontantInteretMensuelle()
        {
            return (GetTauxPeriode() * Resultat.TrackBalance);
        }

        private void SetMensualite()
        {
            Mensualite = Capital * GetTauxPeriode() / (1 - Math.Pow(1 + (double)GetTauxPeriode(), -1 * (double)GetNbPaiementTotal()));
        }

        private double? GetTauxPeriode()
        {
            return TauxAnnuel / Frequence.NbPaimentAnnuel / 100;
        }

        private double? GetNbPaiementTotal()
        {
            return Periode / 12 * Frequence.NbPaimentAnnuel;
        }
    }
}
