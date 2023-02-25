import { vec3, mat4 } from "gl-matrix";
import { GameComponent } from "./gameComponent";
import { GameObject } from "./gameObject";

export class Transform extends GameComponent{

    private _localPosition: vec3 = vec3.fromValues(0,0,0);
    private _localScale: vec3 = vec3.fromValues(1,1,1);

    private _localFoward: vec3 = vec3.fromValues(0,0,1);
    private _localUp: vec3 = vec3.fromValues(0,1,0);
    private _localRight: vec3 = vec3.fromValues(1,0,0);
    
    private _fowardAxis: vec3 = vec3.fromValues(0,0,1);
    private _upAxis: vec3 = vec3.fromValues(0,1,0);
    private _rightAxis: vec3 = vec3.fromValues(1,0,0);

    constructor(gameObject: GameObject){
        super(gameObject, true);
    }
    public set Enabled(value: boolean){
        if (!value) throw new Error('Transform cannot be disabled.');
    }
    public get parentTransform(): Transform{
        if (this.gameObject.Parent === null) return null;
        return this.gameObject.Parent.transform;
    }

    //#region axis
    public get FowardAxis(): vec3{ return this._fowardAxis; }
    public get UpAxis(): vec3{ return this._upAxis; }
    public get RightAxis(): vec3{ return this._rightAxis; }
    public SetFowardUpAxis(foward: vec3, up: vec3){
        vec3.normalize(this._fowardAxis, foward);
        vec3.cross(this._rightAxis, this._fowardAxis, up);
        vec3.normalize(this._rightAxis, this._rightAxis);
        vec3.cross(this._upAxis, this._rightAxis, this._fowardAxis);
    }
    public SetFowardRightAxis(foward: vec3, right: vec3){
        vec3.normalize(this._fowardAxis, foward);
        vec3.cross(this._upAxis, right, this._fowardAxis);
        vec3.normalize(this._upAxis, this._upAxis);
        vec3.cross(this._rightAxis, this._fowardAxis, this._upAxis);
    }
    public SetUpRightAxis(up: vec3, right: vec3){
        vec3.normalize(this._upAxis, up);
        vec3.cross(this._fowardAxis, this._upAxis, right);
        vec3.normalize(this._fowardAxis, this._fowardAxis);
        vec3.cross(this._rightAxis, this._fowardAxis, this._upAxis);
    }
    //#endregion
    
    //#region transform/inverseTransform
    public transformPoint(point: vec3): vec3{
        let pos = vec3.create();
        vec3.transformMat4(pos, point, this.modelMatrix);
        return pos;
    }
    public inverseTransformPoint(point: vec3): vec3{
        let pos = vec3.create();
        vec3.transformMat4(pos, point, this.inverseModelMatrix);
        return pos;
    }
    public transformDirection(dir: vec3): vec3{
        let pos = vec3.create();
        vec3.transformMat4(pos, dir, this.globalRotationMatrix);
        vec3.normalize(pos, pos);
        return pos;
    }
    public inverseTransformDirection(dir: vec3): vec3{
        let pos = vec3.create();
        vec3.transformMat4(pos, dir, this.inverseGlobalRotationMatrix);
        vec3.normalize(pos, pos);
        return pos;
    }
    //#endregion
    
