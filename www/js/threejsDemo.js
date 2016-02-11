/* global Bridge */

"use strict";

Bridge.define('Particles.Constrained', {
    p1: null,
    p2: null,
    restLength: 0,
    constructor: function (pa1, pa2) {
        this.p1 = pa1;
        this.p2 = pa2;

        this.restLength = this.p1.position.distanceTo(this.p2.position);
    },
    setPosition: function () {


        var diff = this.p2.position.clone().sub(this.p1.position);

        var correctionHalf = null;

        var currentLength = diff.length();
        var stiff = 0;

        if (currentLength === 0)
            return;


        var delta = currentLength - this.restLength;

        if (delta < 0)
            return;


        stiff = this.restLength / currentLength;
        var correction = diff.normalize().multiplyScalar(delta);

        var mid = this.p1.isFixed || this.p2.isFixed ? 1.0 : 0.5;

        correctionHalf = correction.multiplyScalar(mid);


        if (!this.p1.isFixed)
            this.p1.position.add(correctionHalf);

        if (!this.p2.isFixed)
            this.p2.position.sub(correctionHalf);

    },
    satisify: function () {

        var diff = Particles.DivVector.sub(this.p2.position, this.p1.position);
        var currentLength = diff.length();

        if (currentLength === 0)
            return; // prevents division by 0


        this.setPosition();

    }
});

Bridge.define('Particles.ConstrainedList', {
    config: {
        init: function () {
            this.items = Bridge.Array.init(0, null);
        }
    },
    append: function (o) {
        this.items.push(o);
    },
    applyConstrained: function (particles) {
        var $t;
        $t = Bridge.getEnumerator(this.items);
        while ($t.moveNext()) {
            var p = $t.getCurrent();
            p.applyConstrained(particles);
        }

    }    ,
    updateMesh: function () {
        var $t;
        $t = Bridge.getEnumerator(this.items);
        while ($t.moveNext()) {
            var p = $t.getCurrent();
            p.updateMesh();
        }

    }
});

Bridge.define('Particles.DivVector', {
    statics: {
        config: {
            init: function () {
                this.diff = new THREE.Vector3();
            }
        },
        sub: function (v1, v2) {
            Particles.DivVector.diff.subVectors(v1, v2);
            return Particles.DivVector.diff;
        }
    }
});

Bridge.define('Particles.GlobalForce', {
    apply: true,
    getForce: function () {
        throw new Bridge.NotImplementedException();
    }
});

Bridge.define('Particles.YGravity', {
    inherits: [Particles.GlobalForce],
    value: 800,
    gravity: null,
    constructor: function (v) {
        Particles.GlobalForce.prototype.$constructor.call(this);

        this.setValue(v);

    },
    setValue: function (v) {
        this.value = v;
        this.gravity = new THREE.Vector3(0, -this.value, 0);
    },
    getForce: function () {
        return this.gravity;

    }
});

Bridge.define('Particles.RandomWindForce', {
    inherits: [Particles.GlobalForce],
    windStrength: 0,
    config: {
        init: function () {
            this.windForce = new THREE.Vector3();
        }
    },
    getForce: function () {
        var time = new Date().getTime() * 10000;

        this.windStrength = Math.cos(time / 7000.0) * 200 + 30;

        var x = Math.sin(Bridge.Int.div(time, 2000));
        var y = 0; // Math.Cos(time / 3000);
        var z = Math.sin(Bridge.Int.div(time, 1000));

        this.windForce.set(x, y, z).normalize().multiplyScalar(this.windStrength);

        return this.windForce;
    }
});

/** @namespace Particles */

/**
 * Abstact ObjectConstrain
 *
 * @abstract
 * @public
 * @class Particles.ObjectConstrain
 */
Bridge.define('Particles.ObjectConstrain', {
    apply: true,
    mesh: null,
    applyConstrained: function (particles) {
        var $t;
        if (!this.apply)
            return;

        $t = Bridge.getEnumerator(particles);
        while ($t.moveNext()) {
            var particle = $t.getCurrent();
            var pos = particle.position;

            var disp = this.constrain(pos);

            if (disp !== null) {
                particle.position.add(disp);


            }
        }
    }
});

Bridge.define('Particles.BoxConstrain', {
    inherits: [Particles.ObjectConstrain],
    statics: {
        config: {
            init: function () {
                this.dirs = Bridge.merge(new Bridge.List$1(THREE.Vector3)(), [
    [new THREE.Vector3(1, 0, 0)],
    [new THREE.Vector3(-1, 0, 0)],
    [new THREE.Vector3(0, 1, 0)],
    [new THREE.Vector3(0, -1, 0)],
    [new THREE.Vector3(0, 0, 1)],
    [new THREE.Vector3(0, 0, -1)]
] );
            }
        },
        isZ: function (i) {
            return i >= 4 && i < 6;
        },
        isY: function (i) {
            return i >= 2 && i < 4;
        },
        isX: function (i) {
            return i >= 0 && i < 2;
        }
    },
    box: null,
    width: 60,
    height: 60,
    depth: 60,
    config: {
        init: function () {
            this.center = new THREE.Vector3();
        }
    },
    constructor: function (center, w, h, d) {
        Particles.ObjectConstrain.prototype.$constructor.call(this);

        this.center = center;
        this.width = w;
        this.height = h;
        this.depth = d;

        this.makeBox();
        this.makeMesh();
    },
    constrain: function (pos) {
        if (this.box.containsPoint(pos)) {
            var minDist = Number.MAX_VALUE;
            var dist = 0;
            var dir = 0;

            for (var i = 0; i < 6; i++) {
                if (Particles.BoxConstrain.isX(i)) {
                    dist = this.distance(pos.x, i);
                    if (Math.abs(dist) < Math.abs(minDist) && dist < 0) {
                        minDist = dist;
                        dir = i;
                    }
                }
                else 
                    if (Particles.BoxConstrain.isY(i)) {
                        dist = -this.distance(pos.y, i);
                        if (Math.abs(dist) < Math.abs(minDist) && dist < 0) {
                            minDist = dist;
                            dir = i;
                        }
                    }
                    else 
                        if (Particles.BoxConstrain.isZ(i)) {
                            dist = this.distance(pos.z, i);
                            if (Math.abs(dist) < Math.abs(minDist) && dist < 0) {
                                minDist = dist;
                                dir = i;
                            }
                        }
            }


            var d = Particles.BoxConstrain.dirs.getItem(dir).clone().multiplyScalar(minDist * 1.3);

            return d;

        }
        return null;
    },
    distance: function (value, index) {
        if (index === 0)
            return this.left() - value;
        if (index === 1)
            return value - this.right();

        if (index === 2)
            return this.bottom() - value;
        if (index === 3)
            return this.top() - value;

        if (index === 4)
            return this.front() - value;
        if (index === 5)
            return value - this.back();

        return -1;

    },
    left: function () {
        return this.center.x - this.width * 0.5;
    },
    right: function () {
        return this.center.x + this.width * 0.5;
    },
    bottom: function () {
        return this.center.y - this.height * 0.5;
    },
    top: function () {
        return this.center.y + this.height * 0.5;
    },
    front: function () {
        return this.center.z - this.depth * 0.5;
    },
    back: function () {
        return this.center.z + this.depth * 0.5;
    },
    makeBox: function () {
        var x = this.center.x - this.width * 0.5;
        var y = this.center.y - this.height * 0.5;
        var z = this.center.z - this.depth * 0.5;

        var min = new THREE.Vector3(x, y, z);

        x = this.center.x + this.width * 0.5;
        y = this.center.y + this.height * 0.5;
        z = this.center.z + this.depth * 0.5;

        var max = new THREE.Vector3(x, y, z);

        this.box = new THREE.Box3(min, max);

    },
    makeMesh: function () {
        var g = new THREE.BoxGeometry(this.width - 25, this.height - 25, this.depth - 25);

        var mat = new THREE.MeshPhongMaterial();
        mat.color = new THREE.Color().setHex(11184810);

        mat.transparent = false;
        mat.opacity = 0.5;
        this.mesh = new THREE.Mesh(g, mat);
        this.mesh.castShadow = true;
        this.mesh.receiveShadow = true;
        this.mesh.visible = this.apply;
        this.mesh.position.copy(this.center);

    },
    updateMesh: function () {
        this.mesh.visible = this.apply;
        this.mesh.position.copy(this.center);
        this.makeBox();
    }
});

/**
 * Concrete ball constrained
 *
 * @public
 * @class Particles.BallConstrain
 * @augments Particles.ObjectConstrain
 */
