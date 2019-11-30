using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfluxNetCore
{
    public interface IInfluxClient
    {
        Task Test();
    }
}
