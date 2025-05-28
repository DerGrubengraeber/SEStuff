using System;
using VRage.Utils;


namespace AsteroidBelt
{
    public class Torus
    {
        public static bool IsPointInTorus(double x, double y, double z, double R, double r)
        {
            //(x^2+y^2+z^2-(a^2+b^2))^2-4a^2(b^2-z^2) source: https://stackoverflow.com/questions/13460711/given-origin-and-radii-how-to-find-out-if-px-y-z-is-inside-torus
            if (Math.Pow((x * x + y * y + z * z - (R * R + r * r)), 2) - 4 * R * R * (r * r - z * z) < 0)
            {
                //MyLog.Default.WriteLine("Asteroid at " + x + " " + y + " " + z + " Was spawned");
                return true;
            }
            //MyLog.Default.WriteLine("Asteroid at " + x + " " + y + " " + z + " Was not spawned");
            return false;
        }
    }
}
