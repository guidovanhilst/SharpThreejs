using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THREE
{
    [ExternalAttribute]
    public class Material
    {
        /// <summary>
        /// Diffuse color of the material. Default is white.
        /// </summary>
        public Color color;


        public ShadingType shading;



        public Texture map;


        /// <summary>
        /// Float in the range of 0.0 - 1.0 indicating how transparent the material is. 
        /// A value of 0.0 indicates fully transparent, 1.0 is fully opaque. 
        /// If transparent is not set to true for the material, 
        /// the material will remain fully opaque and this value will only affect its color.
        /// Default is 1.0.
        /// </summary>
        public double opacity = 1.0;

        /// <summary>
        /// Defines whether this material is transparent. 
        /// This has an effect on rendering as transparent objects need special treatment and are rendered after non-transparent objects. 
        /// For a working example of this behaviour, check the WebGLRenderer code.
        /// Default is false.
        /// </summary>
        public bool transparent;


        /// <summary>
        /// Specifies that the material needs to be updated at the WebGL level. Set it to true if you made changes that need to be reflected in WebGL.
        /// This property is automatically set to true when instancing a new material.
        /// </summary>
        public bool needsUpdate;

        /// <summary>
        /// Defines which of the face sides will be rendered - front, back or both.
        /// Default is THREE.FrontSide. Other options are THREE.BackSide and THREE.DoubleSide.
        /// </summary>
        public SideType side = SideType.FrontSide;


        /// <summary>
        /// Which blending to use when displaying objects with this material. 
        /// Default is NormalBlending. See the blending mode constants for all possible values.
        /// </summary>
        public BlendingType blending = BlendingType.NormalBlending;

        /// <summary>
        /// Blending source. It's one of the blending mode constants defined in Three.js. 
        /// Default is SrcAlphaFactor. See the destination factors constants for all possible values.
        /// </summary>
        public BlendingSourceType blendSrc= BlendingSourceType.SrcAlphaFactor;

        /// <summary>
        /// Blending destination. 
        /// It's one of the blending mode constants defined in Three.js. Default is OneMinusSrcAlphaFactor.
        /// </summary>
        public BlendingSourceType blendDst;

        /// <summary>
        /// Blending equation to use when applying blending. 
        /// It's one of the constants defined in Three.js. Default is AddEquation.
        /// </summary>
        public EquationType blendEquation= EquationType.AddEquation;


        /// <summary>
        /// Whether rendering this material has any effect on the depth buffer. 
        /// Default is true.
        /// When drawing 2D overlays it can be useful to disable the depth writing in order to layer several things together without creating z-index artifacts.
        /// </summary>
        public bool depthWrite;

        /// <summary>
        /// Whether to use polygon offset. Default is false. This corresponds to the POLYGON_OFFSET_FILL WebGL feature.
        /// </summary>
        public bool polygonOffset;


        /// <summary>
        /// Sets the polygon offset factor. Default is 0.
        /// </summary>
        public double polygonOffsetFactor;

        /// <summary>
        /// Sets the polygon offset units. Default is 0.
        /// </summary>
        public double polygonOffsetUnits;

        /// <summary>
        /// Sets the alpha value to be used when running an alpha test. Default is 0.
        /// </summary>
        public double alphaTest;


        /// <summary>
        /// Whether to have depth test enabled when rendering this material. 
        /// Default is true.
        /// </summary>
        public bool depthTest;

        /// <summary>
        /// Defines whether this material is visible. 
        /// Default is true.
        /// </summary>
        public bool visible = true;

        /// <summary>
        /// Amount of triangle expansion at draw time. 
        /// This is a workaround for cases when gaps appear between triangles when using CanvasRenderer. 0.5 tends to give good results across browsers. Default is 0.
        /// </summary>
        public double overdraw;

        /// <summary>
        /// Whether to have depth test enabled when rendering this material. Default is true.
        /// </summary>
        public bool wireframe = false;

        /// <summary>
        /// Line thickness for wireframe mode. Default is 1.0.
        /// Due to limitations in the ANGLE layer, on Windows platforms linewidth will always be 1 regardless of the set value.
        /// </summary>
        public double wireframeLinewidth = 1.0;




    }


    [ExternalAttribute]
    [ObjectLiteral]
    public class Uniform
    {
        public string type;
        public object value;
    }


    [ExternalAttribute]
    [ObjectLiteral]
    public class ShaderMaterialOptions
    {
        /// <summary>
        ///  texture:  { type: ""t"", value: clothTexture }
        /// </summary>
        public Uniform texture;

        /// <summary>
        /// time: { type: "f", value: 1.0 }
        /// </summary>
        public Uniform time;

        /// <summary>
        /// resolution: { type: "v2", value: new THREE.Vector2() }
        /// </summary>
        public Uniform resolution;


        /// <summary>
        /// vertexOpacity: { type: 'f', value: [] }
        /// </summary>
        public Uniform vertexOpacity;



    }

    [ExternalAttribute]
    public class ShaderMaterial : Material
    {

        public ShaderMaterial(
            ShaderMaterialOptions uniforms=null, 
            ShaderMaterialOptions attributes=null, 
            string vertexShader=null, 
            string fragmentShader=null) 
        { 
        }

        public object uniforms;
        public string vertexShader;
        public string fragmentShader;
    }


    [ExternalAttribute]
    public class MeshPhongMaterial : Material
    {
        public bool metal;
        public double shininess;

       
       

        /// <summary>
        /// Emissive (light) color of the material, essentially a solid color unaffected by other lighting. Default is black.
        /// </summary>
        public Color emissive;
        public Color specular;





    }


    [ExternalAttribute]
    public class MeshBasicMaterial : Material
    {


    }



    [ExternalAttribute]
    public class MeshLambertMaterial : Material
    {


        public MeshLambertMaterial() { }
        public MeshLambertMaterial(int p1, int p2)
        {

        }

        public MeshLambertMaterial(Dictionary<string, string> options)
        {

        }
    }
}
