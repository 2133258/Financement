using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using System.Xml;
using CalculatriceHypothèque.Models;
using CalculatriceHypothèque.View;
using System.Drawing;

namespace CalculatriceHypothèque.ViewModels
{
    public class VMGestionSimulation : INotifyPropertyChanged
    {
        #region Déclaration

        private string? _nomEnregistrer, _prenomEnregistrer, _descriptionEnregistrer;
        private double? _totalCapitalEnregistrer, _tauxAnnuelEnregistrer, _totalCapital, _totalInteret, _montantTotal;
        private int? _periodeEnregistrer;
        private Frequence _frequenceEnregistrer;
        private List<Frequence> _listeFrequence;
        private Simulation _simulationEnregistrer, _simulationSelectionner;
        private List<Simulation> _listeSimulation;
        private List<Resultat> _resultats;
        private bool _moisRadioButtonState, _anneeRadioButtonState;


        public string? Nom
        {
            get { return _nomEnregistrer; }
            set 
            {
                ValeurChangee("Nom");
                _nomEnregistrer = value;
                if (_nomEnregistrer is null || _nomEnregistrer == "")
                    _nomBorderValidation = "Red";
                else
                    _nomBorderValidation = "Gray";
                ValeurChangee("NomBorderValidation");
            }
        }

        public string? Prenom
        {
            get { return _prenomEnregistrer; }
            set
            {
                ValeurChangee("Prenom");
                _prenomEnregistrer = value;
                if (_prenomEnregistrer is null || _prenomEnregistrer == "")
                    _prenomBorderValidation = "Red";
                else
                    _prenomBorderValidation = "Gray";
                ValeurChangee("PrenomBorderValidation");
            }
        }

        public string? Description
        {
            get { return _descriptionEnregistrer; }
            set
            {
                ValeurChangee("Description");
                _descriptionEnregistrer = value;
                if (_descriptionEnregistrer is null || _descriptionEnregistrer == "")
                    _descriptionBorderValidation = "Red";
                else
                    _descriptionBorderValidation = "Gray";
                ValeurChangee("DescriptionBorderValidation");
            }
        }

        public double? MontantCapital
        {
            get { return _totalCapitalEnregistrer; }
            set
            {
                ValeurChangee("MontantCapital");
                _totalCapitalEnregistrer = value;
                if (_totalCapitalEnregistrer is null || _totalCapitalEnregistrer <= 0)
                    _capitalBorderValidation = "Red";
                else
                    _capitalBorderValidation = "Gray";
                ValeurChangee("CapitalBorderValidation");
            }
        }

        public double? TotalCapital
        {
            get { return _totalCapital; }
            set
            {
                ValeurChangee("TotalCapital");
                _totalCapital = value;
            }
        }

        public double? TauxAnnuel
        {
            get { return _tauxAnnuelEnregistrer; }
            set 
            {
                ValeurChangee("TauxAnnuel");
                _tauxAnnuelEnregistrer = value;
                if (_tauxAnnuelEnregistrer is null)
                    _tauxAnnuelBorderValidation = "Red";
                else
                    _tauxAnnuelBorderValidation = "Gray";
                ValeurChangee("TauxAnnuelBorderValidation");
            }
        }

        public double? TotalInteret
        {
            get { return _totalInteret; }
            set
            {
                ValeurChangee("TotalInteret");
                _totalInteret = value; 
            }
        }

        public double? MontantTotal
        {
            get { return _montantTotal; }
            set
            {
                ValeurChangee("MontantTotal");
                _montantTotal = value;
            }
        }

        public int? Periode
        {
            get { return _periodeEnregistrer; }
            set
            {
                ValeurChangee("Periode");
                if (AnneeRadioButtonState)
                _periodeEnregistrer = value * 12;
                else _periodeEnregistrer = value;
                if (_periodeEnregistrer is null || _periodeEnregistrer <= 0)
                    _periodeBorderValidation = "Red";
                else
                    _periodeBorderValidation = "Gray";
                ValeurChangee("PeriodeBorderValidation");
            }
        }