Bridge.define('Particles.BallConstrain', {
    inherits: [Particles.ObjectConstrain],
    center: null,
    radius: 0,
    constructor: function (c, ballRadius) {
        Particles.ObjectConstrain.prototype.$constructor.call(this);

        this.center = c;
        this.radius = ballRadius;
        this.makeMesh();
    },
    makeMesh: function () {
        var ballGeo = new THREE.SphereGeometry(this.radius - 10, 20, 20);
        var ballMaterial = new THREE.MeshPhongMaterial();
        ballMaterial.color = new THREE.Color().setHex(11184810);

        this.mesh = new THREE.Mesh(ballGeo, ballMaterial);
        this.mesh.castShadow = true;
        this.mesh.receiveShadow = true;
        this.mesh.visible = this.apply;
    },
    constrain: function (pos) {
        var diff = Particles.DivVector.sub(pos, this.center).clone();
        if (diff.length() < this.radius) {
            var dist = this.radius - diff.length();

            diff.normalize().multiplyScalar(dist);
            return diff;
        }
        return null;
    },
    changeRadius: function (r) {
        this.radius = r;
        this.mesh.geometry = new THREE.SphereGeometry(this.radius - 10, 20, 20);

        this.mesh.geometry.computeFaceNormals();
        this.mesh.geometry.computeVertexNormals();

        this.mesh.geometry.normalsNeedUpdate = true;
        this.mesh.geometry.verticesNeedUpdate = true;


        this.mesh.updateMatrix();
    },
    updateMesh: function () {

        this.mesh.position.copy(this.center);
        this.mesh.visible = this.apply;
    }
});

Bridge.define('Particles.Particle', {
    position: null,
    previous: null,
    original: null,
    acc: null,
    mass: 0,
    invMass: 0,
    isFixed: false,
    constructor: function (m, pos) {
        this.position = pos.clone();
        this.previous = pos.clone();
        this.original = pos.clone();



        this.acc = new THREE.Vector3(0, 0, 0);

        this.mass = m;
        this.invMass = 1.0 / this.mass;

    },
    integrate: function (deltaT) {

        if (this.isFixed) {
            this.toOriginal();
            return;
        }

        this.acc.multiplyScalar(Particles.ParticleConstants.dAMPING);

        var newPos = this.position.clone().sub(this.previous);
        newPos.add(this.acc.multiplyScalar(deltaT * deltaT));
        newPos.add(this.position);


        this.previous = this.position.clone();
        this.position = newPos;


        this.acc.set(0, 0, 0);



    },
    addForce: function (force) {
        var a = force.clone();
        a.multiplyScalar(this.invMass);
        this.acc.add(a);
    },
    toOriginal: function () {
        this.position.copy(this.original);
        this.previous.copy(this.original);

        this.acc.set(0, 0, 0);
    }
});

Bridge.define('Particles.ParticleConstants', {
    statics: {
        sPEEDFACTOR: 1000,
        dAMPING: 0.3,
        mASS: 0.1,
        config: {
            init: function () {
                this.tIMESTEP = Particles.ParticleConstants.sPEEDFACTOR / 60.0;
                this.tIMESQR = Particles.ParticleConstants.tIMESTEP * Particles.ParticleConstants.tIMESTEP;
            }
        }
    }
});

Bridge.define('Particles.ParticleSystem', {
    particles: null,
    constrains: null,
    globalForces: null,
    objectconstraines: null
});

/**
 * @abstract
 * @public
 * @class Particles.ParametricSurface
 * @augments Particles.ParticleSystem
 */
Bridge.define('Particles.ParametricSurface', {
    inherits: [Particles.ParticleSystem],
    geometry: null,
    nrU: 50,
    nrV: 50,
    constructor: function () {
        Particles.ParticleSystem.prototype.$constructor.call(this);


        this.objectconstraines = new Particles.ConstrainedList();
        this.globalForces = Bridge.Array.init(0, null);
    },
    simulate: function (time) {
        var $t, $t1, $t2, $t3;
        if (this.geometry === null)
            return;


        $t = Bridge.getEnumerator(this.globalForces);
        while ($t.moveNext()) {
            var f = $t.getCurrent();
            if (f.apply) {
                $t1 = Bridge.getEnumerator(this.particles);
                while ($t1.moveNext()) {
                    var particle = $t1.getCurrent();
                    particle.addForce(f.getForce());
                }
            }
        }


        this.objectconstraines.applyConstrained(this.particles);

        $t2 = Bridge.getEnumerator(this.particles);
        while ($t2.moveNext()) {
            var particle1 = $t2.getCurrent();
            particle1.integrate(Particles.ParticleConstants.tIMESTEP);

        }



        $t3 = Bridge.getEnumerator(this.constrains);
        while ($t3.moveNext()) {
            var constrain = $t3.getCurrent();
            constrain.satisify();
        }


        this.updateGeometry();



    }    ,
    index: function (u, v) {
        return (u + v * (this.nrU + 1));
    },
    makeConstrains: function () {


        var u;
        var v;
        this.constrains = Bridge.Array.init(0, null);

        // Structural
        for (v = 0; v < this.nrV; v++) {
            for (u = 0; u < this.nrU; u++) {
                this.constrains.push(new Particles.Constrained(this.particles[this.index(u, v)], this.particles[this.index(u, v + 1)]));
                this.constrains.push(new Particles.Constrained(this.particles[this.index(u, v)], this.particles[this.index(u + 1, v)]));
            }
        }


        for (v = 0; v < this.nrV; v++) {
            u = this.nrU;
            this.constrains.push(new Particles.Constrained(this.particles[this.index(u, v)], this.particles[this.index(u, v + 1)]));

        }

        for (u = 0; u < this.nrU; u++) {
            v = this.nrV;
            this.constrains.push(new Particles.Constrained(this.particles[this.index(u, v)], this.particles[this.index(u + 1, v)]));
        }
    },
    createParticles: function () {

        console.log("CreateParticles");
        var u;
        var v;
        this.particles = Bridge.Array.init(0, null);

        // Create particles
        for (v = 0; v <= this.nrV; v++) {
            for (u = 0; u <= this.nrU; u++) {
                var up = u / Bridge.cast(this.nrU, Number);
                var vp = v / Bridge.cast(this.nrV, Number);
                var pos = this.paramFunction(up, vp);
                var mass = Particles.ParticleConstants.mASS;
                this.particles.push(new Particles.Particle(mass, pos));
            }
        }
    },
    setFixed: function (p) {
        var $t;
        $t = Bridge.getEnumerator(p);
        while ($t.moveNext()) {
            var i = $t.getCurrent();
            this.particles[i].isFixed = true;
        }

    }    ,
    updateGeometry: function () {

        if (this.geometry === null)
            return;

        var p = this.particles;
        var il = p.length;

        if (this.geometry.vertices !== null) {

            for (var i = 0; i < il; i++) {
                this.geometry.vertices[i].copy(p[i].position);
            }
        }

        this.geometry.computeFaceNormals();
        this.geometry.computeVertexNormals();

        this.geometry.normalsNeedUpdate = true;
        this.geometry.verticesNeedUpdate = true;


    },
    reset: function () {
        var $t;
        $t = Bridge.getEnumerator(this.particles);
        while ($t.moveNext()) {
            var particle = $t.getCurrent();
            particle.toOriginal();
        }
    }    ,
    fixRange: function (i1, i2) {
        for (var i = i1; i <= i2; i++)
            this.particles[i].isFixed = true;

    }
});

/**
 * Cloth Simulation using a relaxed constrains solver
 *
 * @public
 * @class Particles.Cloth
 * @augments Particles.ParametricSurface
 */
Bridge.define('Particles.Cloth', {
    inherits: [Particles.ParametricSurface],
    statics: {
        uSegs: 25,
        vSegs: 25,
        create: function () {
            return new Particles.Cloth(Particles.Cloth.uSegs, Particles.Cloth.vSegs);
        }
    },
    windForce: null,
    gravityForce: null,
    ballConstrain: null,
    boxConstrain: null,
    width: 250,
    height: 250,
    constructor: function (w, h) {
        Particles.ParametricSurface.prototype.$constructor.call(this);


        this.nrU = w;
        this.nrV = h;


        this.ballConstrain = new Particles.BallConstrain(new THREE.Vector3(0, -30, 0), 55);
        this.ballConstrain.apply = false;
        this.objectconstraines.append(this.ballConstrain);


        this.boxConstrain = new Particles.BoxConstrain(new THREE.Vector3(0, -90, 0), 100.0, 100.0, 100.0);
        this.boxConstrain.apply = true;
        this.objectconstraines.append(this.boxConstrain);


        this.windForce = new Particles.RandomWindForce();
        this.windForce.apply = false;

        this.gravityForce = new Particles.YGravity(Particles.ParticleConstants.sPEEDFACTOR * 4E-05 / Particles.ParticleConstants.tIMESQR);

        this.globalForces.push(this.windForce);
        this.globalForces.push(this.gravityForce);


        this.createParticles();

        this.makeConstrains();


        //SetFixed(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,17,20);
        this.fixRange(0, this.nrU);
        //SetFixed(0, NrU / 2, NrU);
        // cloth geometry
        this.geometry = new THREE.ParametricGeometry(Bridge.fn.bind(this, this.paramFunction), this.nrU, this.nrV);
        this.geometry.dynamic = true;

    },
    paramFunction: function (u, v) {
        var x = (u - 0.5) * this.width;
        var y = (v + 0.5) * this.height;
        var z = 0;
        return new THREE.Vector3(x, y, z);
    },
    simulate: function (time) {
        Particles.ParametricSurface.prototype.simulate.call(this, time);


        this.ballConstrain.center.z = -Math.sin(new Date().getTime() * 10000 / 6000000.0) * 110; //+ 40;
        this.ballConstrain.center.x = Math.cos(new Date().getTime() * 10000 / 4000000.0) * 110;
        this.ballConstrain.updateMesh();

        this.boxConstrain.center.z = -Math.cos(new Date().getTime() * 10000 / 6000000.0) * 110; //+ 40;
        this.boxConstrain.center.x = Math.sin(new Date().getTime() * 10000 / 4000000.0) * 110;
        this.boxConstrain.updateMesh();


    }
});

