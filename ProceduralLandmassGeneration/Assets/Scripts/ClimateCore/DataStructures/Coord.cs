public class Coord
{
    private int x;
    private int y;

    public Coord() : this(0, 0)
    { }

    public Coord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int getX()
    {
        return x;
    }

    public int getY()
    {
        return y;
    }

    public static bool operator ==(Coord c1, Coord c2)
    {
        return c1.Equals(c2);
    }

    public static bool operator !=(Coord c1, Coord c2)
    {
        return !c1.Equals(c2);
    }

    public override int GetHashCode()
    {
        return x + y;
    }

    public override bool Equals(object obj)
    {
        var objToCoord = (Coord)obj;
        return objToCoord.getX() == this.getX() && objToCoord.getY() == this.getY();
    }

    public override string ToString()
    {
        return x + ", " + y + "\n";
    }
}