        public Frequence Frequence
        {
            get { return _frequenceEnregistrer; }
            set
            {
                _frequenceEnregistrer = value;
                ValeurChangee("Frequence");
                if (_frequenceEnregistrer is null)
                    _frequenceBorderValidation = "Red";
                else
                    _frequenceBorderValidation = "Gray";
                ValeurChangee("FrequenceBorderValidation");
            }
        }

        public List<Frequence> ListeFrequence
        {
            get { return new List<Frequence>(_listeFrequence); }
            set
            {
                _listeFrequence = value;
                ValeurChangee("ListeFrequence");
            }
        }

        public Simulation Simulation
        {
            get { return _simulationEnregistrer; }
            set
            {
                _simulationEnregistrer = value; 
                ValeurChangee("Simulation");
            }
        }

        public Simulation SimulationSelectionner
        {
            get { return _simulationSelectionner; }
            set
            {
                _simulationSelectionner = value; 
                ValeurChangee("SimulationSelectionner");
                SetInformation(value);
                SetBorders();
            }
        }

        public List<Simulation> ListeSimulation
        {
            get { return new List<Simulation>(_listeSimulation); }
            set
            {
                _listeSimulation = value; 
                ValeurChangee("ListeSimulation");
            }
        }

        public List<Resultat> Resultats
        {
            get { return new List<Resultat>(_resultats); }
            set
            {
                _resultats = value; 
                ValeurChangee("Resultats");
            }
        }

        public bool MoisRadioButtonState
        {
            get { return _moisRadioButtonState; }
            set
            {
                ValeurChangee("MoisRadioButtonState");
                _moisRadioButtonState = value;
            }
        }

        public bool AnneeRadioButtonState
        {
            get { return _anneeRadioButtonState; }
            set
            {
                ValeurChangee("AnneeRadioButtonState");
                _anneeRadioButtonState = value;
            }
        }

        private double _contentWidth;
        public double ContentWidth
        {
            get { return _contentWidth; }
            set
            {
                _contentWidth = value;
                ValeurChangee("ContentWidth");
            }
        }

        #region CouleurValidation

        private string _nomBorderValidation;
        public string NomBorderValidation
        {
            get { return _nomBorderValidation; }
            set
            {
                _nomBorderValidation = value;
                ValeurChangee("NomBorderValidation");
            }
        }

        private string _prenomBorderValidation;
        public string PrenomBorderValidation
        {
            get { return _prenomBorderValidation; }
            set
            {
                _prenomBorderValidation = value;
                ValeurChangee("PrenomBorderValidation");
            }
        }

        private string _descriptionBorderValidation;
        public string DescriptionBorderValidation
        {
            get { return _descriptionBorderValidation; }
            set
            {
                _descriptionBorderValidation = value;
                ValeurChangee("DescriptionBorderValidation");
            }
        }

        private string _capitalBorderValidation;
        public string CapitalBorderValidation
        {
            get { return _capitalBorderValidation; }
            set
            {
                _capitalBorderValidation = value;
                ValeurChangee("CapitalBorderValidation");
            }
        }

        private string _tauxAnnuelBorderValidation;
        public string TauxAnnuelBorderValidation
        {
            get { return _tauxAnnuelBorderValidation; }
            set
            {
                _tauxAnnuelBorderValidation = value;
                ValeurChangee("TauxAnnuelBorderValidation");
            }
        }

        private string _periodeBorderValidation;
        public string PeriodeBorderValidation
        {
            get { return _periodeBorderValidation; }
            set
            {
                _periodeBorderValidation = value;
                ValeurChangee("PeriodeBorderValidation");
            }
        }

        private string _frequenceBorderValidation;
        public string FrequenceBorderValidation
        {
            get { return _frequenceBorderValidation; }
            set
            {
                _frequenceBorderValidation = value;
                ValeurChangee("FrequenceBorderValidation");
            }
        }

        #endregion

        #endregion

        #region Commande

        private ICommand _AjouterSimulation;