Bridge.define('Particles.Carpet', {
    inherits: [Particles.ParametricSurface],
    windForce: null,
    gravityForce: null,
    ballConstrain: null,
    boxConstrain: null,
    config: {
        init: function () {
            this.uMin = -2 * Math.PI;
            this.uMax = 2 * Math.PI;
            this.vMin = -2 * Math.PI;
            this.vMax = 2 * Math.PI;
        }
    },
    constructor: function () {
        Particles.ParametricSurface.prototype.$constructor.call(this);


        this.nrU = 30;
        this.nrV = 30;



        this.ballConstrain = new Particles.BallConstrain(new THREE.Vector3(-220, -350, 0), 350);
        this.ballConstrain.apply = true;
        this.objectconstraines.append(this.ballConstrain);

        //ballConstrain = new BallConstrain(new THREE.Vector3(150, -80, 0), 80);
        //ballConstrain.Apply = true;
        //Allconstraines.Append(ballConstrain);

        this.boxConstrain = new Particles.BoxConstrain(new THREE.Vector3(180, -20, 0), 250.0, 250.0, 250);
        this.boxConstrain.apply = true;
        this.objectconstraines.append(this.boxConstrain);

        this.windForce = new Particles.RandomWindForce();
        this.windForce.apply = false;
        this.globalForces.push(this.windForce);


        this.gravityForce = new Particles.YGravity(Particles.ParticleConstants.sPEEDFACTOR * 4E-05 / Particles.ParticleConstants.tIMESQR);
        this.globalForces.push(this.gravityForce);


        this.createParticles();
        this.makeConstrains();

        this.geometry = new THREE.ParametricGeometry(Bridge.fn.bind(this, this.paramFunction), this.nrU, this.nrV);
        this.geometry.dynamic = true;
    },
    simulate: function (time) {
        if (this.geometry === null)
            return;




        //ballConstrain.ApplyConstrained(particles);
        // boxConstrain.Center.z = -Math.Cos(DateTime.Now.Ticks / 90000000.0) * 50; //+ 40;
        // boxConstrain.Center.x = Math.Sin(DateTime.Now.Ticks / 90000000.0) * 50;
        //boxConstrain.UpdateMesh();
        //boxConstrain.ApplyConstrained(particles);

        Particles.ParametricSurface.prototype.simulate.call(this, time);


        //Math.Random()
        this.ballConstrain.center.z = -Math.sin(new Date().getTime() * 10000 / 9000000.0) * 200; //+ 40;
        this.ballConstrain.center.x = Math.cos(new Date().getTime() * 10000 / 9000000.0) * 300;
        this.ballConstrain.updateMesh();


        if (this.particles[0].position.y < -800) {
            this.reset();
            var min = 0.7;
            var max = 1.3;

            var factor = min + Math.random() * (max - min);

            this.ballConstrain.changeRadius(this.ballConstrain.radius * factor);
            this.ballConstrain.updateMesh();
        }

    },
    paramFunction: function (u, v) {

        u = this.uMin + u * (this.uMax - this.uMin);
        v = this.vMin + v * (this.vMax - this.vMin);

        var x = u * 50;
        var z = v * 50;
        var y = 20 * Math.sin(u) * Math.cos(v) + 300;
        return new THREE.Vector3(x, y, z);
    }
});

Bridge.define('ThreejsDemo.PanelType', {
    statics: {
        panel_default: 0,
        panel_primary: 1,
        panel_success: 2,
        panel_info: 3,
        panel_warning: 4,
        panel_danger: 5
    },
    enum: true
});

Bridge.define('ThreejsDemo.BussyDlg', {
    statics: {
        make: function () {

            var s = "\r\n\t\t            <div class=\"modal fade\" data-backdrop=\"static\" data-keyboard=\"false\" tabindex=\"-1\" role=\"dialog\" aria-hidden=\"true\" style=\"padding-top:15%; overflow-y:visible;\"> \r\n\t\t                <div class=\"modal-dialog modal-m\"> \r\n\t\t                    <div class=\"modal-content\"> \r\n\t\t\t                    <div class=\"modal-header\"><h4 style=\"margin:0;\"></h4></div> \r\n\t\t\t                        <div class=\"modal-body\"> \r\n\t\t\t                        <div class=\"progress progress-striped active\" style=\"margin-bottom:0;\"><div class=\"progress-bar\" style=\"width: 100%\">\r\n                                    </div>\r\n                                    </div> \r\n\t\t                        </div> \r\n\t\t                    </div>\r\n                        </div>\r\n                    </div>";



            return $(s);


        }
    },
    bussy: null,
    /**
     * sm or lg
     *
     * @instance
     * @public
     * @memberof ThreejsDemo.BussyDlg
     * @default "sm"
     * @type string
     */
    dialogSize: "sm",
    progressType: "default",
    constructor: function () {
        this.bussy = ThreejsDemo.BussyDlg.make().appendTo(document.body);
    },
    show: function (title) {
        this.bussy.find("h4").html(title);
        this.bussy.find(".modal-dialog").attr("class", "modal-dialog").addClass("modal-" + this.dialogSize);
        this.bussy.find(".progress-bar").addClass("progress-bar-" + this.progressType);
        window.setTimeout(Bridge.fn.bind(this, this.doShow));

    },
    doShow: function () {
        this.bussy.modal("show");
    },
    doHide: function () {
        this.bussy.modal("hide");
    },
    hide: function (delay) {
        if (delay === void 0) { delay = 0; }
        window.setTimeout(Bridge.fn.bind(this, this.doHide), delay);
    }
});

Bridge.define('ThreejsDemo.ListMaker', {
    list: null,
    constructor: function () {
        this.list = this.makeList();
    },
    addListItem: function (text, clickHandler, tag) {

        var li = $("<li>").addClass("list-group-item").html(text);
        li.click(tag, clickHandler);
        this.list.append(li);

        return li;
    },
    makeList: function () {

        var ul = $("<ul>").addClass("list-group");
        return ul;


    }
});

Bridge.define('ThreejsDemo.GeometryFunction', {
    $function: null,
    name: null
});

Bridge.define('ThreejsDemo.ModalDlg', {
    statics: {
        modalDialog: null,
        show: function (titleText, bodyText) {

            var bodyContent = document.createElement('p');
            bodyContent.innerHTML = bodyText;

            ThreejsDemo.ModalDlg.showDlgWithContent(titleText, bodyContent);
        },
        showDlgWithContent: function (titleText, bodyContent) {
            ThreejsDemo.ModalDlg.ensureDialog(titleText);

            if (ThreejsDemo.ModalDlg.modalDialog.bodyContent !== null) {
                ThreejsDemo.ModalDlg.modalDialog.bodyContent.remove();
                ThreejsDemo.ModalDlg.modalDialog.bodyContent = null;
            }

            ThreejsDemo.ModalDlg.modalDialog.bodyContent = bodyContent;
            ThreejsDemo.ModalDlg.modalDialog.title.innerHTML = titleText;
            ThreejsDemo.ModalDlg.modalDialog.body.appendChild(bodyContent);

            ThreejsDemo.ModalDlg.modalDialog.show();
        },
        ensureDialog: function (titleText) {
            if (ThreejsDemo.ModalDlg.modalDialog === null)
                ThreejsDemo.ModalDlg.modalDialog = ThreejsDemo.ModalDlg.make(titleText, "content", "myModal");
        },
        onHidden: function () {
            if (ThreejsDemo.ModalDlg.modalDialog !== null) {
                ThreejsDemo.ModalDlg.modalDialog.remove();
                ThreejsDemo.ModalDlg.modalDialog = null;
            }
        },
        make: function (titleText, bodyText, id) {
            var myModal = document.createElement('div');
            myModal.className = "modal fade";
            myModal.id = id;
            myModal.setAttribute("role", "dialog");
            var jq = $(myModal).on("hidden.bs.modal", ThreejsDemo.ModalDlg.onHidden);


            var modalDlg = document.createElement('div');
            modalDlg.className = "modal-dialog-sm";
            myModal.appendChild(modalDlg);


            var modalContent = document.createElement('div');
            modalContent.className = "modal-content";
            modalDlg.appendChild(modalContent);

            var modalHeader = document.createElement('div');
            modalHeader.className = "modal-header";
            modalContent.appendChild(modalHeader);

            var button = document.createElement('button');
            button.setAttribute("type", "button");
            button.className = "close";
            button.setAttribute("data-dismiss", "modal");
            button.innerHTML = "&times;";
            modalHeader.appendChild(button);

            var title = document.createElement("h4");
            title.className = "modal-title";
            title.innerHTML = titleText;
            modalHeader.appendChild(title);


            var modalBody = document.createElement('div');
            modalBody.className = "modal-body";
            modalContent.appendChild(modalBody);

            var bodyContent = document.createElement('p');
            bodyContent.innerHTML = bodyText;
            modalBody.appendChild(bodyContent);

            var footer = document.createElement('div');
            footer.className = "modal-footer";


            var footerButton = document.createElement('button');
            footerButton.setAttribute("type", "button");
            footerButton.className = "btn btn-default";
            footerButton.setAttribute("data-dismiss", "modal");
            footerButton.innerHTML = "Close";

            footer.appendChild(footerButton);
            modalContent.appendChild(footer);

            document.body.appendChild(myModal);

            var dlg = new ThreejsDemo.ModalDlg();
            dlg.topElement = myModal;
            dlg.body = modalBody;
            dlg.title = title;
            dlg.bodyContent = bodyContent;
            dlg.footer = footer;
            dlg.id = id;
            return dlg;

        }
    },
    id: "myModal",
    title: null,
    bodyContent: null,
    footer: null,
    body: null,
    topElement: null,
    show: function () {
        var j = $("#" + this.id).modal("show");
    },
    hide: function () {
        var j = $("#" + this.id).modal("hide");
    },
    remove: function () {
        document.body.removeChild(this.topElement);
    }
});

