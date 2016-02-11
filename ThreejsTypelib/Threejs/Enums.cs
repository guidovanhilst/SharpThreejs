using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace THREE
{
    /// <summary>
    /// Default is THREE.PCFShadowMap.
    /// </summary>
    public enum MapType
    {
        BasicShadowMap = 0,
        PCFShadowMap = 1,
        PCFSoftShadowMap = 2

    }

    /// <summary>
    /// // Wrapping modes
    /// </summary>
     public enum WrapType
    {
        RepeatWrapping = 1000,
        ClampToEdgeWrapping = 1001,
        MirroredRepeatWrapping = 1002
    }

   


    public enum MappingMode
    {
        UVMapping = 300,

        CubeReflectionMapping = 301,
        CubeRefractionMapping = 302,

        EquirectangularReflectionMapping = 303,
        EquirectangularRefractionMapping = 304,

        SphericalReflectionMapping = 305,

    }





    /// <summary>
    /// Default is THREE.SmoothShading.
    /// </summary>
    public enum ShadingType
    {
        FlatShading = 0,
        SmoothShading = 1,
    }



    public enum SideType
    {
        FrontSide = 0,
        BackSide = 1,
        DoubleSide = 2

    }

    public enum EquationType
    {
        AddEquation = 100,
        SubtractEquation = 101,
        ReverseSubtractEquation = 102,
        MinEquation = 103,
        MaxEquation = 104,

    }



    public enum BlendingSourceType
    {
        ZeroFactor = 200,
        OneFactor = 201,
        SrcColorFactor = 202,
        OneMinusSrcColorFactor = 203,
        SrcAlphaFactor = 204,
        OneMinusSrcAlphaFactor = 205,
        DstAlphaFactor = 206,
        OneMinusDstAlphaFactor = 207,
        DstColorFactor = 208,
        OneMinusDstColorFactor = 209,
        SrcAlphaSaturateFactor = 210,

    }





    public enum BlendingType
    {
        NoBlending = 0,
        NormalBlending = 1,
        AdditiveBlending = 2,
        SubtractiveBlending = 3,
        MultiplyBlending = 4,
        CustomBlending = 5


    }

}