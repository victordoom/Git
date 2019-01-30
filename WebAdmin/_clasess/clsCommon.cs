using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace WebAdmin._clasess
{
    public static class ExtensionMethods

    {
        //-------------< Class: ExtensionMethods >-------------
        public static string getUserId(this ClaimsPrincipal user)
        {
            //------------< getUserId(User) >------------
            //*returns the current UserID
            if (!user.Identity.IsAuthenticated)
                return null;

            ClaimsPrincipal currentUser = user;

            //< output >
           return currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            //</ output >
            //------------</ getUserId(User) >------------
        }
        //-------------</ Class: ExtensionMethods >-------------
    }
}