Bridge.define('ThreejsDemo.Shaders', {
    statics: {
        fragment: "\r\n\r\n           uniform sampler2D texture;\r\n\t\t\tvarying vec2 vUV;\r\n\r\n\t\t\tvec4 pack_depth( const in float depth ) {\r\n\r\n\t\t\t\tconst vec4 bit_shift = vec4( 256.0 * 256.0 * 256.0, 256.0 * 256.0, 256.0, 1.0 );\r\n\t\t\t\tconst vec4 bit_mask  = vec4( 0.0, 1.0 / 256.0, 1.0 / 256.0, 1.0 / 256.0 );\r\n\t\t\t\tvec4 res = fract( depth * bit_shift );\r\n\t\t\t\tres -= res.xxyz * bit_mask;\r\n\t\t\t\treturn res;\r\n\r\n\t\t\t}\r\n\r\n\t\t\tvoid main() {\r\n\r\n\t\t\t\tvec4 pixel = texture2D( texture, vUV );\r\n\r\n\t\t\t\tif ( pixel.a < 0.5 ) discard;\r\n\r\n\t\t\t\tgl_FragData[ 0 ] = pack_depth( gl_FragCoord.z );\r\n\r\n\t\t\t}",
        vertex: "\r\n\r\n            varying vec2 vUV;\r\n\r\n\t\t\tvoid main() {\r\n\r\n\t\t\t\tvUV = 0.75 * uv;\r\n\r\n\t\t\t\tvec4 mvPosition = modelViewMatrix * vec4( position, 1.0 );\r\n\r\n\t\t\t\tgl_Position = projectionMatrix * mvPosition;\r\n\r\n\t\t\t}"
    }
});

Bridge.define('ThreejsDemo.AccordionPanel', {
    statics: {
        make: function (headerText, subtext, id, type) {
            var accor = new ThreejsDemo.AccordionPanel();
            accor.id = id;
            accor.pType = type;

            var c = $("<div>");
            if (!Bridge.String.isNullOrEmpty(headerText))
                c.append($("<h2>").html(headerText));

            if (!Bridge.String.isNullOrEmpty(subtext))
                c.append($("<h4>").html(subtext));

            accor.panelGroup = $("<div>").addClass("panel-group").attr("id", id);

            c.append(accor.panelGroup);


            accor.mainContainer = c;

            return accor;

        },
        getPanelTypeName: function (t) {
            return Bridge.String.format("panel {0}", Bridge.String.replaceAll(t.toString(), "_", "-"));
        }
    },
    pType: 0,
    id: null,
    panelGroup: null,
    mainContainer: null,
    panelCount: 0,
    addPanel: function (title, content, clickHandler, panelTag) {
        if (clickHandler === void 0) { clickHandler = null; }
        if (panelTag === void 0) { panelTag = null; }
        this.panelCount++;

        var className = ThreejsDemo.AccordionPanel.getPanelTypeName(this.pType);


        var panel = $("<div>").addClass(className);
        panel.append(this.makePanelTitle(title, "#panel_" + this.id + this.panelCount.toString(), "#" + this.id, clickHandler, panelTag));
        panel.append(this.makeContentPanel(content, "panel_" + this.id + this.panelCount.toString()));

        this.panelGroup.append(panel);

        return panel;
    },
    makePanelTitle: function (title, href, dataParent, clickHandler, panelTag) {
        var panelHeading = $("<div>").addClass("panel-heading");
        var panelTitle = $("<h4>").addClass("panel-title");
        var panelAcoordion = $("<a>").attr("data-toggle", "collapse").attr("data-parent", dataParent).attr("href", href).html(title);

        if (clickHandler !== null)
            panelAcoordion.click(panelTag, clickHandler);

        panelTitle.append(panelAcoordion);
        panelHeading.append(panelTitle);
        return panelHeading;
    },
    collapse: function (e) {
        window.alert("On Collapse");
    },
    makeContentPanel: function (content, id) {
        var panelCollapse = $("<div>").addClass("panel-collapse collapse").attr("id", id);

        if (content !== null) {
            var panelBody = $("<div>").addClass("panel-body");
            panelBody.append(content);
            panelCollapse.append(panelBody);
        }
        return panelCollapse;

    }
});

Bridge.define('ThreejsDemo.BaseDemo', {
    statics: {
        bussy: null
    },
    camera: null,
    renderer: null,
    scene: null,
    controls: null,
    isActive: false,
    demoName: null,
    demoCategory: null,
    container: null,
    width: 800,
    height: 800,
    constructor: function (name, category) {
        this.demoName = name;
        this.demoCategory = category;
        this.container = document.createElement('div');
        this.container.style.width = "100%";
        this.container.style.height = "100%";

    },
    show: function () {
        this.isActive = true;
        if (!this.isInit()) {
            this.doInit();
        }
        else  {
            this.updateRenderSize();
            this.requestFrame();
        }
    },
    hide: function () {
        this.isActive = false;
    },
    doInit: function () {
        if (ThreejsDemo.BaseDemo.bussy === null)
            ThreejsDemo.BaseDemo.bussy = new ThreejsDemo.BussyDlg();

        ThreejsDemo.BaseDemo.bussy.show("Loading scene: " + this.demoName);

        var doStart = Bridge.fn.bind(this, function () {
            this.init();
            ThreejsDemo.BaseDemo.bussy.hide();
            this.updateRenderSize();
            this.requestFrame();
        });

        window.setTimeout(doStart, 500);
    },
    isInit: function () {
        return this.renderer !== null;
    },
    init: function () {
        this.camera = new THREE.PerspectiveCamera(60, Bridge.Int.div(this.width, this.height), 1, 1000);
        this.camera.position.z = 500;
        window.addEventListener("resize", Bridge.fn.bind(this, this.onWindowResize), false);
    },
    requestFrame: function () {
        if (this.isActive) {
            this.render();
            window.requestAnimationFrame(Bridge.fn.bind(this, this.requestFrame));
        }
    },
    render: function () {
        if (this.isActive) {
            if (this.renderer !== null)
                this.renderer.render(this.scene, this.camera);

            if (this.controls !== null)
                this.controls.update();
        }
    },
    onWindowResize: function (arg) {
        this.updateRenderSize();
    },
    updateRenderSize: function () {
        if (!this.isActive)
            return;

        var w = this.width; // parent.ClientWidth;
        var h = this.height; // .ClientHeight;

        if (w === 0 || h === 0)
            return;

        this.camera.aspect = Bridge.Int.div(w, h);
        this.camera.updateProjectionMatrix();

        this.renderer.setSize(w, h);
    }
});

Bridge.define('ThreejsDemo.ParticleBaseDemo', {
    inherits: [ThreejsDemo.BaseDemo],
    constructor: function (name, category) {
        ThreejsDemo.BaseDemo.prototype.$constructor.call(this, name, category);

    },
    makeCamera: function () {
        this.scene = new THREE.Scene();
        this.scene.fog = new THREE.Fog(new THREE.Color().setHex(13426943), 500, 10000);

        // camera
        this.camera = new THREE.PerspectiveCamera(30, Bridge.Int.div(this.height, this.width), 1, 10000);
        this.camera.position.y = 1000;
        this.camera.position.z = 1500;
        this.scene.add(this.camera);
    },
    makeLights: function () {
        // lights
        var light;


        this.scene.add(new THREE.AmbientLight(6710886));

        light = new THREE.DirectionalLight(14674943, 1.75);
        light.position.set(50, 200, 100);
        light.position.multiplyScalar(1.3);

        light.castShadow = true;
        light.shadowCameraVisible = false;

        light.shadowMapWidth = 1024;
        light.shadowMapHeight = 1024;

        var lightBox = 300;

        light.shadowCameraLeft = -lightBox;
        light.shadowCameraRight = lightBox;
        light.shadowCameraTop = lightBox;
        light.shadowCameraBottom = -lightBox;

        light.shadowCameraFar = 1000;

        this.scene.add(light);
    },
    createRenderer: function () {
        this.renderer = new THREE.WebGLRenderer(true);
        this.renderer.setSize(this.width, this.height);
        this.renderer.setClearColor(this.scene.fog.color);
        this.container.appendChild(this.renderer.domElement);
        this.renderer.gammaInput = true;
        this.renderer.gammaOutput = true;
        this.renderer.shadowMapEnabled = true;
    },
    createTrackball: function () {
        this.controls = new THREE.TrackballControls(this.camera, this.renderer.domElement);
        this.controls.rotateSpeed = 4.0;
        this.controls.zoomSpeed = 1.2;
        this.controls.panSpeed = 0.8;
        this.controls.noZoom = false;
        this.controls.noPan = false;
        this.controls.staticMoving = true;
        this.controls.dynamicDampingFactor = 0.3;
    }
});

