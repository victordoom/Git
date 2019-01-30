using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAdmin.Models
{
    public partial class SegUsuarios
    {
        //public SegUsuarios()
        //{
        //    Cases = new HashSet<Cases>();
        //    CasesDetails = new HashSet<CasesDetails>();
        //}
        [Key]
        public int UserID { get; set; }
        public string CodEmp { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidoUsuario { get; set; }
        public string DireccionUsuario { get; set; }
        public string TelefonoUsuario { get; set; }
        public string Cargo { get; set; }
        public string Sexo { get; set; }
        public string Dui { get; set; }
        public string Login { get; set; }
        public string WebPassword { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaUltimoLogeo { get; set; }
        public decimal? VigenciaLogin { get; set; }
        public string EstadoUsuario { get; set; }
        public string Gestor { get; set; }
        public string Acceso { get; set; }
        public string DmUser { get; set; }
        public string ImagenFirma { get; set; }
        public string IdPersona { get; set; }
        public string Ccodana { get; set; }

        public ICollection<Cases> Cases { get; set; }
        public ICollection<CasesDetails> CasesDetails { get; set; }
        //public virtual CasesDetails CasesDetails { get; set; }
        public ICollection<SegSistemaUsuario> SegSistemaUsuario { get; set; }
    }
}
