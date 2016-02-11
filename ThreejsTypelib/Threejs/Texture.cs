using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THREE
{
    [ExternalAttribute]
    public class Texture
    {
        public double anisotropy;
        public MappingMode mapping;
        public WrapType wrapS;
        public WrapType wrapT;
        public Vector2 repeat;
    }


      [ExternalAttribute]
    public class Progress
    {
          public int loaded;
          public int total;
     }


     [ExternalAttribute]
    public class TextureLoader
    {
         public delegate void OnLoadTexture(Texture t);
         public delegate void OnProgress(Progress t);

        public Texture load(string resource,OnLoadTexture onLoad,OnProgress progress=null, OnProgress error=null)
        {
            throw new NotImplementedException();
        }
    }
}
