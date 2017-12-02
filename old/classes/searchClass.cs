using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaCodex
{
    class searchClass
    {
        public searchClass()
        {
            babHigh     = true;
            babMiddle   = true;
            babLow      = true;

            saveFor = false;
            saveRef = false;
            saveWil = false;

            searchedName = "";
        }

        public bool babHigh { get; set; }
        public bool babMiddle { get; set; }
        public bool babLow { get; set; }

        public bool saveFor { get; set; }
        public bool saveRef { get; set; }
        public bool saveWil { get; set; }

        public string searchedName { get; set; }

    }
}
