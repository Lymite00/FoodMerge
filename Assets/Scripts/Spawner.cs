using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int spawnIndex;
    
    private GameObject spawnedObject;
    private bool isFollowing = false;
    private bool isCoroutineRunning = false;

    [SerializeField] private Vector3 mousePosition;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float minZ, maxZ;

    [SerializeField] private GameObject lineObject;

    void Start()
    {
        SpawnObject();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            spawnPoint.transform.position -= new Vector3(0, 0, moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            spawnPoint.transform.position += new Vector3(0, 0, moveSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropObject();
        }
        Vector3 clampedPosition = spawnPoint.transform.position;
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);
        spawnPoint.transform.position = clampedPosition;

        if (spawnedObject != null && isFollowing)
        {
            spawnedObject.transform.position = spawnPoint.position;
        }
    }

    public void DropObject()
    {
        isFollowing = false;
        lineObject.SetActive(false);

        if (spawnedObject != null)
        {
            Rigidbody previousRb = spawnedObject.GetComponent<Rigidbody>();
            if (previousRb != null)
            {
                previousRb.velocity = Vector3.zero;
                previousRb.angularVelocity = Vector3.zero;
                previousRb.useGravity = true;
                previousRb.isKinematic = false;
            }
            previousRb = null;
            spawnedObject = null;
        }

        if (!isCoroutineRunning)
        {
            StartCoroutine(DelayedSpawn());
        }
    }

    private IEnumerator DelayedSpawn()
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(0.5f);

        SpawnObject();
        isCoroutineRunning = false;
    }

    public void SpawnObject()
    {
        lineObject.SetActive(true);
        var clamp = Mathf.Clamp(spawnIndex, 0, 3);
        clamp = Random.Range(0, 4);
        spawnedObject = Instantiate(objects[clamp], spawnPoint.position, Quaternion.identity);

        Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.sleepThreshold = 0.1f;
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        StartCoroutine(EnableGravityAfterDelay(rb));

        isFollowing = true;
    }

    private IEnumerator EnableGravityAfterDelay(Rigidbody rb)
    {
        yield return new WaitForSeconds(0.1f);
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }
}
