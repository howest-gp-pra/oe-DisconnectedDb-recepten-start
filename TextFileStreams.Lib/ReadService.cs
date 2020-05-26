using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFileStreams.Lib
{
    public static class ReadService
    {
        public static string RootPad { get; } = AppDomain.CurrentDomain.BaseDirectory;
        public static string MyDocs { get; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public static string TextFile_To_String(string bestandPad, Encoding encoding = null)
        {
            string bestandsInhoud = "";

            if (encoding == null)
            {
                //encoding = Encoding.UTF8;
                encoding = Encoding.Default;
                //encoding = Encoding.GetEncoding("iso-8859-1");
            }

            if (bestandPad.Trim() == "") throw new Exception("Geef een bestandpad mee");
            else
            {
                try
                {
                    using (StreamReader sr = new StreamReader(bestandPad, encoding))
                    {
                        bestandsInhoud = sr.ReadToEnd();
                    }
                }
                catch (FileNotFoundException)
                {
                    throw new Exception($"Het bestand {bestandPad} is niet gevonden");
                }
                catch (IOException)
                {
                    throw new Exception($"Het bestand {bestandPad} kan niet geopend worden.\nProbeer te sluiten.");
                }
            }

            return bestandsInhoud;
        }

        public static List<string> TxtFile_To_ListOfStrings(string bestandsPad, Encoding encoding = null)
        {
            List<string> resultaat;
            string[] temp;

            string inhoud = TextFile_To_String(bestandsPad, encoding);
            temp = inhoud.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            resultaat = temp.ToList();

            return resultaat;
        }

        public static List<string[]> TxtFile_To_ListOfStringArrays(string bestandsPad, char scheidingsteken, Encoding encoding = null)
        {
            List<string[]> resultaat = new List<string[]>();
            List<string> omzettingsLijnen = TxtFile_To_ListOfStrings(bestandsPad, encoding);
            foreach (string item in omzettingsLijnen)
            {
                string[] arrayVanItem = item.Split(scheidingsteken);
                resultaat.Add(arrayVanItem);
            }
            return resultaat;
        }
    }
}
