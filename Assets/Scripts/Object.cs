using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    [Header("Elements")]
    public List<GameObject> spawnObjects;
    private Rigidbody rb;
    public GameObject particleObject;
    public SoundManager sManager;
    private static HashSet<GameObject> recentlySpawnedObjects = new HashSet<GameObject>();

    [Header("Settings")]
    private bool hasSpawned = false;
    [SerializeField] private int scoreAmount;
    private float spawnCooldown = 0.1f;
    
    private bool isInFinishZone = false;
    private float countdownTime = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sManager = GameObject.Find("AudioManager").GetComponent<SoundManager>();
    }

    private void Start()
    {
        hasSpawned = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void Update()
    {
        GameManager.instance.WarningPanel(isInFinishZone);
        
        if (isInFinishZone)
        {
            countdownTime -= Time.deltaTime;
            if (countdownTime <= 0f)
            {
                GameManager.instance.isFinishedGame = true;
                Debug.Log("Game Finished!");
                isInFinishZone = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            Debug.Log("Finish Line");
            isInFinishZone = true;
            countdownTime = 3f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            Debug.Log("Exited Finish Line");
            isInFinishZone = false;
            countdownTime = 3f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasSpawned && !recentlySpawnedObjects.Contains(gameObject) && !recentlySpawnedObjects.Contains(collision.gameObject))
        {
            for (int i = 1; i <= 10; i++)
            {
                string tag = i.ToString();
                if (collision.gameObject.CompareTag(tag) && gameObject.CompareTag(tag))
                {
                    Vector3 spawnPosition = (transform.position + collision.transform.position) / 2;

                    if (i < spawnObjects.Count)
                    {
                        sManager.PlayImpactSound();
                        GameManager.instance.AddScore(scoreAmount);
                        GameObject spawnedObject = Instantiate(spawnObjects[i], spawnPosition, Quaternion.identity);
                        Instantiate(particleObject, spawnPosition, Quaternion.identity);
                        Rigidbody spawnedRb = spawnedObject.GetComponent<Rigidbody>();
                        if (spawnedRb != null)
                        {
                            spawnedRb.velocity = Vector3.zero;
                            spawnedRb.angularVelocity = Vector3.zero;
                        }
                    }

                    recentlySpawnedObjects.Add(gameObject);
                    recentlySpawnedObjects.Add(collision.gameObject);
                    Invoke(nameof(RemoveFromRecentlySpawned), spawnCooldown);

                    collision.gameObject.GetComponent<Object>().hasSpawned = true;
                    Destroy(collision.gameObject);
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }

    private void RemoveFromRecentlySpawned()
    {
        recentlySpawnedObjects.Remove(gameObject);
    }
}
