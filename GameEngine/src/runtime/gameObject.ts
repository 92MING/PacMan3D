import { GameComponent } from "./gameComponent";
import { Transform } from "./transform";

export class GameObject{

    private _name: string;
    private _tag: string;
    private _parent: GameObject | null;
    private _children: GameObject[] = [];
    public _active: boolean = true;
    private _components: GameComponent[] = [];
    private _transform: Transform;
    private static allGameObjects_name: { [name: string]: GameObject[] } = {};
    private static allGameObjects_tag: { [tag: string]: GameObject[] } = {};
    private static rootGameObjects: GameObject[] = [];

    constructor(parent: GameObject=null, name: string="", active: boolean=true){
        this.Parent = parent;
        this.Name = name;
        this.Active = active;
    }

    //#region instance methods

    public get Name(): string{ return this._name; }
    public set Name(value: string){
        if (this._name === value) return;
        this._name = value;
        if (GameObject.allGameObjects_name[value]) {
            GameObject.allGameObjects_name[value].push(this);
        }
        else {
            GameObject.allGameObjects_name[value] = [this];
        }
    }
    public find(path: string):GameObject{
        if (path === "") return this;
        var pathArray = path.split("/");
        if (pathArray.length == 1) {
            for (let i = 0; i < this._children.length; i++) {
                if (this._children[i].Name === pathArray[0]) return this._children[i];
            }
            return null;
        }
        else{
            for (let i = 0; i < this._children.length; i++) {
                if (this._children[i].Name === pathArray[0]) return this._children[i].find(pathArray.slice(1).join("/"));
            }
        }
    }
    public get Tag(): string{ return this._tag; }
    public set Tag(value: string){
        if (this._tag === value) return;
        this._tag = value;
        if (GameObject.allGameObjects_tag[value]) {
            GameObject.allGameObjects_tag[value].push(this);
        }
        else {
            GameObject.allGameObjects_tag[value] = [this];
        }
    }

    public get Parent(): GameObject{ return this._parent; }
    public set Parent(value: GameObject | null){
        if (this._parent === value) return;
        if (this._parent) {
            this._parent._children.splice(this._parent._children.indexOf(this), 1);
        }
        else{
            GameObject.rootGameObjects.splice(GameObject.rootGameObjects.indexOf(this), 1);
        }
        this._parent = value;
        if (this._parent) {
            this._parent._children.push(this);
        }
        else{
            GameObject.rootGameObjects.push(this);
        }
    }
    public get Children(): GameObject[] { return this._children; }
    public AddChild(child: GameObject): void{
        if (child._parent) {
            child.Parent.RemoveChild(child);
        }
        else{
            GameObject.rootGameObjects.splice(GameObject.rootGameObjects.indexOf(child), 1);
        }
        this._children.push(child);
        child._parent = this;
    }
    public RemoveChild(child: GameObject): void{
        if (child._parent !== this) return;
        this._children.splice(this._children.indexOf(child), 1);
        child._parent = null;
        GameObject.rootGameObjects.push(child);
    }
    public changeSiblingIndex(index: number): void{
        var parentCildren: GameObject[];
        if (this._parent) {
            parentCildren = this._parent._children;
        }
        else{
            parentCildren = GameObject.rootGameObjects;
        }
        if (index < 0 || index >= parentCildren.length) return;
        parentCildren.splice(parentCildren.indexOf(this), 1);
        parentCildren.splice(index, 0, this);
    }

    public get Active(): boolean{ return this._active; }
    public set Active(value: boolean){
        if (this._active === value) return;
        this._active = value;
        this._components.forEach((component) => {
            if (component.Enabled) {
                if (value) component.OnEnable();
                else component.OnDisable();
            }
        });
        this._children.forEach((child) => {
            child.Active = value;
        });
    }

