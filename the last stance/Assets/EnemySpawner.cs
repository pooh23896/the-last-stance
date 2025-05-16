using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Instellingen")]
    public GameObject Meteoor;           // Sleep hier de meteoor prefab in
    public Transform Mars;               // Sleep hier het Mars-object in
    public float enemySpeed = 3f;        // Snelheid van meteoren
    public int numberOfEnemies = 5;      // Aantal meteoren dat gespawned wordt

    void Start()
    {
        if (Mars == null)
        {
            Debug.LogError("Mars is niet toegewezen in de Inspector!");
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

        if (cam == null)
        {
            Debug.LogError("Er is geen camera met de tag 'MainCamera' in de scene!");
            return;
        }

        float zDistance = Mathf.Abs(cam.transform.position.z);

        // Bereken de wereldgrenzen van de camera
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, zDistance));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, zDistance));

        // Spawnpositie buiten de camera
        Vector3 spawnPos = Vector3.zero;

        // Kies een rand willekeurig: links, rechts, boven of onder
        int randEdge = Random.Range(0, 4);
        switch (randEdge)
        {
            case 0: // Links
                spawnPos.x = bottomLeft.x - 2f;
                spawnPos.y = Random.Range(bottomLeft.y, topRight.y);
                break;
            case 1: // Rechts
                spawnPos.x = topRight.x + 2f;
                spawnPos.y = Random.Range(bottomLeft.y, topRight.y);
                break;
            case 2: // Boven
                spawnPos.x = Random.Range(bottomLeft.x, topRight.x);
                spawnPos.y = topRight.y + 2f;
                break;
            case 3: // Onder
                spawnPos.x = Random.Range(bottomLeft.x, topRight.x);
                spawnPos.y = bottomLeft.y - 2f;
                break;
        }

        spawnPos.z = 0f; // Voor 2D zichtbaarheid

        GameObject meteoor = Instantiate(Meteoor, spawnPos, Quaternion.identity);

        // Richt naar Mars
        Vector3 directionToMars = (Mars.position - spawnPos).normalized;
        float angle = Mathf.Atan2(directionToMars.y, directionToMars.x) * Mathf.Rad2Deg;
        meteoor.transform.rotation = Quaternion.Euler(0, 0, angle - 90); // Als de sprite omhoog kijkt

        // Zorg dat de meteoor een EnemyMovement component heeft
        EnemyMovement movement = meteoor.GetComponent<EnemyMovement>();
        if (movement != null)
        {
            movement.Initialize(directionToMars, enemySpeed);
        }
        else
        {
            Debug.LogError("EnemyMovement component ontbreekt op de Meteoor prefab!");
        }
    }
}
