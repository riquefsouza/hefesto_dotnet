using System.Text.RegularExpressions;
using hefesto.base_hefesto.Util;

namespace hefesto.admin.Services
{
    public class ChangePasswordService : IChangePasswordService
    {
        /*
        As minimum requirements for user passwords, the following should be considered:
        Minimum length of 8 characters;
        Presence of at least 3 of the 4 character classes below:
            uppercase characters;
            lowercase characters;
            numbers;
            special characters;
            Absence of strings (eg: 1234) or consecutive identical characters (yyyy);
            Absence of any username identifier, such as: John Silva - user: john.silva - password cannot contain "john" or "silva".
        */
        public bool ValidatePassword(string login, string senha){
            if (senha.Length >= 8) {
                string letterUppercase = "[A-Z]";
                string letterLowercase = "[a-z]";
                string digit = "[0-9]";
                string special = "[!@#$%&*()_+=|<>?{}\\[\\]~-]";
                            
                bool U = Regex.IsMatch(senha, letterUppercase);
                bool L = Regex.IsMatch(senha, letterLowercase);
                bool D = Regex.IsMatch(senha, digit);
                bool S = Regex.IsMatch(senha, special);
                
                bool hasChars = (U && L && D) || (S && U && L) || (D && S && U) || (L && D && S);
                
                return hasChars 
                        && !BaseUtil.containsNumericSequences(4,9, senha) 
                        && !BaseUtil.containsConsecutiveIdenticalCharacters(4,9, senha)
                        && !senha.Contains(login);
        
            } else
                return false;
        }
    }
}