/// <reference path="./bridge.d.ts" />

declare module Particles {
    export interface Constrained {
        p1: Particles.Particle;
        p2: Particles.Particle;
        restLength: number;
        setPosition(): void;
        satisify(): void;
    }
    export interface ConstrainedFunc extends Function {
        prototype: Constrained;
        new (pa1: Particles.Particle, pa2: Particles.Particle): Constrained;
    }
    var Constrained: ConstrainedFunc;

    export interface Carpet extends Particles.ParametricSurface {
        windForce: Particles.GlobalForce;
        gravityForce: Particles.GlobalForce;
        ballConstrain: Particles.BallConstrain;
        boxConstrain: Particles.BoxConstrain;
        simulate(time: number): void;
        paramFunction(u: number, v: number): THREE.Vector3;
    }
    export interface CarpetFunc extends Function {
        prototype: Carpet;
        new (): Carpet;
    }
    var Carpet: CarpetFunc;

    /** @namespace Particles */
    
    /**
     * Cloth Simulation using a relaxed constrains solver
     *
     * @public
     * @class Particles.Cloth
     * @augments Particles.ParametricSurface
     */
    export interface Cloth extends Particles.ParametricSurface {
        windForce: Particles.GlobalForce;
        gravityForce: Particles.GlobalForce;
        ballConstrain: Particles.BallConstrain;
        boxConstrain: Particles.BoxConstrain;
        paramFunction(u: number, v: number): THREE.Vector3;
        simulate(time: number): void;
    }
    export interface ClothFunc extends Function {
        prototype: Cloth;
        uSegs: number;
        vSegs: number;
        create(): Particles.Cloth;
    }
    var Cloth: ClothFunc;

    /**
     * @abstract
     * @public
     * @class Particles.ParametricSurface
     * @augments Particles.ParticleSystem
     */
    export interface ParametricSurface extends Particles.ParticleSystem {
        geometry: THREE.ParametricGeometry;
        nrU: number;
        nrV: number;
        simulate(time: number): void;
        index(u: number, v: number): number;
        makeConstrains(): void;
        createParticles(): void;
        setFixed(p: number[]): void;
        updateGeometry(): void;
        reset(): void;
        fixRange(i1: number, i2: number): void;
    }
    export interface ParametricSurfaceFunc extends Function {
        prototype: ParametricSurface;
        new (): ParametricSurface;
    }
    var ParametricSurface: ParametricSurfaceFunc;

    export interface ParticleConstants {
    }
    export interface ParticleConstantsFunc extends Function {
        prototype: ParticleConstants;
        new (): ParticleConstants;
        sPEEDFACTOR: number;
        tIMESTEP: number;
        tIMESQR: number;
        dAMPING: number;
        mASS: number;
    }
    var ParticleConstants: ParticleConstantsFunc;

    export interface Particle {
        position: THREE.Vector3;
        previous: THREE.Vector3;
        original: THREE.Vector3;
        acc: THREE.Vector3;
        mass: number;
        invMass: number;
        isFixed: boolean;
        integrate(deltaT: number): void;
        addForce(force: THREE.Vector3): void;
        toOriginal(): void;
    }
    export interface ParticleFunc extends Function {
        prototype: Particle;
        new (m: number, pos: THREE.Vector3): Particle;
    }
    var Particle: ParticleFunc;

    /**
     * Concrete ball constrained
     *
     * @public
     * @class Particles.BallConstrain
     * @augments Particles.ObjectConstrain
     */
    export interface BallConstrain extends Particles.ObjectConstrain {
        center: THREE.Vector3;
        radius: number;
        makeMesh(): void;
        constrain(pos: THREE.Vector3): THREE.Vector3;
        changeRadius(r: number): void;
        updateMesh(): void;
    }
    export interface BallConstrainFunc extends Function {
        prototype: BallConstrain;
        new (c: THREE.Vector3, ballRadius: number): BallConstrain;
    }
    var BallConstrain: BallConstrainFunc;

