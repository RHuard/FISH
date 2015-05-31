using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIFish {
    public class FishData {

        private int _lm;
        private List<Coordinate> _points;
        private string _image;
        private int _id;
        private double _scale;
        private string _name;

        public FishData() {

            _lm = 0;
            _points = new List<Coordinate>();
            _image = String.Empty;
            _id = 0;
            _scale = 0.0;
        }

        public int LM {
            get {return _lm;}
            set {_lm = value; }
        }

        public List<Coordinate> Points{
            get{return _points;}
        }

        public string Image {
            get {return _image; }
            set {_image = value; }
        }

        public int Id {
            get {return _id; }
            set {_id = value; }
        }

        public double Scale {
            get {return _scale; }
            set {_scale = value; }
        }

        public void AddPoint(double x, double y) { 
        
            _points.Add(new Coordinate(x, y));
        }

        public void printData() { 
        
            Console.WriteLine("LM = " + _lm.ToString());
            Console.WriteLine("Points: ");
            for(int i = 0; i < _points.Count; i++) { 
                Console.WriteLine("X: " + _points[i].X.ToString() + " Y: " + _points[i].Y.ToString());
            }
            Console.WriteLine("image: " + _image.ToString());
            Console.WriteLine("ID: " + _id.ToString());
            Console.WriteLine("Scale: " + _scale.ToString());
        }

        public string Name {
            get {return _name; }
            set {_name = value; }
        }

    }

    public class Coordinate{
    
        private double _x;
        private double _y;

        public Coordinate(double x, double y){
        
            _x = x;
            _y = y;
        }

        public double X{
           get{return _x;}
           set{_x = value;}
        }

        public double Y{
           get{return _y;}
           set{_y = value;}
        }
    }
}
