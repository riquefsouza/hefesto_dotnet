using System;

namespace hefesto.base_hefesto.Models
{
    public class TokenDTO
    {
        public TokenDTO(string token, string tipo){
            this.Token = token;
            this.Tipo = tipo;
        }
        public string Token { get; set; }
        public string Tipo { get; set; }
    }
}
