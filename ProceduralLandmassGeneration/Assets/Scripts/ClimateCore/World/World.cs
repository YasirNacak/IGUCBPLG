using System.Collections.Generic;
using UnityEngine;

public class World
{
    private const int GRID_DIVIDER = 1;
    private const float MINIMUM_WATER_DISTANCE = 0.7f;
    private const float AVERAGE_WATER_DISTANCE = 4f;
    private const float AVERAGE_ALTITUDE = 0.3f;
    enum BiomeType { BEACH, RAIN_FOREST, MOUNTAIN, TUNDRA, DESERT, SAVANNAH, NO_BIOME };
    private int animalCount = 0;
    private int k = 0;

    private List<Biome> biomeTypes;
    private static BinarySearchTree<LivingThing> database;
    private static List<LivingThing> explored = new List<LivingThing>();

    internal Dictionary<Coord, int> grid;
    private static List<Plant> currentPlants;
    private static List<Animal> currentAnimals;

    public static BinarySearchTree<LivingThing> getDatabase() { return database; }
    public static List<LivingThing> getExplored() { return explored; }
    public static List<Animal> getCurrentAnimals() { return currentAnimals; }
    public static List<Plant> getCurrentPlants() { return currentPlants; }

    private void fillBiomeListFromFile()
    {
        biomeTypes = new List<Biome>();
        var inp = Resources.Load<TextAsset>("biomes");
        var inpContent = inp.text.Split("\n"[0]);
        int i = 0;
        foreach (var line in inpContent)
        {
            string[] tokens = line.Split(',');
            int[] intTokens = new int[tokens.Length - 2];
            for (int j = 2; j < tokens.Length; j++)
                intTokens[j - 2] = int.Parse(tokens[j]) + ((j > 6) ? animalCount : 0);

            biomeTypes.Add(new Biome(tokens[0], tokens[1]));
            biomeTypes[i].setAnimalSet(intTokens[0], intTokens[1], intTokens[2], intTokens[3], intTokens[4]);
            biomeTypes[i].setPlantSet(intTokens[5], intTokens[6], intTokens[7], intTokens[8], intTokens[9]);
            biomeTypes[i].setClimate(intTokens[10], intTokens[11], intTokens[12]);
            i++;
        }
    }

    private void fillDatabaseFromFile()
    {
        database = new BinarySearchTree<LivingThing>();
        var inp = Resources.Load<TextAsset>("animals");
        var inpContent = inp.text.Split("\n"[0]);
        Coord nullCoord = new Coord(-1, -1);
        foreach (var line in inpContent)
        {
            string[] tokens = line.Split(',');
            int id = int.Parse(tokens[0]);
            database.add(new Animal(id, tokens[1], tokens[2], nullCoord));
            animalCount = id;
        }

        inp = Resources.Load<TextAsset>("plants");
        inpContent = inp.text.Split("\n"[0]);
        foreach (var line in inpContent)
        {
            string[] tokens = line.Split(',');
            int id = int.Parse(tokens[0]);
            database.add(new Plant(animalCount + id, tokens[1], tokens[2], nullCoord));
        }
    }

    public float[,] calculateAltitude(float[,] hMap, float waterLevel)
    {
        float[,] altMap = new float[hMap.GetLength(0), hMap.GetLength(1)];
        for (int i = 0; i < hMap.GetLength(0); i++)
            for (int j = 0; j < hMap.GetLength(0); j++)
                altMap[i, j] = hMap[i, j] - waterLevel;
        return altMap;
    }

    public GridNode[,] calcWaterDist(float[,] altitudeMap)
    {
        GridNode[,] tempNode = new GridNode[altitudeMap.GetLength(0), altitudeMap.GetLength(1)];
        for (int i = 0; i < tempNode.GetLength(0); i++)
            for (int j = 0; j < tempNode.GetLength(0); j++)
                tempNode[i, j] = findNearestWater(altitudeMap, new Coord(j, i), tempNode);

        return tempNode;
    }