        public ICommand AjouterSimulation
        {
            get { return _AjouterSimulation; }
            private set { _AjouterSimulation = value; }
        }

        private void AjouterSimulation_Execute(object parSelect)
        {
            //Créer une simulation vide
            Simulation simulation = new Simulation("Nouvelle", "Simulation");
            //Ajouter la simulation dans la liste des simulations
            _listeSimulation.Add(simulation);
            ValeurChangee("ListeSimulation");
            //Définir la simulation selectionner, égale à la nouvelle simulation
            _simulationSelectionner = simulation;
            ValeurChangee("SimulationSelectionner");
            //Set les infos de la simulation
            SetInformation(simulation);
        }

        private bool AjouterSimulation_CanExecute(object param)
        {
            return true;
        }

        private ICommand _SupprimerSimulation;

        public ICommand SupprimerSimulation
        {
            get { return _SupprimerSimulation; }
            private set { _SupprimerSimulation = value; }
        }

        private void SupprimerSimulation_Execute(object parSelect)
        {
            //Prend l'index de la simulation selectionner
            int index = _listeSimulation.IndexOf(_simulationSelectionner);
            //Enlève la simulation selectionner de la liste
            _listeSimulation.Remove(_simulationSelectionner);
            ValeurChangee("ListeSimulation");
            //Si la liste est plus grand que 0
            if (_listeSimulation.Count > 0)
            {
                //Si l'index est plus grand que 0
                if (index > 0) //simulation selectionner est égale a celle au dessus de l'ancienne simulation selectionner
                    _simulationSelectionner = _listeSimulation[index - 1];
                //Sinon 
                else//La simulation selectionner est égale a celle en dessous de l'ancienne simulation selectionner
                    _simulationSelectionner = _listeSimulation[index];
                ValeurChangee("SimulationSelectionner");
                //Set les infos de la simulation
                SetInformation(_simulationSelectionner);
            }
        }

        private bool SupprimerSimulation_CanExecute(object param)
        {
            if (_simulationSelectionner is null)
            {
                return false;
            }
            return true;
        }

        private ICommand _EnregistrerSimulation;

        public ICommand EnregistrerSimulation
        {
            get { return _EnregistrerSimulation; }
            private set { _EnregistrerSimulation = value; }
        }

        private void EnregistrerSimulation_Execute(object parSelect)
        {
            if (_nomEnregistrer is not null && _nomEnregistrer != ""
                && _prenomEnregistrer is not null && _prenomEnregistrer != ""
                && _descriptionEnregistrer is not null && _descriptionEnregistrer != ""
                && _totalCapitalEnregistrer is not null && _totalCapitalEnregistrer > 0
                && _tauxAnnuelEnregistrer is not null
                && _periodeEnregistrer is not null && _periodeEnregistrer > 0
                && _frequenceEnregistrer is not null)
            {
                SetBorders();
                _simulationSelectionner.Nom = _nomEnregistrer;
                _simulationSelectionner.Prenom = _prenomEnregistrer;
                _simulationSelectionner.Description = _descriptionEnregistrer;
                _simulationSelectionner.Capital = _totalCapitalEnregistrer;
                _simulationSelectionner.Frequence = _frequenceEnregistrer;
                _simulationSelectionner.Periode = _periodeEnregistrer;
                _simulationSelectionner.TauxAnnuel = _tauxAnnuelEnregistrer;

                ValeurChangee("SimulationSelectionner");
                ValeurChangee("ListeSimulation");
            }
            else
            {
                if (_nomEnregistrer is null || _nomEnregistrer == "")
                    _nomBorderValidation = "Red";
                if (_prenomEnregistrer is null || _prenomEnregistrer == "")
                    _prenomBorderValidation = "Red";
                if (_descriptionEnregistrer is null || _descriptionEnregistrer == "")
                    _descriptionBorderValidation = "Red";
                if (_totalCapitalEnregistrer is null || _totalCapitalEnregistrer <= 0)
                    _capitalBorderValidation = "Red";
                if (_tauxAnnuelEnregistrer is null)
                    _tauxAnnuelBorderValidation = "Red";
                if (_periodeEnregistrer is null || _periodeEnregistrer <= 0)
                    _periodeBorderValidation = "Red";
                if (_frequenceEnregistrer is null)
                    _frequenceBorderValidation = "Red";
                ValeurChangee("NomBorderValidation");
                ValeurChangee("PrenomBorderValidation");
                ValeurChangee("DescriptionBorderValidation");
                ValeurChangee("CapitalBorderValidation");
                ValeurChangee("TauxAnnuelBorderValidation");
                ValeurChangee("PeriodeBorderValidation");
                ValeurChangee("FrequenceBorderValidation");
            }
        }

