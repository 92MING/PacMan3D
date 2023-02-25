import { Engine } from "../engine/engine";
import { EngineObject } from "../engine/engineObject";

export abstract class GameAsset<DataType = any> extends EngineObject{
    public readonly name: string;
    public readonly data: DataType;
    constructor(engine: Engine, name: string, data: DataType){
        super(engine);
        this.name = name;
        this.data = data;
    }
    public abstract delete(): void;
}