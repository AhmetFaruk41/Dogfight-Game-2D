using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public float thrustForce = 5f;
    public float rotationSpeed = 200f;
    public float maxSpeed = 10f;
    public float thrustAcceleration = 2f;
    public float gravityScale = 0.5f; // Yer çekimi etkisini kontrol etmek için
    public GameObject mermiPrefab; // Mermi prefab'ı
    public Transform mermiCikisNoktasi; // Merminin çıkış noktası
    public float mermiHizi = 10f; // Mermi hızı
    public float mermiYasami = 5f; // Merminin yaşam süresi
    public float atesEtmeAraligi = 0.1f; // Başlangıç ateş etme aralığı
    public float hizliAtesEtmeAraligi = 0.2f; // Hızlı ateş etme aralığı
    public float hizAzalmaSuresi = 2f; // Ateş etme hızının artacağı süre
    public float eskiAtesHiziGeriDonusSuresi = 2f; // Ateş hızının eski haline dönme süresi

    public GameObject roketPrefab; // Roket prefab'ı
    public Transform roketCikisNoktasi; // Roketin çıkış noktası
    public float roketHizi = 15f; // Roket hızı
    public float roketYasami = 5f; // Roketin yaşam süresi
    public int maxRoketSayisi = 4; // Art arda gönderilebilecek maksimum roket sayısı
    public float roketBeklemeSuresi = 4f; // 4 roketten sonra bekleme süresi

    private Rigidbody2D rb;
    private bool isThrusting = false;
    private float currentThrust = 0f;
    private float atesEtmeZamanlayici = 0f;
    private bool atesEdiyor = false;
    private bool hizAzaldi = false;
    private float zamanlayici = 0f;
    private float atesEtmeBeklemeZamanlayici = 0f;
    private bool atesEtmeAraligiDegisti = false;
    private float varsayilanAtesEtmeAraligi;

    private int atilanRoketSayisi = 0;
    private float roketBeklemeZamanlayici = 0f;
    private bool roketAtisBeklemeModu = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale; // Başlangıçta yer çekimi etkisi
        rb.drag = 2; // Hızı yavaşlatmak için sürtünme ekleyin
        rb.angularDrag = 2; // Savrulmayı azaltmak için açısal sürtünme ekleyin
        varsayilanAtesEtmeAraligi = atesEtmeAraligi; // Varsayılan ateş etme aralığını sakla
    }

    void Update()
    {
        HandleInput();
        HandleAtesEtme();
        HandleAtesHiziGeriDonus();
        HandleRoketAtis();
    }

    void HandleInput()
    {
        float moveInput = Input.GetAxis("Vertical");
        float rotationInput = Input.GetAxis("Horizontal");

        isThrusting = moveInput > 0;

        if (isThrusting)
        {
            // Thrust uygulanırken kademeli olarak hızlan
            currentThrust = Mathf.Lerp(currentThrust, thrustForce, thrustAcceleration * Time.deltaTime);
            rb.AddForce(transform.up * currentThrust);
            rb.gravityScale = 0; // Thrust uygulanırken yer çekimini devre dışı bırak
        }
        else
        {
            // Thrust uygulanmadığında kademeli olarak yavaşla
            currentThrust = Mathf.Lerp(currentThrust, 0, thrustAcceleration * Time.deltaTime);
            rb.gravityScale = gravityScale; // Thrust uygulanmadığında yer çekimini etkinleştir
        }

        // Dönüşleri daha akıcı hale getirmek için kademeli dönüş
        if (rotationInput != 0)
        {
            float rotationAmount = -rotationInput * rotationSpeed * Time.deltaTime;
            rb.AddTorque(rotationAmount);
        }

        // Maksimum hızı sınırlamak için hız sınırını kontrol et
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        // Ateş etme kontrolü
        if (Input.GetKeyDown(KeyCode.Space))
        {
            atesEdiyor = true;
            hizAzaldi = false;
            zamanlayici = 0f;
            atesEtmeZamanlayici = atesEtmeAraligi;
            atesEtmeAraligiDegisti = false;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            atesEdiyor = false;
            atesEtmeBeklemeZamanlayici = 0f; // Ateş etmeyi bıraktığımızda zamanlayıcıyı sıfırla
        }

        // Roket ateşleme kontrolü
        if (Input.GetKeyDown(KeyCode.R) && !roketAtisBeklemeModu)
        {
            AtesEtRoket();
            atilanRoketSayisi++;
            if (atilanRoketSayisi >= maxRoketSayisi)
            {
                roketAtisBeklemeModu = true;
                roketBeklemeZamanlayici = 0f;
            }
        }
    }

    void HandleAtesEtme()
    {
        if (atesEdiyor)
        {
            zamanlayici += Time.deltaTime;

            if (zamanlayici > hizAzalmaSuresi && !hizAzaldi)
            {
                atesEtmeAraligi = hizliAtesEtmeAraligi;
                hizAzaldi = true;
            }

            atesEtmeZamanlayici -= Time.deltaTime;
            if (atesEtmeZamanlayici <= 0f)
            {
                AtesEt();
                atesEtmeZamanlayici = atesEtmeAraligi;
            }
        }
    }

    void HandleAtesHiziGeriDonus()
    {
        if (!atesEdiyor && hizAzaldi)
        {
            atesEtmeBeklemeZamanlayici += Time.deltaTime;
            if (atesEtmeBeklemeZamanlayici >= eskiAtesHiziGeriDonusSuresi)
            {
                atesEtmeAraligi = varsayilanAtesEtmeAraligi; // Eski ateş etme aralığına geri dön
                hizAzaldi = false; // Hızı eski haline döndükten sonra bu bayrağı sıfırla
                atesEtmeAraligiDegisti = true;
            }
        }
        else if (atesEdiyor)
        {
            atesEtmeBeklemeZamanlayici = 0f; // Ateş etmeye devam edersek zamanlayıcıyı sıfırla
        }
    }

    void HandleRoketAtis()
    {
        if (roketAtisBeklemeModu)
        {
            roketBeklemeZamanlayici += Time.deltaTime;
            if (roketBeklemeZamanlayici >= roketBeklemeSuresi)
            {
                roketAtisBeklemeModu = false;
                atilanRoketSayisi = 0;
            }
        }
    }

    void AtesEt()
    {
        GameObject mermi = Instantiate(mermiPrefab, mermiCikisNoktasi.position, mermiCikisNoktasi.rotation);
        Rigidbody2D rbMermi = mermi.GetComponent<Rigidbody2D>();
        rbMermi.velocity = rb.velocity + (Vector2)mermiCikisNoktasi.up * mermiHizi;

        // Merminin uçağa çarpmamasını sağlamak için
        Physics2D.IgnoreCollision(mermi.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        Destroy(mermi, mermiYasami);
    }

    void AtesEtRoket()
    {
        GameObject roket = Instantiate(roketPrefab, roketCikisNoktasi.position, roketCikisNoktasi.rotation);
        Rigidbody2D rbRoket = roket.GetComponent<Rigidbody2D>();
        rbRoket.velocity = rb.velocity + (Vector2)roketCikisNoktasi.up * roketHizi;

        // Roketin uçağa çarpmamasını sağlamak için
        Physics2D.IgnoreCollision(roket.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        Destroy(roket, roketYasami);
    }
}
