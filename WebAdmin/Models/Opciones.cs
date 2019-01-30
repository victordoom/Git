using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models
{
    public class Opciones
    {
        public int IdOpcion { get; set; }
        public int? CodigoSistema { get; set; }
        public string NombreOpcion { get; set; }
        public decimal? Nivel1 { get; set; }
        public decimal? Nivel2 { get; set; }
        public decimal? Nivel3 { get; set; }
        public decimal? Nivel4 { get; set; }
        public string Central { get; set; }
        public string Formulario { get; set; }
        public string Icono { get; set; }

        public Sistema CodigoSistemaNavigation { get; set; }
    }
}
