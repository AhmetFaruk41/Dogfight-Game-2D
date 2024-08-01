using UnityEngine;

public class RocketController : MonoBehaviour
{
    public float roketHizi = 10f; // Roketin hızı
    public float roketYasami = 5f; // Roketin yaşam süresi

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * roketHizi; // Roketi düz bir şekilde ileri hareket ettir
        Destroy(gameObject, roketYasami); // Roketi belirli bir süre sonra yok et
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject); // Roketi yok et
            Destroy(other.gameObject); // Düşmanı yok et
        }
    }
}
