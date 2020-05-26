using Recepten.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recepten.Lib.Interfaces
{
    public interface IGerechtData
    {
        List<Gerecht> Gerechten { get; set; }

        List<Artikel> Artikelen { get; set; }

        void LaadGerechten(IArtikelData dataArtikelen);

        void SlaOp(Gerecht opTeSlaan, bool isNieuw = false);

        void Verwijder(Gerecht teVerwijderen);

        void VoegIngredientToe(Artikel toeTeVoegen, Gerecht gerecht);

        void VerwijderIngredient(Artikel teVerwijderen, Gerecht gerecht);
    }
}
