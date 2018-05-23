using System.Collections.Generic;

public class Biome
{
    private HashSet<Animal> animalSet = null;
    private HashSet<Plant> plantSet = null;
    private ClimateFactors climate = null;

    private string name;
    private string description;

    public Biome(string name, string description)
    {
        this.name = name;
        this.description = description;
        animalSet = new HashSet<Animal>();
        plantSet = new HashSet<Plant>();
    }



    public void setAnimalSet(int a, int b, int c, int d, int e)
    {
        animalSet = new HashSet<Animal>();
        animalSet.Add((Animal)World.getDatabase().find(new Animal(a)));
        animalSet.Add((Animal)World.getDatabase().find(new Animal(b)));
        animalSet.Add((Animal)World.getDatabase().find(new Animal(c)));
        animalSet.Add((Animal)World.getDatabase().find(new Animal(d)));
        animalSet.Add((Animal)World.getDatabase().find(new Animal(e)));
    }

    public void setPlantSet(int a, int b, int c, int d, int e)
    {
        plantSet = new HashSet<Plant>();
        plantSet.Add((Plant)World.getDatabase().find((LivingThing)new Plant(a)));
        plantSet.Add((Plant)World.getDatabase().find((LivingThing)new Plant(b)));
        plantSet.Add((Plant)World.getDatabase().find((LivingThing)new Plant(c)));
        plantSet.Add((Plant)World.getDatabase().find((LivingThing)new Plant(d)));
        plantSet.Add((Plant)World.getDatabase().find((LivingThing)new Plant(e)));
    }

    public void setClimate(float temp, float prec, float humid)
    {
        climate = new ClimateFactors(temp, prec, humid);
    }

    public HashSet<Animal> getAnimalSet()
    {
        return animalSet;
    }

    public HashSet<Plant> getPlantSet()
    {
        return plantSet;
    }

    public ClimateFactors getClimate()
    {
        return climate;
    }

    public string getName()
    {
        return name;
    }

    public string getDescription()
    {
        return description;
    }
}
