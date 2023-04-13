using CalculatriceHypothèque.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CalculatriceHypothèque.View
{
    /// <summary>
    /// Logique d'interaction pour GestionSimulation.xaml
    /// </summary>
    public partial class GestionSimulation : Window
    {
        public GestionSimulation()
        {
            InitializeComponent();
            Simulation simul1 = new Simulation("", "", "", 10, 10, 300, 26);
            for (int i = 0; i < length; i++)
            {

            }
            Resultat result1 = new Resultat(1,10, 10, 10, 10);
            simul1.Resultats.Add(result);
        }
    }
}
