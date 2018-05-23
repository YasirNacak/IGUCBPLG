using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MapGenerator : MonoBehaviour
{

    public enum DrawMode { NoiseMap, ColorMap, BiomeMap, MixedMap, BiomeMesh, HeightMesh, MixedMesh };
    public DrawMode drawMode;

    [Range(20, 200)]
    public int mapSize;

    [Range(10, 40)]
    public float noiseScale;

    [Range(1, 6)]
    public int octaves;

    [Range(0.25f, 0.75f)]
    public float persistance;

    [Range(1, 2)]
    public float lacunarity;

    [Range(1, 512)]
    public int seed;

    public Vector2 offset;

    [Range(1, 40)]
    public int meshHeightMultiplier;

    [Range(0, 1)]
    public float waterLevel = 0.3f;

    public bool autoUpdate;

    public BiomeType[] biomeRegions;
    public HeightType[] heightRegions;

    public GameObject meshObject;
    public GameObject waterObject;
    public GameObject planeObject;

    [HideInInspector]
    public float[,] noiseMap;

    [HideInInspector]
    public bool isCreating = true;

    private World world;

    public void GenerateMap()
    {
        noiseMap = Noise.GenerateNoiseMap(mapSize, mapSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] biomeColorMap = new Color[mapSize * mapSize];
        Color[] heightColorMap = new Color[mapSize * mapSize];
        Color[] mixedColorMap = new Color[mapSize * mapSize];

        float deltaHeight = (1.0f - waterLevel) / 5;

        heightRegions[0].height = waterLevel;

        for (int i = 1; i < 6; i++)
        {
            heightRegions[i].height = waterLevel + i * deltaHeight;
        }

        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                float currentHeight = noiseMap[i, j];
                for (int k = 0; k < heightRegions.Length; k++)
                {
                    if (currentHeight <= heightRegions[k].height)
                    {
                        heightColorMap[i * mapSize + j] = heightRegions[k].color;
                        break;
                    }
                }
            }
        }

        world = new World();

        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                float currentBiome = world.grid[new Coord(i, j)];

                for (int k = 0; k < biomeRegions.Length; k++)
                {
                    if (currentBiome == biomeRegions[k].biomeValue)
                    {
                        biomeColorMap[i * mapSize + j] = biomeRegions[k].color;
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                float currentBiome = world.grid[new Coord(i, j)];

                for (int k = 0; k < biomeRegions.Length; k++)
                {
                    if (currentBiome == biomeRegions[k].biomeValue)
                    {
                        float cMR = heightColorMap[i * mapSize + j].r;
                        float cMG = heightColorMap[i * mapSize + j].g;
                        float cMB = heightColorMap[i * mapSize + j].b;

                        float bMR = biomeRegions[k].color.r;
                        float bMG = biomeRegions[k].color.g;
                        float bMB = biomeRegions[k].color.b;

                        Color final = new Color(Mathf.Sqrt((cMR * cMR + bMR * bMR) / 2), Mathf.Sqrt((cMG * cMG + bMG * bMG) / 2), Mathf.Sqrt((cMB * cMB + bMB * bMB) / 2), 1.0f);

                        mixedColorMap[i * mapSize + j] = final;
                        break;
                    }
                }
            }
        }

        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();

        switch (drawMode)
        {
            case DrawMode.NoiseMap: mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap)); break;
            case DrawMode.ColorMap: mapDisplay.DrawTexture(TextureGenerator.TextureFromColorMap(heightColorMap, mapSize, mapSize)); break;
            case DrawMode.BiomeMap: mapDisplay.DrawTexture(TextureGenerator.TextureFromColorMap(biomeColorMap, mapSize, mapSize)); break;
            case DrawMode.MixedMap: mapDisplay.DrawTexture(TextureGenerator.TextureFromColorMap(mixedColorMap, mapSize, mapSize)); break;
            case DrawMode.HeightMesh: mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier), TextureGenerator.TextureFromColorMap(heightColorMap, mapSize, mapSize)); break;
            case DrawMode.BiomeMesh: mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier), TextureGenerator.TextureFromColorMap(biomeColorMap, mapSize, mapSize)); break;
            case DrawMode.MixedMesh: mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier), TextureGenerator.TextureFromColorMap(mixedColorMap, mapSize, mapSize)); break;
        }
    }

    private void OnValidate()
    {
        if (mapSize < 1) { mapSize = 1; }
        if (mapSize < 1) { mapSize = 1; }
        if (lacunarity < 1) { lacunarity = 1; }
        if (octaves < 0) { octaves = 0; }
    }

    public void setSeed()
    {
        seed = (int)GameObject.Find("SeedSlider").GetComponent<Slider>().value;
        GenerateMap();
    }

    public void setMapSize()
    {
        mapSize = (int)GameObject.Find("MapSizeSlider").GetComponent<Slider>().value;
        GenerateMap();
    }

    public void setNoiseScale()
    {
        noiseScale = (float)GameObject.Find("NoiseScaleSlider").GetComponent<Slider>().value;
        GenerateMap();
    }

    public void setOctaves()
    {
        octaves = (int)GameObject.Find("OctavesSlider").GetComponent<Slider>().value;
        GenerateMap();
    }

    public void setPersistance()
    {
        persistance = (float)GameObject.Find("PersistanceSlider").GetComponent<Slider>().value;
        GenerateMap();
    }

    public void setLacunarity()
    {
        lacunarity = (float)GameObject.Find("LacunaritySlider").GetComponent<Slider>().value;
        GenerateMap();
    }

    public void setHeightMultiplier()
    {
        meshHeightMultiplier = (int)GameObject.Find("HeightMultiplierSlider").GetComponent<Slider>().value;
        GenerateMap();
    }

    public void setWaterLevel()
    {
        waterLevel = (float)GameObject.Find("WaterLevelSlider").GetComponent<Slider>().value;
        GenerateMap();
    }

    public void setDisplayToHeight()
    {
        if (isCreating) drawMode = DrawMode.ColorMap;
        else drawMode = DrawMode.HeightMesh;
        GenerateMap();
    }

    public void setDisplayToBiome()
    {
        if (isCreating) drawMode = DrawMode.BiomeMap;
        else drawMode = DrawMode.BiomeMesh;
        GenerateMap();
    }

    public void setDisplayToMixed()
    {
        if (isCreating) drawMode = DrawMode.MixedMap;
        else drawMode = DrawMode.MixedMesh;
        GenerateMap();
    }

    public void startGame()
    {
        GameObject.Find("Main Camera").transform.position = new Vector3(0, 250f, 0);
        GameObject.Find("Main Camera").transform.localEulerAngles = new Vector3(60f, 0, 0);
        meshObject.SetActive(true);

        waterObject.SetActive(true);
        planeObject.SetActive(false);
        drawMode = DrawMode.HeightMesh;
        isCreating = false;
        GenerateMap();
        meshObject.AddComponent<MeshCollider>();
        GameObject.Find("Water").transform.position = new Vector3(0, waterLevel * 150f, 0);
        GameObject.Find("Water").transform.localScale = new Vector3(mapSize, 1, mapSize);

        GameObject.Find("PlayButton").SetActive(false);
        GameObject.Find("SeedSlider").SetActive(false);
        GameObject.Find("MapSizeSlider").SetActive(false);
        GameObject.Find("NoiseScaleSlider").SetActive(false);
        GameObject.Find("OctavesSlider").SetActive(false);
        GameObject.Find("PersistanceSlider").SetActive(false);
        GameObject.Find("LacunaritySlider").SetActive(false);
        GameObject.Find("HeightMultiplierSlider").SetActive(false);
        GameObject.Find("WaterLevelSlider").SetActive(false);

        GameObject.Find("SeedText").SetActive(false);
        GameObject.Find("MapSizeText").SetActive(false);
        GameObject.Find("NoiseScaleText").SetActive(false);
        GameObject.Find("OctavesText").SetActive(false);
        GameObject.Find("PersistanceText").SetActive(false);
        GameObject.Find("LacunarityText").SetActive(false);
        GameObject.Find("HeightMultiplierText").SetActive(false);
        GameObject.Find("WaterLevelText").SetActive(false);

        //GameObject go = Instantiate(Resources.Load("Prefabs/Crab")) as GameObject;

        //Debug.Log(world.fillCurrentsCallCount);

        Debug.Log("Animals In This World");
        foreach (Animal a in World.getCurrentAnimals())
        {
            Debug.Log(a);
        }

        /*Debug.Log("Plants In This World");
        foreach (Plant p in World.getCurrentPlants()){
            Debug.Log(p);
        }*/
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

[System.Serializable]
public struct BiomeType
{
    public string name;
    public int biomeValue;
    public Color color;
}

[System.Serializable]
public struct HeightType
{
    public string name;
    public float height;
    public Color color;
}
