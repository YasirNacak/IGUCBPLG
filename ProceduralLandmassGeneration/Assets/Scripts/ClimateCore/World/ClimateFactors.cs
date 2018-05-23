public class ClimateFactors
{
    /*
    private float tempMax;
    private float tempMin;
    private float precMax;
    private float precMin;
    private float humidMax;
    private float humidMin;
    */
    private float currentTemp; //Celsius
    private float currentPrec; //Percentage
    private float currentHumid; //Percentage

    public ClimateFactors(float temp, float prec, float humid)
    {
        currentTemp = temp;
        currentPrec = prec;
        currentHumid = humid;
    }

    public float getCurrentTemperature()
    {
        return currentTemp;
    }

    public float getCurrentPrecipitation()
    {
        return currentPrec;
    }

    public float getCurrentHumidity()
    {
        return currentHumid;
    }
}
