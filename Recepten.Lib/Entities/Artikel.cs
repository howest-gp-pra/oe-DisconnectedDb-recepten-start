using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recepten.Lib.Entities
{
    public class Artikel
    {
		static int maxId = 0;
		
		public static string[] Verpakkingen { get; private set; } =  { "100 g", "100 ml", "200 cl", "50 cl", "500 g", "75 cl", "Bot", "Doos",
										"doosje", "Fles", "kg", "Kg", "Liter", "liter", "pak", "Portie", "stuk", "Stuk", "zak" };

		private int id;

		public int Id
		{
			get { return id; }
			set 
			{
				if (value > 0) id = value;
				else throw new Exception("De id moet groter zijn dan 0");
			}
		}

		private string naam;

		public string Naam
		{
			get { return naam; }
			set
			{
				if (value.Trim().Length >= 2) naam = value;
				else throw new Exception("De artikelnaam moet minstens twee letters bevatten.");
			}
		}

		private string eenheid;

		public string Eenheid
		{
			get { return eenheid; }
			set 
			{ 
				if(Verpakkingen.Contains(value)) eenheid = value;
				else throw new Exception("Neem een geldige eenheid uit de lijst.");
			}
		}

		private decimal prijs;

		public decimal Prijs
		{
			get { return prijs; }
			set
			{
				if (value > 0.001M && value < 1300) prijs = value;
				else throw new Exception("De prijs moet groter zijn dan 0 en kleiner dan 1300");
			}
		}

		public Artikel(string naam, string bestelEenheid, decimal prijs, int id = 0)
		{
			Naam = naam;
			Eenheid = bestelEenheid;
			Prijs = prijs;
			Id = (id == 0) ? Id = maxId + 1 : id;
			if (Id > maxId) maxId = Id;
		}

		public override string ToString()
		{
			string info;
			info = $"{Id} - {Naam}\n\t€ {Prijs}/{Eenheid}\n--------------------";
			return info;
		}
	}
}
