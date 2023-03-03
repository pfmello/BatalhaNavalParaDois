using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatalhaNavalLibrary.Models
{
    public class PlayerModel
    {
        public string NomeDoUsuario { get; set; }
        public List<LocalPinoModel> LocalidadeNavios {get; set;} = new List<LocalPinoModel>();
        public List<LocalPinoModel> Pinos { get; set; } = new List<LocalPinoModel>();
    }
}
