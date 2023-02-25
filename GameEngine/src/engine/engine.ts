export class Engine{
    readonly canvas: HTMLCanvasElement;
    readonly gl: WebGLRenderingContext;
    constructor(canvas: HTMLCanvasElement){
        this.canvas = canvas;
        this.gl = this.canvas.getContext("webgl");
    }
}