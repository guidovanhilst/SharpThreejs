using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace THREE
{
    [ExternalAttribute]
    public class Color
    {
        public static implicit operator Color(int d) { throw new NotImplementedException(); }
        public static implicit operator int(Color c) { throw new NotImplementedException(); }

        public static implicit operator Color(string d) { throw new NotImplementedException(); }
        public static implicit operator string(Color c) { throw new NotImplementedException(); }

        public static implicit operator Color(double d) { throw new NotImplementedException(); }
        public static implicit operator double(Color c) { throw new NotImplementedException(); }


        public double r;
        public double g;
        public double b;


        /// <summary>
        /// var color = new THREE.Color();
        /// </summary>
        public Color() { }

        /// <summary>
        /// var color = new THREE.Color("rgb(255, 0, 0)");
        /// var color = new THREE.Color("rgb(100%, 0%, 0%)");
        /// var color = new THREE.Color("hsl(0, 100%, 50%)");
        /// </summary>
        /// <param name="s"></param>
        public Color(string s) { }


        /// <summary>
        /// var color = new THREE.Color( 0xff0000 );
        /// </summary>
        /// <param name="hex"></param>
        public Color(int hex) { }


        /// <summary>
        /// var color = new THREE.Color( 1, 0, 0 );
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public Color(double r, double g, double b)
        {

        }



        public Color setHex(int hex) { throw new NotImplementedException(); }
        public Color setHex(double hex) { throw new NotImplementedException(); }
        public Color setStyle(string style) { throw new NotImplementedException(); }
        public Color copy(Color org) { throw new NotImplementedException(); }
        public Color setRGB(double r, double g, double b) { throw new NotImplementedException(); }
        public Color setHSL(double h, double s, double l) { throw new NotImplementedException(); }
        public Color add(Color c) { throw new NotImplementedException(); }
        public Color addColors(Color c1, Color c2) { throw new NotImplementedException(); }
        public Color addScalar(double s) { throw new NotImplementedException(); }
        public Color multiply(Color s) { throw new NotImplementedException(); }
        public Color multiplyScalar(Color s) { throw new NotImplementedException(); }
    }
}
