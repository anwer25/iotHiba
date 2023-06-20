using System;
using System.Collections;
using UnityEngine.Networking;
using System.Text;
using UnityEngine;
using Random = System.Random;


namespace Methods
{
    public class Methods
    {
        public delegate void OnSenderMethodComplete(string responseJson);
        public delegate void OnGetterMethodComplete(string responseJson);
        public event OnSenderMethodComplete OnSenderComplete;
        public event OnGetterMethodComplete OnGetterComplete;
        private static readonly Random random = new Random();
        private string _responseData;
        
        public string GenerateUniqueId()
        {
            string timestamp = DateTime.Now.Ticks.ToString(); // Get current timestamp
            string randomPart = random.Next(10000, 99999).ToString(); // Generate random number

            string uniqueId = timestamp + randomPart; // Combine timestamp and random number
            return uniqueId;
        }
        
        
        public string GenerateRandomZone(string[] zoneNames)
        {
            return zoneNames[random.Next(zoneNames.Length)];
        }
        
        public int GetRandomNumber(int min, int max)
        {
            int randomValue = random.Next(min, max + 1);
            return randomValue;
        }

        public IEnumerator SenderMethod<TResponseType, TDataType>(TDataType data, string url)
            where TResponseType : new()
        {
            var jsonData = JsonUtility.ToJson(data);
            var request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success && request.result != UnityWebRequest.Result.InProgress)
            {
                Debug.LogError(request.error);
                yield break;
            }

            if (!request.downloadHandler.isDone)
                yield break;

            if (request.downloadHandler == null)
            {
                Debug.LogError("DownloadHandler is null");
                yield break;
            }

            var responseText = request.downloadHandler.text;
            _responseData = responseText;
            OnSenderComplete?.Invoke(responseText);
        }


        // ReSharper disable Unity.PerformanceAnalysis
        public IEnumerator GetterMethod(string url)
        {
            var method = UnityWebRequest.Get(url);
            yield return method.SendWebRequest();
            if (method.result != UnityWebRequest.Result.Success & method.result != UnityWebRequest.Result.InProgress)
            {
                Debug.LogError(method.error);
                yield break;

            }

            if (!method.downloadHandler.isDone) yield break;
            if (method.downloadHandler == null)
            {
                Debug.LogError("DownloadHandler is null");
                yield break;
            }

            var responseText = method.downloadHandler.text;
            _responseData = responseText;
            OnGetterComplete?.Invoke(responseText);
        }
        
        public string GetResponseData()
        {
            return _responseData;
        }
    }
}