using LLMUnity;
using LLMUnitySamples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class GameManager : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public List<Character> listaPersonajes = new List<Character>();
    public GameObject canvasPrefab;
    public GameObject chatbotPrefab;

    public Crime crimenActual;

    private List<ChatBot> chatbotsInstanciados = new List<ChatBot>();

    public CrimenSolver crimenSolver;

    public Collider[] spawnAreasPersonajes; // Áreas de spawn (por habitación)
    private List<Collider> usedAreas = new List<Collider>();

    [Header("Prefabs de Pistas")]
    public GameObject armaCuchilloPrefab;
    public GameObject armaPistolaPrefab;
    public GameObject armaMartilloPrefab;
    public GameObject armaCuerdaPrefab;
    public GameObject armaVenenoPrefab;
    public List<GameObject> prefabsSangre;
    public List<GameObject> prefabsFingerprints;
    public List<GameObject> prefabsFootprints;


    private Dictionary<WeaponType, Quaternion> rotacionArmas = new Dictionary<WeaponType, Quaternion>
    {
        { WeaponType.ArmaBlanca, Quaternion.Euler(90, 0, 0) },
        { WeaponType.ArmaDeFuego, Quaternion.Euler(90, 0, 0) },
        { WeaponType.ObjetoContundente, Quaternion.Euler(0, 0, 90) },
        { WeaponType.Veneno, Quaternion.Euler(0, 0, 0) },
        { WeaponType.Cuerda, Quaternion.Euler(90, 0, 0) }
    };

    void Start()
    {
        // Inicializar personajes
        GenerarPersonajes();

        // Generación del crimen
        GenerarCrimen();
       // StartCoroutine(PrecalentarTodosLosChatbots());

        string jsonCrime = JsonUtility.ToJson(crimenActual, true);
        Debug.Log("Crimen generado en formato JSON:\n" + jsonCrime);

        InstanciarElementosFisicos();
        StartCoroutine(PrecalentarTodosLosChatbots());
    }
    /*
        void GenerarPersonajes()
        {
            List<GameObject> seleccionados = new List<GameObject>();

            while (seleccionados.Count < 4 && characterPrefabs.Length > 0)
            {
                GameObject personajePrefab = characterPrefabs[Random.Range(0, characterPrefabs.Length)];
                if (!seleccionados.Contains(personajePrefab))
                {
                    // Instanciar personaje
                    GameObject personajeInstancia = Instantiate(personajePrefab);
                    seleccionados.Add(personajePrefab);
                    listaPersonajes.Add(personajeInstancia.GetComponent<Character>());

                    // Instanciar Canvas y Chatbot únicos
                    GameObject canvasInstancia = Instantiate(canvasPrefab);
                    canvasInstancia.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    GameObject chatbotInstancia = Instantiate(chatbotPrefab);


                    // Buscar el container dentro del canvas recién instanciado
                    Transform container = canvasInstancia.transform.Find("ChatPanel");
                    Button stopButton = canvasInstancia.transform.Find("StopButton")?.GetComponent<Button>();
                    LLMCharacter llm = chatbotInstancia.GetComponentInChildren<LLMCharacter>();
                    ChatBot chatBotScript = chatbotInstancia.GetComponent<ChatBot>();

                    if (chatBotScript != null)
                    {
                        if (container != null)
                            chatBotScript.chatContainer = container;
                        else
                            Debug.LogWarning("❌ No se encontró 'ChatContainer' en el canvas duplicado.");

                        if (stopButton != null)
                            chatBotScript.stopButton = stopButton;
                        else
                            Debug.LogWarning("❌ No se encontró 'StopButton' dentro del canvas duplicado.");

                        if (llm != null)
                            chatBotScript.llmCharacter = llm;
                        else
                            Debug.LogWarning("❌ No se encontró LLMCharacter dentro del chatbot instanciado.");

                        chatbotsInstanciados.Add(chatBotScript);
                        chatBotScript.Inicializar();
                    }
                    else
                    {
                        Debug.LogWarning("❌ No se encontró el script ChatBot en el chatbotInstancia.");
                    }

                    canvasInstancia.SetActive(false);
                    chatbotInstancia.SetActive(false);

                    ChatTrigger chatTrigger = personajeInstancia.GetComponentInChildren<ChatTrigger>();
                    if (chatTrigger != null)
                    {
                        chatTrigger.chatCanvas = canvasInstancia;
                        chatTrigger.chatbot = chatbotInstancia;

                        Debug.Log($"✅ Asignados canvas y chatbot a {personajeInstancia.name}");
                    }
                    else
                    {
                        Debug.LogWarning($"⚠️ {personajeInstancia.name} no tiene ChatTrigger");
                    }
                }
            }
        }
    */
    void GenerarPersonajes()
    {
        List<GameObject> seleccionados = new List<GameObject>();

        while (seleccionados.Count < 4 && characterPrefabs.Length > 0)
        {
            GameObject personajePrefab = characterPrefabs[Random.Range(0, characterPrefabs.Length)];
            if (!seleccionados.Contains(personajePrefab))
            {
                // Instanciar personaje
                GameObject personajeInstancia = Instantiate(personajePrefab);
                seleccionados.Add(personajePrefab);
                listaPersonajes.Add(personajeInstancia.GetComponent<Character>());

                // Instanciar Canvas y Chatbot únicos
                GameObject canvasInstancia = Instantiate(canvasPrefab);
                canvasInstancia.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                GameObject chatbotInstancia = Instantiate(chatbotPrefab);


                // Buscar el container dentro del canvas recién instanciado
                Transform container = canvasInstancia.transform.Find("ChatPanel");
                if (container != null)
                {
                    ChatBot chatBotScript = chatbotInstancia.GetComponent<ChatBot>();
                    if (chatBotScript != null)
                    {
                        chatBotScript.chatContainer = container;
                        chatbotsInstanciados.Add(chatBotScript);
                    }
                    else
                    {
                        Debug.LogWarning("❌ No se encontró ChatBot en el chatbotInstancia.");
                    }
                }
                else
                {
                    Debug.LogWarning("❌ No se encontró 'ChatContainer' en el canvas duplicado.");
                }

                // Buscar el StopButton dentro del Canvas instanciado
                Button stopButton = canvasInstancia.transform.Find("StopButton")?.GetComponent<Button>();

                if (stopButton != null)
                {
                    ChatBot chatBotScript = chatbotInstancia.GetComponent<ChatBot>();
                    if (chatBotScript != null)
                    {
                        chatBotScript.stopButton = stopButton;
                    }
                    else
                    {
                        Debug.LogWarning("❌ No se encontró el script ChatBot en el chatbotInstancia.");
                    }
                }
                else
                {
                    Debug.LogWarning("❌ No se encontró 'StopButton' dentro del canvas duplicado.");
                }

                canvasInstancia.SetActive(false);
                chatbotInstancia.SetActive(false);

                // Asignar al ChatTrigger
                ChatTrigger chatTrigger = personajeInstancia.GetComponentInChildren<ChatTrigger>();
                if (chatTrigger != null)
                {
                    chatTrigger.chatCanvas = canvasInstancia;
                    chatTrigger.chatbot = chatbotInstancia;

                    Debug.Log($"✅ Asignados canvas y chatbot a {personajeInstancia.name}");
                }
                else
                {
                    Debug.LogWarning($"⚠️ {personajeInstancia.name} no tiene ChatTrigger");
                }
            }
        }
    }
    void GenerarCrimen()
    {
        if (listaPersonajes.Count < 2)
        {
            Debug.LogError("No hay suficientes personajes para generar un crimen.");
            return;
        }

        crimenActual = new Crime();

        //1. Selección culpable y victima
        int indexVictima = UnityEngine.Random.Range(0, listaPersonajes.Count);
        int indexCulpable;
        do
        {
            indexCulpable = UnityEngine.Random.Range(0, listaPersonajes.Count);
        } while (indexCulpable == indexVictima);

        Character victima = listaPersonajes[indexVictima];
        Character culpable = listaPersonajes[indexCulpable];

        victima.rol = CharacterRole.Victima;
        culpable.rol = CharacterRole.Asesino;
        crimenActual.victim = victima;
        crimenActual.culprit = culpable;

        //2. Selección otros atributos
        //Arma:
        int numWeaponTypes = System.Enum.GetValues(typeof(WeaponType)).Length;
        crimenActual.weapon = (WeaponType)UnityEngine.Random.Range(0, numWeaponTypes);

        // Habitación:
        int numRoomTypes = System.Enum.GetValues(typeof(RoomType)).Length;
        crimenActual.room = (RoomType)UnityEngine.Random.Range(0, numRoomTypes);

        // Motivo:
        int numMotiveTypes = System.Enum.GetValues(typeof(MotiveType)).Length;
        crimenActual.motive = (MotiveType)UnityEngine.Random.Range(0, numMotiveTypes);

        //3. Generación de las pistas
        string descripcionArma = ""; //Una pista obligatoria relacionada con el arma
        switch (crimenActual.weapon)
        {
            case WeaponType.ArmaBlanca:
                descripcionArma = "Cuchillo ensangrentado";
                break;
            case WeaponType.ArmaDeFuego:
                descripcionArma = "Arma de fuego con residuos de pólvora";
                break;
            case WeaponType.ObjetoContundente:
                descripcionArma = "Objeto contundente, como un martillo";
                break;
            case WeaponType.Veneno:
                descripcionArma = "Bote de veneno; sustancia tóxica";
                break;
            case WeaponType.Cuerda:
                descripcionArma = "Cuerda con rastros de uso violento";
                break;
        }
        crimenActual.clues.Add(new Clue(ClueType.Arma, descripcionArma));

        ClueType[] posiblesClues = {
            ClueType.Sangre,
            ClueType.HuellasDactilares,
            ClueType.MarcaDeZapatos,
            ClueType.Nota,
        };

        // Agregar pistas adicionales aleatorias de una lista de opciones
        int numAditionalClues = 2;
        for (int i = 0; i < numAditionalClues; i++)
        {
            int indicePista = UnityEngine.Random.Range(0, posiblesClues.Length);
            ClueType pistaSeleccionada = posiblesClues[indicePista];
            string descripcion = "";

            switch (pistaSeleccionada)
            {
                case ClueType.Sangre:
                    descripcion = "Manchas de sangre en el lugar del crimen";
                    break;
                case ClueType.HuellasDactilares:
                    descripcion = "Huellas dactilares en superficies";
                    break;
                case ClueType.MarcaDeZapatos:
                    descripcion = "Huella de zapatos cerca del escenario";
                    break;
                case ClueType.Nota:
                    descripcion = "Una nota críptica encontrada en la escena";
                    break;
            }

            bool yaExiste = crimenActual.clues.Exists(c => c.clueType == pistaSeleccionada);
            if (!yaExiste)
            {
                crimenActual.clues.Add(new Clue(pistaSeleccionada, descripcion));
            }
        }

        foreach (Character personaje in listaPersonajes)
        {
            ChatTrigger trigger = personaje.GetComponentInChildren<ChatTrigger>();
            if (trigger != null && trigger.chatbot != null)
            {
                ChatBot chatBotScript = trigger.chatbot.GetComponent<ChatBot>();
                if (chatBotScript != null && chatBotScript.llmCharacter != null)
                {
                    string prompt = GenerarPromptParaPersonaje(personaje);
                    chatBotScript.llmCharacter.SetPrompt(prompt, true);                
                    Debug.Log($"🧠 Prompt asignado a {personaje.nombre}:\n{prompt}");
                }
                else
                {
                    Debug.LogWarning($"❌ El ChatBot de {personaje.nombre} no tiene LLMCharacter asignado en el componente ChatBot.");
                }
            }
        }

        if (crimenSolver != null)
        {
            crimenSolver.victimaCorrecta = crimenActual.victim.nombre;
            crimenSolver.culpableCorrecto = crimenActual.culprit.nombre;

            // Arma como texto
            switch (crimenActual.weapon)
            {
                case WeaponType.ArmaBlanca:
                    crimenSolver.armaCorrecta = "Cuchillo";
                    break;
                case WeaponType.ArmaDeFuego:
                    crimenSolver.armaCorrecta = "Pistola";
                    break;
                case WeaponType.ObjetoContundente:
                    crimenSolver.armaCorrecta = "Martillo";
                    break;
                case WeaponType.Cuerda:
                    crimenSolver.armaCorrecta = "Cuerda";
                    break;
                case WeaponType.Veneno:
                    crimenSolver.armaCorrecta = "Veneno";
                    break;
                default:
                    crimenSolver.armaCorrecta = "???";
                    break;
            }
        }
    }
    
    void InstanciarElementosFisicos()
    {
        usedAreas.Clear();
        Collider areaVictima = ObtenerAreaPorNombre(crimenActual.room.ToString());

        foreach (Character personaje in listaPersonajes)
        {
            GameObject go = personaje.gameObject;

            if (personaje.rol == CharacterRole.Victima)
            {
                if (areaVictima != null)
                {
                    go.transform.position = PosicionAleatoriaEnArea(areaVictima);
                    go.transform.rotation = Quaternion.Euler(0, 10, 90);
                    usedAreas.Add(areaVictima);
                }
            }
            else
            {
                Collider area;
                do
                {
                    area = spawnAreasPersonajes[Random.Range(0, spawnAreasPersonajes.Length)];
                } while (usedAreas.Contains(area));

                go.transform.position = PosicionAleatoriaEnArea(area);
                go.transform.rotation = Quaternion.identity;
                usedAreas.Add(area);
            }
        }
        Collider areaAsesino = ObtenerAreaDePersonaje(crimenActual.culprit);
        InstanciarPistasFisicas(areaVictima, areaAsesino);
    }

    void InstanciarPistasFisicas(Collider areaVictima, Collider areaAsesino)
    {

        Character culpable = crimenActual.culprit;
        Character victima = crimenActual.victim;

        GameObject armaPrefab = ObtenerPrefabArma(crimenActual.weapon);
        if (armaPrefab != null && areaAsesino != null)
        {
            Quaternion rotacion = rotacionArmas.ContainsKey(crimenActual.weapon) ? rotacionArmas[crimenActual.weapon] : Quaternion.identity;
            Instantiate(armaPrefab, PosicionAleatoriaEnArea(areaAsesino), rotacion);
        }

        if (crimenActual.weapon == WeaponType.ArmaBlanca ||
            crimenActual.weapon == WeaponType.ArmaDeFuego ||
            crimenActual.weapon == WeaponType.ObjetoContundente)
        {
            if (prefabsSangre != null && prefabsSangre.Count > 0)
            {
                if (areaVictima != null)
                {
                    GameObject sangre = prefabsSangre[Random.Range(0, prefabsSangre.Count)];
                    Instantiate(sangre, PosicionAleatoriaEnArea(areaVictima), Quaternion.identity);

                    ScanableObject so = sangre.GetComponent<ScanableObject>();
                    if (so != null)
                    {
                        so.tipoPista = ScanClueType.Sangre;
                        so.descripcionPista = "Manchas de sangre cerca del cuerpo.";
                        so.tipoSangre = victima.bloodType.ToString(); // Asume que bloodType es un enum o string
                    }
                }
                if (areaAsesino != null && Random.value < 0.5f)
                {
                    GameObject sangre = prefabsSangre[Random.Range(0, prefabsSangre.Count)];
                    Instantiate(sangre, PosicionAleatoriaEnArea(areaAsesino), Quaternion.identity);

                    ScanableObject so = sangre.GetComponent<ScanableObject>();
                    if (so != null)
                    {
                        so.tipoPista = ScanClueType.Sangre;
                        so.descripcionPista = "Manchas de sangre cerca del cuerpo.";
                        so.tipoSangre = culpable.bloodType.ToString(); // Asume que bloodType es un enum o string
                    }
                }
            }
        }

        // 🧬 Huellas dactilares (culpable)
        if (crimenActual.clues.Exists(c => c.clueType == ClueType.HuellasDactilares))
        {
            if (prefabsFingerprints != null && prefabsFingerprints.Count > 0 && areaAsesino != null)
            {
                GameObject huella = Instantiate(
                    prefabsFingerprints[Random.Range(0, prefabsFingerprints.Count)],
                    PosicionAleatoriaEnArea(areaAsesino),
                    Quaternion.identity
                );

                ScanableObject so = huella.GetComponent<ScanableObject>();
                if (so != null)
                {
                    so.tipoPista = ScanClueType.HuellasDactilares;
                    so.descripcionPista = "Huellas encontradas cerca del lugar.";
                    so.origenHuellasDactilares = $"Código {culpable.fingerprint}";
                    so.propietarioPista = culpable;
                }
            }
        }

        // 👟 Huellas de zapato (culpable)
        if (crimenActual.clues.Exists(c => c.clueType == ClueType.MarcaDeZapatos))
        {
            if (prefabsFootprints != null && prefabsFootprints.Count > 0 && areaAsesino != null)
            {
                GameObject zapato = Instantiate(
                    prefabsFootprints[Random.Range(0, prefabsFootprints.Count)],
                    PosicionAleatoriaEnArea(areaAsesino),
                    Quaternion.identity
                );

                ScanableObject so = zapato.GetComponent<ScanableObject>();
                if (so != null)
                {
                    so.tipoPista = ScanClueType.HuellasDeZapato;
                    so.descripcionPista = "Huellas de zapato encontradas en la escena.";
                    so.tipoZapato = $"Talla {culpable.footSize}";
                }
            }
        }
    }

    GameObject ObtenerPrefabArma(WeaponType tipo)
    {
        switch (tipo)
        {
            case WeaponType.ArmaBlanca: return armaCuchilloPrefab;
            case WeaponType.ArmaDeFuego: return armaPistolaPrefab;
            case WeaponType.ObjetoContundente: return armaMartilloPrefab;
            case WeaponType.Veneno: return armaVenenoPrefab;
            case WeaponType.Cuerda: return armaCuerdaPrefab;
            default: return null;
        }
    }

    Collider ObtenerAreaPorNombre(string nombre)
    {
        foreach (Collider col in spawnAreasPersonajes)
        {
            if (col.name == nombre)
            {
                return col;
            }
        }
        return null;
    }

    Collider ObtenerAreaDePersonaje(Character personaje)
    {
        foreach (Collider col in spawnAreasPersonajes)
        {
            if (col.bounds.Contains(personaje.transform.position))
            {
                return col;
            }
        }
        return null;
    }

    Vector3 PosicionAleatoriaEnArea(Collider area)
    {
        Vector3 min = area.bounds.min;
        Vector3 max = area.bounds.max;

        Vector3 randomPos = new Vector3(
            Random.Range(min.x, max.x),
            area.bounds.max.y + 1f,
            Random.Range(min.z, max.z)
        );

        RaycastHit hit;
        if (Physics.Raycast(randomPos, Vector3.down, out hit, 20f))
        {
            return hit.point;
        }

        return area.bounds.center;
    }
    string GenerarPromptParaPersonaje(Character personaje)
    {
        string intro = "Eres un personaje jugable dentro de un videojuego de simulación de crímenes generados de forma procedural, el cual será interrogado por el jugador, no te salgas del personaje. \n\n";

        string personajes = "Los distintos personajes son\r\n los siguientes: Edmund Gandia, 32 años, Cirujano, personalidad pragmática; Clara\r\n Cebique, 24 años, estudiante, personalidad nerviosa; Gerard Tule, 47 años, abogado,\r\npersonalidad Optimista; Calvo Otelo, 36 años, profesor, personalidad desorganizada. \n\n";

        string datosCrimen = $"Los datos del crimen actual son:\n" +
                             $"- Asesino = {crimenActual.culprit.nombre}\n" +
                             $"- Víctima = {crimenActual.victim.nombre}\n" +
                             $"- Lugar = {crimenActual.room}\n" +
                             $"- Motivo = {crimenActual.motive}\n" +
                             $"- Arma = {crimenActual.weapon}\n";

        string pistas = "- Pistas disponibles:\n";
        foreach (var clue in crimenActual.clues)
        {
            pistas += $"  • {clue.descripcion}\n";
        }

        string rol = $"Tu personaje es el de {(personaje == crimenActual.culprit ? "CULPABLE" : personaje == crimenActual.victim ? "VÍCTIMA" : "TESTIGO")}.\n";

        string instruccion = "Sigue el interrogatorio del jugador de la forma más natural posible, responde como si fueras ese personaje.";

        return intro + personajes + datosCrimen + pistas + "\n" + rol + instruccion;
    }

    IEnumerator PrecalentarTodosLosChatbots()
    {
        foreach (ChatBot bot in chatbotsInstanciados)
        {
            if (bot != null && bot.llmCharacter != null)
            {
                Debug.Log($"🚀 Iniciando warmup de: {bot.gameObject.name}");
                yield return bot.llmCharacter.Warmup(bot.WarmUpCallback);
            }
            else
            {
                Debug.LogWarning($"⚠️ ChatBot inválido o sin LLMCharacter: {bot?.gameObject.name ?? "null"}");
            }
        }
    }
} 