    public get Components(): GameComponent[] { return this._components; }
    public AddNewComponent<T extends GameComponent>(enabled: boolean = true,...args:any): T{
        let component = new(this,enabled, args) as T;
        this._components.push(component);
        return component;
    }
    public AddComponent<T extends GameComponent>(component: T): T{
        if (this._components.indexOf(component) !== -1) return component;
        if (component.gameObject) component.gameObject.RemoveComponent(component);
        this._components.push(component);
        return component;
    }
    public GetComponent<T extends GameComponent>(type: { new(...args:any): T }): T{
        for (let i = 0; i < this._components.length; i++) {
            if (this._components[i] instanceof type) return this._components[i] as T;
        }
        return null;
    }
    public GetComponents<T extends GameComponent>(type: { new(...args:any): T }): T[]{
        let components: T[] = [];
        for (let i = 0; i < this._components.length; i++) {
            if (this._components[i] instanceof type) components.push(this._components[i] as T);
        }
        return components;
    }
    public GetComponentsInChildren<T extends GameComponent>(type: { new(...args:any): T }): T[]{
        let components: T[] = [];
        for (let i = 0; i < this._components.length; i++) {
            if (this._components[i] instanceof type) components.push(this._components[i] as T);
        }
        for (let i = 0; i < this._children.length; i++) {
            components.push(...this._children[i].GetComponentsInChildren(type));
        }
        return components;
    }
    public GetComponentsInParent<T extends GameComponent>(type: { new(...args:any): T }): T[]{
        let components: T[] = [];
        for (let i = 0; i < this._components.length; i++) {
            if (this._components[i] instanceof type) components.push(this._components[i] as T);
        }
        if (this._parent) components.push(...this._parent.GetComponentsInParent(type));
        return components;
    }
    public GetComponentInChildren<T extends GameComponent>(type: { new(...args:any): T }): T{
        for (let i = 0; i < this._components.length; i++) {
            if (this._components[i] instanceof type) return this._components[i] as T;
        }
        for (let i = 0; i < this._children.length; i++) {
            let component = this._children[i].GetComponentInChildren(type);
            if (component) return component;
        }
        return null;
    }
    public GetComponentInParent<T extends GameComponent>(type: { new(...args:any): T }): T{
        for (let i = 0; i < this._components.length; i++) {
            if (this._components[i] instanceof type) return this._components[i] as T;
        }
        if (this._parent) return this._parent.GetComponentInParent(type);
        return null;
    }
    public RemoveComponent<T extends GameComponent>(component: T): void{
        if (this._components.indexOf(component) === -1) return;
        var index = this._components.indexOf(component);
        this._components[index].OnDestroy();
        delete this._components[index];
    }
    public RemoveComponent_ByType<T extends GameComponent>(type: { new(...args:any): T }): void{
        for (let i = 0; i < this._components.length; i++) {
            if (this._components[i] instanceof type) {
                this._components[i].OnDestroy();
                delete this._components[i];
                return;
            }
        }
    }
    public RemoveComponents_ByType<T extends GameComponent>(type: { new(...args:any): T }): void{
        for (let i = this._components.length-1; i >=0; i--) {
            if (this._components[i] instanceof type) {
                this._components[i].OnDestroy();
                delete this._components[i];
            }
        }
    }
    public get transform(): Transform{ 
        if (!this._transform) this._transform = this.AddNewComponent<Transform>();
        return this._transform; 
    }

    public FixedUpdate(): void{
        if (!this._active) return;
        for (let i = 0; i < this._components.length; i++) {
            if (this._components[i].Enabled) this._components[i].FixedUpdate();
        }
    }
    public Update(): void{
        if (!this._active) return;
        for (let i = 0; i < this._components.length; i++) {
            if (this._components[i].Enabled) this._components[i].Update();
        }
    }
    public LateUpdate(): void{
        if (!this._active) return;
        for (let i = 0; i < this._components.length; i++) {
            if (this._components[i].Enabled) this._components[i].LateUpdate();
        }
    }
    public Destroy(): void{
        for (let i = this._components.length-1; i >=0; i--) {
            this._components.splice(i, 1)[0].OnDestroy();
            delete this._components[i];
        }
        for (let i = this._children.length-1; i >=0; i--) {
            this._children.splice(i, 1)[0].Destroy();
            delete this._children[i];
        }
        if (this._parent) this._parent.RemoveChild(this);
        else GameObject.rootGameObjects.splice(GameObject.rootGameObjects.indexOf(this), 1);
        GameObject.allGameObjects_tag[this._tag].splice(GameObject.allGameObjects_tag[this._tag].indexOf(this), 1);
        delete GameObject.allGameObjects_name[this._name][GameObject.allGameObjects_name[this._name].indexOf(this)];
    }

    //#endregion

    //#region static methods
    
    public static Find(name: string): GameObject{
        return GameObject.allGameObjects_name[name][0];
    }
    public static FindAll(name: string): GameObject[]{
        return GameObject.allGameObjects_name[name];
    }
    public static FindWithTag(tag: string): GameObject{
        return GameObject.allGameObjects_tag[tag][0];
    }
    public static FindGameObjectsWithTag(tag: string): GameObject[]{
        return GameObject.allGameObjects_tag[tag];
    }
    public static RootGameObjects(): GameObject[]{ return GameObject.rootGameObjects; }

    //#endregion
}