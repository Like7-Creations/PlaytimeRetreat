using UnityEngine;

public class Ability : ScriptableObject
{
    public new string name;
    public float cooldown;
    public float activeTime;

    public virtual void Activate() { }
    public virtual void BeginCooldown() { }
}