        private bool EnregistrerSimulation_CanExecute(object param)
        {
            if (_simulationSelectionner is null)
            {
                return false;
            }
            return true;
        }

        private ICommand _CalculerSimulation;

        public ICommand CalculerSimulation
        {
            get { return _CalculerSimulation; }
            private set { _CalculerSimulation = value; }
        }

        private void CalculerSimulation_Execute(object parSelect)
        {
            if (_nomEnregistrer is not null && _nomEnregistrer != ""
                && _prenomEnregistrer is not null && _prenomEnregistrer != ""
                && _descriptionEnregistrer is not null && _descriptionEnregistrer != ""
                && _totalCapitalEnregistrer is not null && _totalCapitalEnregistrer > 0
                && _tauxAnnuelEnregistrer is not null
                && _periodeEnregistrer is not null && _periodeEnregistrer > 0
                && _frequenceEnregistrer is not null)
            {
                SetBorders();
                EnregistrerSimulation_Execute(parSelect);
                _simulationSelectionner.GenererResultat();
                SetResultat(_simulationSelectionner);
                ListeResultat view = new ListeResultat();
                view.DataContext = this;
                view.ShowDialog();
                return;
            }
            else
            {
                if (_nomEnregistrer is null || _nomEnregistrer == "")
                    _nomBorderValidation = "Red";
                if (_prenomEnregistrer is null || _prenomEnregistrer == "")
                    _prenomBorderValidation = "Red";
                if (_descriptionEnregistrer is null || _descriptionEnregistrer == "")
                    _descriptionBorderValidation = "Red";
                if (_totalCapitalEnregistrer is null || _totalCapitalEnregistrer <= 0)
                    _capitalBorderValidation = "Red";
                if (_tauxAnnuelEnregistrer is null)
                    _tauxAnnuelBorderValidation = "Red";
                if (_periodeEnregistrer is null || _periodeEnregistrer <= 0)
                    _periodeBorderValidation = "Red";
                if (_frequenceEnregistrer is null)
                    _frequenceBorderValidation = "Red";
                ValeurChangee("NomBorderValidation");
                ValeurChangee("PrenomBorderValidation");
                ValeurChangee("DescriptionBorderValidation");
                ValeurChangee("CapitalBorderValidation");
                ValeurChangee("TauxAnnuelBorderValidation");
                ValeurChangee("PeriodeBorderValidation");
                ValeurChangee("FrequenceBorderValidation");
            }
        }

