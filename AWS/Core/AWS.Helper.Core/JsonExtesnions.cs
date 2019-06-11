using Newtonsoft.Json;

namespace AWS.Helper.Core
{
    public static class JsonExtesnions
    {
        public static string ToJsonString(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T GetObjectFromJsonString<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
