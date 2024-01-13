using Newtonsoft.Json;

namespace EduQuiz_5P.Helpers
{
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value != null ? JsonConvert.DeserializeObject<T>(value) : default(T);
        }
    }
}
