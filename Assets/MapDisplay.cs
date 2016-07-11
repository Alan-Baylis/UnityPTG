using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapDisplay : MonoBehaviour {
    public Renderer textureRenderer;

    public MapGenerator mapGenerator;

    public void DrawPoisson(List<Vector2> samples) {
        int width = mapGenerator.mapWidth;
        int height = mapGenerator.mapHeight;

        Texture2D texture = new Texture2D(width, height);
        /*
        Color[] colourMap = new Color[width * height];
        for(int y = 0; y < height; y++) {
            for(int x = 0; x < width; x++) {
                colourMap[y * width + x] = Color.white;
            }
        }
        */

        foreach(Vector2 sample in samples) {
            texture.SetPixel((int)sample.x, (int)sample.y, Color.red);
        }
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(width, 1, height);
    }
}
