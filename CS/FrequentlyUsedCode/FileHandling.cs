using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrequentlyUsedCode
{
    class FileHandling
    {
       public List<string> GetAllFilesinSubdirectories (string directory) {
            List<string> allPaths = new List<string>();
            if (!System.IO.Directory.Exists(directory)){
                throw new Exception("Directory does not exist");
            }

            allPaths =  System.IO.Directory.GetFiles(directory, "*.txt", System.IO.SearchOption.AllDirectories).ToList();

            return allPaths;
        }
    }
}
