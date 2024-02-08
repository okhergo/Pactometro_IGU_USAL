using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.VM
{
    internal class Columnas
    {
        public int Votos { get; set; }
        public double Altura { get; set; }
        public string Nombre { get; set; }

        public Columnas(string s)
        {
            Nombre = s;
        }
    }
}
