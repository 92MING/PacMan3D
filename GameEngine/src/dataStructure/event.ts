export class GameMultiEvent<T extends [...args:any]>{
    private listeners: ((...args: T) => void)[] = [];
    constructor() {}
    public addListener(listener: (...args: T) => void): void {
        if (this.listeners.indexOf(listener) !== -1) return;
        this.listeners.push(listener);
    }
    public removeListener(listener: (...args: T) => void): void {
        this.listeners.splice(this.listeners.indexOf(listener), 1);
    }
    public invoke(...args: T): void {
        for (let i = 0; i < this.listeners.length; i++) {
            this.listeners[i](...args);
        }
    }
}

export class GameEvent<T>{
    private listeners: ((args: T) => void)[] = [];
    constructor() {}
    public addListener(listener: (args: T) => void): void {
        if (this.listeners.indexOf(listener) !== -1) return;
        this.listeners.push(listener);
    }
    public removeListener(listener: (args: T) => void): void {
        this.listeners.splice(this.listeners.indexOf(listener), 1);
    }
    public invoke(args: T): void {
        for (let i = 0; i < this.listeners.length; i++) {
            this.listeners[i](args);
        }
    }
}
