using System;
using System.Collections.Generic;
using UnityEngine;

namespace HeurekaGames
{
    public class Heureka_Serializer
    {
        internal static string Serialize(List<string> items)
        {
            return JsonUtility.ToJson(new StringList(items));
        }

        internal static List<string> DeserializeStringList(string json)
        {
            StringList list = JsonUtility.FromJson<StringList>(json);

            return (list != null) ? list.Items : new List<string>();
        }

        internal static Type DeSerializeType(string serializedType)
        {
            return Type.GetType(serializedType);
        }

        internal static string SerializeType(Type type)
        {
            return type.AssemblyQualifiedName;
        }

        [SerializeField]
        public class StringList
        {
            public List<string> Items = new List<string>();

            public StringList(List<string> items)
            {
                this.Items = items;
            }
        }
    }
}