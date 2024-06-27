using System;
using System.Collections;
using Agent;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ChatPanel : PanelBase
    {
        [Header("AgentChatController")] 
        public AgentChatController agentChatController;
        public Agent.Agent playerAgent;
        [Header("UIWidget")]
        public InputField inputField;
        public Text characterName;
        public Image characterSprite;
        public Text content;
        public Button btn;
        public GameObject contentParent;
        public float textSpeed;
    
        [Header("Boolean")]
        public bool isDialogue;
        // Start is called before the first frame update
        private void Start()
        {
            inputField.onSubmit.AddListener(SubmitUserInput);
            btn.onClick.AddListener(EnableInput);
        }

        private void OnEnable()
        {
            inputField.text = playerAgent.prompt;
            characterName.text = playerAgent.agentName;
            characterSprite.sprite = playerAgent.agentSprite;
        }

        private void OnDisable()
        {
            content.text = "";
            inputField.text = "";
        }

        public void OnReceiveMessage(Agent.Agent agent,string message)
        {
            ShowDialogueContent(agent,message);
        }
        
        private void SubmitUserInput(string input)
        {
            DisableInput();
            //Send Request and Update Agent Text
            agentChatController.SendRequestToServer(input,OnReceiveMessage);
        }

        private void ShowDialogueContent(Agent.Agent agent,string message)
        {
            characterName.text = agent.agentName;
            characterSprite.sprite = agent.agentSprite;
            StartCoroutine(DialogueContent(message));
        }

        private void EnableInput()
        {
            inputField.text = string.Empty;
            inputField.interactable = true;
            characterName.text = playerAgent.agentName;
            characterSprite.sprite = playerAgent.agentSprite;
        }
        public void DisableInput()
        {
            btn.gameObject.SetActive(false);
            contentParent.SetActive(true);
            inputField.interactable = false;
        }

        private IEnumerator DialogueContent(string message)
        {
            isDialogue = true;
            content.text = "";
            foreach (var c in message)
            {
                content.text += c;
                yield return new WaitForSeconds(textSpeed);
            }

            isDialogue = false;
            btn.gameObject.SetActive(true);
        }
    }
}
