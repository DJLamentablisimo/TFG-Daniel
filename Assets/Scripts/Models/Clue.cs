using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

[System.Serializable]
public class Clue : MonoBehaviour
{
    public ClueType clueType;
    public string descripcion;
    public bool esPistaFalsa;

    public Clue(ClueType clueType, string descripcion, bool esFalsa = false)
    {
        this.clueType = clueType;
        this.descripcion = descripcion;
        this.esPistaFalsa = esFalsa;
    }
}
