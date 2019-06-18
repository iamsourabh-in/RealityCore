using System.Reflection;
using System.Text;

namespace Reality.Cognito.Models
{
    public class CognitoModel
    {

        public string ToString<T>()
        {
            StringBuilder sBuilder = new StringBuilder();

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                sBuilder.Append($"{property.Name} = {property.GetValue(this, null)} , ");
            }
            base.ToString();
            return sBuilder.ToString();
        }
    }
}