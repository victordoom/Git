using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models
{
    public class Sistema
    {
        public int CodigoSistema { get; set; }
        public string NombreSistema { get; set; }
        public string NombreCorto { get; set; }
        public string ObjetivoModulo { get; set; }
        public string DescripcionModulo { get; set; }
        public string Abreviatura { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaImplantacion { get; set; }
        public decimal? Nivel1 { get; set; }
        public decimal? Nivel2 { get; set; }
        public string Estado { get; set; }
        public string Cdgmodulo { get; set; }
        public string RutaCarpetaFotos { get; set; }
        public decimal? CodigoSistemaAnt { get; set; }

        public ICollection<Opciones> Opciones { get; set; }
    }
}