Bridge.define('ThreejsDemo.geometry_demo', {
    inherits: [ThreejsDemo.BaseDemo],
    functions: null,
    geometryIndex: 0,
    currentMesh: null,
    dropDownButton: null,
    constructor: function (name, category) {
        ThreejsDemo.BaseDemo.prototype.$constructor.call(this, name, category);

        this.geometryIndex = 0;
        this.currentMesh = null;
        this.functions = new Bridge.List$1(ThreejsDemo.GeometryFunction)();
    },
    init: function () {

        this.functions.add(Bridge.merge(new ThreejsDemo.GeometryFunction(), {
            $function: Bridge.fn.bind(this, this.makeSphere),
            name: "Sphere"
        } ));
        this.functions.add(Bridge.merge(new ThreejsDemo.GeometryFunction(), {
            $function: Bridge.fn.bind(this, this.makeBox),
            name: "Box"
        } ));
        this.functions.add(Bridge.merge(new ThreejsDemo.GeometryFunction(), {
            $function: Bridge.fn.bind(this, this.makeCilinder),
            name: "Cilinder"
        } ));

        this.makeComboBox();

        this.camera = new THREE.PerspectiveCamera(70, Bridge.Int.div(this.width, this.height), 1, 10000);
        this.camera.position.z = 500;
        this.scene = new THREE.Scene();

        this.createTrackballControl();
        this.createLights();
        this.createRenderer();
        this.showGeometry();

    },
    createTrackballControl: function () {
        this.controls = new THREE.TrackballControls(this.camera);
        this.controls.rotateSpeed = 3.0;
        this.controls.zoomSpeed = 1.2;
        this.controls.panSpeed = 0.8;
        this.controls.noZoom = false;
        this.controls.noPan = false;
        this.controls.staticMoving = true;
        this.controls.dynamicDampingFactor = 0.3;
    },
    createRenderer: function () {
        this.renderer = new THREE.WebGLRenderer(false);
        this.renderer.antialias = false;
        this.renderer.setClearColor("white");

        this.renderer.setSize(this.width, this.height);

        this.container.appendChild(this.renderer.domElement);
    },
    createLights: function () {
        this.scene.add(new THREE.AmbientLight(5263440));

        var light = new THREE.SpotLight(16777215, 1.5);
        light.position.set(0, 500, 2000);
        light.castShadow = true;

        light.shadowCameraNear = 200;
        light.shadowCameraFar = this.camera.far;
        light.shadowCameraFov = 50;

        light.shadowBias = -0.00022;
        light.shadowDarkness = 0.5;

        light.shadowMapWidth = 2048;
        light.shadowMapHeight = 2048;

        this.scene.add(light);
    },
    makeSphere: function () {
        return new THREE.SphereGeometry(100, 50, 50);
    },
    makeBox: function () {
        return new THREE.BoxGeometry(100, 100, 100);
    },
    makeCilinder: function () {
        return new THREE.CylinderGeometry(100, 100, 100, 50, 50);
    },
    showGeometry: function () {
        if (this.currentMesh !== null)
            this.scene.remove(this.currentMesh);

        if (this.geometryIndex < 0)
            this.geometryIndex = 0;

        var f = this.functions.getItem(this.geometryIndex);
        var geometry = f.$function();

        var mat = new THREE.MeshLambertMaterial();
        mat.color = new THREE.Color().setHex(Math.random() * 16777215);


        this.currentMesh = new THREE.Mesh(geometry, mat);
        this.scene.add(this.currentMesh);

        this.dropDownButton.text(f.name);
        this.render();


    },
    makeComboBox: function () {
        var $t;

        var dd = $("<div>").addClass("dropdown").attr("id", "dropDown");

        this.dropDownButton = $("<button>").addClass("btn btn-primary dropdown-toggle").attr("id", "dropDownButton").attr("type", "button").attr("data-toggle", "dropdown").html("Choose geometry ").appendTo(dd);

        $("<span>").addClass("caret").appendTo(this.dropDownButton);

        var ul = $("<ul>").addClass("dropdown-menu").attr("role", "menu").attr("aria-labelledby", "dropDownButton").appendTo(dd);

        $t = Bridge.getEnumerator(this.functions);
        while ($t.moveNext()) {
            var kvp = $t.getCurrent();
            var il = $("<li>").attr("role", "presentation");

            var a = $("<a>").attr("tabindex", "-1").attr("tabindex", "-1").attr("href", "#").html(kvp.name).appendTo(il);

            il.appendTo(ul);

            il.click(kvp, Bridge.fn.bind(this, this.listClick));
        }

        dd.appendTo(this.container);

    }    ,
    listClick: function (e) {
        var f = Bridge.as(e.data, ThreejsDemo.GeometryFunction);
        this.geometryIndex = this.functions.indexOf(f);
        this.showGeometry();
    }
});

Bridge.define('ThreejsDemo.misc_controls_transform', {
    inherits: [ThreejsDemo.BaseDemo],
    ctrl: null,
    constructor: function (name, category) {
        ThreejsDemo.BaseDemo.prototype.$constructor.call(this, name, category);

    },
    init: function () {
        var div = Bridge.merge(document.createElement('div'), {
            innerHTML: "\r\n                W = translate |\r\n                E = rotate | \r\n                + = increase size |  \r\n               - = decrise seize <br />\r\n               Press Q to toggle world/local space"
        } );

        this.container.appendChild(div);

        this.renderer = new THREE.WebGLRenderer();

        this.renderer.setSize(this.width, this.height);
        this.renderer.sortObjects = false;
        this.container.appendChild(this.renderer.domElement);

        //

        this.camera = new THREE.PerspectiveCamera(70, Bridge.Int.div(this.width, this.height), 1, 3000);
        this.camera.position.set(1000, 500, 1000);
        this.camera.lookAt(new THREE.Vector3(0, 200, 0));

        this.scene = new THREE.Scene();
        this.scene.add(new THREE.GridHelper(500, 100));

        var light = new THREE.DirectionalLight(16777215, 2);
        light.position.set(1, 1, 1);
        this.scene.add(light);


        var texture = THREE.ImageUtils.loadTexture("textures/crate.gif", THREE.MappingMode.uVMapping, Bridge.fn.bind(ThreejsDemo.BaseDemo.prototype, ThreejsDemo.BaseDemo.prototype.render)); //render
        texture.mapping = THREE.MappingMode.uVMapping;

        texture.anisotropy = this.renderer.getMaxAnisotropy();

        var geometry = new THREE.BoxGeometry(200, 200, 200);
        var material = new THREE.MeshLambertMaterial();
        material.map = texture;

        this.controls = new THREE.TransformControls(this.camera, this.renderer.domElement);
        this.controls.addEventListener("change", Bridge.fn.bind(ThreejsDemo.BaseDemo.prototype, ThreejsDemo.BaseDemo.prototype.render));

        var mesh = new THREE.Mesh(geometry, material);
        this.scene.add(mesh);

        this.controls.attach(mesh);
        this.scene.add(this.controls);

        this.createTrackball();
        window.addEventListener("keydown", Bridge.fn.bind(this, this.switchCase), false);


    },
    render: function () {
        ThreejsDemo.BaseDemo.prototype.render.call(this);

        this.ctrl.update();
    },
    createTrackball: function () {
        this.ctrl = new THREE.TrackballControls(this.camera, this.renderer.domElement);
        this.ctrl.rotateSpeed = 4.0;
        this.ctrl.zoomSpeed = 1.2;
        this.ctrl.panSpeed = 0.8;
        this.ctrl.noZoom = false;
        this.ctrl.noPan = false;
        this.ctrl.staticMoving = true;
        this.ctrl.dynamicDampingFactor = 0.3;
    },
    switchCase: function (arg) {

        var e = arg;

        switch (e.keyCode) {
            case 81: 
                this.controls.setSpace(this.controls.space === "local" ? "world" : "local");
                break;
            case 87: 
                this.controls.setMode("translate");
                break;
            case 69: 
                this.controls.setMode("rotate");
                break;
            case 82: 
                this.controls.setMode("scale");
                break;
            case 187: 
            case 107: 
                this.controls.setSize(this.controls.size + 0.1);
                break;
            case 189: 
            case 10: 
                this.controls.setSize(Math.max(this.controls.size - 0.1, 0.1));
                break;
        }
    }
});

