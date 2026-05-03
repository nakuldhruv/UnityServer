using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace WebApi
{
    public class WebApiManager : MonoBehaviour
    {
        private string _url = "http://127.0.0.1:5115/api/";

        private void Start()
        {
            PlayerProfile playerProfile = new PlayerProfile()
            {
                Id = 1,
                Name = "LiuSir",
                Gold = 999,
            };

            StartCoroutine(SavePlayerCoroutine(playerProfile));
            StartCoroutine(GetPlayerCoroutine("LiuSir", player =>
            {
                Debug.Log($"查询成功！玩家名: {player.Name}, 金币: {player.Gold}");
            }));
        }

        public IEnumerator GetPlayerCoroutine(string name, Action<PlayerProfile> onSuccess)
        {
            string requestUrl = _url + $"player/{name}";
            using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
            {
                yield return request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"[查询失败] URL: {requestUrl} | 状态码: {request.responseCode} | 错误: {request.error}");
                }
                else
                {
                    string jsonResponse = request.downloadHandler.text;
                    Debug.Log($"[查询成功] 原始数据: {jsonResponse}");
                    try
                    {
                        PlayerProfile player = JsonUtility.FromJson<PlayerProfile>(jsonResponse);
                        onSuccess?.Invoke(player);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[解析失败] JSON 格式可能不匹配: {e.Message}");
                    }
                }
            }
        }

        public IEnumerator SavePlayerCoroutine(PlayerProfile playerProfile)
        {
            // 将 C# 对象转换为 JSON 字符串
            string json = JsonUtility.ToJson(playerProfile);
            // 将字符串转换为字节数组 (二进制流)，以便在 HTTP 网络中传输
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            string requestUrl = _url + "player/save";
            using (UnityWebRequest request = new UnityWebRequest(requestUrl, "POST"))
            {
                // UploadHandlerRaw: 负责发送原始字节流数据
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                // DownloadHandlerBuffer: 负责开辟内存缓冲区来接收服务器返回的响应
                request.downloadHandler = new DownloadHandlerBuffer();
                // 告诉后端：我发过去的数据格式是 JSON，请用 JSON 解析器处理
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"[请求失败] URL: {requestUrl} | 状态码: {request.responseCode} | 错误: {request.error}");
                }
                else
                {
                    Debug.Log($"[请求成功] 后端响应: {request.downloadHandler.text}");
                }
            }
        }
    }
}