    private GridNode findNearestWater(float[,] altitudeMap, Coord current, GridNode[,] grid)
    {
        GridNode tempNode = new GridNode(altitudeMap[current.getY(), current.getX()], current, current);
        if (tempNode.getAltitude() < 0) return tempNode;
        bool isFirstIter = true;
        int i = 0, j = 0;
        if (current.getX() > 0) j = current.getX() - 1;
        if (current.getY() > 0) i = current.getY() - 1;
        int lastWaterY = 0;
        int lastWaterX = 0;
        if (i < current.getY() && j < current.getX())
        {
            lastWaterY = grid[i, j].getNearestWater().getY();
            lastWaterX = grid[i, j].getNearestWater().getX();
        }
        for (i = lastWaterY; i < altitudeMap.GetLength(0); i++)
            for (j = lastWaterX; j < altitudeMap.GetLength(0); j++)
            {
                k++;
                if (altitudeMap[i, j] < 0)
                {
                    int x = current.getX() - j;
                    int y = current.getY() - i;
                    if (isFirstIter)
                    {
                        tempNode.setNearestWater(new Coord(j, i));
                        isFirstIter = false;
                    }
                    else if (Mathf.Abs(y) > tempNode.getDistanceFromWater()) return tempNode;
                    if (Mathf.Sqrt(x * x + y * y) < tempNode.getDistanceFromWater())
                        tempNode.setNearestWater(new Coord(j, i));
                }
            }
        return tempNode;
    }

    public World()
    {
        fillDatabaseFromFile();
        fillBiomeListFromFile();

        MapGenerator mapGen = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
        int size = mapGen.mapSize;

        float[,] hMap = new float[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                hMap[i, j] = mapGen.noiseMap[j, i];
            }
        }

        float[,] aMap = calculateAltitude(hMap, mapGen.waterLevel);

        GridNode[,] gMap = calcWaterDist(aMap);

        float altAv = 0;
        float wDisAv = 0;
        int m = 0;
        grid = new Dictionary<Coord, int>();
        currentPlants = new List<Plant>();
        currentAnimals = new List<Animal>();
        int tempM = 0;
        Stack<Coord> coords = new Stack<Coord>();

