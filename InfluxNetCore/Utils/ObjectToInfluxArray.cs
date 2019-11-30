using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace InfluxNetCore.Utils
{
    public static class ObjectToInfluxArray
    {

        public static Dictionary<string, object> Convert(object rawObject)
        {
            var type = rawObject.GetType();
            var props = type.GetProperties();

            var res = new Dictionary<string, object>();

            foreach (var prop in props)
            {
                object value;
                try
                {
                    value = type.GetProperty(prop.Name).GetValue(rawObject);
                }
                catch (Exception)
                {
                    Debug.Print($"Cant convert field \"{prop.Name}\" of Object \"{type.FullName}\" to Influx element! Skipped!");
                    continue;
                }

                // Convert some datatypes
                DoConvertIfNeeded(ref value);

                if (!value.GetType().IsPrimitive && !(value is string))
                {
                    var deepDict = Convert(value);
                    foreach(var entry in deepDict)
                    {
                        res.Add(prop.Name + "/" + entry.Key, entry.Value);
                    }
                } else
                    res.Add(prop.Name, value);
            }
            return res;
        }

        private static void DoConvertIfNeeded(ref object value)
        {
            if (value is DateTime)
                value = value.ToString();
        }
    }
}
