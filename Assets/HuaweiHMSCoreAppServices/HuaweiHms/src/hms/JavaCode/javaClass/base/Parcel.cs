using UnityEngine;
using System.Collections.Generic;

namespace HuaweiHms
{
    public class Parcel_Data : IHmsBaseClass{
        public string name => "android.os.Parcel";
    }
    public class Parcel :HmsClass<Parcel_Data>
    {
        public Parcel (): base() { }
    }
}