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

namespace CalculatriceHypothèque.View
{
    /// <summary>
    /// Interaction logic for ListeResultat.xaml
    /// </summary>
    public partial class ListeResultat : Window
    {
        public ListeResultat()
        {
            InitializeComponent();
        }

        private void ProductsListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth - 9; // take into account vertical scrollbar
            var col1 = 0.20;
            var col2 = 0.20;
            var col3 = 0.20;
            var col4 = 0.20;
            var col5 = 0.20;

            gView.Columns[0].Width = workingWidth * col1;
            gView.Columns[1].Width = workingWidth * col2;
            gView.Columns[2].Width = workingWidth * col3;
            gView.Columns[3].Width = workingWidth * col4;
            gView.Columns[4].Width = workingWidth * col5;
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
    }
}
