using System;
using System.Collections;
using System.Text;
using UI;
using UnityEngine;
using UnityEngine.Networking;

namespace Agent
{
    public enum LLMModel
    {
        glm40520,
        glm4,
        glm4air,
        glm4airx,
        glm4flash
    }
    
    public class AgentChatController : MonoBehaviour
    {
        private static AgentChatController _agentChatController;
        public static AgentChatController Instance => _agentChatController;    
        
        [SerializeField] private string modelName;
        [SerializeField] private string apiKey;
        [SerializeField] private string apiUrl = "https://open.bigmodel.cn/api/paas/v4/chat/completions";
        public ChatPanel chatPanel;
        
        private Agent m_currentAgent;
        
        private void Awake()
        {
            if (_agentChatController == null)
            {
                _agentChatController = this;
            }
            else
            {
                Destroy(this);
            }
        }

        /// <summary>
        /// 开启一段新对话
        /// </summary>
        /// <param name="newAgent">对话对象</param>
        public void StartChat(Agent newAgent)
        {
            if (m_currentAgent == null)
            {
                m_currentAgent = newAgent;
            }
            
            RequestData requestData = new RequestData(modelName);
            requestData.messages = m_currentAgent.GetAgentMemory().ToArray();
            string requestStr = JsonUtility.ToJson(requestData);
            
            chatPanel.DisableInput();
            //Send To Server
            StartCoroutine(SendRequest(requestStr,chatPanel.OnReceiveMessage));
        }

        /// <summary>
        /// 结束当前对话
        /// </summary>
        public void FinishChat()
        {
            m_currentAgent.ClearMemory();
            m_currentAgent = null;
        }
        
        /// <summary>
        /// 向服务器发送对话
        /// </summary>
        /// <param name="userInput">用户输入</param>
        public void SendRequestToServer(string userInput,Action<Agent,string> callBack)
        {
            m_currentAgent.AddMemory(AgentRole.user,userInput);
            
            RequestData requestData = new RequestData(modelName);
            requestData.messages = m_currentAgent.GetAgentMemory().ToArray();
            string requestStr = JsonUtility.ToJson(requestData);
            
            StartCoroutine(SendRequest(requestStr,callBack));
        }

        private IEnumerator SendRequest(string data,Action<Agent,string> callBack)
        {
            using UnityWebRequest webRequest = UnityWebRequest.Post(apiUrl,data);

            webRequest.uploadHandler.Dispose();
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));
            webRequest.downloadHandler.Dispose();
            webRequest.downloadHandler = new DownloadHandlerBuffer();
                
            webRequest.SetRequestHeader("Content-Type","application/json;charset=utf-8");
            webRequest.SetRequestHeader("Authorization",$"Bearer {apiKey}");
            
            yield return webRequest.SendWebRequest();
            
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
            else
            {
                // 解析响应数据
                string responseJson = webRequest.downloadHandler.text;
                    
                if (responseJson == "") yield return null;
                
                Response response = JsonUtility.FromJson<Response>(responseJson);
                string content = response.choices[0].message.content;
                Debug.Log(content);

                if (m_currentAgent)
                {
                    m_currentAgent.AddMemory(response.choices[0].message.content);
                    callBack?.Invoke(m_currentAgent,content);
                }

            }
            
            webRequest.disposeUploadHandlerOnDispose = true;
            webRequest.disposeDownloadHandlerOnDispose = true;
            webRequest.disposeCertificateHandlerOnDispose = true;
        }
    }
}