using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;

namespace PF.M
{
    enum tipoEleccion
    {
        AUTONOMICAS,
        GENERALES
    }
    enum comunidadAutonoma
    {
        GALICIA,
        ASTURIAS,
        CANTABRIA,
        PAIS_VASCO,
        NAVARRA,
        LA_RIOJA,
        ARAGON,
        CATALUNYA,
        CYL,
        MADRID,
        COMUNITAT_VALENCIANA,
        EXTREMADURA,
        CASTILLA_LAMANCHA,
        MURCIA,
        ANDALUCIA,
        BALEARES,
        CANARIAS,
        CEUTA,
        MELILLA
    }
    public class Eleccion
    {
        public String titulo { get; set; }
        public String fecha { get; set; }
        public int escañosTotales { get; set; }
        public int mayoria { get; set; }
        public ObservableCollection<Partido> partidos { get; set; }

        public Eleccion(String tipoEleccion, String fecha, ObservableCollection<Partido> partidos)
        {
            orderPartidos(partidos);
            this.titulo = "ELECCIONES" + ' ' + tipoEleccion;
            this.fecha = fecha;
            this.escañosTotales = 350;
            this.mayoria = 176;
            this.partidos = partidos;
        }
        public Eleccion(String tipoEleccion, String comunidadAutonoma, String fecha, ObservableCollection<Partido> partidos) 
        {
            orderPartidos(partidos);
            this.titulo = "ELECCIONES" + ' ' + tipoEleccion + ' ' + comunidadAutonoma;
            this.fecha = fecha;
            this.partidos = partidos;
            setTotales(comunidadAutonoma);
        }
        public void setTotales(String comunidadAutonoma)
        {
            switch (comunidadAutonoma)
            {
                case "GALICIA":
                    this.escañosTotales = 75;
                    this.mayoria = 38;
                    break;
                case "ASTURIAS":
                    this.escañosTotales = 45;
                    this.mayoria = 23;
                    break;
                case "CANTABRIA":
                    this.escañosTotales = 35;
                    this.mayoria = 18;
                    break;
                case "PAIS_VASCO":
                    this.escañosTotales = 51;
                    this.mayoria = 26;
                    break;
                case "NAVARRA":
                    this.escañosTotales = 50;
                    this.mayoria = 26;
                    break;
                case "LA_RIOJA":
                    this.escañosTotales = 33;
                    this.mayoria = 17;
                    break;
                case "ARAGON":
                    this.escañosTotales = 67;
                    this.mayoria = 34;
                    break;
                case "CATALUNYA":
                    this.escañosTotales = 135;
                    this.mayoria = 68;
                    break;
                case "CYL":
                    this.escañosTotales = 81;
                    this.mayoria = 41;
                    break;
                case "MADRID":
                    this.escañosTotales = 135;
                    this.mayoria = 68;
                    break;
                case "COMUNITAT_VALENCIANA":
                    this.escañosTotales = 99;
                    this.mayoria = 50;
                    break;
                case "EXTREMADURA":
                    this.escañosTotales = 65;
                    this.mayoria = 33;
                    break;
                case "CASTILLA_LAMANCHA":
                    this.escañosTotales = 33;
                    this.mayoria = 17;
                    break;
                case "MURCIA":
                    this.escañosTotales = 45;
                    this.mayoria = 23;
                    break;

                case "ANDALUCIA":
                    this.escañosTotales = 109;
                    this.mayoria = 55;
                    break;
                case "BALEARES":
                    this.escañosTotales = 59;
                    this.mayoria = 30;
                    break;
                case "CANARIAS":
                    this.escañosTotales = 61;
                    this.mayoria = 31;
                    break;
                case "CEUTA":
                    this.escañosTotales = 25;
                    this.mayoria = 13;
                    break;
                case "MELILLA":
                    this.escañosTotales = 25;
                    this.mayoria = 13;
                    break;
            }
        }
        public Eleccion() { }

        private void orderPartidos(ObservableCollection<Partido> partidos)
        {
            List<Partido> listaPartidos = new List<Partido>();
            foreach (Partido partido in partidos)
            {
                listaPartidos.Add(partido);
            }
            var listaOrdenada = listaPartidos.OrderBy(p => p.escaños).ToList();
            listaOrdenada.Reverse();
            partidos.Clear();
            foreach (Partido partido in listaOrdenada)
            {
                partidos.Add(partido);
            }
        }
    }
}
