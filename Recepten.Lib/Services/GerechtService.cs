using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recepten.Lib.Entities;
using Recepten.Lib.Interfaces;

namespace Recepten.Lib.Services
{
    public class GerechtService
    {
        private IGerechtData gekozenDataSource;

        public List<Gerecht> Gerechten { get; set; }

        public GerechtService(IGerechtData dataSource)
        {
            Gerechten = dataSource.Gerechten;
            gekozenDataSource = dataSource;
        }

        public void Verwijder(Gerecht teVerwijderen)
        {
            VerwijderInGeheugen(teVerwijderen);
            gekozenDataSource.Verwijder(teVerwijderen);
        }

        void VerwijderInGeheugen(Gerecht teVerwijderen)
        {
            if (teVerwijderen != null && BehoortObjectTotLijst(teVerwijderen))
            {
                Gerechten.Remove(teVerwijderen);
            }
            else throw new Exception("Geef een geldig gerecht door om te verwijderen");

        }

        public void SlaOp(Gerecht opTeSlaan)
        {
            bool isNieuw = SlaOpInGeheugen(opTeSlaan);
            gekozenDataSource.SlaOp(opTeSlaan, isNieuw);
        }

        bool SlaOpInGeheugen(Gerecht opTeSlaan)
        {
            bool isNieuw = true;
            if (opTeSlaan == null) throw new Exception("Geef een geldig gerecht door om op te slaan");

            else if (!BehoortObjectTotLijst(opTeSlaan)) Gerechten.Add(opTeSlaan);
            else
            {
                int indexObject = GeefIndexInLijst(opTeSlaan);
                Gerechten[indexObject] = opTeSlaan;
                isNieuw = false;
            }
            return isNieuw;
        }

        public void VoegIngredientToe(Artikel toeTeVoegen, Gerecht gerecht)
        {
            VoegIngredientToeInGeheugen(toeTeVoegen, gerecht);
            gekozenDataSource.VoegIngredientToe(toeTeVoegen, gerecht);
        }

        public void VoegIngredientToeInGeheugen(Artikel toeTeVoegen, Gerecht gerecht)
        {
            if (toeTeVoegen == null || !gekozenDataSource.Artikelen.Contains(toeTeVoegen)) throw new Exception("Geef een geldig artikel om toe te voegen");
            else if (gerecht == null || !Gerechten.Contains(gerecht)) throw new Exception($"Geef een geldig gerecht om {toeTeVoegen.Naam} aan toe te voegen");
            else if (gerecht.Ingredienten.Contains(toeTeVoegen)) throw new Exception($"Dit gerecht bevat reeds {toeTeVoegen.Naam}");
            else gerecht.Ingredienten.Add(toeTeVoegen);
        }

        public void VerwijderIngredient(Artikel teVerwijderen, Gerecht gerecht)
        {
            VerwijderIngredientInGeheugen(teVerwijderen, gerecht);
            gekozenDataSource.VerwijderIngredient(teVerwijderen, gerecht);
        }

        public void VerwijderIngredientInGeheugen(Artikel teVerwijderen, Gerecht gerecht)
        {
            if (teVerwijderen == null || !gekozenDataSource.Artikelen.Contains(teVerwijderen)) throw new Exception("Geef een geldig artikel om te verwijderen");
            else if (gerecht == null || !Gerechten.Contains(gerecht)) throw new Exception($"Geef een geldig gerecht om {teVerwijderen.Naam} uit te verwijderen");
            else if (!gerecht.Ingredienten.Contains(teVerwijderen)) throw new Exception($"Dit gerecht bevat geen {teVerwijderen.Naam}");
            else gerecht.Ingredienten.Remove(teVerwijderen);
        }

        public int GeefIndexInLijst(Gerecht teChecken)
        {
            int indexTeCheckenInstance = -1;
            for (int i = 0; i < Gerechten.Count; i++)
            {
                if (Gerechten[i].Id == teChecken.Id)
                {
                    indexTeCheckenInstance = i;
                    break;
                }
            }
            return indexTeCheckenInstance;
        }

        public bool BehoortObjectTotLijst(Gerecht teChecken)
        {
            bool gevonden = false;
            foreach (Gerecht instance in Gerechten)
            {
                if (instance.Id == teChecken.Id)
                {
                    gevonden = true;
                    break;
                }
            }
            return gevonden;
        }
    }
}
