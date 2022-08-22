using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Http : MonoBehaviour
{
    public delegate void HttpRespone(string jsonResponse, bool successful);
    public static event HttpRespone HttpResponseEvent;
    
    static IEnumerator HTTPCall(string url, string json, string method)
    {
        using(UnityWebRequest request = new UnityWebRequest(url, method))
        {
            byte[] bodyraw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyraw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            bool successful = request.error == null ? true : false;

     
            HttpResponseEvent(request.downloadHandler.text, successful);
            
        }
    }

    public static IEnumerator POST(string url, string json)
    {
        yield return HTTPCall(url, json, "POST");
    }
    public static IEnumerator PUT(string url, string json)
    {
        yield return HTTPCall(url, json, "PUT");
    }
     public static IEnumerator GET(string url, string urlEncoding)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url + urlEncoding))
        {
            //request.SetRequestHeader("Content-Type", "application/json");
            
            yield return request.SendWebRequest();

            bool successful = request.error == null ? true : false;
            
            if (!successful)
            {
                HttpResponseEvent(request.downloadHandler.text, successful);
            }
            else HttpResponseEvent(request.downloadHandler.text, successful);
        }
    }
}
