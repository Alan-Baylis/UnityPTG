using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

    public int mapWidth;
    public int mapHeight;
    public float poissonRadius;
    public int seed;

    public bool autoUpdate;

	public void GenerateMap() {
        PoissonSampler ps = new PoissonSampler(mapWidth, mapHeight, poissonRadius, seed);
        List<Vector2> samples = ps.GeneratePoisson();

        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawPoisson(samples);
    }
}
