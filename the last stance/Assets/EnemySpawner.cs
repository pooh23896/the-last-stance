using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Instellingen")]
    public GameObject Meteoor;       // Sleep hier je Meteoor prefab in
    public Transform MarsObject;           // Doelobject (bijv. Mars in midden)
    public float enemySpeed = 3f;    // Snelheid van meteoren
    public int numberOfEnemies = 5;  // Hoeveel meteoren spawnen bij start
    public float spawnOffset = 1f;   // Hoeveelheid buiten het scherm (in world units)

    void Start()
    {
        if (MarsObject == null)
        {
            Debug.LogError("Mars is NIET gekoppeld! Sleep het Mars-object naar het script in de Inspector.");
            return;
        }

        for (int i = 0; i < numberOfEnemies; i++)
        {
            SpawnMeteoorJustOutsideCamera();
        }
    }

    void SpawnMeteoorJustOutsideCamera()
    {
        Camera cam = Camera.main;

        // Bereken world-space grenzen van de camera
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        float minX = bottomLeft.x;
        float maxX = topRight.x;
        float minY = bottomLeft.y;
        float maxY = topRight.y;

        Vector3 spawnPos = Vector3.zero;
        int edge = Random.Range(0, 4); // 0 = links, 1 = rechts, 2 = onder, 3 = boven

        switch (edge)
        {
            case 0: // Links
                spawnPos.x = minX - spawnOffset;
                spawnPos.y = Random.Range(minY, maxY);
                break;
            case 1: // Rechts
                spawnPos.x = maxX + spawnOffset;
                spawnPos.y = Random.Range(minY, maxY);
                break;
            case 2: // Onder
                spawnPos.x = Random.Range(minX, maxX);
                spawnPos.y = minY - spawnOffset;
                break;
            case 3: // Boven
                spawnPos.x = Random.Range(minX, maxX);
                spawnPos.y = maxY + spawnOffset;
                break;
        }

        // Instantieer de meteoor
        GameObject meteoor = Instantiate(Meteoor, spawnPos, Quaternion.identity);

        // Richt op Mars
        Vector3 directionToMars = (MarsObject.position - spawnPos).normalized;
        float angle = Mathf.Atan2(directionToMars.y, directionToMars.x) * Mathf.Rad2Deg;
        meteoor.transform.rotation = Quaternion.Euler(0, 0, angle - 90); // pas aan als je sprite anders gericht is

        // Initialiseer beweging
        EnemyMovement movement = meteoor.GetComponent<EnemyMovement>();
        if (movement != null)
        {
            movement.Initialize(directionToMars, enemySpeed);
        }
        else
        {
            Debug.LogError("EnemyMovement script ontbreekt op de Meteoor prefab!");
        }
    }
}
