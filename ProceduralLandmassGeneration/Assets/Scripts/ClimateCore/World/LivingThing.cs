using System;

public abstract class LivingThing : IComparable<LivingThing>
{
    protected string name = null;
    protected string description = null;
    protected int id;
    protected Coord position = null;

    public LivingThing(int id, string name, string description, Coord position)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.position = position;
    }

    public LivingThing(int id) { this.id = id; }

    public string getName()
    {
        return name;
    }

    public string getDefinition()
    {
        return description;
    }

    public int getId()
    {
        return id;
    }

    public Coord getPosition() { return position; }

    public void setPosition(Coord tmp) { position = tmp; }

    public int CompareTo(LivingThing obj)
    {
        return id.CompareTo(obj.id);
    }

    public override bool Equals(object obj)
    {
        LivingThing objToLT = ((LivingThing)obj);
        return id.Equals(objToLT.id) && position.Equals(objToLT.position);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return "id: " + id + " name: " + name + " description: " + description + " position: " + position;
    }
}
