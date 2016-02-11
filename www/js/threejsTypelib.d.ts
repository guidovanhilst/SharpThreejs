/// <reference path="./bridge.d.ts" />

declare module THREE {
    export enum BlendingSourceType {
        zeroFactor = 200,
        oneFactor = 201,
        srcColorFactor = 202,
        oneMinusSrcColorFactor = 203,
        srcAlphaFactor = 204,
        oneMinusSrcAlphaFactor = 205,
        dstAlphaFactor = 206,
        oneMinusDstAlphaFactor = 207,
        dstColorFactor = 208,
        oneMinusDstColorFactor = 209,
        srcAlphaSaturateFactor = 210
    }

    export enum BlendingType {
        noBlending = 0,
        normalBlending = 1,
        additiveBlending = 2,
        subtractiveBlending = 3,
        multiplyBlending = 4,
        customBlending = 5
    }

    export enum EquationType {
        addEquation = 100,
        subtractEquation = 101,
        reverseSubtractEquation = 102,
        minEquation = 103,
        maxEquation = 104
    }

    export enum MappingMode {
        uVMapping = 300,
        cubeReflectionMapping = 301,
        cubeRefractionMapping = 302,
        equirectangularReflectionMapping = 303,
        equirectangularRefractionMapping = 304,
        sphericalReflectionMapping = 305
    }

    /** @namespace THREE */
    
    /**
     * Default is THREE.PCFShadowMap.
     *
     * @public
     * @class THREE.MapType
     */
    export enum MapType {
        basicShadowMap = 0,
        pCFShadowMap = 1,
        pCFSoftShadowMap = 2
    }

    /**
     * Default is THREE.SmoothShading.
     *
     * @public
     * @class THREE.ShadingType
     */
    export enum ShadingType {
        flatShading = 0,
        smoothShading = 1
    }

    export enum SideType {
        frontSide = 0,
        backSide = 1,
        doubleSide = 2
    }

    /**
     * // Wrapping modes
     *
     * @public
     * @class THREE.WrapType
     */
    export enum WrapType {
        repeatWrapping = 1000,
        clampToEdgeWrapping = 1001,
        mirroredRepeatWrapping = 1002
    }
}
