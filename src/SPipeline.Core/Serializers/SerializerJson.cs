namespace SPipeline.Core.Serializers
{
    using Newtonsoft.Json;

    public static class SerializerJson
    {
        private static readonly JsonSerializerSettings Settings;

        static SerializerJson()
        {
            Settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
        }

        public static string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, Settings);
        }

        public static object Deserialize(string value)
        {
            return JsonConvert.DeserializeObject(value, Settings);
        }
    }
}
