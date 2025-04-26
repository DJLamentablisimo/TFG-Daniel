using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scanner : MonoBehaviour
{
    public Camera playerCamera;  // Cámara del jugador
    public float scanRange = 10f; // Rango del escaneo
    public LayerMask scanLayer;  // Capa de los objetos escaneables
    public GameObject scanUI; // UI de escaneo
    public GameObject scanModeOverlay; // Oscurece la pantalla en modo de escaneo
    public Image scanProgressBar; // Barra de progreso del escaneo
    public TMP_Text scanText; // Texto de la UI

    private bool isScanning = false;
    private float scanTime = 2f; // Tiempo para completar el escaneo
    private float scanProgress = 0f;
    private ScanableObject currentTarget;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            StartScanningMode();
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            StopScanningMode();
        }

        if (isScanning)
        {
            ScanDetection();
        }
    }

    void StartScanningMode()
    {
        isScanning = true;
        scanModeOverlay.SetActive(true);
        scanUI.SetActive(true);
        scanText.text = "Mantén la mira en un objeto para escanear...";
    }

    void StopScanningMode()
    {
        isScanning = false;
        scanModeOverlay.SetActive(false);
        scanUI.SetActive(false);
        scanProgress = 0f;
        scanProgressBar.fillAmount = 0f;
        currentTarget = null;
    }

    void SetRenderColor(GameObject obj, Color color)
    {
        if (obj.CompareTag("pistol"))
        {
            foreach (Renderer r in obj.GetComponentsInChildren<Renderer>())
            {
                r.material.color = color;
            }
        }
        else
        {
            Renderer r = obj.GetComponent<Renderer>();
            if (r != null)
            {
                r.material.color = color;
            }
        }
    }

    void ScanDetection()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, scanRange, scanLayer))
        {
            ScanableObject target = hit.collider.GetComponentInParent<ScanableObject>();

            if (target != null)
            {
                GameObject targetGO = target.gameObject;

                if (currentTarget != target)
                {
                    if (currentTarget != null)
                        SetRenderColor(currentTarget.gameObject, Color.white);

                    currentTarget = target;
                    SetRenderColor(targetGO, Color.yellow);
                    scanProgress = 0f;
                    scanProgressBar.fillAmount = 0f;
                }

                if (scanProgress < scanTime)
                {
                    scanProgress += Time.deltaTime;
                    scanProgressBar.fillAmount = scanProgress / scanTime;
                }

                if (scanProgress >= scanTime)
                {
                    scanProgress = scanTime;
                    scanProgressBar.fillAmount = 1f;
                    SetRenderColor(targetGO, Color.green);

                    if (scanText.text == "Mantén la mira en un objeto para escanear..." || scanText.text == "")
                    {
                        DisplayScanInfo(currentTarget);
                    }
                }
            }
        }
        else
        {
            if (currentTarget != null)
                SetRenderColor(currentTarget.gameObject, Color.white);

            scanProgress = 0f;
            scanProgressBar.fillAmount = 0f;
            currentTarget = null;
        }
    }

    void DisplayScanInfo(ScanableObject obj)
    {
        scanUI.SetActive(true);
        scanText.text = obj.GetScanData();

        // Abrir canvas especial si aplica
        obj.IntentarAbrirCanvas();
    }
}
