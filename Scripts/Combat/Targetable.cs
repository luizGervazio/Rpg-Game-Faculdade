using UnityEngine;

public class Targetable : MonoBehaviour
{
    private CombatStats combatStats;

    public CombatStats CombatStats => combatStats;

    private void Awake()
    {
        combatStats = GetComponent<CombatStats>();

        if (combatStats == null)
        {
            Debug.LogError($"{gameObject.name} está sem CombatStats.");
        }
    }

    public bool CanBeTargeted()
    {
        return combatStats != null && !combatStats.IsDead && gameObject.activeInHierarchy;
    }
}