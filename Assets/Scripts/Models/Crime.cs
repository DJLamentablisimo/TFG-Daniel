using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

[System.Serializable]
public class Crime : MonoBehaviour
{
    public Character victim;
    public Character culprit;
    public WeaponType weapon;
    public RoomType room;
    public MotiveType motive;
    public List<Clue> clues;

    /* DATOS EXTRA QUE NO SE SI LLEGARÉ A IMPLEMENTAR
    public string horaDelCrimen;
    public string modusOperandi;
    public string factoresAmbientales; 
     */

    public Crime()
    {
        clues = new List<Clue>();
    }
}
