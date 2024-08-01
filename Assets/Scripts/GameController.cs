using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
    public GameObject enemyPrefab; // Düşman prefab'ı
    public int initialEnemyCount = 3; // İlk dalgada kaç düşman spawn olacak
    public float timeBetweenWaves = 5f; // Dalgalar arasındaki süre
    public float spawnRate = 1f; // Düşmanların spawn olma hızı
    public Text waveText; // Dalga sayısını göstermek için UI Text
    public Text gameOverText; // Oyun bittiğinde gösterilecek UI Text
    public float spawnAreaWidth = 10f; // Spawn alanının genişliği
    public float spawnAreaHeight = 5f; // Spawn alanının yüksekliği

    private int waveNumber = 1; // Dalga sayısı
    private int maxWaves = 10; // Toplam dalga sayısı
    private int currentEnemyCount = 0; // Mevcut düşman sayısı

    void Start()
    {
        if (gameOverText != null)
        {
            gameOverText.enabled = false; // Oyun başında gameOverText'i gizle
        }
        StartCoroutine(SpawnWaves());
        UpdateWaveText();
    }

    IEnumerator SpawnWaves()
    {
        while (waveNumber <= maxWaves)
        {
            yield return new WaitForSeconds(timeBetweenWaves);

            int enemyCount = initialEnemyCount + (waveNumber - 1);
            currentEnemyCount = enemyCount; // Mevcut düşman sayısını güncelle

            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(1f / spawnRate);
            }

            // Tüm düşmanlar yok edilene kadar bekle
            while (currentEnemyCount > 0)
            {
                yield return null;
            }

            waveNumber++;
            UpdateWaveText();
        }

        // 10 dalga tamamlandığında oyunu bitir
        if (gameOverText != null)
        {
            gameOverText.enabled = true;
            gameOverText.text = "Game Over! All waves completed!";
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPosition = GetRandomSpawnPosition();
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.GetComponent<EnemyController>().OnDestroyed += OnEnemyDestroyed;
    }

    Vector2 GetRandomSpawnPosition()
    {
        float spawnX = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
        float spawnY = Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2);
        return new Vector2(spawnX, spawnY);
    }

    void OnEnemyDestroyed()
    {
        currentEnemyCount--; // Mevcut düşman sayısını azalt
    }

    void UpdateWaveText()
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + waveNumber + "/" + maxWaves;
        }
    }
}
