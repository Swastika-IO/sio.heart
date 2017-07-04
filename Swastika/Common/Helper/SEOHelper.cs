using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Swastika.Common.Helper
{
    public class SEOHelper
    {
        public static string GetSEOString(string s)
        {
            return WhiteSpaceToHyphen(ConvertToUnSign(DeleteSpecialCharaters(s)));
        }

        //delete special charaters
        public static string DeleteSpecialCharaters(string str)
        {
            string replaceChar = "";
            string[] pattern = { ".", "/", "\\", "&", ":", "%" };

            foreach (string item in pattern)
            {
                str = str.Replace(item, replaceChar);
            }
            return str;
        }

        // Chuyển tiếng việt có dấu thành không dấu


        #region convert tieng viet ko dau
        public static string ConvertToUnSign(string text)
        {
            if (text != null)
            {

                for (int i = 33; i < 48; i++)
                {

                    text = text.Replace(((char)i).ToString(), "");

                }



                for (int i = 58; i < 65; i++)
                {

                    text = text.Replace(((char)i).ToString(), "");

                }



                for (int i = 91; i < 97; i++)
                {

                    text = text.Replace(((char)i).ToString(), "");

                }

                for (int i = 123; i < 127; i++)
                {

                    text = text.Replace(((char)i).ToString(), "");

                }
            }
            else
            {
                text = "";
            }
            //text = text.Replace(" ", "-");

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\p{IsCombiningDiacriticalMarks}+");

            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);

            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

        }
        #endregion
        //change white-space to hyphen
        public static string WhiteSpaceToHyphen(string str)
        {
            char replaceChar = '-';
            string pattern = " |–";
            MatchCollection matchs = Regex.Matches(str, pattern, RegexOptions.IgnoreCase);
            foreach (Match m in matchs)
            {
                str = str.Replace(m.Value[0], replaceChar);
            }
            replaceChar = '\'';
            pattern = "\"|“|”";
            matchs = Regex.Matches(str, pattern, RegexOptions.IgnoreCase);
            foreach (Match m in matchs)
            {
                str = str.Replace(m.Value[0], replaceChar);
            }
            return str;
        }
    }
}
