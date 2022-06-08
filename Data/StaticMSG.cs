using System;
namespace api.Data{
    public static class StaticMSG{
        public static string NotFound(string notfoundName="Data")
        {
            return notfoundName+" Not Found";
        }

    }
}