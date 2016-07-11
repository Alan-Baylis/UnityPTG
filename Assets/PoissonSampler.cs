using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoissonSampler {

    int width, height;
    float radius;
    int seed;

    Vector2[,] grid;
    List<Vector2> processList;
    float cellSize;
    float radius2;

    Rect bounds;

    int maxAttempts = 10;

    List<Vector2> samples = new List<Vector2>();

    public PoissonSampler(int width, int height, float radius, int seed) {
        this.width = width;
        this.height = height;
        this.radius2 = radius * radius;
        this.cellSize = radius / Mathf.Sqrt(2);
        Random.seed = seed;
        this.bounds = new Rect(0, 0, width, height);
    }

    public List<Vector2> GeneratePoisson() {
        grid = new Vector2[Mathf.CeilToInt(width / cellSize), Mathf.CeilToInt(height / cellSize)];
        processList = new List<Vector2>();

        // Create the first point randomly
        NewSample(new Vector2(Random.Range(0, width), Random.Range(0, height)));

        while(processList.Count > 0) {
            int i = Random.Range(0, processList.Count);
            Vector2 sample = processList[i];

            bool success = false;
            for(int n = 0; n < maxAttempts; ++n) {
                // Generate a candidate point
                float angle = 2 * Mathf.PI * Random.value;
                float r = Mathf.Sqrt(Random.value * 3 * radius2 + radius2);
                Vector2 candidate = sample + r * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                // Check the candidate is valid
                if(bounds.Contains(candidate) && ValidRange(candidate)) {
                    success = true;
                    NewSample(candidate);
                    break;
                }
            }

            if(!success) {
                // Remove failed sample from list
                //Debug.Log("candidate failed");
                processList[i] = processList[processList.Count - 1];
                processList.RemoveAt(processList.Count - 1);
            }
        }

        return samples;
    }

    private bool ValidRange(Vector2 sample) {
        GridPosition pos = new GridPosition(sample, cellSize);

        int xmin = Mathf.Max(pos.x - 2, 0);
        int ymin = Mathf.Max(pos.y - 2, 0);
        int xmax = Mathf.Min(pos.x + 2, grid.GetLength(0) - 1);
        int ymax = Mathf.Min(pos.y + 2, grid.GetLength(1) - 1);

        for(int y = ymin; y <= ymax; y++) {
            for(int x = xmin; x <= xmax; x++) {
                Vector2 s = grid[x, y];
                if(s != Vector2.zero) {
                    Vector2 diff = s - sample;
                    if(diff.x * diff.x + diff.y * diff.y < radius2) {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    private Vector2 NewSample(Vector2 sample) {
        processList.Add(sample);
        GridPosition pos = new GridPosition(sample, cellSize);
        grid[pos.x, pos.y] = sample;
        samples.Add(sample);

        return sample;
    }

    private struct GridPosition {
        public int x;
        public int y;

        public GridPosition(Vector2 sample, float cellSize) {
            x = (int)(sample.x / cellSize);
            y = (int)(sample.y / cellSize);
        }
    }
}
