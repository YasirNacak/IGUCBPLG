using UnityEngine;

public static class Noise {
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        for(int i=0; i<octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0) scale = 0.0001f;

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2.0f;
        float halfHeight = mapHeight / 2.0f;

        for(int i=0; i<mapHeight; i++)
        {
            for(int j=0; j<mapWidth; j++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for(int k=0; k<octaves; k++)
                {
                    float sampleX = (i - halfWidth) / scale * frequency + octaveOffsets[k].x;
                    float sampleY = (j - halfHeight) / scale * frequency + octaveOffsets[k].y;
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    //perlinValue = Random.Range(0.0f, 1.0f) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                noiseMap[i, j] = noiseHeight;
            }
        }

        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                noiseMap[i, j] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[i,j]);
            }
        }
        return noiseMap;
    }
}
