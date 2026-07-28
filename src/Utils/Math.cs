namespace Utils.CMath;

// custom math class
public class CMath
{
    private static float r_unit = ((float)Math.PI)/180.0f;

    //converts degrees to radians
    public static float rad(float x)
    {
        return x * r_unit;
    }

    // converts radians to degrees
    public static double deg(double x)
    {
        return x / r_unit;
    }
}