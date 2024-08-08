using UnityEngine;

public class UnitRanger : MonoBehaviour
{
    public float health = 100f;
    public float moveSpeed = 3f;
    public float attackRange = 10f;
    public float attackDamage = 20f;
    public float minAttackCooldown = 1f;
    public float maxAttackCooldown = 5f;
    public GameObject bulletPrefab; // Mermi prefab'ı
    public Transform firePoint; // Merminin ateşleneceği nokta

    private float lastAttackTime;
    private float attackCooldown;
    private MonoBehaviour target; // Genel bir tür olarak değiştirildi
    private bool isBattleMode = false;

    void Start()
    {
        SetRandomAttackCooldown();
    }

    void Update()
    {
        if (isBattleMode)
        {
            FindTarget();
            if (target != null)
            {
                // Hedefin menzil içinde olup olmadığını kontrol et
                if (IsTargetInRange())
                {
                    TryAttack();
                }
                else
                {
                    MoveTowardsTarget();
                }

                // Hedefe nişan al
                AimAtTarget();
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
            Shoot();
            lastAttackTime = Time.time;
            SetRandomAttackCooldown();
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.damage = attackDamage;
            }
        }
    }

    void SetRandomAttackCooldown()
    {
        attackCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);
    }

    public void SetBattleMode(bool battleMode)
    {
        isBattleMode = battleMode;
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

    bool IsTargetInRange()
    {
        return target != null && Vector3.Distance(transform.position, target.transform.position) <= attackRange;
    }

    void AimAtTarget()
    {
        if (target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}