Bridge.define('ThreejsDemo.misc_controls_trackball', {
    inherits: [ThreejsDemo.BaseDemo],
    constructor: function (name, category) {
        ThreejsDemo.BaseDemo.prototype.$constructor.call(this, name, category);

    },
    init: function () {
        ThreejsDemo.BaseDemo.prototype.init.call(this);

        this.controls = new THREE.TrackballControls(this.camera, this.container);

        this.controls.rotateSpeed = 3.0;
        this.controls.zoomSpeed = 1.2;
        this.controls.panSpeed = 0.8;

        this.controls.noZoom = false;
        this.controls.noPan = false;

        this.controls.staticMoving = true;
        this.controls.dynamicDampingFactor = 0.3;

        this.controls.keys = [65, 83, 68];


        this.scene = new THREE.Scene();
        // THREE.FogExp2 fg2 = new THREE.FogExp2(new THREE.Color().setHex(0xcccccc), 0.002);

        var fg2 = new THREE.FogExp2(13421772, 0.002);
        this.scene.fog = fg2;


        var geometry = new THREE.CylinderGeometry(0, 10, 30, 4, 1);
        var material = new THREE.MeshPhongMaterial();

        material.color = new THREE.Color().setHex(16777215); // 0xffffff;
        material.shading = THREE.ShadingType.flatShading;


        for (var i = 0; i < 500; i++) {

            var mesh = new THREE.Mesh(geometry, material);
            mesh.position.x = (Math.random() - 0.5) * 1000;
            mesh.position.y = (Math.random() - 0.5) * 1000;
            mesh.position.z = (Math.random() - 0.5) * 1000;
            mesh.updateMatrix();
            mesh.matrixAutoUpdate = false;
            this.scene.add(mesh);

        }


        // lights

        var light = new THREE.DirectionalLight();
        light.color = new THREE.Color().setHex(16777215);
        light.position.set(1, 1, 1);
        this.scene.add(light);

        light = new THREE.DirectionalLight();
        light.color = new THREE.Color().setHex(8840);
        light.position.set(-1, -1, -1);
        this.scene.add(light);

        light = new THREE.AmbientLight(2236962);
        light.color = new THREE.Color().setHex(2236962);
        this.scene.add(light);


        // renderer

        this.renderer = new THREE.WebGLRenderer(false);
        this.renderer.antialias = false;
        this.renderer.setClearColor(this.scene.fog.color);
        //TODO: renderer.setPixelRatio( Window.devicePixelRatio );
        this.renderer.setSize(this.width, this.height);

        this.container.appendChild(this.renderer.domElement);

        //stats = new Stats();
        //stats.domElement.style.position = 'absolute';
        //stats.domElement.style.top = '0px';
        //stats.domElement.style.zIndex = 100;
        //container.appendChild( stats.domElement );

        //


        //


    }
});

Bridge.define('ThreejsDemo.webgl_interactive_draggablecubes', {
    inherits: [ThreejsDemo.BaseDemo],
    mouse: null,
    raycaster: null,
    plane: null,
    offset: null,
    intersected: null,
    selected: null,
    allObjects: null,
    constructor: function (name, category) {
        ThreejsDemo.BaseDemo.prototype.$constructor.call(this, name, category);


    },
    init: function () {
        this.mouse = new THREE.Vector2();
        this.raycaster = new THREE.Raycaster();
        this.offset = new THREE.Vector3();
        this.allObjects = [];

        this.createCamera();
        this.createScene();

        this.createBoxes();

        this.plane = new THREE.Mesh(new THREE.PlaneBufferGeometry(2000.0, 2000.0, 8.0, 8.0), Bridge.merge(new THREE.MeshBasicMaterial(), {
            color: 0,
            opacity: 0.25,
            transparent: true
        } ));
        this.plane.visible = false;
        this.scene.add(this.plane);

        this.createRenderer();
        this.renderer.domElement.onmousemove = Bridge.fn.bind(this, this.onDocumentMouseMove);
        this.renderer.domElement.onmousedown = Bridge.fn.bind(this, this.onDocumentMouseDown);
        this.renderer.domElement.onmouseup = Bridge.fn.bind(this, this.onDocumentMouseUp);

        this.createTrackball();

    },
    createRenderer: function () {
        this.renderer = Bridge.merge(new THREE.WebGLRenderer(), {
            antialias: true
        } );
        this.renderer.setClearColor(15790320);

        this.renderer.sortObjects = false;

        this.renderer.shadowMapEnabled = true;

        this.renderer.shadowMapType = THREE.MapType.basicShadowMap;

        this.container.appendChild(this.renderer.domElement);


    },
    createBoxes: function () {
        var geometry = new THREE.BoxGeometry(40, 40, 40);

        for (var i = 0; i < 200; i++) {
            var mesh = this.makeMesh(geometry);
            this.scene.add(mesh);
            this.allObjects.push(mesh);
        }
    },
    createScene: function () {
        this.scene = new THREE.Scene();

        this.scene.add(new THREE.AmbientLight(5263440));

        var light = new THREE.SpotLight(16777215, 1.5);
        light.position.set(0, 500, 2000);
        light.castShadow = true;

        light.shadowCameraNear = 200;
        light.shadowCameraFar = this.camera.far;
        light.shadowCameraFov = 50;

        light.shadowBias = -0.00022;
        light.shadowDarkness = 0.5;

        light.shadowMapWidth = 2048;
        light.shadowMapHeight = 2048;

        this.scene.add(light);
    },
    createTrackball: function () {
        this.controls = new THREE.TrackballControls(this.camera, this.renderer.domElement);
        this.controls.rotateSpeed = 4.0;
        this.controls.zoomSpeed = 1.2;
        this.controls.panSpeed = 0.8;
        this.controls.noZoom = false;
        this.controls.noPan = false;
        this.controls.staticMoving = true;
        this.controls.dynamicDampingFactor = 0.3;
    },
    createCamera: function () {
        this.camera = new THREE.PerspectiveCamera(70, Bridge.Int.div(this.width, this.height), 1, 10000);
        this.camera.position.z = 1000;
    },
    makeMesh: function (geometry) {
        var mat = null;

        mat = new THREE.MeshLambertMaterial();
        mat.color = new THREE.Color();
        mat.color.setHex(Math.random() * 16777215);



        var mesh = new THREE.Mesh(geometry, mat);

        mesh.position.x = Math.random() * 1000 - 500;
        mesh.position.y = Math.random() * 600 - 300;
        mesh.position.z = Math.random() * 800 - 400;

        mesh.rotation.x = Math.random() * 2 * Math.PI;
        mesh.rotation.y = Math.random() * 2 * Math.PI;
        mesh.rotation.z = Math.random() * 2 * Math.PI;

        mesh.scale.x = Math.random() * 2 + 1;
        mesh.scale.y = Math.random() * 2 + 1;
        mesh.scale.z = Math.random() * 2 + 1;

        mesh.castShadow = true;
        mesh.receiveShadow = true;
        return mesh;
    },
    onDocumentMouseUp: function (arg) {
        if (!this.isActive)
            return;

        var e = arg;

        e.preventDefault();

        this.controls.enabled = true;

        if (this.intersected !== null) {
            this.plane.position.copy(this.intersected.position);
            this.selected = null;
        }

        this.container.style.cursor = "auto";

    },
    onDocumentMouseDown: function (arg) {
        if (!this.isActive)
            return;

        var e = arg;
        e.preventDefault();

        var vector = new THREE.Vector3(this.mouse.x, this.mouse.y, 0.5).unproject(this.camera);

        var raycaster = new THREE.Raycaster(this.camera.position, vector.sub(this.camera.position).normalize());

        var intersects = raycaster.intersectObjects(this.allObjects);

        if (intersects.length > 0) {
            var interSec = intersects[0];
            var m = Bridge.as(interSec.object, THREE.Mesh);

            if (m !== null) {
                this.controls.enabled = false;
                this.selected = m;
                intersects = raycaster.intersectObject(this.plane);
                this.offset.copy(interSec.point).sub(this.plane.position);
                this.container.style.cursor = "move";
            }
        }

    },
    onDocumentMouseMove: function (arg) {

        if (!this.isActive)
            return;

        var e = arg;
        e.preventDefault();

        this.setMousePos(e);
        this.raycaster.setFromCamera(this.mouse, this.camera);


        var intersects;

        if (this.selected !== null) {
            intersects = this.raycaster.intersectObject(this.plane);
            this.selected.position.copy(intersects[0].point.sub(this.offset));
            return;

        }

        intersects = this.raycaster.intersectObjects(this.allObjects);

        if (intersects.length > 0) {
            var i = intersects[0];
            var m = Bridge.as(i.object, THREE.Mesh);

            if (m !== null && m !== this.intersected) {
                this.intersected = m;
                this.plane.position.copy(this.intersected.position);
                this.plane.lookAt(this.camera.position);

            }
            this.container.style.cursor = "pointer";
        }
        else  {
            this.intersected = null;
            this.container.style.cursor = "auto";
        }

    },
    setMousePos: function (e) {

        var r = this.renderer.domElement.getBoundingClientRect();

        // calculate mouse position in normalized device coordinates
        // (-1 to +1) for both components
        this.mouse.x = ((e.clientX - r.left) / Bridge.cast(this.width, Number)) * 2 - 1;
        this.mouse.y = -((e.clientY - r.top) / Bridge.cast(this.height, Number)) * 2 + 1;

    }
});

