let canvas: HTMLCanvasElement = null;
import vertexShader from '../resources/shader/standardVS.glsl';
import fragmentShader from '../resources/shader/standardFS.glsl';
import {Engine} from './engine/engine';
import {Shader} from './asset';

// when window is loaded
window.onload = () => {
    // create canvas
    canvas = document.createElement("canvas");
    canvas.width = window.innerWidth;
    canvas.height = window.innerHeight;
    canvas.style.width = '' + window.innerWidth;
    canvas.style.height = '' + window.innerHeight;

    document.body.append(canvas);
    window.onresize = function(evt: Event) {
        console.log(evt);
        canvas.width = window.innerWidth ;
        canvas.height = window.innerHeight ;
        canvas.style.width = '' + window.innerWidth;
        canvas.style.height = '' + window.innerHeight;
    };	

    var engine = new Engine(canvas);
	var shader = Shader.CreateShader(engine, "test", vertexShader, fragmentShader);
	shader.use();
	
	let gl = engine.gl;
	let program = shader.program;
	var vertexData: WebGLBuffer= gl.createBuffer();
    let vertices = [
        -0.5, -0.5, 0, // 左下角
        0.5, -0.5, 0, // 右下角
        0, 0.5, 0, // 中上
    ];
	
    gl.bindBuffer(gl.ARRAY_BUFFER, vertexData);
    gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(vertices), gl.STATIC_DRAW);
    let positionLoc = gl.getAttribLocation(program, 'position');
    gl.enableVertexAttribArray(positionLoc);
    gl.vertexAttribPointer(positionLoc, 3, gl.FLOAT, false, 4*3, 0);
    //#endregion

    gl.viewport(0,0,canvas.width,canvas.height);

    render(engine);
}


function render(engine: Engine) {
    // logic code
    engine.gl.clear(engine.gl.COLOR_BUFFER_BIT);
    engine.gl.clearColor(0.0, 0.0, 0.0, 1.0);
    engine.gl.drawArrays(engine.gl.TRIANGLES, 0, 3);
    requestAnimationFrame(()=>render(engine));
}

