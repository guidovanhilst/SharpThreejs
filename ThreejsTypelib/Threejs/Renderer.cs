using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace THREE
{
    [ExternalAttribute]
    public class Renderer
    {
        public Bridge.Html5.Element domElement;
        public bool antialias;
        public bool sortObjects;
        public bool shadowMapEnabled;
        public MapType shadowMapType;
        public bool gammaInput;
        public bool gammaOutput;

        public void setClearColor(Color p)
        {
            throw new NotImplementedException();
        }

        public void setSize(double width, double height)
        {
            throw new NotImplementedException();
        }



        public void setClearColor(string p)
        {
            throw new NotImplementedException();
        }

        public void render(Scene scene, Camera camera)
        {

        }



        public double getMaxAnisotropy()
        {
            throw new NotImplementedException();
        }
    }


    [ExternalAttribute]
    public class WebGLRenderer : Renderer
    {


        public WebGLRenderer(bool antialias)
        {

        }

        public WebGLRenderer()
        {
            // TODO: Complete member initialization
        }
    }


    [ExternalAttribute]
    public class CanvasRenderer : Renderer
    {

    }

}