Bridge.define('ThreejsDemo.demo_shadow', {
    inherits: [ThreejsDemo.BaseDemo],
    constructor: function (name, category) {
        ThreejsDemo.BaseDemo.prototype.$constructor.call(this, name, category);

    },
    init: function () {

        // create a scene, that will hold all our elements such as objects, cameras and lights.
        this.scene = new THREE.Scene();

        // create a camera, which defines where we're looking at.
        this.camera = new THREE.PerspectiveCamera(45, Bridge.Int.div(this.width, this.height), 0.1, 1000);

        // create a render and set the size
        this.renderer = new THREE.WebGLRenderer();

        this.renderer.setClearColor(new THREE.Color(15658734));
        this.renderer.setSize(this.width, this.height);
        this.renderer.shadowMapEnabled = true;
        // add the output of the renderer to the html element
        this.container.appendChild(this.renderer.domElement);


        // create the ground plane
        var planeGeometry = new THREE.PlaneGeometry(60, 20);
        var planeMaterial = new THREE.MeshLambertMaterial();
        planeMaterial.color = new THREE.Color(0.9, 0.9, 0.9);
        var plane = new THREE.Mesh(planeGeometry, planeMaterial);
        plane.receiveShadow = true;

        // rotate and position the plane
        plane.rotation.x = -0.5 * Math.PI;
        plane.position.x = 15;
        plane.position.y = 0;
        plane.position.z = 0;

        // add the plane to the scene
        this.scene.add(plane);

        // create a cube
        var cubeGeometry = new THREE.CubeGeometry(4, 4, 4);
        var cubeMaterial = new THREE.MeshLambertMaterial(); // { color = 0xff0000 };
        cubeMaterial.color = new THREE.Color(1, 0, 0);
        var cube = new THREE.Mesh(cubeGeometry, cubeMaterial);
        cube.castShadow = true;

        // position the cube
        cube.position.x = -4;
        cube.position.y = 3;
        cube.position.z = 0;

        // add the cube to the scene
        this.scene.add(cube);

        var sphereGeometry = new THREE.SphereGeometry(4, 20, 20);
        var sphereMaterial = new THREE.MeshLambertMaterial(); // { color = 0x7777ff };
        sphereMaterial.color = new THREE.Color(0, 0, 1);
        var sphere = new THREE.Mesh(sphereGeometry, sphereMaterial);

        // position the sphere
        sphere.position.x = 20;
        sphere.position.y = 4;
        sphere.position.z = 2;
        sphere.castShadow = true;

        // add the sphere to the scene
        this.scene.add(sphere);

        // position and point the camera to the center of the scene
        this.camera.position.x = -30;
        this.camera.position.y = 40;
        this.camera.position.z = 30;
        this.camera.lookAt(this.scene.position);

        // add spotlight for the shadows
        var spotLight = new THREE.SpotLight(); //0xffffff);
        spotLight.color = new THREE.Color(1, 1, 1);
        spotLight.position.set(-40, 60, -10);
        spotLight.castShadow = true;
        this.scene.add(spotLight);






        this.controls = new THREE.TrackballControls(this.camera);

        this.controls.rotateSpeed = 10.0;
        this.controls.zoomSpeed = 1.2;
        this.controls.panSpeed = 0.8;

        this.controls.noZoom = false;
        this.controls.noPan = false;

        this.controls.staticMoving = true;
        this.controls.dynamicDampingFactor = 0.3;

        this.controls.keys = [65, 83, 68];




    }
});

Bridge.define('ThreejsDemo.demo_cloths', {
    inherits: [ThreejsDemo.ParticleBaseDemo],
    cloth: null,
    constructor: function (name, category) {
        ThreejsDemo.ParticleBaseDemo.prototype.$constructor.call(this, name, category);

    },
    init: function () {
        ThreejsDemo.ParticleBaseDemo.prototype.init.call(this);

        this.cloth = Particles.Cloth.create();

        this.makeCamera();
        this.makeLights();

        //make cloth mesh
        var loader = new THREE.TextureLoader();
        loader.load("./bridge.gif", Bridge.fn.bind(this, this.makeClothMesh));


        this.scene.add(this.cloth.ballConstrain.mesh);
        this.scene.add(this.cloth.boxConstrain.mesh);

        // ground
        loader = new THREE.TextureLoader();
        loader.load("./threejs/textures/terrain/backgrounddetailed6.jpg", Bridge.fn.bind(this, this.makeGroundPlane));


        this.makePortal();
        this.createRenderer();
        this.createTrackball();

    },
    makePortal: function () {
        // poles
        var poleGeo = new THREE.BoxGeometry(5, 375, 5);
        var poleMat = new THREE.MeshPhongMaterial(); // { color: 0xffffff, specular: 0x111111, shininess: 100 } );
        poleMat.color = new THREE.Color().setHex(16777215);
        poleMat.specular = new THREE.Color().setHex(1118481);
        poleMat.shininess = 100;

        var mesh = new THREE.Mesh(poleGeo, poleMat);
        mesh.position.x = -125;
        mesh.position.y = -62;
        mesh.receiveShadow = true;
        mesh.castShadow = true;
        this.scene.add(mesh);

        mesh = new THREE.Mesh(poleGeo, poleMat);
        mesh.position.x = 125;
        mesh.position.y = -62;
        mesh.receiveShadow = true;
        mesh.castShadow = true;
        this.scene.add(mesh);

        mesh = new THREE.Mesh(new THREE.BoxGeometry(255, 5, 5), poleMat);
        mesh.position.y = 125;
        mesh.position.x = 0;
        mesh.receiveShadow = true;
        mesh.castShadow = true;
        this.scene.add(mesh);

        var gg = new THREE.BoxGeometry(10, 10, 10);
        mesh = new THREE.Mesh(gg, poleMat);
        mesh.position.y = -250;
        mesh.position.x = 125;
        mesh.receiveShadow = true;
        mesh.castShadow = true;
        this.scene.add(mesh);

        mesh = new THREE.Mesh(gg, poleMat);
        mesh.position.y = -250;
        mesh.position.x = -125;
        mesh.receiveShadow = true;
        mesh.castShadow = true;
        this.scene.add(mesh);
    },
    makeClothMesh: function (t) {
        var clothTexture = t;

        clothTexture.wrapS = THREE.WrapType.mirroredRepeatWrapping;
        clothTexture.wrapT = THREE.WrapType.repeatWrapping;

        clothTexture.repeat.y = -1;
        clothTexture.anisotropy = 16;


        var clothMaterial = new THREE.MeshPhongMaterial();

        clothMaterial.specular = new THREE.Color().setHex(197379);
        //clothMaterial.color = new THREE.Color(1, 0.4, 0);
        clothMaterial.map = clothTexture;
        clothMaterial.side = THREE.SideType.doubleSide;
        clothMaterial.alphaTest = 0.5;
        clothMaterial.wireframe = false;
        clothMaterial.wireframeLinewidth = 2;

        // cloth mesh
        var clothMesh = new THREE.Mesh(this.cloth.geometry, clothMaterial);
        clothMesh.position.set(0, 0, 0);
        clothMesh.castShadow = true;


        var shaderMat = null;
        ;

        var o = { texture: { type: "t", value: clothTexture } };

        shaderMat = new THREE.ShaderMaterial();

        shaderMat.side = THREE.SideType.doubleSide;

        var vertexShader = ThreejsDemo.Shaders.vertex;
        var fragmentShader = ThreejsDemo.Shaders.fragment;
        shaderMat.vertexShader = vertexShader;
        shaderMat.fragmentShader = fragmentShader;

        clothMesh.customDepthMaterial = shaderMat;
        this.scene.add(clothMesh);
    },
    makeGroundPlane: function (t) {
        var groundTexture = t;
        groundTexture.wrapS = THREE.WrapType.repeatWrapping;
        groundTexture.wrapT = THREE.WrapType.repeatWrapping;
        groundTexture.repeat.set(25, 25);
        groundTexture.anisotropy = 16;

        var groundMaterial = new THREE.MeshPhongMaterial();
        groundMaterial.color = new THREE.Color().setHex(16777215);
        groundMaterial.specular = new THREE.Color().setHex(1118481);
        groundMaterial.map = groundTexture;
        ;

        var planeMesh = new THREE.Mesh(new THREE.PlaneBufferGeometry(20000, 20000), groundMaterial);
        planeMesh.position.y = -250;
        planeMesh.rotation.x = -Math.PI / 2;
        planeMesh.receiveShadow = true;
        this.scene.add(planeMesh);
    },
    requestFrame: function () {
        var time = new Date().getTime() * 10000;

        this.cloth.simulate(time);

        ThreejsDemo.ParticleBaseDemo.prototype.requestFrame.call(this);
    }
});

