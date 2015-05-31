//Ryan Huard
//V 1.0.1

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JpegToTPS {
    class Program {
        static void Main(string[] args) {

            Console.WriteLine("JPG to TPS Automated TOOL v 1.1.0");
            Console.WriteLine("	Ryan Huard\n");

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.JPG");

            foreach(string file in files) { 
            
                string []parts = file.Split('\\');

                string name = parts[parts.Count() - 1];

                parts = name.Split('_');

                string tps_name = parts[1];

                parts = tps_name.Split('.');

                tps_name = parts[0];

                tps_name = tps_name + ".TPS";

                using(FileStream fs = new FileStream(tps_name, FileMode.OpenOrCreate))
                using(StreamWriter sw = new StreamWriter(fs)) { 
                
                    Console.WriteLine("creating: " + tps_name);
                    sw.WriteLine("LM=0");
                    sw.WriteLine("IMAGE=" + name);
                }    
            }
            Console.WriteLine("Done. Press any key to continue");
            Console.ReadKey();
        }
    }
}
