using UnityEngine;

public class Ability : ScriptableObject
{
    public AbilityHolder abilityHolder;

    public new string name;
    public float cooldown;
    public float activeTime;

    private void Awake()
    {
        //abilityHolder = FindObjectOfType<AbilityHolder>();
    }

    public virtual void Activate() { }
    public virtual void BeginCooldown() { }
}
