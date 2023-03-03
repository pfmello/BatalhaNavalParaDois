using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatalhaNavalLibrary.Models
{
    public class LocalPinoModel
    {
        public string LetraDaPosicao { get; set; }
        public int NumeroDaPosicao { get; set; }
        public StatusPino Status { get; set; } = StatusPino.Vazio;
    }
}
