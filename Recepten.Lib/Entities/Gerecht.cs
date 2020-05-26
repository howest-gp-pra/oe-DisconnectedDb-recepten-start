using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recepten.Lib.Entities
{
    public class Gerecht
    {
        public Guid Id { get; set; }

		private string gerechtNaam;

		public string GerechtNaam
		{
			get { return gerechtNaam; }
			set 
			{
				if (value.Trim().Length > 5) gerechtNaam = value;
				else throw new Exception("Geef het gerecht een naam van minimum 5 letters lang");
			}
		}

		public List<Artikel> Ingredienten { get; set; }

		public Gerecht(string naam, Guid? id = null, List<Artikel> ingredienten = null)
		{
			GerechtNaam = naam;
			Id = (id == null) ? Guid.NewGuid() : (Guid)id;
			Ingredienten = (ingredienten == null) ? new List<Artikel>() : ingredienten;
		}

		public override string ToString()
		{
			return $"{GerechtNaam} ({Ingredienten.Count}) ingred." ;
		}
	}
}
