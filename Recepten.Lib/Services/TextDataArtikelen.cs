using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recepten.Lib.Entities;
using Recepten.Lib.Interfaces;
using TextFileStreams.Lib;

namespace Recepten.Lib.Services
{
    public class TextDataArtikelen : IArtikelData
    {
        const int CnId = 0;
        const int CnNaam = 1;
        const int CnEenheid = 2;
        const int CnPrijs = 3;
        const char Separator = ';';

        const string FolderPad = @"../../Assets/";
        const string BestandsNaam = "Artikelen.csv";

        static char huidigDecimaalTeken;

        public List<Artikel> Artikelen { get; set; } 

        public TextDataArtikelen()
        {
            huidigDecimaalTeken = GeefDecimaalTeken();
            LaadArtikelen();
        }

        char GeefDecimaalTeken()
        {
            float testGetal = 0.1F;
            string testString = testGetal.ToString();
            return testString[1];
        }

        public void SchrijfArtikelenNaarTextFile(string folderPad, string bestandsNaam)
        {
            List<string[]> artikelenOmgezet = new List<string[]>();
            foreach (Artikel instance in Artikelen)
            {
                string prijs = instance.Prijs.ToString().Replace('.', huidigDecimaalTeken);
                prijs = instance.Prijs.ToString().Replace(',', huidigDecimaalTeken);
                string[] artikelInfo =
                {
                    instance.Id.ToString(),
                    instance.Naam,
                    instance.Eenheid,
                    prijs
                };
                artikelenOmgezet.Add(artikelInfo);
            }
            WriteService.ListVanArrays_To_TextFile(artikelenOmgezet, folderPad, bestandsNaam, Separator.ToString());
        }

        public void SlaOp(Artikel opTeSlaan)
        {
            if (opTeSlaan == null) throw new Exception("Geef een geldig artikel door om op te slaan");
            else if (!BehoortObjectTotLijst(opTeSlaan)) Artikelen.Add(opTeSlaan);
            else
            {
                int indexObject = GeefIndexInLijst(opTeSlaan);
                Artikelen[indexObject] = opTeSlaan;
            }
            SchrijfArtikelenNaarTextFile(FolderPad, BestandsNaam);
        }

        public void Verwijder(Artikel teVerwijderen)
        {
            if (teVerwijderen != null && BehoortObjectTotLijst(teVerwijderen))
            {
                Artikelen.Remove(teVerwijderen);
                SchrijfArtikelenNaarTextFile(FolderPad, BestandsNaam);
            }
            else throw new Exception("Geef een geldig artikel door om te verwijderen");
        }

        int GeefIndexInLijst(Artikel teChecken)
        {
            int indexTeCheckenInstance = -1;
            for (int i = 0; i < Artikelen.Count; i++)
            {
                if (Artikelen[i].Id == teChecken.Id)
                {
                    indexTeCheckenInstance = i;
                    break;
                }
            }
            return indexTeCheckenInstance;
        }

        bool BehoortObjectTotLijst(Artikel teChecken)
        {
            bool gevonden = false;
            foreach (Artikel instance in Artikelen)
            {
                if (instance.Id == teChecken.Id)
                {
                    gevonden = true;
                    break;
                }
            }
            return gevonden;
        }

        List<Artikel> ZetArtikelArraysOmNaarObjecten(List<string[]> artikelArray, char decimaalTeken)
        {
            List<Artikel> omgezet = new List<Artikel>();
            huidigDecimaalTeken = decimaalTeken;
            foreach (string[] artikelInfo in artikelArray)
            {
                int id = int.Parse(artikelInfo[CnId]);
                string naam = artikelInfo[CnNaam];
                string eenheid = artikelInfo[CnEenheid];
                string gegevenPrijs = artikelInfo[CnPrijs].Replace(',', decimaalTeken);
                gegevenPrijs = gegevenPrijs.Replace('.', decimaalTeken);
                decimal prijs = decimal.Parse(gegevenPrijs);
                Artikel artikel = new Artikel(naam, eenheid, prijs, id);
                omgezet.Add(artikel);
            }
            return omgezet;
        }

        public void LaadArtikelen()
        {
            string pad = FolderPad + BestandsNaam;
            List<string[]> artikelLijst = ReadService.TxtFile_To_ListOfStringArrays(pad, Separator);
            Artikelen = ZetArtikelArraysOmNaarObjecten(artikelLijst, huidigDecimaalTeken);

        }
    }
}
