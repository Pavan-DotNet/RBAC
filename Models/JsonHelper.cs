using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;


/// <summary>
/// Summary description for JsonHelper
/// </summary>
public class JsonHelper
{
    public JsonHelper()
    {        
    }

    public string ConvertObjectToJSon<T>(T obj)
    {
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        MemoryStream ms = new MemoryStream();
        ser.WriteObject(ms, obj);
        string jsonString = Encoding.UTF8.GetString(ms.ToArray());
        ms.Close();
        return jsonString;
    }

    public T ConvertJSonToObject<T>(string jsonString)
    {  

        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
        using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
        {
            ms.Position = 0;
            T obj = (T)serializer.ReadObject(ms);
            return obj;
        }
    }
}



