using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform player; // Karakterin referansı
    public float smoothTime = 0.3f; // Kameranın pürüzsüz hareket süresi
    public Vector2 followOffset = new Vector2(2.0f, 2.0f); // Kameranın karaktere olan offseti

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        // Kameranın ve karakterin pozisyonlarını alın
        Vector3 playerPosition = player.position;

        // Kameranın yeni hedef pozisyonunu hesaplayın
        Vector3 targetPosition = new Vector3(playerPosition.x + followOffset.x, playerPosition.y + followOffset.y, transform.position.z);

        // Kameranın yeni pozisyonunu pürüzsüz bir şekilde ayarlayın
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
