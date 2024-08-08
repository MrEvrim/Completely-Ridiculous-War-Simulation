using UnityEngine;

public class Unit : MonoBehaviour
{
    public float health = 100f;
    public float moveSpeed = 3f;
    public float attackRange = 3f;
    public float attackDamage = 20f;
    public float minAttackCooldown = 1f;
    public float maxAttackCooldown = 5f;
    public float rollForce = 300f;
    public float damageCooldown = 5f; // Hasar alma gecikmesi

    private float lastAttackTime;
    private float attackCooldown;
    private float lastDamageTime; // Son hasar zamanı
    private MonoBehaviour target;
    private Rigidbody rb;
    private bool isBattleMode = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        SetRandomAttackCooldown();
        lastDamageTime = -damageCooldown; // Başlangıçta hasar almayı engelle
    }

    void Update()
    {
        if (isBattleMode)
        {
            FindTarget();
            if (target != null)
            {
                if (Vector3.Distance(transform.position, target.transform.position) > attackRange)
                {
                    MoveTowardsTarget();
                }
                else
                {
                    TryAttack();
                }
            }
        }
    }

    void FindTarget()
    {
        float closestDistance = Mathf.Infinity;
        MonoBehaviour closestEnemy = null;

        // Tüm Unit ve UnitRanger nesnelerini bul
        Unit[] allUnits = FindObjectsOfType<Unit>();
        UnitRanger[] allRangers = FindObjectsOfType<UnitRanger>();

        // Unit nesneleri arasında hedef bul
        foreach (Unit unit in allUnits)
        {
            if (unit != this && !IsSameTeam(unit))
            {
                float distance = Vector3.Distance(transform.position, unit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = unit;
                }
            }
        }

        // UnitRanger nesneleri arasında hedef bul
        foreach (UnitRanger ranger in allRangers)
        {
            if (ranger != this && !IsSameTeam(ranger))
            {
                float distance = Vector3.Distance(transform.position, ranger.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = ranger;
                }
            }
        }

        target = closestEnemy;
    }

    void MoveTowardsTarget()
    {
        if (target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            RollTowardsTarget();
            lastAttackTime = Time.time;
            SetRandomAttackCooldown();
        }
    }

    void RollTowardsTarget()
    {
        if (target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            rb.AddForce(direction * rollForce, ForceMode.Impulse);
        }
    }

    void SetRandomAttackCooldown()
    {
        attackCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);
    }

    void OnCollisionEnter(Collision collision)
    {
        Unit otherUnit = collision.gameObject.GetComponent<Unit>();
        UnitRanger otherRanger = collision.gameObject.GetComponent<UnitRanger>();

        if ((otherUnit != null && otherUnit != this && !IsSameTeam(otherUnit)) ||
            (otherRanger != null && otherRanger != this && !IsSameTeam(otherRanger)))
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                if (otherUnit != null)
                {
                    otherUnit.TakeDamage(attackDamage);
                }
                else if (otherRanger != null)
                {
                    otherRanger.TakeDamage(attackDamage);
                }
                lastDamageTime = Time.time;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log($"{gameObject.name} has taken {damage} damage");
        health -= damage;
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    bool IsSameTeam(MonoBehaviour otherUnit)
    {
        // Tag kontrolü, diğer nesnenin tag'i ile karşılaştır
        return this.CompareTag(otherUnit.tag);
    }

    public void SetBattleMode(bool battleMode)
    {
        isBattleMode = battleMode;
    }
}
