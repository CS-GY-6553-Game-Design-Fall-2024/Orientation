using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0f,1f)] public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    [Range(0f,1f)] public float falloffStart = 1f;
    [Range(0f,1f)] public float falloffEnd = 1f;

    public Vector2 heightRange = new Vector2(-1,5);

    public bool autoUpdate;

    public void GenerateMap() {
        float[,] noiseMap = Noise.GenerateNoiseMap(
            mapWidth, mapHeight, noiseScale,
            seed,
            octaves, persistance, lacunarity,
            offset
        );
        float[,] falloffMap = Noise.GenerateFalloffMap(
            mapWidth, mapHeight, 
            falloffStart, falloffEnd
        );
        float[,] combinedMap = Generators.MultiplyMap(
            mapWidth, mapHeight,
            noiseMap,
            falloffMap
        );

        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawNoiseMap(combinedMap);
    }

    void OnValidate() {
        if (mapWidth < 1) mapWidth = 1;
        if (mapHeight < 1) mapHeight = 1;
        if (lacunarity < 1) lacunarity = 1;
        if (octaves < 0) octaves = 0;
    }
}
