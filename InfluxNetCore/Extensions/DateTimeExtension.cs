using System;
using System.Collections.Generic;
using System.Text;

namespace InfluxNetCore.Extensions
{
    internal static class DateTimeExtension
    {

        public static ulong GetUnixTimestampNanoseconds(this DateTime datetime)
        {
            return (ulong)((datetime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds * 1e6);
        }

    }
}
