using CS3D.dataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3D
{
    static class StaticTools
    {
        //private static int test = 5;

        public static float DegreesToRadians(float degrees)
        {
            return degrees * (float)Math.PI / 180.0f;
        }

        public static Int64 TimeInMS()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static Int64 TimerSet(Int64 ms)
        {
            return TimeInMS() + ms;
        }

        public static bool TimerPassed(Int64 timer)
        {
            return timer < TimeInMS();
        }
    }
}
