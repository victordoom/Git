using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.UserRol
{
    public class UserRol
    {
        private static List<string> rol = new List<string>();

        public UserRol()
        {

        }
        public UserRol(List<string> roles): this()
        {
            rol = roles;
        }

        public List<string> Rol { get => rol; set => rol = value; }

        public List<string> GetRoles()
        {
            return this.Rol;
        }
    }
}
