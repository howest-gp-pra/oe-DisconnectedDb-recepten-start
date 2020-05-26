using Recepten.Lib.Entities;
using Recepten.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextFileStreams.Lib;

namespace Recepten.Lib.Services
{
    public class TextDataGerechten : IGerechtData
    {

        const int CnId = 0;
        const int CnNaam = 1;
        const int CnIngredienten = 2;
        const char Separator = ';';

        const string FolderPad = @"../../Assets/";
        const string BestandsNaam = "Gerechten.csv";

        public List<Gerecht> Gerechten { get; set ; }

        public List<Artikel> Artikelen { get; set; }

        public TextDataGerechten(IArtikelData dataArtikelen)
        {
            Artikelen = dataArtikelen.Artikelen;
            LaadGerechten(dataArtikelen);
        }

        public void LaadGerechten(IArtikelData dataArtikelen)
        {
            string pad = FolderPad + BestandsNaam;
            List<string[]> gerechtenLijst = ReadService.TxtFile_To_ListOfStringArrays(pad, Separator);
            Gerechten = ZetGerechtArraysOmNaarObjecten(gerechtenLijst, dataArtikelen);
        }

        List<Gerecht> ZetGerechtArraysOmNaarObjecten(List<string[]> gerechtArray, IArtikelData dataArtikelen)
        {
            List<Gerecht> omgezet = new List<Gerecht>();

            foreach (string[] gerechtInfo in gerechtArray)
            {
                Guid id = Guid.Parse(gerechtInfo[CnId]);
                string naam = gerechtInfo[CnNaam];
                string ingredienten = gerechtInfo[CnIngredienten];
                List<Artikel> ingredientList = ArtikelService.GeefArtikelLijst(ingredienten, '|', dataArtikelen.Artikelen);

                Gerecht gerecht = new Gerecht(naam, id, ingredientList);
                omgezet.Add(gerecht);
            }
            return omgezet;
        }

        public void SlaOp(Gerecht opTeSlaan, bool nietNodig)
        {
            SchrijfGerechtenNaarTextFile(FolderPad, BestandsNaam);
        }

        public void Verwijder(Gerecht teVerwijderen)
        {
            SchrijfGerechtenNaarTextFile(FolderPad, BestandsNaam);
        }

        public void VerwijderIngredient(Artikel teVerwijderen, Gerecht gerecht)
        {
            SchrijfGerechtenNaarTextFile(FolderPad, BestandsNaam);
        }

        public void VoegIngredientToe(Artikel toeTeVoegen, Gerecht gerecht)
        {
            SchrijfGerechtenNaarTextFile(FolderPad, BestandsNaam);
        }

        public void SchrijfGerechtenNaarTextFile(string folderPad, string bestandsNaam)
        {
            List<string[]> gerechtenOmgezet = new List<string[]>();
            foreach (Gerecht instance in Gerechten)
            {
                string ingredientenCsv = ArtikelService.GeefCsvMetIds(instance.Ingredienten, '|');
                string[] gerechtInfo =
                {
                    instance.Id.ToString(),
                    instance.GerechtNaam,
                    ingredientenCsv
                };
                gerechtenOmgezet.Add(gerechtInfo);
            }
            WriteService.ListVanArrays_To_TextFile(gerechtenOmgezet, folderPad, bestandsNaam, Separator.ToString());
        }
    }
}
