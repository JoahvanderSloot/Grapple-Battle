using System.Collections.Generic;
using UnityEngine;

public class MiniOpdrachtSLB : MonoBehaviour
{
    [SerializeField] List<GameObject> gameObjects;

    [SerializeField] GameObject cubePrefab;

    private void Start()
    {
        gameObjects = new List<GameObject>();
    }

    public void Position(Vector3 position, GameObject cube)
    {
        cube.transform.position = position;
        gameObjects.Add(cube);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 newPosition = new Vector3(Random.Range(-10, 10), Random.Range(-6, 6), Random.Range(-4, 4));

            GameObject newCube = Instantiate(cubePrefab);
            Position(newPosition, newCube);
        }
    }
}
