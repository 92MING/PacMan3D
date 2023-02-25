import { Engine } from "./engine";

export abstract class EngineObject{
    public readonly engine: Engine;
    public readonly gl: WebGLRenderingContext;
    public readonly canvas: HTMLCanvasElement;
    constructor(engine: Engine){
        this.engine = engine;
        this.gl = engine.gl;
        this.canvas = engine.canvas;
    }
}