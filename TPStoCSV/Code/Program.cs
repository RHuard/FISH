//Ryan Huard
//v 1.1.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FishTPStoCSV1 {
    class Program {

        private static CSVWriter _writer;
        private static List<FishData> _data;

        static void Main(string[] args) {

            Console.WriteLine("TPS to CSV Automated TOOL v 1.1.0");
            Console.WriteLine("	Ryan Huard\n");

            int n = getNumPoints();

            string outfile = getOutFileName();

            _data = new List<FishData>();

            _writer = new CSVWriter(outfile);

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.TPS");

            Console.WriteLine("\nGrabbing Data From Files Now");
            Console.WriteLine("================================================================================");

            for(int i = 0; i < files.Count(); i++) {

                try {
                    System.IO.StreamReader file = new System.IO.StreamReader(files[i]);
                    GetData(file, files[i], n);
                } catch {

                    Console.WriteLine("Could not get Data from: " + files[i]);
                }
            }
            Console.WriteLine("\nGrabbing Data Done! Any files listed above should not be in the csv file");

            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("Writing to CSV File");
            Console.WriteLine("================================================================================");
            WriteData(n);
            Console.WriteLine("\nDone writing to CSV File! Any errors listed above may have caused corruption");
            Console.WriteLine("in the csv. It is recommended you resolve those errors before using it");

            Console.WriteLine();
            Console.WriteLine("Checking Standard Deviations");
            int num_dev = getNumberStdDeviations();
            checkSTDDeveation(num_dev);

            Console.WriteLine();
            Console.WriteLine("Done. Press any key to continue");
            Console.ReadKey();
        }

        static public int getNumPoints() {

            int n;
            while(true) {
                Console.Write("Please enter number of points for this batch [15 is default]:");
                string num = Console.ReadLine();
                if(num == String.Empty) {

                    return 15;
                }

                try {

                    n = Convert.ToInt32(num);
                    return n;
                } catch {
                    Console.WriteLine("sorry,could not convert input into a integer, please try again");
                    continue;
                }

            }
        }

        static public string getOutFileName() {

            string outfile;
            while(true) {
                Console.Write("output name: ");
                outfile = Console.ReadLine();
                if(String.Empty == outfile) {
                    Console.WriteLine("You need to put in a valid file name, please try again");
                    continue;
                } else {
                    outfile = outfile + ".csv";
                    return outfile;
                }
            }
        }

        static public void GetData(System.IO.StreamReader file, string filename, int n) {

            string line;

            FishData fd = new FishData();

            string[] parts = filename.Split('\\');
            parts = parts[parts.Count() - 1].Split('.');

            fd.Name = parts[0];

            line = file.ReadLine();
            if(null == line) {
                Console.WriteLine("could not get the LM from: " + fd.Name);
                return;
            }
            parts = line.Split('=');

            try {
                fd.LM = Convert.ToInt32(parts[1]);
            } catch {

                Console.WriteLine("could not get the LM from: " + fd.Name);
                return;
            }

            if(fd.LM != n) {

                Console.WriteLine(fd.Name + " does not have the correct LM");
                return;
            }

            for(int i = 0; i < fd.LM; i++) {

                line = file.ReadLine();
                if(null == line) {
                    Console.WriteLine("could not get the point " + i.ToString() + " from: " + fd.Name);
                    return;
                }
                parts = line.Split(' ');
                try {
                    fd.AddPoint(Convert.ToDouble(parts[0]), Convert.ToDouble(parts[1]));
                } catch {
                    Console.WriteLine("could not get the point " + i.ToString() + " from: " + fd.Name);
                    return;
                }
            }

            line = file.ReadLine();
            parts = line.Split('=');
            fd.Image = parts[1];

            line = file.ReadLine();
            if(null == line) {
                Console.WriteLine("could not get the ID from:  " + fd.Name);
                return;
            }
            parts = line.Split('=');
            try {
                fd.Id = Convert.ToInt32(parts[1]);
            } catch {

                Console.WriteLine("could not get the ID from: " + fd.Name);
                return;
            }

            line = file.ReadLine();
            if(null == line) {
                Console.WriteLine("could not get the scale from: " + fd.Name);
                return;
            }
            parts = line.Split('=');
            try {
                fd.Scale = Convert.ToDouble(parts[1]);
            } catch {
                Console.WriteLine("could not get the scale from: " + fd.Name);
                return;
            }

            _data.Add(fd);
        }

        static public void WriteData(int num) {

            for(int i = 0; i < _data.Count; i++) {//names 

                try {
                    _writer.addToCurrent(_data[i].Name);
                    _writer.addToCurrent("");
                } catch {

                    Console.WriteLine("Error Writing the name: " + _data[i].Name);
                }
            }
            _writer.WriteLine();

            for(int i = 0; i < _data.Count; i++) {//X Y label

                try {
                    _writer.addToCurrent("X");
                    _writer.addToCurrent("Y");
                } catch {
                    Console.WriteLine("Error with File: " + _data[i].Name);
                }

            }
            _writer.WriteLine();
            for(int n = 0; n < num; n++) {
                for(int i = 0; i < _data.Count; i++) {//Coordinates
                    try {
                        _writer.addToCurrent(_data[i].Points[n].X.ToString());
                        _writer.addToCurrent(_data[i].Points[n].Y.ToString());

                    } catch {
                        Console.WriteLine("error, could not write number " + n.ToString() + " coordinates for: " + _data[i].Name);
                    }

                }
                _writer.WriteLine();
            }

            for(int i = 0; i < _data.Count; i++) {

                try {
                    _writer.addToCurrent("Scale: ");
                    _writer.addToCurrent(_data[i].Scale.ToString());
                } catch {

                    Console.WriteLine("could not write scale for: " + _data[i].Name);
                }

            }
            _writer.WriteLine();

        }

        static public int getNumberStdDeviations() { 
                 
            int n;
            while(true) {
                Console.Write("please enter a number of standard deviations to check against [default is 3]: "); ;
                string num = Console.ReadLine();
                if(num == String.Empty) {

                    return 3;
                }

                try {

                    n = Convert.ToInt32(num);
                    return n;
                } catch {
                    Console.WriteLine("sorry,could not convert input into a integer, please try again");
                    continue;
                }

            }
        }

        static public void checkSTDDeveation(int num) {

            double mean = 0.0;

            foreach(FishData fd in _data) { 
            
                mean += fd.Scale;
            }

            mean /= _data.Count;

            Console.WriteLine("mean scale: " + mean.ToString());

            double std_dev = 0.0;

            foreach(FishData fd in _data) { 
            
                std_dev += Math.Pow((fd.Scale - mean), 2);
            }

            std_dev /= (_data.Count - 1);

            std_dev = Math.Sqrt(Math.Abs(std_dev));

            Console.WriteLine("Standard Deviation: " + std_dev.ToString());

            Console.WriteLine("any files that are off by " + num.ToString() + " standard deviations will show up below:");
            //check if number is more than 3 std_devs from the mean
            foreach(FishData fd in _data) {

                if((num * std_dev) < Math.Abs(fd.Scale - mean)) { 
                
                    Console.WriteLine(fd.Name);
                }
            }
        }

    }
}
