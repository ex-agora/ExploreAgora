#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("poaCExjssnZQ36+aK3AeU8TmIlSkD7TA5hmRuwivPY84DPC+8oxzL9ieB03VdDMdrRZp8aYRz3krQvotNAFAnDYsW5I3dsEiP2v7Nl+XKX6ass0ysU+44I7t9UzkzbcwP1EvR8hwP9Xvp39Hj/8tscAjKGZgo1i7XKZ6waRXRewdUxj3GqiSIhaeFc4HhIqFtQeEj4cHhISFGkX4S7sPNYJb/RZogwR6xE/j6tUDJtcrwRxevz6x6Bi/i5WTE0ydMiJ0RBlRTmLSKNMHJJgzuyqjxCwdCuP73Y57S1GBKNHXjTxwDB+ntsvD6011Wt6jtQeEp7WIg4yvA80DcoiEhISAhYawcDv53c7Es1tGB4i6yrn1EuTCxHRHOimeduGBxIeGhIWE");
        private static int[] order = new int[] { 7,13,8,9,4,12,12,13,8,10,11,11,13,13,14 };
        private static int key = 133;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
