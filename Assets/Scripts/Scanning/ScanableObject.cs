using Unity.VisualScripting;
using UnityEngine;
using static Enums;

public enum ScanClueType
{
    Ninguno,
    Arma,
    Sangre
}

public class ScanableObject : MonoBehaviour
{
    private Character character;
    private GameManager gameManager;

    [Header("Configuración de pista")]
    public ScanClueType tipoPista = ScanClueType.Ninguno;
    [TextArea] public string descripcionPista;

    [Header("Datos de arma")]
    public WeaponType tipoArma;
    public bool tieneHuellas;
    public string tipoHuellas = "Desconocidas";

    [Header("Datos de sangre")]
    public string tipoSangre = "Desconocido";

    [Header("Interacción especial")]
    public bool abreCanvasResolucion = false; 
    public GameObject canvasResolucion; 

    void Start()
    {
        character = GetComponent<Character>();
        gameManager = FindObjectOfType<GameManager>(); // Busca el GameManager en la escena
    }

    public string GetScanData()
    {
        if (character != null)
        {
            string data = $"<b>{character.nombre}, {character.edad}</b>\n{character.ocupacion}</b>\nRasgos de personalidad: {character.personalidad}";

            if (gameManager != null && gameManager.crimenActual.victim == character)
            {
                data += "\n\n<b>Estado: Fallecido</b>";
                data += $"\nCausa de muerte: {gameManager.crimenActual.weapon}";
                data += $"\nLugar del crimen: {gameManager.crimenActual.room}";
            }

            return data;
        }

        // Si es una pista física
        if (tipoPista != ScanClueType.Ninguno)
        {
            string data = $"<b>Pista encontrada:</b> {tipoPista}\n{descripcionPista}";

            if (tipoPista == ScanClueType.Arma)
            {
                data += $"\n\n<b>Tipo de arma:</b> {tipoArma}";
                data += $"\n<b>Huellas encontradas:</b> {(tieneHuellas ? tipoHuellas : "No se encontraron")}";
            }
            else if (tipoPista == ScanClueType.Sangre)
            {
                data += $"\n\n<b>Tipo de sangre:</b> {tipoSangre}";
            }

            return data;
        }

        return "Datos no disponibles.";
    }

    public void IntentarAbrirCanvas()
    {
        if (abreCanvasResolucion && canvasResolucion != null)
        {
            canvasResolucion.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
    }
}
