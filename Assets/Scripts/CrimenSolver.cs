using UnityEngine;
using TMPro;

public class CrimenSolver : MonoBehaviour
{
    public TMP_InputField inputVictima;
    public TMP_InputField inputCulpable;
    public TMP_InputField inputArma;
    public GameObject resultadoCorrecto;
    public GameObject resultadoIncorrecto;

    // Respuestas correctas (estas puedes cambiarlas din�micamente)
    public string victimaCorrecta = "Juan P�rez";
    public string culpableCorrecto = "Marta L�pez";
    public string armaCorrecta = "Cuchillo";

    public void ComprobarCrimen()
    {
        bool victimaOK = inputVictima.text.Trim().ToLower() == victimaCorrecta.ToLower();
        bool culpableOK = inputCulpable.text.Trim().ToLower() == culpableCorrecto.ToLower();
        bool armaOK = inputArma.text.Trim().ToLower() == armaCorrecta.ToLower();

        if (victimaOK && culpableOK && armaOK)
        {
            resultadoCorrecto.SetActive(true);
            resultadoIncorrecto.SetActive(false);
            // aqu� podr�as hacer avanzar el juego
        }
        else
        {
            resultadoCorrecto.SetActive(false);
            resultadoIncorrecto.SetActive(true);
        }
    }
}