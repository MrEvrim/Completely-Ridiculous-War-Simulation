using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 20f;

    private void Start()
    {
        // Mermi hareket eder
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, 3f); // Mermi 3 saniye sonra yok edilir
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Çarpışan nesnenin Unit veya UnitRanger olup olmadığını kontrol et
        Unit target = collision.gameObject.GetComponent<Unit>();
        UnitRanger targetRanger = collision.gameObject.GetComponent<UnitRanger>();

        if (target != null)
        {
            target.TakeDamage(damage);
            Destroy(gameObject); // Mermi hedefe çarptıktan sonra yok edilir
        }
        else if (targetRanger != null)
        {
            targetRanger.TakeDamage(damage);
            Destroy(gameObject); // Mermi hedefe çarptıktan sonra yok edilir
        }
    }
}
