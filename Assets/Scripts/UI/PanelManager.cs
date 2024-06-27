using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public enum PanelType
    {
        ChatPanel
    }
    public class PanelManager:MonoBehaviour
    {
        private Dictionary<PanelType, PanelBase> panelDiction = new ();
        private Dictionary<PanelType, PanelBase> invokedPanel = new ();

        [Header("Panel")]
        public ChatPanel chatPanel;
        [Header("PanelGroup")] 
        public CanvasGroup chatCanvas;
        private void Start()
        {
            chatCanvas = chatPanel.GetComponent<CanvasGroup>();
            
            panelDiction.Add(PanelType.ChatPanel,chatPanel);

            foreach (var p in panelDiction)
            {
                p.Value.gameObject.SetActive(false);
            }
        }

        public void InvokePanel(PanelType type)
        {
            if (!panelDiction[type]) return;

            var panelBase = panelDiction[type];
            panelDiction[type].gameObject.SetActive(true);
            StartCoroutine(FadeIn(1.0f));
            invokedPanel.Add(type,panelBase);
        }

        public void ClosePanel(PanelType type)
        {
            if (!invokedPanel[type]) return;
            
            StartCoroutine(FadeOut(1.0f,RemovePanel,type));
        }

        private void RemovePanel(PanelType type)
        {
            invokedPanel[type].gameObject.SetActive(false);
            invokedPanel.Remove(type);
        }
        
        IEnumerator FadeIn(float duration = 1.0f)
        {
            chatCanvas.alpha = 0.0f;
            float time = 0.0f;
            while (time<duration)
            {
                time += Time.deltaTime;
                chatCanvas.alpha = time / duration;
                yield return null;
            }
            chatCanvas.alpha = 1.0f;
        }
        
        IEnumerator FadeOut(float duration,Action<PanelType> callBack,PanelType type)
        {
            chatCanvas.alpha = 1.0f;
            float time = duration;
            while (time>0.0f)
            {
                time -= Time.deltaTime;
                chatCanvas.alpha = time / duration;
                yield return null;
            }
            chatCanvas.alpha = 0.0f;
            callBack?.Invoke(type);
        }
    }
}