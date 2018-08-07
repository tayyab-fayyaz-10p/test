using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSH.Core.Helper
{
    public class LocationHelper
    {
        public static double DistanceTo(double lat1, double lon1, double lat2, double lon2)
        {
            var cord1 = new GeoCoordinate(lat1, lon1);
            var cord2 = new GeoCoordinate(lat2, lon2);

            var distance = cord1.GetDistanceTo(cord2);
            return distance / 1000;

            //double rlat1 = Math.PI * lat1 / 180;
            //double rlat2 = Math.PI * lat2 / 180;
            //double theta = lon1 - lon2;
            //double rtheta = Math.PI * theta / 180;
            //double dist = (Math.Sin(rlat1) * Math.Sin(rlat2)) + (Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta));
            //dist = Math.Acos(dist);
            //dist = dist * 180 / Math.PI;
            //dist = dist * 60 * 1.1515;

            //return dist * 1.60944; //KiloMeters
        }
    }
}