    export interface ParticleSystem {
        particles: Particles.Particle[];
        constrains: Particles.Constrained[];
        globalForces: Particles.GlobalForce[];
        objectconstraines: Particles.ConstrainedList;
    }
    export interface ParticleSystemFunc extends Function {
        prototype: ParticleSystem;
        new (): ParticleSystem;
    }
    var ParticleSystem: ParticleSystemFunc;

    /**
     * Abstact ObjectConstrain
     *
     * @abstract
     * @public
     * @class Particles.ObjectConstrain
     */
    export interface ObjectConstrain {
        apply: boolean;
        mesh: THREE.Mesh;
        applyConstrained(particles: Bridge.IEnumerable$1<Particles.Particle>): void;
    }
    export interface ObjectConstrainFunc extends Function {
        prototype: ObjectConstrain;
        new (): ObjectConstrain;
    }
    var ObjectConstrain: ObjectConstrainFunc;

    export interface RandomWindForce extends Particles.GlobalForce {
        getForce(): THREE.Vector3;
    }
    export interface RandomWindForceFunc extends Function {
        prototype: RandomWindForce;
        new (): RandomWindForce;
    }
    var RandomWindForce: RandomWindForceFunc;

    export interface YGravity extends Particles.GlobalForce {
        setValue(v: number): void;
        getForce(): THREE.Vector3;
    }
    export interface YGravityFunc extends Function {
        prototype: YGravity;
        new (v: number): YGravity;
    }
    var YGravity: YGravityFunc;

    export interface GlobalForce {
        apply: boolean;
        getForce(): THREE.Vector3;
    }
    export interface GlobalForceFunc extends Function {
        prototype: GlobalForce;
        new (): GlobalForce;
    }
    var GlobalForce: GlobalForceFunc;

    export interface DivVector {
    }
    export interface DivVectorFunc extends Function {
        prototype: DivVector;
        new (): DivVector;
        sub(v1: THREE.Vector3, v2: THREE.Vector3): THREE.Vector3;
    }
    var DivVector: DivVectorFunc;

    export interface ConstrainedList {
        items: Particles.ObjectConstrain[];
        append(o: Particles.ObjectConstrain): void;
        applyConstrained(particles: Bridge.IEnumerable$1<Particles.Particle>): void;
        updateMesh(): void;
    }
    export interface ConstrainedListFunc extends Function {
        prototype: ConstrainedList;
        new (): ConstrainedList;
    }
    var ConstrainedList: ConstrainedListFunc;

    export interface BoxConstrain extends Particles.ObjectConstrain {
        width: number;
        height: number;
        depth: number;
        center: THREE.Vector3;
        constrain(pos: THREE.Vector3): THREE.Vector3;
        distance(value: number, index: number): number;
        left(): number;
        right(): number;
        bottom(): number;
        top(): number;
        front(): number;
        back(): number;
        makeBox(): void;
        makeMesh(): void;
        updateMesh(): void;
    }
    export interface BoxConstrainFunc extends Function {
        prototype: BoxConstrain;
        new (center: THREE.Vector3, w: number, h: number, d: number): BoxConstrain;
        isZ(i: number): boolean;
        isY(i: number): boolean;
        isX(i: number): boolean;
    }
    var BoxConstrain: BoxConstrainFunc;

}

/// <reference path="./bridge.d.ts" />

declare module ThreejsDemo {
    export interface misc_controls_transform extends ThreejsDemo.BaseDemo {
        init(): void;
        render(): void;
        createTrackball(): void;
        switchCase(arg: Event): void;
    }
    export interface misc_controls_transformFunc extends Function {
        prototype: misc_controls_transform;
        new (name: string, category: string): misc_controls_transform;
    }
    var misc_controls_transform: misc_controls_transformFunc;

