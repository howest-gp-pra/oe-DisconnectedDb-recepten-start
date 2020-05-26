using Recepten.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recepten.Lib.Interfaces
{
    public interface IArtikelData
    {
        List<Artikel> Artikelen { get; set; }

        void LaadArtikelen();

        void SlaOp(Artikel opTeSlaan);

        void Verwijder(Artikel teVerwijderen);

    }
}
