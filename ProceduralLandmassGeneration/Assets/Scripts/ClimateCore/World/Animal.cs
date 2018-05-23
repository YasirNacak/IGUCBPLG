using System.Collections.Generic;

public class Animal : LivingThing  {

    public Animal(int id, string name, string description, Coord position) 
        : base(id,name,description,position)
    {  }

    public Animal(int id) : base(id)
    { }
    

    public static List<Coord> move(World currentWorld){
        return new List<Coord>();
    }
}
