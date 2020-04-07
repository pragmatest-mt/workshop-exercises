using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Pragmatest.Wallets.TestUtilities
{
    public static class ObjectExtensions
    {
        public static StringContent AsStringContent(this object payload)
        {
            return new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        }
    }
}
