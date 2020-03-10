using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class StringStringDictionary : SerializableDictionary<string, string> {}

[Serializable]
public class ObjectColorDictionary : SerializableDictionary<UnityEngine.Object, Color> {}

[Serializable]
public class ColorArrayStorage : SerializableDictionary.Storage<Color[]> {}

[Serializable]
public class StringColorArrayDictionary : SerializableDictionary<string, Color[], ColorArrayStorage> {}

[Serializable]
public class StringAudioClipsDictionary : SerializableDictionary<string, AudioClip> {}

[Serializable]
public class StringIntDictionary : SerializableDictionary<string, int> {}

[Serializable]
public class StringRankRangeDictionary : SerializableDictionary<string, RankRange> {}


[Serializable]
public class MyClass
{
    public int i;
    public string str;
}

[System.Serializable]
public class RankRange {
    public uint min;
    public uint max;
}

[Serializable]
public class QuaternionMyClassDictionary : SerializableDictionary<Quaternion, MyClass> {}