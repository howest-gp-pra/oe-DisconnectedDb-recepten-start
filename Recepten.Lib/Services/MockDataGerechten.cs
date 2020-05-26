using Recepten.Lib.Entities;
using Recepten.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recepten.Lib.Services
{
    public class MockDataGerechten : IGerechtData
    {
        public List<Gerecht> Gerechten { get; set; }

        public List<Artikel> Artikelen { get; set; }

        public MockDataGerechten(IArtikelData dataArtikelen)
        {
            Artikelen = dataArtikelen.Artikelen;
            LaadGerechten(dataArtikelen);
        }

        public void LaadGerechten(IArtikelData dataArtikelen)
        {
            Gerechten = new List<Gerecht>
            {
                new Gerecht("Spek met eiers", ingredienten: new List<Artikel>
                {
                    ArtikelService.GeefArtikelMetId(533, dataArtikelen.Artikelen),
                    ArtikelService.GeefArtikelMetId(858, dataArtikelen.Artikelen)
                }),
                new Gerecht("Frieten", ingredienten: new List<Artikel>
                {
                    ArtikelService.GeefArtikelMetId(8, dataArtikelen.Artikelen),
                    ArtikelService.GeefArtikelMetId(2123, dataArtikelen.Artikelen)
                }),
                new Gerecht("Asperges à la Flamande", ingredienten: new List<Artikel>
                {
                    ArtikelService.GeefArtikelMetId(103, dataArtikelen.Artikelen),
                    ArtikelService.GeefArtikelMetId(251, dataArtikelen.Artikelen),
                    ArtikelService.GeefArtikelMetId(533, dataArtikelen.Artikelen)
                }),
                new Gerecht("Eendeborst in de wijn", ingredienten: new List<Artikel>
                {
                    ArtikelService.GeefArtikelMetId(251, dataArtikelen.Artikelen),
                    ArtikelService.GeefArtikelMetId(514, dataArtikelen.Artikelen),
                    ArtikelService.GeefArtikelMetId(2031, dataArtikelen.Artikelen)
                }),
                new Gerecht("Gestoofd witloof", ingredienten: new List<Artikel>
                {
                    ArtikelService.GeefArtikelMetId(533, dataArtikelen.Artikelen),
                    ArtikelService.GeefArtikelMetId(251, dataArtikelen.Artikelen),
                    ArtikelService.GeefArtikelMetId(1340, dataArtikelen.Artikelen),
                    ArtikelService.GeefArtikelMetId(2042, dataArtikelen.Artikelen)
                })
            };
        }

        public void VoegIngredientToe(Artikel toeTeVoegen, Gerecht gerecht)
        {
            //Geen verdere opslag nodig
        }

        public void VerwijderIngredient(Artikel teVerwijderen, Gerecht gerecht)
        {
            //Geen verdere opslag nodig
        }

        public void SlaOp(Gerecht opTeSlaan)
        {
            //Geen verdere opslag nodig
        }

        public void Verwijder(Gerecht teVerwijderen)
        {
            //Geen verdere opslag nodig
        }
    }
}
