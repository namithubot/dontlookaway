using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
public class RandomSpawns : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The behavior to use to spawn objects.")]
    ObjectSpawner m_ObjectSpawner;

    [SerializeField]
    float minSpawnDistance = 10.0f;

    public ARPlaneManager arPlaneManager;
    public AudioClip spawnSound;
    public GameObject startCanvas;

    void Start()
    {
        startCanvas.SetActive(true);
    }

    public void StartSpawning()
    {
        startCanvas.SetActive(false);
        StartCoroutine(RandomSpawnCoroutine());
    }

    IEnumerator RandomSpawnCoroutine()
    {   
        while(true)
        {   
            yield return new WaitForSeconds(Random.Range(2, 5));
            PlaceObjectAtRandomPosition();
        }
    }

    public void PlaceObjectAtRandomPosition()
    {
        List<ARPlane> arPlanes = new List<ARPlane>();
        foreach (var plan in arPlaneManager.trackables)
        {
            arPlanes.Add(plan);
        }

        if (arPlanes.Count == 0)
        {
            Debug.Log("No AR planes detected.");
            return;
        }

        ARPlane randomPlane = arPlanes[Random.Range(0, arPlanes.Count)];
        if(randomPlane.alignment !=PlaneAlignment.HorizontalUp)
        {
            Debug.Log("Plane is not horizontal.");
            return;
        }
        NativeArray<Vector2> boundaryPoints = randomPlane.boundary;
        Vector3 randomPoint = CalculateRandomPointWithinPlane(randomPlane, boundaryPoints);

        if (Vector3.Distance(Camera.main.transform.position, randomPoint) < minSpawnDistance)
        {
            Debug.Log("Spawn point is too close to the camera.");
            return;
        }

        m_ObjectSpawner.TrySpawnObject(randomPoint, randomPlane.normal);
        AudioSource.PlayClipAtPoint(this.spawnSound, randomPoint);
    }

        
    private Vector3 CalculateRandomPointWithinPlane(ARPlane plane, NativeArray<Vector2> boundaryPoints)
    {
        List<Vector2> boundaryList = new List<Vector2>(boundaryPoints);

        Vector2 pointA = boundaryList[Random.Range(0, boundaryList.Count)];
        Vector2 pointB = boundaryList[Random.Range(0, boundaryList.Count)];
        Vector2 pointC = boundaryList[Random.Range(0, boundaryList.Count)];

        Vector3 worldPointA = plane.transform.TransformPoint(new Vector3(pointA.x, 0, pointA.y));
        Vector3 worldPointB = plane.transform.TransformPoint(new Vector3(pointB.x, 0, pointB.y));
        Vector3 worldPointC = plane.transform.TransformPoint(new Vector3(pointC.x, 0, pointC.y));
        float u = Random.value;
        float v = Random.value;

        if (u + v > 1)
        {
            u = 1 - u;
            v = 1 - v;
        }

        Vector3 randomPoint = worldPointA + u * (worldPointB - worldPointA) + v * (worldPointC - worldPointA);
        return randomPoint;
    }
}
