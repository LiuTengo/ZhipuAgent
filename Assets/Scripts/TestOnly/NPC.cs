using Agent;
using UI;
using UnityEngine;

namespace TestOnly
{
    public class NPC : MonoBehaviour
    {
        public PanelManager panelManager;
        
        private PlayerController playerController;
        private Agent.Agent m_Agent;
        private bool isChatting = false;

        private void Start()
        {
            m_Agent = GetComponent<Agent.Agent>();
        }

        private void Update()
        {
            if (!playerController) return;
            
            if (!isChatting && Input.GetKeyDown(KeyCode.E))
            {
                if (!m_Agent) return;
                StartChat();
            }

            if (isChatting && Input.GetKeyDown(KeyCode.Escape))
            {
                FinishChat();
            }
        }

        private void StartChat()
        {
            isChatting = true;
            panelManager.InvokePanel(PanelType.ChatPanel);
            playerController.DisableMovement();
            AgentChatController.Instance.StartChat(m_Agent);
        }

        private void FinishChat()
        {
            isChatting = false;
            panelManager.ClosePanel(PanelType.ChatPanel);
            playerController.EnableMovement();
            AgentChatController.Instance.FinishChat();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerController = other.GetComponent<PlayerController>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerController = null;
            }
        }
    }
}