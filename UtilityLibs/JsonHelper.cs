using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityLibs
{
    public class JsonHelper
    {
        public static string SerializerToJson(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static T DeserializeObjectFromJson<T>(string jsonStr)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonStr);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