    export interface canvas_ascii_effect extends ThreejsDemo.BaseDemo {
        init(): void;
        render(): void;
    }
    export interface canvas_ascii_effectFunc extends Function {
        prototype: canvas_ascii_effect;
        new (name: string, category: string): canvas_ascii_effect;
    }
    var canvas_ascii_effect: canvas_ascii_effectFunc;

    export interface demo_carpet extends ThreejsDemo.ParticleBaseDemo {
        init(): void;
        requestFrame(): void;
        makeCarpetMesh(t: THREE.Texture): void;
    }
    export interface demo_carpetFunc extends Function {
        prototype: demo_carpet;
        new (name: string, category: string): demo_carpet;
    }
    var demo_carpet: demo_carpetFunc;

    export interface demo_cloths extends ThreejsDemo.ParticleBaseDemo {
        init(): void;
        makePortal(): void;
        makeClothMesh(t: THREE.Texture): void;
        makeGroundPlane(t: THREE.Texture): void;
        requestFrame(): void;
    }
    export interface demo_clothsFunc extends Function {
        prototype: demo_cloths;
        new (name: string, category: string): demo_cloths;
    }
    var demo_cloths: demo_clothsFunc;

    export interface demo_shadow extends ThreejsDemo.BaseDemo {
        init(): void;
    }
    export interface demo_shadowFunc extends Function {
        prototype: demo_shadow;
        new (name: string, category: string): demo_shadow;
    }
    var demo_shadow: demo_shadowFunc;

    export interface webgl_interactive_draggablecubes extends ThreejsDemo.BaseDemo {
        init(): void;
        createRenderer(): void;
        createBoxes(): void;
        createScene(): void;
        createTrackball(): void;
        createCamera(): void;
        makeMesh(geometry: THREE.BoxGeometry): THREE.Mesh;
        onDocumentMouseUp(arg: Event): void;
        onDocumentMouseDown(arg: Event): void;
        onDocumentMouseMove(arg: Event): void;
        setMousePos(e: MouseEvent): void;
    }
    export interface webgl_interactive_draggablecubesFunc extends Function {
        prototype: webgl_interactive_draggablecubes;
        new (name: string, category: string): webgl_interactive_draggablecubes;
    }
    var webgl_interactive_draggablecubes: webgl_interactive_draggablecubesFunc;

    export interface misc_controls_trackball extends ThreejsDemo.BaseDemo {
        init(): void;
    }
    export interface misc_controls_trackballFunc extends Function {
        prototype: misc_controls_trackball;
        new (name: string, category: string): misc_controls_trackball;
    }
    var misc_controls_trackball: misc_controls_trackballFunc;

    export interface geometry_demo extends ThreejsDemo.BaseDemo {
        init(): void;
        createTrackballControl(): void;
        createRenderer(): void;
        createLights(): void;
        makeSphere(): THREE.Geometry;
        makeBox(): THREE.Geometry;
        makeCilinder(): THREE.Geometry;
        showGeometry(): void;
        makeComboBox(): void;
        listClick(e: jQuery.Event): void;
    }
    export interface geometry_demoFunc extends Function {
        prototype: geometry_demo;
        new (name: string, category: string): geometry_demo;
    }
    var geometry_demo: geometry_demoFunc;

    export interface BussyDlg {
        /**
         * sm or lg
         *
         * @instance
         * @public
         * @memberof ThreejsDemo.BussyDlg
         * @default "sm"
         * @type string
         */
        dialogSize: string;
        progressType: string;
        show(title: string): void;
        doShow(): void;
        doHide(): void;
        hide(delay?: number): void;
    }
    export interface BussyDlgFunc extends Function {
        prototype: BussyDlg;
        new (): BussyDlg;
        make(): $;
    }
    var BussyDlg: BussyDlgFunc;

