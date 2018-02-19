using System.Web.Script.Serialization;

namespace Entity.json
{
    public static class JSONHelper
    {
        public static string ToJson(this object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

    }
}
