namespace RoRamu.Utils
{
    using Newtonsoft.Json;

    public static class JsonUtils
    {
        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
        };

        public static string SerializeToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, _jsonSerializerSettings);
        }

        public static T DeserializeFromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _jsonSerializerSettings);
        }

        public static object FromJson(string json)
        {
            return JsonConvert.DeserializeObject(json, _jsonSerializerSettings);
        }
    }
}
