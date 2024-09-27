using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ScriptableObject
{
    public string skillName;
    public string triggerName;
    public string description;
    public GameObject prefab;
    public MonoBehaviour script;
}