    //#region localPos/localRot/localScale
    public get localPosition(): vec3{ return this._localPosition; }
    public set localPosition(value: vec3){
        this._localPosition = value;
    }
    public get localRotation_radius(): vec3{ 
        let euler = vec3.create();
        euler[0] = Math.asin(this._localFoward[1]);
        euler[1] = Math.atan2(this._localFoward[0], this._localFoward[2]);
        euler[2] = Math.atan2(this._localFoward[1], this._localFoward[2]);
        //prevebt NaN
        if (isNaN(euler[0])) euler[0] = 0;
        if (isNaN(euler[1])) euler[1] = 0;
        if (isNaN(euler[2])) euler[2] = 0;
        return euler;
    }
    public get localRotation_degree(): vec3{
        var euler = this.localRotation_radius;
        euler[0] *= 180 / Math.PI;
        euler[1] *= 180 / Math.PI;
        euler[2] *= 180 / Math.PI;
        return euler;
    }
    public set localRotation_radius(value: vec3){
        this._localFoward[0] = Math.cos(value[0]) * Math.cos(value[1]);
        this._localFoward[1] = Math.sin(value[0]);
        this._localFoward[2] = Math.cos(value[0]) * Math.sin(value[1]);
        vec3.normalize(this._localFoward, this._localFoward);
    }
    public set localRotation_degree(value: vec3){
        let euler = vec3.create();
        euler[0] = value[0] * Math.PI / 180;
        euler[1] = value[1] * Math.PI / 180;
        euler[2] = value[2] * Math.PI / 180;
        this.localRotation_radius = euler;
    }
    public get localScale(): vec3{ return this._localScale; }
    public set localScale(value: vec3){
        this._localScale = value;
    }
    //#endregion

    //#region localForward/localUp/localRight
    
    public get localForward(): vec3{ return this._localFoward;}
    public get localUp(): vec3{ return this._localUp; }
    public get localRight(): vec3{ return this._localRight; }
    public set localForward(value: vec3){
        vec3.normalize(this._localFoward, value);
        vec3.cross(this._localRight, this._localFoward, this._upAxis);
        vec3.normalize(this._localRight, this._localRight);
        vec3.cross(this._localUp, this._localRight, this._localFoward);
    }
    public set localUp(value: vec3){
        vec3.normalize(this._localUp, value);
        vec3.cross(this._localFoward, this._localUp, this._rightAxis);
        vec3.normalize(this._localFoward, this._localFoward);
        vec3.cross(this._localRight, this._localFoward, this._localUp);
    }
    public set localRight(value: vec3){
        vec3.normalize(this._localRight, value);
        vec3.cross(this._localUp, this._localRight, this._fowardAxis);
        vec3.normalize(this._localUp, this._localUp);
        vec3.cross(this._localFoward, this._localUp, this._localRight);
    }
    //#endregion

    //#region globalForward/globalUp/globalRight
    public get globalForward(): vec3{ return this.transformDirection(this._localFoward);}
    public get globalUp(): vec3{ return this.transformDirection(this._localUp); }
    public get globalRight(): vec3{ return this.transformDirection(this._localRight); }
    //#endregion

    //#region globalPosition/globalRot/globalScale
    public get globalPosition(): vec3{
        return this.transformPoint(this._localPosition);
    }
    public set globalPosition(value: vec3){
        this._localPosition = this.inverseTransformPoint(value);
    }
    public get globalRotation_radius(): vec3{
        let euler = vec3.create();
        var f = this.globalForward;
        euler[0] = Math.asin(f[1]);
        euler[1] = Math.atan2(f[0], f[2]);
        euler[2] = Math.atan2(f[1], f[2]);
        //prevebt NaN
        if (isNaN(euler[0])) euler[0] = 0;
        if (isNaN(euler[1])) euler[1] = 0;
        if (isNaN(euler[2])) euler[2] = 0;
        return euler;
    }
    public get globalRotation_degree(): vec3{
        var euler = this.globalRotation_radius;
        euler[0] *= 180 / Math.PI;
        euler[1] *= 180 / Math.PI;
        euler[2] *= 180 / Math.PI;
        return euler;
    }
    public set globalRotation_radius(value: vec3){
        var globalF = vec3.create();
        globalF[0] = Math.cos(value[0]) * Math.cos(value[1]);
        globalF[1] = Math.sin(value[0]);
        globalF[2] = Math.cos(value[0]) * Math.sin(value[1]);
        this.localForward = this.inverseTransformDirection(globalF);
    }
    public set globalRotation_degree(value: vec3){
        let euler = vec3.create();
        euler[0] = value[0] * Math.PI / 180;
        euler[1] = value[1] * Math.PI / 180;
        euler[2] = value[2] * Math.PI / 180;
        this.globalRotation_radius = euler;
    }
    public get globalScale(): vec3{
        return vec3.transformMat4(vec3.create(), this._localScale, this.globalScaleMatrix);
    }
    public set globalScale(value: vec3){
        this._localScale = vec3.transformMat4(vec3.create(), value, this.inverseGlobalScaleMatrix);
    }

