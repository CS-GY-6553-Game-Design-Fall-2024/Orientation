using System.Collections;
using UnityEngine;

public static class Noise {
    public static float[,] GenerateNoiseMap(
            int mapWidth, int mapHeight, float scale,
            int seed,
            int octaves, float persistance, float lacunarity,
            Vector2 offset
    ) {
        float[,] noiseMap = new float[mapWidth,mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for(int i = 0; i < octaves; i++) {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0f) scale = 0.0001f;

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth/2f;
        float halfHeight = mapHeight/2f;

        for(int y = 0; y < mapHeight; y++) {
            for(int x = 0; x < mapWidth; x++) {
                
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int o = 0; o < octaves; o++) {
                    float sampleX = (float)(x-halfWidth) / scale * frequency + octaveOffsets[o].x * frequency;
                    float sampleY = (float)(y-halfHeight) / scale * frequency - octaveOffsets[o].y * frequency;
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
                if (noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;
                noiseMap[x,y] = noiseHeight;
            }
        }

        for(int y = 0; y < mapHeight; y++) {
            for(int x = 0; x < mapWidth; x++) {
                noiseMap[x,y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x,y]);
            }
        }

        return noiseMap;
    }

    public static float[,] GenerateFalloffMap(int mapWidth, int mapHeight, float falloffStart, float falloffEnd) {
        float[,] heightMap = new float[mapWidth, mapHeight];

        float halfWidth = mapWidth/2f;
        float halfHeight = mapHeight/2f;
        
        for(int y = 0; y < mapHeight; y++) {
            for(int x = 0; x < mapWidth; x++) {
                float xPos = (float)(x) / mapWidth * 2f - 1;
                float yPos = (float)(y) / mapHeight * 2f - 1;

                // Find which value is closer to the edge
                float t = Mathf.Max(Mathf.Abs(xPos), Mathf.Abs(yPos));
                if (t < falloffStart) heightMap[x,y] = 1;
                else if (t > falloffEnd) heightMap[x,y] = 0;
                else heightMap[x,y] = Mathf.SmoothStep(1,0,Mathf.InverseLerp(falloffStart, falloffEnd, t));
            }
        }

        return heightMap;
    }

    public static float[,] MultiplyMap(int mapWidth, int mapHeight, float[,] firstMap, float[,] secondMap) {
        float[,] multMap = new float[mapWidth, mapHeight];
        
        for(int y = 0; y < mapHeight; y++) {
            for(int x = 0; x < mapWidth; x++) {
                multMap[x,y] = firstMap[x,y] * secondMap[x,y];
            }
        }

        return multMap;
    }
}
