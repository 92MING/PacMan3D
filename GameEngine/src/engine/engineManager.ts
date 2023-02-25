import { Engine } from "./engine";

export abstract class EngineManager{

    protected readonly engine: Engine;
    protected readonly gl: WebGLRenderingContext;
    protected readonly canvas: HTMLCanvasElement;
    protected _managers: { [engineID: number]: EngineManager[] } = {};

    constructor(engine: Engine){
        this.engine = engine;
    }

    public abstract delete(): void;

}

