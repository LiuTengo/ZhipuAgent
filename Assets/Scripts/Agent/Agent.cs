using System.Collections.Generic;
using UnityEngine;

namespace Agent
{
    public enum AgentRole
    {
        user,
        system,
        assistant,
        tool
    }
    
    public class Agent : MonoBehaviour
    {
        [SerializeField] public string agentName = "Agent";
        [SerializeField] public Sprite agentSprite;
        [SerializeField] public AgentRole role = AgentRole.system;
        [SerializeField] private bool usingPrompt = true;
        [SerializeField,TextArea] public string prompt = "你好";
        [SerializeField] private bool hasMemoryLimit;
        [SerializeField] private int maxMemory = 10;
        [SerializeField] private List<MessageData> memory = new();

        private void Awake()
        {
            if (!usingPrompt && role == AgentRole.user) return;
            
            AddMemory(AgentRole.user,prompt);
        }

        public void ClearMemory()
        {
            if (hasMemoryLimit)
            {
                memory.Clear();
                AddMemory(AgentRole.user,prompt);
            }
        }
        
        public void AddMemory(AgentRole agentRole,string memoryContent)
        {
            MessageData memoryData = new MessageData(agentRole.ToString(),memoryContent);
            memory.Add(memoryData);
            
            if (memory.Count>maxMemory)
            {
                var m = memory[1]; //为了维护初始的prompt，因此这里填1
                memory.Remove(m);
            }
        }
        
        public void AddMemory(string memoryContent)
        {
            MessageData memoryData = new MessageData(role.ToString(),memoryContent);
            memory.Add(memoryData);
            
            if (memory.Count>maxMemory)
            {
                var m = memory[1]; //为了维护初始的prompt，因此这里填1
                memory.Remove(m);
            }
        }
        
        public List<MessageData> GetAgentMemory()
        {
            return memory;
        }
    }
}