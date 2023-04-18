using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatriceHypothèque.Models
{
    public class Frequence
    {
        private static int NbFrequence = 0;
        public int Id { get; set; }
        public int NbPaimentAnnuel { get; set; }
        public string Description { get; set; }

        public Frequence() { }

        public Frequence(int nbPaimentAnnuel, string description)
        {
            NbFrequence++;
            Id = NbFrequence;
            NbPaimentAnnuel = nbPaimentAnnuel;
            Description = description;
        }
    }
}
