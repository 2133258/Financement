﻿using CalculatriceHypothèque.Models;
using CalculatriceHypothèque.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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
using CalculatriceHypothèque.ViewModels;
using System.Text.RegularExpressions;

namespace CalculatriceHypothèque.View
{
    /// <summary>
    /// Logique d'interaction pour GestionSimulation.xaml
    /// </summary>
    public partial class GestionSimulation : Window
    {
        private bool verifChangeRadioButton { get; set; }

        public GestionSimulation()
        {
            InitializeComponent();
            this.DataContext = new VMGestionSimulation();
            verifChangeRadioButton = true;

            NomTextbox.IsEnabled = false;
            PrenomTextbox.IsEnabled = false;
            DescriptionTextbox.IsEnabled = false;
            CapitalTextBox.IsEnabled = false;
            TauxAnnuelTextbox.IsEnabled = false;
            PeriodeTextBox.IsEnabled = false;
            FrequenceComboBox.IsEnabled = false;
            AnneeRadioButton.IsChecked = false;
            MoisRadioButton.IsChecked = false;
            AnneeRadioButton.IsEnabled = false;
            MoisRadioButton.IsEnabled = false;
        }

        #region Commande SortItem
        // Source : https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/how-to-sort-a-gridview-column-when-a-header-is-clicked?view=netframeworkdesktop-4.8

        //Code trouver sur stackoverflow, permet de "sort" la liste des simulations selon la colonne cliquer
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    Sort(sortBy, direction, sender);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void Sort(string sortBy, ListSortDirection direction, object sender)
        {
             ListView listview = sender as ListView;
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(listview.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        #endregion Commande SortItem

        #region ListView UpdateColumnsWidth
        //Source : https://dlaa.me/blog/post/9425496

        // Handler for the ListView's TargetUpdated event
        private void ListViewTargetUpdated(object sender, DataTransferEventArgs e)
        {
            // Get a reference to the ListView's GridView...
            var listView = sender as ListView;
            if (null != listView)
            {
                var gridView = listView.View as GridView;
                if (null != gridView)
                {
                    // ... and update its column widths
                    ListViewBehaviors.UpdateColumnWidths(gridView);
                }
            }
        }

        // Class implementing handy behaviors for the ListView control
        public static class ListViewBehaviors
        {
            // Technique for updating column widths of a ListView's GridView manually
            public static void UpdateColumnWidths(GridView gridView)
            {
                // For each column...
                foreach (var column in gridView.Columns)
                {
                    // If this is an "auto width" column...
                    if (double.IsNaN(column.Width))
                    {
                        // Set its Width back to NaN to auto-size again
                        column.Width = 0;
                        column.Width = double.NaN;
                    }
                }
            }

            // Definition of the IsAutoUpdatingColumnWidthsProperty attached DependencyProperty
            public static readonly DependencyProperty IsAutoUpdatingColumnWidthsProperty =
                DependencyProperty.RegisterAttached(
                    "IsAutoUpdatingColumnWidths",
                    typeof(bool),
                    typeof(ListViewBehaviors),
                    new UIPropertyMetadata(false, OnIsAutoUpdatingColumnWidthsChanged));

            // Get/set methods for the attached DependencyProperty
            [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
                Justification = "Only applies to ListView instances.")]
            public static bool GetIsAutoUpdatingColumnWidths(ListView listView)
            {
                return (bool)listView.GetValue(IsAutoUpdatingColumnWidthsProperty);
            }
            [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
                Justification = "Only applies to ListView instances.")]
            public static void SetIsAutoUpdatingColumnWidths(ListView listView, bool value)
            {
                listView.SetValue(IsAutoUpdatingColumnWidthsProperty, value);
            }

            // Change handler for the attached DependencyProperty
            private static void OnIsAutoUpdatingColumnWidthsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
            {
                // Get the ListView instance and new bool value
                var listView = o as ListView;
                if ((null != listView) && (e.NewValue is bool))
                {
                    // Get a descriptor for the ListView's ItemsSource property
                    var descriptor = DependencyPropertyDescriptor.FromProperty(ListView.ItemsSourceProperty, typeof(ListView));
                    if ((bool)e.NewValue)
                    {
                        // Enabling the feature, so add the change handler
                        descriptor.AddValueChanged(listView, OnListViewItemsSourceValueChanged);
                    }
                    else
                    {
                        // Disabling the feature, so remove the change handler
                        descriptor.RemoveValueChanged(listView, OnListViewItemsSourceValueChanged);
                    }
                }
            }

            // Handler for changes to the ListView's ItemsSource updates the column widths
            private static void OnListViewItemsSourceValueChanged(object sender, EventArgs e)
            {
                // Get a reference to the ListView's GridView...
                var listView = sender as ListView;
                if (null != listView)
                {
                    var gridView = listView.View as GridView;
                    if (null != gridView)
                    {
                        // And update its column widths
                        UpdateColumnWidths(gridView);
                    }
                }
            }
        }

        #endregion ListView UpdateColumnsWidth

        private void ListViewSimulation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewSimulation.SelectedItem is not null)
            {
                NomTextbox.IsEnabled = true;
                PrenomTextbox.IsEnabled = true;
                DescriptionTextbox.IsEnabled = true;
                CapitalTextBox.IsEnabled = true;
                TauxAnnuelTextbox.IsEnabled = true;
                PeriodeTextBox.IsEnabled = true;
                FrequenceComboBox.IsEnabled = true;
                AnneeRadioButton.IsChecked = false;
                MoisRadioButton.IsChecked = false;
                AnneeRadioButton.IsEnabled = true;
                MoisRadioButton.IsEnabled = true;

                Simulation simul = ListViewSimulation.SelectedItem as Simulation;
                if (simul.Frequence is not null)
                    FrequenceComboBox.SelectedIndex = simul.Frequence.Id - 1;

                verifChangeRadioButton = false;
                AnneeRadioButton.IsChecked = true;
                verifChangeRadioButton = false;
                MoisRadioButton.IsChecked = true;
            }
            else
            {
                NomTextbox.IsEnabled = false;
                PrenomTextbox.IsEnabled = false;
                DescriptionTextbox.IsEnabled = false;
                CapitalTextBox.IsEnabled = false;
                TauxAnnuelTextbox.IsEnabled = false;
                PeriodeTextBox.IsEnabled = false;
                FrequenceComboBox.IsEnabled = false;
                AnneeRadioButton.IsChecked = false;
                MoisRadioButton.IsChecked = false;
                AnneeRadioButton.IsEnabled = false;
                MoisRadioButton.IsEnabled = false;
            }
        }

        private void TextBoxNomPrenom_TextOnly(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, "^[a-zA-Z]+$") || ((TextBox)sender).Text.Length >= 20)
            {
                e.Handled = true;
            }
        }

        private void TextBoxPeriode_NumberOnly(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, "^[0-9,]+$") || ((TextBox)sender).Text.Length >= 8)
            {
                e.Handled = true;

            }
        }

        private void MoisRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (PeriodeTextBox.Text != "" && PeriodeTextBox.Text is not null && verifChangeRadioButton)
            {
                PeriodeTextBox.Text = (Convert.ToDouble(PeriodeTextBox.Text)*12).ToString();
            }
            verifChangeRadioButton = true;
        }

        private void AnneeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (PeriodeTextBox.Text != "" && PeriodeTextBox.Text is not null && verifChangeRadioButton)
            {
                PeriodeTextBox.Text = (Convert.ToDouble(PeriodeTextBox.Text)/12).ToString();
            }
            verifChangeRadioButton = true;
        }

        
    }
}
