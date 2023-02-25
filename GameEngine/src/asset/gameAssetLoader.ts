import { GameAsset } from "./gameAsset";

export abstract class GameAssetLoader<AssetClass extends GameAsset = GameAsset> {

    readonly supportedExtensions: string[];
    public abstract loadAsset(assetName: string): null | AssetClass;

}