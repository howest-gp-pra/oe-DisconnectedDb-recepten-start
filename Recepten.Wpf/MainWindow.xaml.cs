using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Recepten.Lib.Entities;
using Recepten.Lib.Services;
using Recepten.Lib.Interfaces;



namespace Recepten.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataSources startDataSource = DataSources.TextFile;

        enum DataSources
        {
            MockData, TextFile, DatabaseDisconnected, DatabaseConnected
        }

        Artikel huidigArtikel;
        Gerecht huidigGerecht;
        ArtikelService artikelService;
        GerechtService gerechtService;

        public MainWindow()
        {
            InitializeComponent();
        }

        void BepaalData(DataSources dataSource)
        {
            IArtikelData artikelBestand = null;
            IGerechtData gerechtBestand = null;
            switch (dataSource)
            {
                case DataSources.MockData:
                    artikelBestand = new MockDataArtikelen();
                    gerechtBestand = new MockDataGerechten(artikelBestand);
                    break;
                case DataSources.TextFile:
                    artikelBestand = new TextDataArtikelen();
                    gerechtBestand = new TextDataGerechten(artikelBestand);
                    break;
                case DataSources.DatabaseDisconnected:
                    break;
                case DataSources.DatabaseConnected:
                    break;
                default:
                    break;
            }
            artikelService = new ArtikelService(artikelBestand);
            gerechtService = new GerechtService(gerechtBestand);
        }

        private void MaakGuiLeeg()
        {
            lstArtikelen.SelectedItem = null;
            ClearPanel(grdArtikel);
            cmbEenheid.SelectedIndex = 0;
            txtPrijs.Text = "0";
            txtArtikelnaam.Focus();
            tbkFeedback.Visibility = Visibility.Hidden;
        }

        Artikel GeefIngegevenArtikel()
        {
            Artikel ingegevenArtikel = null;
            string naam = txtArtikelnaam.Text;
            string eenheid = cmbEenheid.SelectedItem.ToString();
            try
            {
                decimal prijs = decimal.Parse(txtPrijs.Text);
                try
                {
                    int id = (huidigArtikel != null) ? huidigArtikel.Id : 0;
                    ingegevenArtikel = new Artikel(naam, eenheid, prijs, id);
                }
                catch (Exception ex)
                {
                    ToonMelding(ex.Message);
                }
            }
            catch (Exception)
            {
                ToonMelding("De prijs is geen geldig getal");
            }
            return ingegevenArtikel;
        }

        void ToonArtikelen()
        {
            lstArtikelen.ItemsSource = artikelService.Artikelen;
            lstArtikelen.Items.Refresh();
        }

        void ToonGerechten()
        {
            cmbGerechten.ItemsSource = null;
            cmbGerechten.ItemsSource = gerechtService.Gerechten;
            cmbGerechten.Items.Refresh();
        }

        void ToonIngredienten()
        {
            lstIngredienten.ItemsSource = huidigGerecht.Ingredienten;
            lstIngredienten.Items.Refresh();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbDataSource.ItemsSource = Enum.GetValues(typeof(DataSources));
            cmbDataSource.SelectedItem = startDataSource;
            txtPrijs.TextChanged += DecimalTextBox_TextChanged;
            cmbEenheid.ItemsSource = Artikel.Verpakkingen;
        }

        void LaadBeginSituatie()
        {
            ToonArtikelen();
            ToonGerechten();
            lstArtikelen.SelectedIndex = 0;
            cmbGerechten.SelectedIndex = 0;
            tbkFeedback.Visibility = Visibility.Hidden;
        }

        private void lstArtikelen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            huidigArtikel = (Artikel)lstArtikelen.SelectedItem;
            if (huidigArtikel != null)
            {
                lblId.Content = huidigArtikel.Id;
                txtArtikelnaam.Text = huidigArtikel.Naam;
                txtPrijs.Text = huidigArtikel.Prijs.ToString();
                cmbEenheid.SelectedItem = huidigArtikel.Eenheid;
            }
            else ClearPanel(grdArtikel);
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            MaakGuiLeeg();
        }

        private void btnVerwijder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string artikelNaam = (huidigArtikel != null) ? huidigArtikel.Naam : "";
                artikelService.Verwijder(huidigArtikel);
                ToonArtikelen();
                MaakGuiLeeg();
                ToonMelding($"{artikelNaam} is verwijderd", true);
            }
            catch (Exception ex)
            {
                ToonMelding(ex.Message);
            }
        }

        private void btnSlaOp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                huidigArtikel = GeefIngegevenArtikel();
                if(huidigArtikel != null)
                {
                    string artikelNaam = (huidigArtikel != null) ? huidigArtikel.Naam : "";
                    artikelService.SlaOp(huidigArtikel);
                    ToonArtikelen();
                    MaakGuiLeeg();
                    ToonMelding($"{artikelNaam} is opgeslagen", true);
                }
            }
            catch (Exception ex)
            {
                ToonMelding(ex.Message);
            }
        }

        private void cmbDataSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataSources dataSource = (DataSources)cmbDataSource.SelectedItem;
            try
            {
                BepaalData(dataSource);                
                LaadBeginSituatie();
                tbkFeedback.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                ToonMelding(ex.Message);
            }
        }

        private void btnVoegGerechtToe_Click(object sender, RoutedEventArgs e)
        {
            string receptNaam = txtReceptNaam.Text;
            try
            {
                gerechtService.SlaOp(new Gerecht(receptNaam));
                ToonGerechten();
                txtReceptNaam.Clear();
                ToonMelding($"{receptNaam} is opgeslagen", true);
            }
            catch (Exception ex)
            {
                ToonMelding(ex.Message);
            }
        }

        private void btnVoegIngredientToe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int indexGerecht = cmbGerechten.SelectedIndex;
                gerechtService.VoegIngredientToe(huidigArtikel, huidigGerecht);
                ToonIngredienten();
                ToonGerechten();
                cmbGerechten.SelectedIndex = indexGerecht;
                tbkFeedback.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                ToonMelding(ex.Message);
            }
        }

        private void cmbGerechten_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            huidigGerecht = (cmbGerechten.SelectedItem != null) ? (Gerecht)cmbGerechten.SelectedItem : null;
            if(huidigGerecht != null) ToonIngredienten();
        }

        private void lstIngredienten_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Artikel huidigIngredient = (lstIngredienten.SelectedItem != null) ? (Artikel)lstIngredienten.SelectedItem : null;
            try
            {
                int indexGerecht = cmbGerechten.SelectedIndex;
                if (huidigIngredient != null) gerechtService.VerwijderIngredient(huidigIngredient, huidigGerecht);
                ToonIngredienten();
                ToonGerechten();
                cmbGerechten.SelectedIndex = indexGerecht;
                tbkFeedback.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                ToonMelding(ex.Message);
            }
        }
    }
}
