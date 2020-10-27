using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Utilities
{
    public static class CheckOrFormat
    {
        //This method checks for slink and formats it
        public static string FormatSlink(string slink, string titleOrName)
        {
            if (string.IsNullOrWhiteSpace(slink))
            {
                slink = titleOrName.Replace(" ", "-").ToLower();
            }
            else
            {
                slink = slink.Replace(" ", "-").ToLower();
            }
            return slink;
        }
    }
}