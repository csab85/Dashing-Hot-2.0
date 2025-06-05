using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLab : MonoBehaviour
{
    [SerializeField] Vector2 spawnArea;
    [SerializeField] float spawnTime;
    [SerializeField] GameObject target;

    IEnumerator SpawnTarget()
    {
        float xPosit = Random.Range(0, spawnArea.x);
        float zPosit = Random .Range(0, spawnArea.y);
        
        Instantiate(target, new Vector3(xPosit, 0, zPosit), Quaternion.identity, null);

        yield return new WaitForSeconds(spawnTime);

        StartCoroutine(SpawnTarget());
    }

    private void Start()
    {
        StartCoroutine(SpawnTarget());
    }
}