        private bool CalculerSimulation_CanExecute(object param)
        {
            if (_simulationSelectionner is null)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Serialize

        private void SerialiserSimulations()
        {
            System.Xml.Serialization.XmlSerializer mySerialiser = new System.Xml.Serialization.XmlSerializer(_listeSimulation.GetType());
            XmlWriter writer = XmlWriter.Create("Simulations.xml");
            mySerialiser.Serialize(writer, _listeSimulation);
            writer.Close();
        }

        private void DeserialiserSimulations()
        {
            XmlSerializer ser = null;
            XmlReader reader = null;
            try
            {
                ser = new XmlSerializer(typeof(List<Simulation>));
                reader = XmlReader.Create("Simulations.xml");
                _listeSimulation = (List<Simulation>)ser.Deserialize(reader);
                ValeurChangee("listeSimulation");
            }
            catch (FileNotFoundException)
            {
                _listeSimulation = new List<Simulation>();
            }
            catch (System.InvalidOperationException e)
            {
                throw (e);
                //Ouvrire la page d'erreur
                //écrire : Fichier Corompu : e.message
            }
            finally
            {
                reader?.Close();
            }
        }

        #endregion Serialize

        #region Notification
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void ValeurChangee(string nomPropriete)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(nomPropriete));
            }
        }
        #endregion Notification;

        public VMGestionSimulation()
        {
            _listeSimulation = new List<Simulation>();
            _listeFrequence = new List<Frequence>();
            _resultats = new List<Resultat>();
            DeserialiserSimulations();
            SetFrequence();
            SetBorders();

            this._AjouterSimulation = new CommandeRelais(AjouterSimulation_Execute, AjouterSimulation_CanExecute);
            this._SupprimerSimulation = new CommandeRelais(SupprimerSimulation_Execute, SupprimerSimulation_CanExecute);
            this._EnregistrerSimulation = new CommandeRelais(EnregistrerSimulation_Execute, EnregistrerSimulation_CanExecute);
            this._CalculerSimulation = new CommandeRelais(CalculerSimulation_Execute, CalculerSimulation_CanExecute);
        }

        #region FonctionAide

        private void SetBorders()
        {
            _nomBorderValidation = "Gray";
            _prenomBorderValidation = "Gray";
            _descriptionBorderValidation = "Gray";
            _capitalBorderValidation = "Gray";
            _tauxAnnuelBorderValidation = "Gray";
            _periodeBorderValidation = "Gray";
            _frequenceBorderValidation = "Gray";
            ValeurChangee("NomBorderValidation");
            ValeurChangee("PrenomBorderValidation");
            ValeurChangee("DescriptionBorderValidation");
            ValeurChangee("CapitalBorderValidation");
            ValeurChangee("TauxAnnuelBorderValidation");
            ValeurChangee("PeriodeBorderValidation");
            ValeurChangee("FrequenceBorderValidation");
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            SerialiserSimulations();
        }

        private void SetFrequence()
        {
            _listeFrequence.Add(new Frequence(12,"Mensuelle"));
            _listeFrequence.Add(new Frequence(26,"Bihebdomadaire"));
            _listeFrequence.Add(new Frequence(52,"Hebdomadaire"));
            ValeurChangee("ListeFrequence");
        }

        private void SetInformation(Simulation value)
        {
            if (value is null)
            {
                _nomEnregistrer = null;
                _prenomEnregistrer = null;
                _descriptionEnregistrer = null;
                _totalCapitalEnregistrer = null;
                _tauxAnnuelEnregistrer = null;
                _periodeEnregistrer = null;
                _frequenceEnregistrer = null;
            }
            else
            {
                _nomEnregistrer = value.Nom;
                _prenomEnregistrer = value.Prenom;
                _descriptionEnregistrer = value.Description;
                _totalCapitalEnregistrer = value.Capital;
                _tauxAnnuelEnregistrer = value.TauxAnnuel;
                _periodeEnregistrer = value.Periode;
                _frequenceEnregistrer = value.Frequence;

            }
            ValeurChangee("Nom");
            ValeurChangee("Prenom");
            ValeurChangee("Description");
            ValeurChangee("MontantCapital");
            ValeurChangee("TauxAnnuel");
            ValeurChangee("Periode");
            ValeurChangee("Frequence");
        }
        
        private void SetResultat(Simulation value)
        {
            if (value is null)
            {
                _resultats = null;
                _totalCapital = null;
                _totalInteret = null;
                _montantTotal = null;
            }
            else
            {
                _totalInteret = 0;  
                _resultats = value.Resultats;
                _totalCapital = value.Capital;
                for (int i = 0; i < value.Resultats.Count; i++)
                {
                    _totalInteret += value.Resultats[i].Interet;
                }
                _montantTotal = _totalInteret + _totalCapital;
            }
            ValeurChangee("Resultats");
            ValeurChangee("TotalCapital");
            ValeurChangee("TotalInteret");
            ValeurChangee("MontantTotal");
        }

        

        #endregion FonctionAide
    }
}
