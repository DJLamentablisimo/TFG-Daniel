using UnityEngine;

public class ChatTrigger : MonoBehaviour
{
    public GameObject chatCanvas;
    public GameObject chatbot;

    private bool conversationStarted = false;
    private bool hasInteracted = false;

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
        if (other.CompareTag("Player") && !hasInteracted)
        {
            chatCanvas.SetActive(true);
            chatbot.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;

            var chatBotScript = chatbot.GetComponent<LLMUnitySamples.ChatBot>();
            if (chatBotScript != null)
            {
                chatBotScript.StartConversation();
            }

            conversationStarted = true;
            hasInteracted = true; // Marcamos que ya se activó
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chatCanvas.SetActive(false);
            chatbot.SetActive(false);
            conversationStarted = false;
            hasInteracted = false; // Permitimos que vuelva a activarse la próxima vez que entre
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && chatCanvas.activeSelf)
        {
            CerrarCanvas();
        }
    }

    public void CerrarCanvas()
    {
        chatCanvas.SetActive(false);
        chatbot.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
}