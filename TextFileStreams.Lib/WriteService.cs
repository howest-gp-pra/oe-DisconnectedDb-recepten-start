using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFileStreams.Lib
{
    public static class WriteService
    {
        public static void String_To_TextFile(string tekst, string folderPad, string bestandsNaam, Encoding encoding = null)
        {
            string bestandsPad = folderPad + bestandsNaam;

            if (encoding == null) encoding = Encoding.Default;

            try
            {
                using(StreamWriter sw = new StreamWriter(
                    new FileStream(bestandsPad, FileMode.Create, FileAccess.ReadWrite), encoding))
                {
                    sw.Write(tekst);
                    sw.Close();
                }
            }
            catch (DirectoryNotFoundException)
            {
                throw new Exception($"De map {folderPad} bestaat niet op deze computer");
            }
            catch (IOException)
            {
                //Soms zijn bestanden gelocked, met deze exception voor gevolg
                throw new Exception($"Het bestand {bestandsPad} kan niet weggeschreven worden.\n" +
                                $"Probeer het geopende bestand op die locatie te sluiten.");
            }
        }

        public static void ListVanArrays_To_TextFile(List<string[]> listVanArrays, string folderPad, string bestandsNaam, string separator, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.Default;

            List<string> wegTeSchrijven = ArrayList_To_CharacterSeparatedList(listVanArrays, separator);

            ListVanStrings_To_TextFile(wegTeSchrijven, folderPad, bestandsNaam, encoding);

        }

        public static void ListVanStrings_To_TextFile(List<string> data, string folderPad, string bestandsNaam, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.Default;
            string omgezet = "";
            foreach (string item in data)
            {
                omgezet += item + "\n";
            }
            String_To_TextFile(omgezet, folderPad, bestandsNaam);

        }

        private static List<string> ArrayList_To_CharacterSeparatedList(List<string[]> listVanArrays, string separator)
        {
            List<string> omgezet = new List<string>();
            foreach (string[] record in listVanArrays)
            {
                string characterSeparatedString = String.Join(separator, record);
                omgezet.Add(characterSeparatedString);
            }
            return omgezet;

        }
    }
}