Bridge.define('ThreejsDemo.demo_carpet', {
    inherits: [ThreejsDemo.ParticleBaseDemo],
    carpet: null,
    constructor: function (name, category) {
        ThreejsDemo.ParticleBaseDemo.prototype.$constructor.call(this, name, category);

    },
    init: function () {
        var $t;
        ThreejsDemo.ParticleBaseDemo.prototype.init.call(this);

        this.carpet = new Particles.Carpet();

        this.makeCamera();
        this.makeLights();


        //make carpet mesh
        var loader = new THREE.TextureLoader();
        loader.load("./bridge.gif", Bridge.fn.bind(this, this.makeCarpetMesh));


        this.createRenderer();
        this.createTrackball();

        $t = Bridge.getEnumerator(this.carpet.objectconstraines.items);
        while ($t.moveNext()) {
            var o = $t.getCurrent();
            this.scene.add(o.mesh);
        }
        this.camera.lookAt(this.scene.position);

    }    ,
    requestFrame: function () {
        var time = new Date().getTime() * 10000;

        this.carpet.simulate(time);

        ThreejsDemo.ParticleBaseDemo.prototype.requestFrame.call(this);
    },
    makeCarpetMesh: function (t) {
        var clothTexture = t;

        clothTexture.wrapS = THREE.WrapType.mirroredRepeatWrapping;
        clothTexture.wrapT = THREE.WrapType.repeatWrapping;

        clothTexture.repeat.y = -1;
        clothTexture.anisotropy = 16;


        var clothMaterial = new THREE.MeshPhongMaterial();

        clothMaterial.specular = new THREE.Color().setHex(197379);
        //clothMaterial.color = new THREE.Color(1, 0.4, 0);
        clothMaterial.map = clothTexture;
        clothMaterial.side = THREE.SideType.doubleSide;
        clothMaterial.alphaTest = 0.5;


        // cloth mesh
        var clothMesh = new THREE.Mesh(this.carpet.geometry, clothMaterial);
        clothMesh.position.set(0, 0, 0);
        clothMesh.castShadow = true;


        var shaderMat = null;
        ;

        var o = { texture: { type: "t", value: clothTexture } };

        shaderMat = new THREE.ShaderMaterial();

        shaderMat.side = THREE.SideType.doubleSide;

        //string vertexShader = Shaders.vertex;
        //string fragmentShader = Shaders.fragment;
        //shaderMat.vertexShader = vertexShader;
        //shaderMat.fragmentShader = fragmentShader;

        clothMesh.customDepthMaterial = shaderMat;
        this.scene.add(clothMesh);
    }
});

Bridge.define('ThreejsDemo.canvas_ascii_effect', {
    inherits: [ThreejsDemo.BaseDemo],
    effect: null,
    sphere: null,
    config: {
        init: function () {
            this.startTime = new Date().getTime();
        }
    },
    constructor: function (name, category) {
        ThreejsDemo.BaseDemo.prototype.$constructor.call(this, name, category);

    },
    init: function () {

        var width = this.width;
        var height = this.height;


        var info = document.createElement("div");
        info.innerHTML = "Drag to change the view";
        this.container.appendChild(info);

        this.camera = new THREE.PerspectiveCamera(70, Bridge.Int.div(width, height), 1, 1000);
        this.camera.position.y = 150;
        this.camera.position.z = 500;

        this.controls = new THREE.TrackballControls(this.camera);

        this.scene = new THREE.Scene();

        var light = new THREE.PointLight(16777215);
        light.position.set(500, 500, 500);
        this.scene.add(light);



        var mat = new THREE.MeshLambertMaterial();


        mat.shading = THREE.ShadingType.flatShading;



        this.sphere = new THREE.Mesh(new THREE.SphereGeometry(200, 20, 10), mat);
        this.scene.add(this.sphere);

        var mat2 = new THREE.MeshBasicMaterial();
        mat2.color = 14737632;


        var plane = new THREE.Mesh(new THREE.PlaneBufferGeometry(400, 400), mat2);
        plane.position.y = -200;
        plane.rotation.x = -Math.PI / 2;
        this.scene.add(plane);

        this.renderer = new THREE.CanvasRenderer();
        this.renderer.setClearColor(15790320);
        this.renderer.setSize(width, height);

        this.effect = new THREE.AsciiEffect(this.renderer, " +-Hgxyz");
        this.effect.setSize(width, height);
        this.container.appendChild(this.effect.domElement);

    },
    render: function () {
        if (!this.isActive)
            return;

        ThreejsDemo.BaseDemo.prototype.render.call(this);

        //Date start = Date.now();
        var timer = new Date().getTime() - this.startTime;

        this.sphere.position.y = Math.abs(Math.sin(timer * 0.002)) * 150;
        this.sphere.rotation.x = timer * 0.0003;
        this.sphere.rotation.z = timer * 0.0002;

        this.effect.render(this.scene, this.camera);
    }
});

Bridge.define('ThreejsDemo.DemoLauncher', {
    statics: {
        demos: null,
        activeDemo: null,
        demoContainer: null,
        launch: function () {
            ThreejsDemo.DemoLauncher.activeDemo = null;
            ThreejsDemo.DemoLauncher.demos = new Bridge.Dictionary$2(String,Bridge.List$1(ThreejsDemo.BaseDemo))();

            ThreejsDemo.DemoLauncher.addDemo(new ThreejsDemo.webgl_interactive_draggablecubes("draggablecubes", "Interactive"));
            ThreejsDemo.DemoLauncher.addDemo(new ThreejsDemo.misc_controls_trackball("trackball", "Interactive"));
            ThreejsDemo.DemoLauncher.addDemo(new ThreejsDemo.misc_controls_transform("transform", "Interactive"));
            ThreejsDemo.DemoLauncher.addDemo(new ThreejsDemo.demo_shadow("Shadow", "Geometry"));
            ThreejsDemo.DemoLauncher.addDemo(new ThreejsDemo.geometry_demo("GeometryDemo", "Geometry"));
            ThreejsDemo.DemoLauncher.addDemo(new ThreejsDemo.canvas_ascii_effect("ascii_effect", "Effects"));
            ThreejsDemo.DemoLauncher.addDemo(new ThreejsDemo.demo_cloths("demo_cloths", "Effects"));
            ThreejsDemo.DemoLauncher.addDemo(new ThreejsDemo.demo_carpet("demo_carpet", "Effects"));


            ThreejsDemo.DemoLauncher.makeList();


        },
        makeList: function () {
            var $t, $t1;
            var row = $("<div>").addClass("row");

            var col1 = $("<div>").addClass("col-sm-3").appendTo(row);
            var col2 = $("<div>").addClass("col-sm-9").appendTo(row);

            row.get(0).style.padding = "20px";

            var p = ThreejsDemo.AccordionPanel.make("Sharp THREEjs", "(jus a try out :)", "myID", "panel_default");

            $t = Bridge.getEnumerator(ThreejsDemo.DemoLauncher.demos);
            while ($t.moveNext()) {
                var kvp = $t.getCurrent();
                var m = new ThreejsDemo.ListMaker();
                $t1 = Bridge.getEnumerator(kvp.value);
                while ($t1.moveNext()) {
                    var d = $t1.getCurrent();
                    m.addListItem(d.demoName, ThreejsDemo.DemoLauncher.clickDemo, d);
                }

                p.addPanel(kvp.key, m.list);
            }

            col1.append(p.mainContainer);

            row.appendTo(document.body);

            ThreejsDemo.DemoLauncher.demoContainer = col2.get(0);
        }        ,
        addDemo: function (v) {
            if (!ThreejsDemo.DemoLauncher.demos.containsKey(v.demoCategory))
                ThreejsDemo.DemoLauncher.demos.add(v.demoCategory, new Bridge.List$1(ThreejsDemo.BaseDemo)());

            ThreejsDemo.DemoLauncher.demos.get(v.demoCategory).add(v);
        },
        clickDemo: function (arg) {
            var d = Bridge.as(arg.data, ThreejsDemo.BaseDemo);

            if (d === ThreejsDemo.DemoLauncher.activeDemo)
                return;

            if (ThreejsDemo.DemoLauncher.activeDemo !== null) {
                ThreejsDemo.DemoLauncher.activeDemo.hide();
                ThreejsDemo.DemoLauncher.demoContainer.removeChild(ThreejsDemo.DemoLauncher.activeDemo.container);
                ThreejsDemo.DemoLauncher.activeDemo = null;
            }

            ThreejsDemo.DemoLauncher.activeDemo = d;
            ThreejsDemo.DemoLauncher.demoContainer.appendChild(ThreejsDemo.DemoLauncher.activeDemo.container);

            var doShow = function () {
                ThreejsDemo.DemoLauncher.activeDemo.show();
            };

            window.setTimeout(doShow, 500);

        }
    }
});

Bridge.define('ThreejsDemo.App', {
    statics: {
        config: {
            init: function () {
                Bridge.ready(this.main);
            }
        },
        main: function () {
            ThreejsDemo.DemoLauncher.launch();
        }
    }
});

Bridge.init();