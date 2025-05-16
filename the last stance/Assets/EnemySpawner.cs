using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs & Objecten")]
    public GameObject meteoorPrefab;      // Sleep hier de Meteoor prefab in
    public Transform marsTransform;       // Sleep hier het Mars object in

    [Header("Spawn Instellingen")]
    public float spawnDistanceFromMars = 20f;  // Afstand waar meteoren buiten Mars verschijnen
    public float enemySpeed = 3f;               // Snelheid van de meteoren

    [Header("Wave Instellingen")]
    public int startingEnemiesPerWave = 5;      // Hoeveel enemies per wave bij start
    public int enemiesIncreasePerWave = 2;      // Hoeveel enemies erbij elke wave
    public float waveCooldown = 6f;              // Vaste cooldown tussen waves (in seconden)

    private int currentWave = 0;
    private int enemiesToSpawnThisWave;

    private WaveUI waveUI; // Referentie naar UI script om wave te tonen

    void Start()
    {
        waveUI = FindObjectOfType<WaveUI>(); // zoekt automatisch je WaveUI in de scene
        StartCoroutine(WaveRoutine());
    }

    IEnumerator WaveRoutine()
    {
        while (true)
        {
            currentWave++;
            enemiesToSpawnThisWave = startingEnemiesPerWave + enemiesIncreasePerWave * (currentWave - 1);

            if (waveUI != null)
                waveUI.UpdateWave(currentWave);

            Debug.Log($"Wave {currentWave} gestart met {enemiesToSpawnThisWave} meteoren.");

            // Spawn alle meteoren van deze wave
            for (int i = 0; i < enemiesToSpawnThisWave; i++)
            {
                SpawnMeteoor();
                Debug.Log($"Enemie wave{currentWave}");
                yield return new WaitForSeconds(Random.Range(0.5f, 1f));
            }

            Debug.Log($"Wave {currentWave} afgerond. Cooldown {waveCooldown} seconden.");
            yield return new WaitForSeconds(waveCooldown);
        }
    }

    void SpawnMeteoor()
    {
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        Vector3 spawnPos = marsTransform.position + (Vector3)(randomDir * spawnDistanceFromMars);

        GameObject meteoor = Instantiate(meteoorPrefab, spawnPos, Quaternion.identity);

        // Richt meteoor naar Mars
        Vector3 dirToMars = (marsTransform.position - meteoor.transform.position).normalized;
        float angle = Mathf.Atan2(dirToMars.y, dirToMars.x) * Mathf.Rad2Deg;
        meteoor.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        // Initialiseert movement script
        EnemyMovement movement = meteoor.GetComponent<EnemyMovement>();
        if (movement != null)
        {
            movement.Initialize(marsTransform, enemySpeed);
        }
        else
        {
            Debug.LogError("Geen EnemyMovement component gevonden op de meteoor prefab!");
        }
    }
}