        for (int b = 0; b < size / GRID_DIVIDER; b++)
        {
            for (int a = 0; a < size / GRID_DIVIDER; a++)
            {
                tempM = m;
                for (int i = 0; i < GRID_DIVIDER; i++)
                {
                    for (int k = b * GRID_DIVIDER; k < b * GRID_DIVIDER + GRID_DIVIDER; k++)
                    {
                        wDisAv += gMap[m, k].getDistanceFromWater();
                        altAv += gMap[m, k].getAltitude();
                    }
                    m++;
                }
                wDisAv /= (float)GRID_DIVIDER * GRID_DIVIDER;
                altAv /= (float)GRID_DIVIDER * GRID_DIVIDER;
                Coord newCoord = null;
                if (altAv < AVERAGE_ALTITUDE && wDisAv < MINIMUM_WATER_DISTANCE)
                {
                    for (int i = 0; i < GRID_DIVIDER; i++)
                    {
                        for (int k = b * GRID_DIVIDER; k < b * GRID_DIVIDER + GRID_DIVIDER; k++)
                        {
                            newCoord = new Coord(k, tempM);
                            grid[newCoord] = (int)BiomeType.BEACH;
                            coords.Push(newCoord);
                        }
                        tempM++;
                    }
                    fillCurrents(coords, (int)BiomeType.BEACH);
                }
                else if (altAv < AVERAGE_ALTITUDE && wDisAv >= MINIMUM_WATER_DISTANCE && wDisAv < AVERAGE_WATER_DISTANCE)
                {
                    for (int i = 0; i < GRID_DIVIDER; i++)
                    {
                        for (int k = b * GRID_DIVIDER; k < b * GRID_DIVIDER + GRID_DIVIDER; k++)
                        {
                            newCoord = new Coord(k, tempM);
                            grid[newCoord] = (int)BiomeType.RAIN_FOREST;
                            coords.Push(newCoord);
                        }
                        tempM++;
                    }
                    fillCurrents(coords, (int)BiomeType.RAIN_FOREST);
                }
                else if (altAv >= AVERAGE_ALTITUDE && wDisAv < AVERAGE_WATER_DISTANCE)
                {
                    for (int i = 0; i < GRID_DIVIDER; i++)
                    {
                        for (int k = b * GRID_DIVIDER; k < b * GRID_DIVIDER + GRID_DIVIDER; k++)
                        {
                            newCoord = new Coord(k, tempM);
                            grid[newCoord] = (int)BiomeType.MOUNTAIN;
                            coords.Push(newCoord);
                        }
                        tempM++;
                    }
                    fillCurrents(coords, (int)BiomeType.MOUNTAIN);
                }
                else if (altAv >= AVERAGE_ALTITUDE && wDisAv >= AVERAGE_WATER_DISTANCE)
                {
                    for (int i = 0; i < GRID_DIVIDER; i++)
                    {
                        for (int k = b * GRID_DIVIDER; k < b * GRID_DIVIDER + GRID_DIVIDER; k++)
                        {
                            newCoord = new Coord(k, tempM);
                            grid[newCoord] = (int)BiomeType.TUNDRA;
                            coords.Push(newCoord);
                        }
                        tempM++;
                    }
                    fillCurrents(coords, (int)BiomeType.TUNDRA);
                }
                else if (altAv >= 0 && altAv < AVERAGE_ALTITUDE / 2 && wDisAv >= AVERAGE_WATER_DISTANCE)
                {
                    for (int i = 0; i < GRID_DIVIDER; i++)
                    {
                        for (int k = b * GRID_DIVIDER; k < b * GRID_DIVIDER + GRID_DIVIDER; k++)
                        {
                            newCoord = new Coord(k, tempM);
                            grid[newCoord] = (int)BiomeType.DESERT;
                            coords.Push(newCoord);
                        }
                        tempM++;
                    }
                    fillCurrents(coords, (int)BiomeType.DESERT);
                }
                else if (altAv >= AVERAGE_ALTITUDE / 2 && altAv < AVERAGE_ALTITUDE && wDisAv >= AVERAGE_WATER_DISTANCE)
                {
                    for (int i = 0; i < GRID_DIVIDER; i++)
                    {
                        for (int k = b * GRID_DIVIDER; k < b * GRID_DIVIDER + GRID_DIVIDER; k++)
                        {
                            newCoord = new Coord(k, tempM);
                            grid[newCoord] = (int)BiomeType.SAVANNAH;
                            coords.Push(newCoord);
                        }
                        tempM++;
                    }
                    fillCurrents(coords, (int)BiomeType.SAVANNAH);
                }
                wDisAv = 0;
                altAv = 0;
            }
            m = 0;
        }
    }


    private void fillCurrents(Stack<Coord> Coords, int type)
    {

        /*Animal[] animalArray = new Animal[5];
        biomeTypes[type].getAnimalSet().CopyTo(animalArray);
        Plant[] plantArray = new Plant[5];
        biomeTypes[type].getPlantSet().CopyTo(plantArray);
        System.Random generator = new System.Random();
        while (Coords.Count > 0) {
            Coord point = Coords.Pop();
            int i = generator.Next(3);
            if (i == 1) {
                i = generator.Next(5);
                Animal newAnimal = animalArray[i];
                newAnimal.setPosition(point);
                if (!currentAnimals.Contains(newAnimal)) { }
                    currentAnimals.Add(newAnimal);
                //Debug.Log("added: " + newAnimal);
            }
            if (i == 2) {
                i = generator.Next(5);
                Plant newPlant = plantArray[i];
                newPlant.setPosition(point);
                if(!currentPlants.Contains(newPlant))
                    currentPlants.Add(newPlant);
                //Debug.Log("added: " + newPlant);
            }
        }*/
    }
}
