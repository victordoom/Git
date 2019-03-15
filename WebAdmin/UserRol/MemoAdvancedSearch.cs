using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.UserRol
{
    public class MemoAdvancedSearch
    {
        public static int Luser { get; set; }
        public static int Lcate { get; set; }
        public static int Lhow { get; set; }
        public static int Lstatus { get; set; }
        public static string Lrating { get; set; }
        

        public MemoAdvancedSearch()
        {

        }

        public MemoAdvancedSearch(int user, int cate, int how, int sta, string ra) : this()
        {
            Luser = user;
            Lcate = cate;
            Lhow = how;
            Lstatus = sta;
            Lrating = ra;
        }


        public  int LBusuarios { get => Luser; set => Luser = value; }
    }
}
