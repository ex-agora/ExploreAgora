using UnityEngine;

namespace HuaweiHms{
    public class PushListenerRegister{
        public static void RegisterListener(IPushServiceListener listener){
            AndroidJavaClass cl = new AndroidJavaClass("com.unity.hms.push.MyPushService");
            cl.CallStatic("SetListener",listener);
        }
    }
}