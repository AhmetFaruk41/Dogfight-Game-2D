using UnityEngine;

public class Missile : MonoBehaviour
{
    public float hizAzaltmaOrani = 0.98f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.velocity *= hizAzaltmaOrani;
    }
}
