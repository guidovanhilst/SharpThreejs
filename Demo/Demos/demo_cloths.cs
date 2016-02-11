using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreejsDemo
{

    public static class Shaders
    {
        public static string fragment = @"

           uniform sampler2D texture;
			varying vec2 vUV;

			vec4 pack_depth( const in float depth ) {

				const vec4 bit_shift = vec4( 256.0 * 256.0 * 256.0, 256.0 * 256.0, 256.0, 1.0 );
				const vec4 bit_mask  = vec4( 0.0, 1.0 / 256.0, 1.0 / 256.0, 1.0 / 256.0 );
				vec4 res = fract( depth * bit_shift );
				res -= res.xxyz * bit_mask;
				return res;

			}

			void main() {

				vec4 pixel = texture2D( texture, vUV );

				if ( pixel.a < 0.5 ) discard;

				gl_FragData[ 0 ] = pack_depth( gl_FragCoord.z );

			}";

        public static string vertex = @"

            varying vec2 vUV;

			void main() {

				vUV = 0.75 * uv;

				vec4 mvPosition = modelViewMatrix * vec4( position, 1.0 );

				gl_Position = projectionMatrix * mvPosition;

			}";

    }

    public class demo_cloths : ParticleBaseDemo
    {
        private Particles.Cloth cloth = null;
       
        public demo_cloths(string name, string category)
            : base(name, category) {  }


        public override void Init()
        {
            base.Init();

            cloth = Particles.Cloth.Create();

            MakeCamera();
            MakeLights();

            //make cloth mesh
            var loader = new THREE.TextureLoader();
            loader.load(@"./bridge.gif", this.MakeClothMesh);

           
            scene.add(cloth.ballConstrain.Mesh);
            scene.add(cloth.boxConstrain.Mesh);

            // ground
            loader = new THREE.TextureLoader();
            loader.load(@"./threejs/textures/terrain/backgrounddetailed6.jpg", this.MakeGroundPlane);


            MakePortal();
            CreateRenderer();
            CreateTrackball();

        }
       

        private void MakePortal()
        {
            // poles
            var poleGeo = new THREE.BoxGeometry(5, 375, 5);
            var poleMat = new THREE.MeshPhongMaterial(); // { color: 0xffffff, specular: 0x111111, shininess: 100 } );
            poleMat.color = new THREE.Color().setHex(0xffffff);
            poleMat.specular = new THREE.Color().setHex(0x111111);
            poleMat.shininess = 100;

            THREE.Mesh mesh = new THREE.Mesh(poleGeo, poleMat);
            mesh.position.x = -125;
            mesh.position.y = -62;
            mesh.receiveShadow = true;
            mesh.castShadow = true;
            scene.add(mesh);

            mesh = new THREE.Mesh(poleGeo, poleMat);
            mesh.position.x = 125;
            mesh.position.y = -62;
            mesh.receiveShadow = true;
            mesh.castShadow = true;
            scene.add(mesh);

            mesh = new THREE.Mesh(new THREE.BoxGeometry(255, 5, 5), poleMat);
            mesh.position.y = -250 + 750 / 2;
            mesh.position.x = 0;
            mesh.receiveShadow = true;
            mesh.castShadow = true;
            scene.add(mesh);

            var gg = new THREE.BoxGeometry(10, 10, 10);
            mesh = new THREE.Mesh(gg, poleMat);
            mesh.position.y = -250;
            mesh.position.x = 125;
            mesh.receiveShadow = true;
            mesh.castShadow = true;
            scene.add(mesh);

            mesh = new THREE.Mesh(gg, poleMat);
            mesh.position.y = -250;
            mesh.position.x = -125;
            mesh.receiveShadow = true;
            mesh.castShadow = true;
            scene.add(mesh);
        }

        private void MakeClothMesh(THREE.Texture t)
        {
            THREE.Texture clothTexture = t;

            clothTexture.wrapS = THREE.WrapType.MirroredRepeatWrapping;
            clothTexture.wrapT = THREE.WrapType.RepeatWrapping;

            clothTexture.repeat.y = -1;
            clothTexture.anisotropy = 16;


            THREE.MeshPhongMaterial clothMaterial = new THREE.MeshPhongMaterial();

            clothMaterial.specular = new THREE.Color().setHex(0x030303);
            //clothMaterial.color = new THREE.Color(1, 0.4, 0);
            clothMaterial.map = clothTexture;
            clothMaterial.side = THREE.SideType.DoubleSide;
            clothMaterial.alphaTest = 0.5;
            clothMaterial.wireframe = false;
            clothMaterial.wireframeLinewidth= 2;

            // cloth mesh
            THREE.Mesh clothMesh = new THREE.Mesh(cloth.Geometry, clothMaterial);
            clothMesh.position.set(0, 0, 0);
            clothMesh.castShadow = true;
            

            THREE.ShaderMaterial shaderMat = null; ;
    
            THREE.ShaderMaterialOptions o = new THREE.ShaderMaterialOptions()
            {
                texture = new THREE.Uniform() { type = "t", value = clothTexture }
            };

            shaderMat = new THREE.ShaderMaterial();

            shaderMat.side = THREE.SideType.DoubleSide;

            string vertexShader = Shaders.vertex;
            string fragmentShader = Shaders.fragment;
            shaderMat.vertexShader = vertexShader;
            shaderMat.fragmentShader = fragmentShader;

            clothMesh.customDepthMaterial = shaderMat;
            scene.add(clothMesh);
        }

        private void MakeGroundPlane(THREE.Texture t)
        {
            THREE.Texture groundTexture = t;
            groundTexture.wrapS = THREE.WrapType.RepeatWrapping;
            groundTexture.wrapT = THREE.WrapType.RepeatWrapping;
            groundTexture.repeat.set(25, 25);
            groundTexture.anisotropy = 16;

            var groundMaterial = new THREE.MeshPhongMaterial();
            groundMaterial.color = new THREE.Color().setHex(0xffffff);
            groundMaterial.specular = new THREE.Color().setHex(0x111111);
            groundMaterial.map = groundTexture; ;

            THREE.Mesh planeMesh = new THREE.Mesh(new THREE.PlaneBufferGeometry(20000, 20000), groundMaterial);
            planeMesh.position.y = -250;
            planeMesh.rotation.x = -Math.PI / 2;
            planeMesh.receiveShadow = true;
            scene.add(planeMesh);
        }

       


     


        public override void RequestFrame()
        {
            var time = DateTime.Now.Ticks;

            cloth.simulate(time);

            base.RequestFrame();
        }

    }
}
