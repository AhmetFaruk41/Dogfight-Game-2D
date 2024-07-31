using UnityEngine;

public class RocketController : MonoBehaviour
{
    public float hizAzaltmaOrani = 0.98f;
    public float roketHizi = 10f;
    public float yonDegistirmeHizi = 2f; // Roketin hedefe doğru yön değiştirme hızı
    public float hedefAramaYaricapi = 10f; // Hedef arama yarıçapı
    public float roketYasami = 5f; // Roketin yaşam süresi

    private Rigidbody2D rb;
    private Transform hedef;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hedef = EnYakinHedefiBul();
        rb.velocity = transform.up * roketHizi;
        Destroy(gameObject, roketYasami); // Roketi belirli bir süre sonra yok et
    }

    void FixedUpdate()
    {
        if (hedef != null)
        {
            Vector2 yon = (hedef.position - transform.position).normalized;
            Vector2 yeniYon = Vector2.Lerp(rb.velocity.normalized, yon, yonDegistirmeHizi * Time.fixedDeltaTime);
            rb.velocity = yeniYon * roketHizi;
        }
        else
        {
            rb.velocity = transform.up * roketHizi;
        }

        rb.velocity *= hizAzaltmaOrani;
    }

    Transform EnYakinHedefiBul()
    {
        GameObject[] dusmanlar = GameObject.FindGameObjectsWithTag("Enemy");
        Transform enYakinHedef = null;
        float enYakinMesafe = Mathf.Infinity;

        foreach (GameObject dusman in dusmanlar)
        {
            float mesafe = Vector2.Distance(transform.position, dusman.transform.position);
            if (mesafe < enYakinMesafe && mesafe <= hedefAramaYaricapi)
            {
                enYakinHedef = dusman.transform;
                enYakinMesafe = mesafe;
            }
        }

        return enYakinHedef;
    }
}
