using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

[System.Serializable]
public class Clue : MonoBehaviour
{
    public ClueType clueType;
    public string descripcion;

    public Clue(ClueType clueType, string descripcion)
    {
        this.clueType = clueType;
        this.descripcion = descripcion;
    }
}
