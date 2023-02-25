import { mat4, mat3, mat2, vec4, vec3, vec2 } from "gl-matrix";
import { GameAsset } from "./gameAsset";
import { Engine } from "../engine/engine";

export class Shader extends GameAsset<[string,string]>{

    public readonly program: WebGLProgram;
    public readonly vertexShader: WebGLShader;
    public readonly fragmentShader: WebGLShader;
    private static allShaders: { [name: string]: Shader } = {};

    constructor(engine: Engine, shaderName:string, vertexShaderSource: string, fragmentShaderSource: string){
        if (Shader.allShaders[shaderName]) {
            console.log("Shader already exists, returning existing shader");
            return Shader.allShaders[shaderName];
        }
        super(engine, shaderName, [vertexShaderSource, fragmentShaderSource]);
        Shader.allShaders[shaderName] = this;
        var gl = engine.gl;
        this.program = gl.createProgram();
        this.vertexShader = gl.createShader(gl.VERTEX_SHADER);
        this.fragmentShader = gl.createShader(gl.FRAGMENT_SHADER);
        try{
            gl.shaderSource(this.vertexShader, vertexShaderSource);
            gl.compileShader(this.vertexShader);
            gl.shaderSource(this.fragmentShader, fragmentShaderSource);
            gl.compileShader(this.fragmentShader);
            gl.attachShader(this.program, this.vertexShader);
            gl.attachShader(this.program, this.fragmentShader);
            gl.linkProgram(this.program);
        }
        catch(e){
            console.log("Error creating shader: " + e);
            gl.deleteShader(this.vertexShader);
            gl.deleteShader(this.fragmentShader);
            gl.deleteProgram(this.program);
            delete Shader.allShaders[shaderName];
            return;
        }
    }

    //#region instance methods

    public use(): void {
        this.engine.gl.useProgram(this.program);
    }
    public delete(): void {
        Shader.DeleteShader(this);
    }
    public setMat4(name: string, value: mat4): void {
        var location = this.gl.getUniformLocation(this.program, name);
        this.gl.uniformMatrix4fv(location, false, value);
    }
    public setMat3(name: string, value: mat3): void {
        var location = this.gl.getUniformLocation(this.program, name);
        this.gl.uniformMatrix3fv(location, false, value);
    }
    public setMat2(name: string, value: mat2): void {
        var location = this.gl.getUniformLocation(this.program, name);
        this.gl.uniformMatrix2fv(location, false, value);
    }
    public setVec4(name: string, value: vec4): void {
        var location = this.gl.getUniformLocation(this.program, name);
        this.gl.uniform4fv(location, value);
    }
    public setVec3(name: string, value: vec3): void {
        var location = this.gl.getUniformLocation(this.program, name);
        this.gl.uniform3fv(location, value);
    }
    public setVec2(name: string, value: vec2): void {
        var location = this.gl.getUniformLocation(this.program, name);
        this.gl.uniform2fv(location, value);
    }
    public setFloat(name: string, value: number): void {
        var location = this.gl.getUniformLocation(this.program, name);
        this.gl.uniform1f(location, value);
    }
    public setInt(name: string, value: number): void {
        var location = this.gl.getUniformLocation(this.program, name);
        this.gl.uniform1i(location, value);
    }
    public setBool(name: string, value: boolean): void {
        var location = this.gl.getUniformLocation(this.program, name);
        this.gl.uniform1i(location, value ? 1 : 0);
    }

    //#endregion

    //#region static methods

    public static Use(shader: Shader | string): void {
        var _shader: Shader;
        if (typeof shader === "string") {
            var _shader = Shader.FindShader(shader);
        }
        else _shader = shader;
        _shader.use();
    }
    public static CreateShader(engine: Engine, shaderName: string, vertexShaderSource: string, fragmentShaderSource: string): Shader {
        if (Shader.allShaders[shaderName]) {
            console.log("Shader already exists, returning existing shader");
            return Shader.allShaders[shaderName];
        }
        return new Shader(engine, shaderName, vertexShaderSource, fragmentShaderSource);
    }
    public static FindShader(shaderName: string): Shader {
        if (!Shader.allShaders[shaderName]) {
            console.log("Shader not found");
            return null;
        }
        return Shader.allShaders[shaderName];
    }
    public static DeleteShader(shader: Shader | string): void {
        var _shader: Shader;
        if (typeof shader === "string") {
            _shader = Shader.FindShader(shader);
            if (!_shader) return;
        }
        else _shader = shader;
        var gl = _shader.engine.gl;
        gl.deleteShader(gl.getAttachedShaders(_shader.program)[0]);
        gl.deleteShader(gl.getAttachedShaders(_shader.program)[1]);
        gl.deleteProgram(_shader.program);
        delete Shader.allShaders[_shader.name];
    }
    public static DeleteShader_ByName(shaderName: string): void {
        Shader.DeleteShader(Shader.FindShader(shaderName));
    }
    
    //#endregion
}