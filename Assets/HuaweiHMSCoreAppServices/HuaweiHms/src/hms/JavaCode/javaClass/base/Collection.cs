using UnityEngine;
using System.Collections.Generic;

namespace HuaweiHms
{
    public class Collection_Data : IHmsBaseClass{
        public string name => "java.util.Collection";
    }
    public class Collection :HmsClass<Collection_Data>
    {
        public Collection (): base() { }
    }
}