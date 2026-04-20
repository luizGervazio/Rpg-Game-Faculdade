using UnityEngine;

public class SkeletonDummy : MonoBehaviour
{
    private CombatStats combatStats;

    public bool IsDead => combatStats != null && combatStats.IsDead;

    private void Awake()
    {
        combatStats = GetComponent<CombatStats>();

        if (combatStats == null)
        {
            Debug.LogError($"{gameObject.name} está sem CombatStats.");
        }
    }
}