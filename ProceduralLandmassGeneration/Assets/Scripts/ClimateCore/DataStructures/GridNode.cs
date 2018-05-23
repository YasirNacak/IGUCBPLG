using System;

public class GridNode
{
    private float altitude;
    private Coord nearestWater;
    private Coord self;

    public GridNode(float altitude, Coord waterCoord, Coord selfCoord)
    {
        this.altitude = altitude;
        nearestWater = waterCoord;
        self = selfCoord;
    }

    public GridNode() : this(0, new Coord(), new Coord())
    { }


    public float getAltitude()
    {
        return altitude;
    }

    public Coord getNearestWater() { return nearestWater; }

    public void setNearestWater(Coord nearestWater)
    {
        this.nearestWater = nearestWater;
    }

    public float getDistanceFromWater()
    {
        float distance;
        float x = self.getX() - nearestWater.getX();
        float y = self.getY() - nearestWater.getY();

        distance = (float)Math.Sqrt(x * x + y * y);
        return distance;
    }
}