    export interface BaseDemo {
        demoName: string;
        demoCategory: string;
        container: HTMLElement;
        show(): void;
        hide(): void;
        doInit(): void;
        isInit(): boolean;
        init(): void;
        requestFrame(): void;
        render(): void;
        onWindowResize(arg: Event): void;
        updateRenderSize(): void;
    }
    export interface BaseDemoFunc extends Function {
        prototype: BaseDemo;
        new (name: string, category: string): BaseDemo;
    }
    var BaseDemo: BaseDemoFunc;

    export interface AccordionPanel {
        pType: ThreejsDemo.PanelType;
        id: string;
        panelGroup: $;
        mainContainer: $;
        panelCount: number;
        addPanel(title: string, content: $, clickHandler?: {(arg: jQuery.Event): void}, panelTag?: Object): $;
        makePanelTitle(title: string, href: string, dataParent: string, clickHandler: {(arg: jQuery.Event): void}, panelTag: Object): $;
        collapse(e: jQuery.Event): void;
        makeContentPanel(content: $, id: string): $;
    }
    export interface AccordionPanelFunc extends Function {
        prototype: AccordionPanel;
        new (): AccordionPanel;
        make(headerText: string, subtext: string, id: string, type: ThreejsDemo.PanelType): ThreejsDemo.AccordionPanel;
        getPanelTypeName(t: ThreejsDemo.PanelType): string;
    }
    var AccordionPanel: AccordionPanelFunc;

    export interface Shaders {
    }
    export interface ShadersFunc extends Function {
        prototype: Shaders;
        new (): Shaders;
        fragment: string;
        vertex: string;
    }
    var Shaders: ShadersFunc;

    export interface ModalDlg {
        show(): void;
        hide(): void;
        remove(): void;
    }
    export interface ModalDlgFunc extends Function {
        prototype: ModalDlg;
        new (): ModalDlg;
        show(titleText: string, bodyText: string): void;
        showDlgWithContent(titleText: string, bodyContent: HTMLElement): void;
        ensureDialog(titleText: string): void;
        onHidden(): void;
        make(titleText: string, bodyText: string, id: string): ThreejsDemo.ModalDlg;
    }
    var ModalDlg: ModalDlgFunc;

    export interface GeometryFunction {
        $function: {(): THREE.Geometry};
        name: string;
    }
    export interface GeometryFunctionFunc extends Function {
        prototype: GeometryFunction;
        new (): GeometryFunction;
    }
    var GeometryFunction: GeometryFunctionFunc;

    export interface ListMaker {
        list: $;
        addListItem(text: string, clickHandler: {(arg: jQuery.Event): void}, tag: Object): $;
        makeList(): $;
    }
    export interface ListMakerFunc extends Function {
        prototype: ListMaker;
        new (): ListMaker;
    }
    var ListMaker: ListMakerFunc;

    export interface DemoLauncher {
    }
    export interface DemoLauncherFunc extends Function {
        prototype: DemoLauncher;
        new (): DemoLauncher;
        demoContainer: HTMLElement;
        launch(): void;
        makeList(): void;
        addDemo(v: ThreejsDemo.BaseDemo): void;
        clickDemo(arg: jQuery.Event): void;
    }
    var DemoLauncher: DemoLauncherFunc;

    export enum PanelType {
        panel_default = 0,
        panel_primary = 1,
        panel_success = 2,
        panel_info = 3,
        panel_warning = 4,
        panel_danger = 5
    }

    export interface ParticleBaseDemo extends ThreejsDemo.BaseDemo {
        makeCamera(): void;
        makeLights(): void;
        createRenderer(): void;
        createTrackball(): void;
    }
    export interface ParticleBaseDemoFunc extends Function {
        prototype: ParticleBaseDemo;
        new (name: string, category: string): ParticleBaseDemo;
    }
    var ParticleBaseDemo: ParticleBaseDemoFunc;

    export interface App {
    }
    export interface AppFunc extends Function {
        prototype: App;
        new (): App;
        main(): void;
    }
    var App: AppFunc;
}
