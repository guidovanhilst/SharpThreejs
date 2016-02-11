using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THREE
{
    [ExternalAttribute]
    public class Light : Object3D
    {



        public double shadowCameraFov;
        public double shadowBias;
        public double shadowDarkness;
        public int shadowMapWidth;
        public int shadowMapHeight;
        public double shadowCameraNear;
        public double shadowCameraFar;
        public Color color;


        public double shadowCameraLeft;
        public double shadowCameraRight;
        public double shadowCameraTop;
        public double shadowCameraBottom;
        public bool shadowCameraVisible;

    }

    [ExternalAttribute]
    public class DirectionalLight : Light
    {


        //hex -- Numeric value of the RGB component of the color. 
        //intensity -- Numeric value of the light's strength/intensity.
        public DirectionalLight(int hexColor)
        {

        }

        public DirectionalLight(int hexColor, double intensity)
        {

        }

        public DirectionalLight()
        {
            // TODO: Complete member initialization
        }
    }


    [ExternalAttribute]
    public class AmbientLight : Light
    {

        public AmbientLight() { }
        public AmbientLight(int p)
        {

        }

        public AmbientLight(Color c)
        {

        }


    }



    [ExternalAttribute]
    public class SpotLight : Light
    {
        private int p;



        public SpotLight(int p1, double p2)
        {

        }

        public SpotLight()
        {
            // TODO: Complete member initialization
        }

        public SpotLight(int p)
        {
            // TODO: Complete member initialization
            this.p = p;
        }


    }

    [ExternalAttribute]
    public class PointLight : Light
    {

        //hex — Numeric value of the RGB component of the color. 
        //intensity — Numeric value of the light's strength/intensity. 
        //distance -- The distance of the light where the intensity is 0. When distance is 0, then the distance is endless. 
        //decay -- The amount the light dims along the distance of the light.
        public PointLight(int hex)
        {

        }

        public PointLight(int hex, double intensity)
        {

        }

        public PointLight()
        {
            // TODO: Complete member initialization
        }
    }




}
