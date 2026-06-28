using System;
using VRage.Utils;


namespace AsteroidBelt
{
    public class Torus
    {
        //r1: Inner Radius
        //r2 Outer Radius
        //h: Height
        public static bool IsPointInTorus(double x, double y, double z, double r1, double r2, double h)
        {
            //Source: https://www.desmos.com/3d/654a76328a?lang=de
            
            double rMaj = (r2 + r1) / 2.0;
            double rMin = (r2 - r1) / 2.0;
            double distXY = Math.Sqrt(x * x + y * y);
            double term1 = (distXY - rMaj) / rMin;
            double term2 = z / h;
            bool isInside = (term1 * term1 + term2 * term2) <= 1.0;
    
            if (isInside)
            {
                //MyLog.Default.WriteLine("Asteroid at " + x + " " + y + " " + z + " Was spawned");
                return true;
            }
            //MyLog.Default.WriteLine("Asteroid at " + x + " " + y + " " + z + " Was not spawned");
            return false;
        }
    }
}
