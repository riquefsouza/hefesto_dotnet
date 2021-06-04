using System;
using System.Collections.Generic;
using System.Linq;

namespace hefesto.base_hefesto.Util
{
    public class BaseUtil
    {
        public static bool containsNumericSequences(int min, int max, string stexto)
        {
            List<string> lista = new List<string>();
            string sValorMin = "";
            int[] vnum = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for (int n = 0; n < 7; n++)
            {
                for (int qtd = min - 1; qtd <= max; qtd++)
                {
                    sValorMin = "";
                    for (int i = n; i <= (qtd + n); i++)
                    {
                        if (i <= max)
                        {
                            sValorMin += Convert.ToString(vnum[i]);
                        }
                    }
                    lista.Add(sValorMin);
                }
            }
            lista = lista.Distinct().ToList();
            return lista.Contains(stexto);
        }

        public static bool containsConsecutiveIdenticalCharacters(int min, int max, string stexto)
        {
            List<string> lista = new List<string>();
            string sAlfaMin = "";

            for (char c = 'a'; c <= 'z'; c++)
            {
                for (int qtd = min; qtd <= max; qtd++)
                {
                    sAlfaMin = "";
                    for (int i = 1; i <= qtd; i++)
                    {
                        sAlfaMin += Char.ToString(c);
                    }
                    lista.Add(sAlfaMin);
                    lista.Add(sAlfaMin.ToUpper());
                }
            }
            return lista.Contains(stexto);
        }

    }
}
