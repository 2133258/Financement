using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using CalculatriceHypothèque.Models;

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
            }
        }

        public string? Prenom
        {
            get { return _prenomEnregistrer; }
            set
            {
                ValeurChangee("Prenom");
                _prenomEnregistrer = value; 
            }
        }

        public string? Description
        {
            get { return _descriptionEnregistrer; }
            set
            {
                ValeurChangee("Description");
                _descriptionEnregistrer = value; 
            }
        }

        public double? MontantCapital
        {
            get { return _totalCapitalEnregistrer; }
            set
            {
                ValeurChangee("MontantCapital");
                _totalCapitalEnregistrer = value; 
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
            }
        }

        public Frequence Frequence
        {
            get { return _frequenceEnregistrer; }
            set
            {
                _frequenceEnregistrer = value; 
                ValeurChangee("Frequence");
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

        private bool EnregistrerSimulation_CanExecute(object param)
        {
            if (_nomEnregistrer is null || _prenomEnregistrer is null || _descriptionEnregistrer is null || _totalCapitalEnregistrer is null || _tauxAnnuelEnregistrer is null || _periodeEnregistrer is null || _frequenceEnregistrer is null)
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
            _simulationSelectionner.GenererResultat();
            SetResultat(_simulationSelectionner);
        }

        private bool CalculerSimulation_CanExecute(object param)
        {
            if (_nomEnregistrer is null || _prenomEnregistrer is null || _descriptionEnregistrer is null || _totalCapitalEnregistrer is null || _tauxAnnuelEnregistrer is null || _periodeEnregistrer is null || _frequenceEnregistrer is null)
            {
                return false;
            }
            return true;
        }

        #endregion

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
            _autoUpdateService = new AutoUpdateService();
            _currentVersion = "(v" + _autoUpdateService.GetCurrentVersion() + ")";
            _currentVersionTitle = "Calculatrice hypothecaire " + _currentVersion;
            _listeSimulation = new List<Simulation>();
            _listeFrequence = new List<Frequence>();
            _resultats = new List<Resultat>();
            SetFrequence();
            
            _listeSimulation.Add(new Simulation("Savard", "William", "B", 230000, 6.5, 300, _listeFrequence[0]));
            _listeSimulation.Add(new Simulation("Lavoie", "Xavier", "A", 230000, 6.5, 300, _listeFrequence[0]));

            this._AjouterSimulation = new CommandeRelais(AjouterSimulation_Execute, AjouterSimulation_CanExecute);
            this._SupprimerSimulation = new CommandeRelais(SupprimerSimulation_Execute, SupprimerSimulation_CanExecute);
            this._EnregistrerSimulation = new CommandeRelais(EnregistrerSimulation_Execute, EnregistrerSimulation_CanExecute);
            this._CalculerSimulation = new CommandeRelais(CalculerSimulation_Execute, CalculerSimulation_CanExecute); 
            this._CheckUpdateCommand = new CommandeRelais(CheckUpdateCommand_Execute, CheckUpdateCommand_CanExecute);
        }

        #region FonctionAide

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

        #region Auto-Update Implementation

        private AutoUpdateService _autoUpdateService;
        private string _updateMessage;
        private string _currentVersion;
        private string _currentVersionTitle;
        private bool _isUpdateAvailable;

        public string CurrentVersion
        {
            get { return _currentVersion; }
            set { _currentVersion = value; ValeurChangee(nameof(CurrentVersion)); }
        }

        public string CurrentVersionTitle
        {
            get { return _currentVersionTitle; }
            set { _currentVersionTitle = value; ValeurChangee(nameof(CurrentVersionTitle)); }
        }

        public string UpdateMessage
        {
            get { return _updateMessage; }
            set { _updateMessage = value; ValeurChangee(nameof(UpdateMessage)); }
        }

        public bool IsUpdateAvailable
        {
            get { return _isUpdateAvailable; }
            set { _isUpdateAvailable = value; ValeurChangee(nameof(IsUpdateAvailable)); }
        }

        public async Task CheckForUpdates()
        {
            try
            {
                var updateInfo = await _autoUpdateService.CheckForUpdatesAsync();
                if (updateInfo != null)
                {
                    IsUpdateAvailable = true;
                    UpdateMessage = $"Mise a jour disponible : {updateInfo.TagName}";
                    // Optionally prompt the user to download the update
                    _autoUpdateService.DownloadAndUpdate(updateInfo);
                }
                else
                {
                    UpdateMessage = "Votre application est mise a jour " + _currentVersion;
                }
            }
            catch (Exception ex)
            {
                UpdateMessage = "Erreur lors de la verification fde mise a jour.";
                // Log the exception as needed
            }
        }


        private ICommand _CheckUpdateCommand;
        public ICommand CheckUpdateCommand
        {
            get { return _CheckUpdateCommand; }
            private set { _CheckUpdateCommand = value; }
        }

        private void CheckUpdateCommand_Execute(object parSelect)
        {
            CheckForUpdates();
        }

        private bool CheckUpdateCommand_CanExecute(object param)
        {
            return true;
        }

        #endregion
    }
}
