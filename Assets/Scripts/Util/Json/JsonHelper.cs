using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GDD.JsonHelper
{
    public static class JsonHelper
    {
        //CreateJsonObject
        public static JObject CreateJsonObject<T>(object data)
        {
            string j_data = JsonConvert.SerializeObject((T)data);
            return JsonConvert.DeserializeObject<JObject>(j_data);
        }
    }
}