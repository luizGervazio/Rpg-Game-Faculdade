using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CombatStats))]
public class SkeletonDummy : MonoBehaviour
{
    [SerializeField] private float disableDelayAfterDeath = 1.2f;

    private CombatStats combatStats;
    private SkeletonAnimation skeletonAnimation;
    private SkeletonCombat skeletonCombat;

    public bool IsDead => combatStats != null && combatStats.IsDead;

    private void Awake()
    {
        combatStats = GetComponent<CombatStats>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonCombat = GetComponent<SkeletonCombat>();

        if (combatStats == null)
        {
            Debug.LogError($"{gameObject.name} está sem CombatStats.");
        }
    }

    private void OnEnable()
    {
        if (combatStats != null)
        {
            combatStats.OnDamageTaken += HandleDamageTaken;
            combatStats.OnDeath += HandleDeath;
        }
    }

    private void OnDisable()
    {
        if (combatStats != null)
        {
            combatStats.OnDamageTaken -= HandleDamageTaken;
            combatStats.OnDeath -= HandleDeath;
        }
    }

    private void HandleDamageTaken(int damage)
    {
        if (combatStats != null && combatStats.IsDead)
            return;

        skeletonAnimation?.PlayTakeDamage();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            skeletonCombat?.SetTarget(player.transform);
        }
    }

    private void HandleDeath()
    {
        skeletonAnimation?.PlayDeath();
        StartCoroutine(DisableAfterDeath());
    }

    private IEnumerator DisableAfterDeath()
    {
        yield return new WaitForSeconds(disableDelayAfterDeath);
        gameObject.SetActive(false);
    }
}