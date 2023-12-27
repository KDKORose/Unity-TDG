using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "Tower")]
public class TowerScriptableObject : ScriptableObject
{
    public String towerName;
    public int damage;
    public float attackSpeed;
    public float range;
    public float fireRate;
}
