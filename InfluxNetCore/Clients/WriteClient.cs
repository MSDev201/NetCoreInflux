using InfluxNetCore.Extensions;
using InfluxNetCore.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfluxNetCore.Clients
{
    internal class WriteClient
    {

        public static async Task MakeWriteRequest(
            string measurement,
            object fields,
            object tags = null,
            string timestamp = null,
            string precision = null,
            string db = null)
        {
            if (tags == null && fields == null || fields == null)
                return;

            if (db == null)
                db = InfluxClient.Connection.DefaultDatabase;
            if (db == null)
                return;

            if (timestamp == null)
                timestamp = DateTime.UtcNow.GetUnixTimestampNanoseconds().ToString();

            var extraParams = new List<string>();
            if (precision != null)
                extraParams.Add("precision=" + precision);

            extraParams.Add("db=" + db);

            var fieldsString = string.Empty;
            var tagsString = string.Empty;

            if (fields != null)
            {
                var fieldsDict = ObjectToInfluxArray.Convert(fields);
                fieldsString = MakeWriteStringFromDict(fieldsDict);
                if (tags == null)
                    fieldsString = " " + fieldsString;
            }
                

            if (tags != null)
            {
                var tagsDict = ObjectToInfluxArray.Convert(tags);
                foreach (var tag in tagsDict)
                {
                    tagsString += "," + tag.Key.Replace(" ", "\\ ") + "=" + tag.Value.ToString().Replace(" ", "\\ ");
                }
                tagsString = tagsString + " ";
            }

            await InfluxRequest.MakePostAsync("write", measurement + tagsString + fieldsString + " " + timestamp, extraParams.ToArray());
        }

        private static string MakeWriteStringFromDict(Dictionary<string, object> input)
        {
            var props = new List<string>();
            foreach(var prop in input)
            {
                var value = prop.Value;
                if (prop.Value is string)
                    value = $"\"{value}\"";
                if (prop.Value is int || value is uint || value is short || value is ushort || value is long || value is ulong)
                    value = prop.Value + "i";
                if (prop.Value is bool)
                    value = prop.Value.ToString();
                if (value is double)
                    value = ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
                if (value is float)
                    value = ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
                props.Add(prop.Key.ToLower() + "=" + value);
            }

            return string.Join(',', props);
        }

    }
}
