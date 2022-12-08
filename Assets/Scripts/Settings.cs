using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private Player playerSettings;
    public Player PlayerSettings { get { return playerSettings;} set { playerSettings = value; } }
}
