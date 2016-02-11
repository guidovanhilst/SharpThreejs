using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THREE
{
    [ExternalAttribute]
    public class Scene : Object3D
    {
        /// <summary>
        /// Scenes allow you to set up what and where is to be rendered by three.js. 
        /// This is where you place objects, lights and cameras.
        /// </summary>
        public Scene() { }

        /// <summary>
        /// A fog instance defining the type of fog that affects everything rendered in the scene. Default is null.
        /// </summary>
        public Fog fog;

        /// <summary>
        /// If not null, it will force everything in the scene to be rendered with that material. Default is null.
        /// </summary>
        public Material overrideMaterial;

        /// <summary>
        /// Default is true. 
        /// If set, then the renderer checks every frame if the scene and its objects needs matrix updates.
        /// When it isn't, then you have to maintain all matrices in the scene yourself.
        /// </summary>
        public bool autoUpdate;

    }
}
