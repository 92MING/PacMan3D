import { GameAsset } from "./gameAsset";
import { GameAssetLoader } from "./gameAssetLoader";

export class GameAssetManager {

    private static _loaders: GameAssetLoader[] = [];
    private static _loadedAssets: { [name: string]: GameAsset } = {};

    private constructor() {}

    public static initialize(): void {}

    public static registerLoader(loader: GameAssetLoader): void {
        GameAssetManager._loaders.push(loader);
    }

    public static onAssetLoaded(asset: GameAsset): void {
        GameAssetManager._loadedAssets[asset.name] = asset;
    }

    /**
     * Attempts to load an asset using a registered asset loader.
     * @param assetName The name/url of the asset to be loaded.
     */
    public static loadAsset(assetName: string): void {
        let extension = assetName.split('.').pop().toLowerCase();
        for (let l of GameAssetManager._loaders) {
            if (l.supportedExtensions.indexOf(extension) !== -1) {
                l.loadAsset(assetName);
                return;
            }
        }

        console.warn("Unable to load asset with extension " + extension + " because there is no loader associated with it.");
    }

    /**
     * Indicates if an asset with the provided name has been loaded.
     * @param assetName The asset name to check.
     */
    public static isAssetLoaded(assetName: string): boolean {
        return GameAssetManager._loadedAssets[assetName] !== undefined;
    }

    /**
     * Attempts to get an asset with the provided name. If found, it is returned; otherwise, undefined is returned.
     * @param assetName The asset name to get.
     */
    public static getAsset(assetName: string): IAsset {
        if (AssetManager._loadedAssets[assetName] !== undefined) {
            return AssetManager._loadedAssets[assetName];
        } else {
            AssetManager.loadAsset(assetName);
        }

        return undefined;
    }
}