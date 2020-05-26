using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Recepten.Lib.Entities;
using Recepten.Lib.Interfaces;
using TextFileStreams.Lib;

namespace Recepten.Lib.Services
{
    public class ArtikelService
    {

        private IArtikelData gekozenDataSource;

        public List<Artikel> Artikelen { get; set; }

        public ArtikelService(IArtikelData dataSource)
        {
            Artikelen = dataSource.Artikelen;
            gekozenDataSource = dataSource;
        }

        public void Verwijder(Artikel teVerwijderen)
        {
            VerwijderInGeheugen(teVerwijderen);
            gekozenDataSource.Verwijder(teVerwijderen);
        }

        void VerwijderInGeheugen(Artikel teVerwijderen)
        {
            if (teVerwijderen != null && BehoortObjectTotLijst(teVerwijderen, Artikelen))
            {
                Artikelen.Remove(teVerwijderen);
            }
            else throw new Exception("Geef een geldig artikel door om te verwijderen");
        }

        public void SlaOp(Artikel opTeSlaan)
        {
            SlaOpInGeheugen(opTeSlaan);
            gekozenDataSource.SlaOp(opTeSlaan);
        }

        public void SlaOpInGeheugen(Artikel opTeSlaan)
        {
            if (opTeSlaan == null) throw new Exception("Geef een geldig artikel door om op te slaan");
            else if (!BehoortObjectTotLijst(opTeSlaan, Artikelen)) Artikelen.Add(opTeSlaan);
            else
            {
                int indexObject = GeefIndexInLijst(opTeSlaan, Artikelen);
                Artikelen[indexObject] = opTeSlaan;
            }
        }

        static int GeefIndexInLijst(Artikel teChecken, List<Artikel> artikelen)
        {
            int indexTeCheckenInstance = -1;
            for (int i = 0; i < artikelen.Count; i++)
            {
                if (artikelen[i].Id == teChecken.Id)
                {
                    indexTeCheckenInstance = i;
                    break;
                }
            }
            return indexTeCheckenInstance;
        }

        static bool BehoortObjectTotLijst(Artikel teChecken, List<Artikel> artikelen)
        {
            bool gevonden = false;
            foreach (Artikel instance in artikelen)
            {
                if (instance.Id == teChecken.Id)
                {
                    gevonden = true;
                    break;
                }
            }
            return gevonden;
        }

        public static List<Artikel> GeefArtikelLijst(string ingredientenCsv, char separator, List<Artikel> artikelen)
        {
            List<Artikel> artikelLijst = new List<Artikel>();
            string[] ingredientIdArray = ingredientenCsv.Split(separator);
            foreach (string ingredientId in ingredientIdArray)
            {
                Artikel artikel = GeefArtikelMetId(int.Parse(ingredientId), artikelen);
                artikelLijst.Add(artikel);
            }
            return artikelLijst;
        }

        public static string GeefCsvMetIds(List<Artikel> artikelen, char separator)
        {
            string csv = "";
            for (int i = 0; i < artikelen.Count; i++)
            {
                if (i > 0) csv += separator;
                csv += artikelen[i].Id;
            }
            return csv;
        }

        public static Artikel GeefArtikelMetId(int id, List<Artikel> artikelen)
        {
            Artikel gezochtArtikel = null;
            foreach (Artikel artikel in artikelen)
            {
                if(artikel.Id == id)
                {
                    gezochtArtikel = artikel;
                    break;
                }
            }
            if (gezochtArtikel == null) throw new Exception($"Er is geen artikel gevonden met id {id}");
            return gezochtArtikel;
        }
    }
}