    //#endregion
    
    //#region LookAt/translate/rotate
    public lookAt(target: vec3, up: vec3 = this._upAxis){
        let f = vec3.create();
        vec3.sub(f, target, this.globalPosition);
        vec3.normalize(this.localForward, f);
    }
    public translate(value: vec3){
        vec3.add(this._localPosition, this._localPosition, value);
    }
    public rotate(globalRotationAxis: vec3, globalRotationCenter:vec3, rotationAngle:number, isRadius:boolean=false){
        var rotateAngle = rotationAngle;
        if(!isRadius) rotateAngle *= Math.PI / 180;
        let rotationMatrix = mat4.create();
        mat4.rotate(rotationMatrix, rotationMatrix, rotateAngle, globalRotationAxis);
        let globalPosition = this.globalPosition;
        vec3.sub(globalPosition, globalPosition, globalRotationCenter);
        vec3.transformMat4(globalPosition, globalPosition, rotationMatrix);
        vec3.add(globalPosition, globalPosition, globalRotationCenter);
        this.globalPosition = globalPosition;

        var newForward = vec3.create();
        vec3.transformMat4(newForward, this._localFoward, rotationMatrix);
        this.localForward = newForward;
    }
    public rotateSelf(localRotationAxis: vec3, rotationAngle:number, isRadius:boolean=false){
        return this.rotate(localRotationAxis, this.globalPosition, rotationAngle, isRadius);
    }
    //#endregion

    //#region matrix

    public get localTranslationMatrix(): mat4{
        let mat = mat4.create();
        mat4.translate(mat, mat, this._localPosition);
        return mat;
    }
    public get localRotationMatrix(): mat4{
        let mat = mat4.create();
        mat = [
            this._localRight[0], this._localRight[1], this._localRight[2], 0,
            this._localUp[0], this._localUp[1], this._localUp[2], 0,
            this._localFoward[0], this._localFoward[1], this._localFoward[2], 0,
            0, 0, 0, 1
        ]
        return mat;
    }
    public get localScaleMatrix(): mat4{
        let mat = mat4.create();
        mat4.scale(mat, mat, this._localScale);
        return mat;
    }
    public get globalScaleMatrix(): mat4{
        let mat = mat4.create();
        if (this.gameObject.Parent != null){
            return mat4.multiply(mat, this.parentTransform.globalScaleMatrix, this.localScaleMatrix);
        }
        else{
            return this.localScaleMatrix;
        }
        return mat;
    }
    public get inverseGlobalScaleMatrix(): mat4{
        let mat = mat4.create();
        mat4.invert(mat, this.globalScaleMatrix);
        return mat;
    }
    public get globalRotationMatrix(): mat4{
        let mat = mat4.create();
        if (this.gameObject.Parent != null){
            return mat4.multiply(mat, this.parentTransform.globalRotationMatrix, this.localRotationMatrix);
        }
        else{
            return this.localRotationMatrix;
        }
    }
    public get inverseGlobalRotationMatrix(): mat4{
        let mat = mat4.create();
        mat4.invert(mat, this.globalRotationMatrix);
        return mat;
    }
    public get localModelMatrix(): mat4{
        let mat = mat4.create();
        mat4.multiply(mat, this.localTranslationMatrix, this.localRotationMatrix);
        mat4.multiply(mat, mat, this.localScaleMatrix);
        return mat;
    }
    public get modelMatrix(): mat4{
        let mat = mat4.create();
        if (this.gameObject.Parent != null){
            return mat4.multiply(mat, this.parentTransform.modelMatrix, this.localModelMatrix);
        }else{
            return this.localModelMatrix;
        }
    }
    public get inverseModelMatrix(): mat4{
        let mat = mat4.create();
        mat4.invert(mat, this.modelMatrix);
        return mat;
    }

    //#endregion

}