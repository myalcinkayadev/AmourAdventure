using UnityEngine;

public enum WeaponType {
    Magic,
    Melee
}

[CreateAssetMenu(fileName = "Weapon_")]
public class Weapon : ScriptableObject
{
    [Header("General Settings")]
    public Sprite Icon;
    public WeaponType WeaponType;
    public float Damage;

    [Header("Projectile Settings")]
    public Projectile ProjectilePrefab;
    public float ManaCost;
}
