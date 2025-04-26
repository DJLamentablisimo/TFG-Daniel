using UnityEngine;

public class ChatTrigger : MonoBehaviour
{
    // Arrastra y suelta el Canvas de chat desde el Inspector
    public GameObject chatCanvas;
    public GameObject chatbot;

    private bool conversationStarted = false;

    void Start()
    {
        if (chatCanvas == null)
        {
            chatCanvas = GameObject.Find("CanvasChat");
        } 
        if (chatbot == null)
        {
            chatbot = GameObject.Find("ChatBot");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chatCanvas.SetActive(true);
            chatbot.SetActive(true);

            if (!conversationStarted)
            {
                var chatBotScript = chatbot.GetComponent<LLMUnitySamples.ChatBot>();
                if (chatBotScript != null)
                {
                    chatBotScript.StartConversation();
                }
                conversationStarted = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chatCanvas.SetActive(false);
            chatbot.SetActive(false);
            conversationStarted = false;
        }
    }
}