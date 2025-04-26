using UnityEngine;
using TMPro;

public class CrimenSolver : MonoBehaviour
{
    public TMP_InputField inputVictima;
    public TMP_InputField inputCulpable;
    public TMP_InputField inputArma;
    public GameObject resultadoCorrecto;
    public GameObject resultadoIncorrecto;

    public string victimaCorrecta;
    public string culpableCorrecto;
    public string armaCorrecta;

    public void ComprobarCrimen()
    {
        bool victimaOK = inputVictima.text.Trim().ToLower() == victimaCorrecta.ToLower();
        bool culpableOK = inputCulpable.text.Trim().ToLower() == culpableCorrecto.ToLower();
        bool armaOK = inputArma.text.Trim().ToLower() == armaCorrecta.ToLower();

        if (victimaOK && culpableOK && armaOK)
        {
            resultadoCorrecto.SetActive(true);
            resultadoIncorrecto.SetActive(false);
            // aquí podrías hacer avanzar el juego
        }
        else
        {
            resultadoCorrecto.SetActive(false);
            resultadoIncorrecto.SetActive(true);
        }
    }
}