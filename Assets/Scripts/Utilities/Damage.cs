using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    // how much damage
    public int damage;
    // Who dealt it.
    public GameObject caster;
    //incase this is in a proc chain. THIS is who dealt the damage
    public GameObject originalCaster;
    public List<DamageSource> procChain = new List<DamageSource>();
    public Vector3 initialPosition;
    public Vector3 initialDirection;
    /// <summary>
    /// how much the next roll to deal damage is multiplied by
    /// </summary>
    public float procCoefficient = 1f;
    // tags used to tel what time of damage it is
    public DamageTags tags;
    public bool crit;

    public bool CanProc(int maxChainDepth = 8)
    {
        return procCoefficient > 0f && procChain.Count < maxChainDepth;
    }

    public DamageSource? PreviousSource()
    {
        return procChain.Count > 0 ? procChain[procChain.Count - 1] : (DamageSource?)null;
    }

    /// <summary>
    /// Creates the next proc damage object from itself.
    /// </summary>
    /// <param name="procDamage"></param>
    /// <param name="procCoeff"></param>
    /// <param name="procCaster"></param>
    /// <param name="procTags"></param>
    /// <returns></returns>
    public Damage CreateProcDamage(int procDamage, float procCoeff, GameObject procCaster, DamageTags procTags)
    {
        Damage next = new Damage
        {
            damage = procDamage,
            procCoefficient = this.procCoefficient * procCoeff,
            caster = procCaster,
            originalCaster = this.originalCaster,
            tags = procTags,
            initialPosition = this.initialPosition,
            initialDirection = this.initialDirection,
            procChain = new List<DamageSource>(this.procChain)
        };

        next.procChain.Add(new DamageSource(caster));
        return next;
    }
}

[System.Serializable]
public struct DamageSource
{
    ///The GameObject that triggered this stage in the proc coeff
    public GameObject source;

    public DamageSource(GameObject source)
    {
        this.source = source;
    }
}

/// <summary>
/// Damage Flags for tracking procs and such.
/// </summary>
[System.Flags]
public enum DamageTags
{
    Physical = 0,
    Bleed = 1 << 0,  
    AoE = 1 << 1,   
    Stunning = 1 << 2,
    Fire = 1 << 3,
    Ice = 1 << 4,   
    Electric = 1 << 5,   
    Proc = 1 << 6,   // Damage was spawned by a proc
}

public interface IDamageable
{
    public void TakeDamage(Damage damage);
}