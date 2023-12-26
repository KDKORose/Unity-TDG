using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public float health;
    public float damage;
    public float moveSpeed;
    public Sprite image;
    public String type;
}
