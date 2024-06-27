using System;
using UnityEngine;

namespace Agent
{
    [Serializable]
    public struct MessageData
    {
        public string role;
        public string content;

        public MessageData(string r,string cont)
        {
            role = r;
            content = cont;
        } 
    }
    
    [Serializable]
    public struct RequestData
    {
        public string model;
        public MessageData[] messages;
        // public float top_p; //=0.7f
        // public float temperature; //0.95f
        // public int max_tokens; //4095

        public RequestData(string _model)//,float _topP = 0.7f,float _temp = 0.95f,int _maxTokens = 4095)
        {
            model = _model;
            messages = null;
        }
    }

    [Serializable]
    public struct ResponseChoice
    {
        public int index;
        public MessageData message;
    }

    [Serializable]
    public struct Response
    {
        public string id;
        public ResponseChoice[] choices;
    }

}