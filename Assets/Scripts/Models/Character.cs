using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public enum BloodType
{
    A,
    B,
    O,
    AB
}

[System.Serializable]
public class Character : MonoBehaviour
{
    public string nombre;
    public int edad;
    public string ocupacion;
    public string personalidad;
    public BloodType bloodType;
    public int fingerprint;
    public int footSize;
    public CharacterRole rol;

    // Otros datos adicionales
    public bool tieneRasgoExtra;  // Para el añadido (ej. acento extraño, expresiones peculiares, etc.)
    public bool haPresenciadoAlgoImportante; // Para el añadido del testigo.

    // Constructor (opcional)
    public Character(string nombre, int edad, string ocupacion, string personalidad)
    {
        this.nombre = nombre;
        this.edad = edad;
        this.ocupacion = ocupacion;
        this.personalidad = personalidad;
        this.rol = CharacterRole.None;
        this.tieneRasgoExtra = false;
        this.haPresenciadoAlgoImportante = false;
    }
}
