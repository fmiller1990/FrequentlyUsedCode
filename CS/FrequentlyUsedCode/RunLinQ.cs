﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrequentlyUsedCode
{
    class RunLinQ
    {
        public static void MinimalExample() {
            var allNumbers = new List<int>() { 1, 2, 3, 4, 5 };
            var smallNumbers = allNumbers.Where(x => x < 3);
            var largeNumbers = from x in allNumbers where x > 3 select x;
        }
    }
}
