

public class Plant : LivingThing {
    private float height;

    public Plant(int id, string name, string description, Coord position) 
        : base(id, name, description, position)
    { }

    public Plant(int id) : base(id)
    { }

    public float getHeight() {
        return height;
    }
}
