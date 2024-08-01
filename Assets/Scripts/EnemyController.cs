using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    public float hareketHizi = 2f; // Düşmanın hareket hızı
    public float donusHizi = 200f; // Düşmanın dönüş hızı
    public float durmaMesafesi = 1f; // Düşmanın durma mesafesi
    public Transform hedef; // Uçağın transform'u
    private Rigidbody2D rb;
    public event Action OnDestroyed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (hedef == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                hedef = player.transform; // Uçağı hedef olarak al
            }
            else
            {
                Debug.LogError("Hedef atanmadı! Lütfen Inspector penceresinden hedefi atayın.");
            }
        }
    }

    void FixedUpdate()
    {
        if (hedef != null)
        {
            // Hedefe doğru yönel
            Vector2 yon = (hedef.position - transform.position).normalized;
            float angle = Mathf.Atan2(yon.y, yon.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = Mathf.LerpAngle(rb.rotation, angle, donusHizi * Time.fixedDeltaTime);

            // Hedefe doğru hareket et
            float mesafe = Vector2.Distance(transform.position, hedef.position);
            if (mesafe > durmaMesafesi)
            {
                rb.velocity = transform.up * hareketHizi;
            }
            else
            {
                rb.velocity = Vector2.zero; // Hedefe ulaşıldığında dur
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        
            Destroy(gameObject); // Düşmanı yok et
            Destroy(other.gameObject); // Roketi yok et
     
    }

    

    void OnDestroy()
    {
        if (OnDestroyed != null)
        {
            OnDestroyed();
        }
    }
}
