using UnityEngine;

public class Missile : MonoBehaviour
{
    public float hizAzaltmaOrani = 0.98f;
    public float yasamSuresi = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, yasamSuresi); // Füzenin yaşam süresi dolduğunda yok et
    }

    void FixedUpdate()
    {
        rb.velocity *= hizAzaltmaOrani;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject); // Füze yok et
            Destroy(other.gameObject); // Düşmanı yok et
        }
    }
}
