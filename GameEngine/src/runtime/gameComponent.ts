import { GameObject } from "./gameObject";

export abstract class GameComponent{
    
    protected enabled: boolean;
    protected started: boolean = false;
    public readonly gameObject: GameObject;
    
    constructor(gameObject: GameObject, enabled: boolean = true){
        if (gameObject === undefined || gameObject === null) {
            throw new TypeError('GameComponent constructor parameter "gameObject" cannot be null or undefined.');
        }
        this.gameObject = gameObject;
        this.enabled = enabled;
    }

    public Awake(): void{}
    public Start(): void{}
    public FixedUpdate(): void {
        if (!this.started) {
            this.started = true;
            this.Start();
        }
    }
    public Update(): void {
        if (!this.started) {
            this.started = true;
            this.Start();
        }
    }
    public LateUpdate(): void {
        if (!this.started) {
            this.started = true;
            this.Start();
        }
    }
    public OnEnable(): void {}
    public OnDisable(): void {}
    public OnDestroy(): void {}
    
    public get Enabled(): boolean{ return this.enabled; }
    public set Enabled(value: boolean){
        if (value === this.enabled) return;
        this.enabled = value;
        if (this.enabled) this.OnEnable();
        else this.OnDisable();
    }

}

