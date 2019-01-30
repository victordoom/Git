using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models
{
    public class SegSistemaUsuario
    {
        public int Id { get; set; }
        public int? IdUsuario { get; set; }
        public int? CodigoSistema { get; set; }
        public int? Alcance { get; set; }
        public int? CodigoPerfil { get; set; }

        public SegUsuarios IdUsuarioNavigation { get; set; }
    }
}
