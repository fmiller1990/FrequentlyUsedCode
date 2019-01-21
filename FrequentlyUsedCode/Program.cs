using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace FrequentlyUsedCode
{
    class Program
    {
        static void Main(string[] args) {
            var allNumbers = new List<int>() {1,2,3,4,5 };
            var smallNumbers = allNumbers.Where(x => x<3);
            var largeNumbers = from x in allNumbers where x > 3 select x;

            foreach (var smallNumber in smallNumbers) {

            }
        }
    }
}
