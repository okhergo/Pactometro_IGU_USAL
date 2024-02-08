using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PF.M
{
    public class Partido
    {
        public String nombre { get; set; }
        public int escaños { get; set; }
        public Brush color { get; set; }

        public Partido(String nombre, int escaños, Color color){
            this.nombre = nombre.ToUpper();
            this.escaños = escaños;
            this.color = new SolidColorBrush(color);
        }
    }
}
