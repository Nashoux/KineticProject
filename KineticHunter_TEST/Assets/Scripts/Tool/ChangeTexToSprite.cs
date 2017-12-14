using UnityEngine;
using System.Collections;
using UnityEditor;

public class ChangeTexToSprite : AssetPostprocessor {

    void OnPostprocessTexture(Texture2D texture)
    {
        string lowerCaseAssetPath = assetPath.ToLower ();

        if (lowerCaseAssetPath.IndexOf ("/UI/") != -1) 
        {
            TextureImporter textureImporter = (TextureImporter) assetImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
             textureImporter.spriteImportMode = SpriteImportMode.Single;
             textureImporter.mipmapEnabled = false;
             textureImporter.maxTextureSize = 512;
             textureImporter.alphaSource = TextureImporterAlphaSource.FromInput;
             textureImporter.alphaIsTransparency = true;
             textureImporter.npotScale = TextureImporterNPOTScale.ToNearest;
        }else {
            TextureImporter textureImporter = (TextureImporter) assetImporter;
            textureImporter.textureType = TextureImporterType.Default;
        }
    }



}