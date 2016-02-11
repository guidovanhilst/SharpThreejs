using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THREE
{
    [ExternalAttribute]
    public static class ImageUtils
    {
        public static  Texture loadTexture(string p1,THREE.MappingMode m, object render)
        {
            return null;
        }

        public static Texture loadTexture(string p1, THREE.MappingMode m, Action action)
        {
            throw new NotImplementedException();
        }
    }
}
