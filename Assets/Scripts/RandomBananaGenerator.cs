using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBananaGenerator : MonoBehaviour
{
    [SerializeField] private List<Transform> dropBananaTransformPositions;
    [SerializeField] private float _timeBetweenBananaSpawns;
    [SerializeField] private Banana banana;
    [SerializeField] private bool _stopInstantiating;

    private void Start()
    {
        StartCoroutine(InstantiateBanana());
    }

    private IEnumerator InstantiateBanana()
    {
        Debug.Log("InstantiateBanana");
        yield return new WaitForSeconds(_timeBetweenBananaSpawns);
        Vector2 spawnPosition = dropBananaTransformPositions[Random.Range(0, dropBananaTransformPositions.Count)].position;
        Banana newBanana = Instantiate(banana, spawnPosition, Quaternion.identity, null);
        newBanana.gameObject.SetActive(true);
        if (_stopInstantiating)
        {
            yield break;
        }
        StartCoroutine(InstantiateBanana());
    }
}