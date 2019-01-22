using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrequentlyUsedCode
{
    // read write text file textfile streamreader streamwriter stream
    class ReadWriteTextFile
    {
        public void ReadFile( string path) {
            using (System.IO.StreamReader sr = new System.IO.StreamReader(path)) {
                if (!System.IO.File.Exists(path)) {
                    //TODO Error handling
                    //File does not exist
                }
                while (sr.Peek() >= 0) {
                    string line = sr.ReadLine();
                    //TODO Logic
                }
            }
        }


    }
}
