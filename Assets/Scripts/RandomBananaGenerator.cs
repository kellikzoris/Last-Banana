using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBananaGenerator : MonoBehaviour
{
    [SerializeField] private List<Transform> dropBananaTransformPositions;
    [SerializeField] private Dictionary<Transform, Banana> dropBananaTransformDictionary;
    [SerializeField] private float _timeBetweenBananaSpawns;
    [SerializeField] private Banana banana;
    [SerializeField] private bool _stopInstantiating;
    [SerializeField] Transform _possibleTransform;

    private void Start()
    {
        dropBananaTransformDictionary = new Dictionary<Transform, Banana>();
        StartCoroutine(InstantiateBanana());
    }

    private IEnumerator InstantiateBanana()
    {
        Debug.Log("InstantiateBanana");
        yield return new WaitForSeconds(_timeBetweenBananaSpawns);
        _possibleTransform = GetRandomAvailableTransformPositionWhereThereisNoBanana();
        if (_possibleTransform == null)
        {
            StartCoroutine(InstantiateBanana());
            yield break;
        }

        Vector2 spawnPosition = _possibleTransform.position;
        Banana newBanana = Instantiate(banana, spawnPosition, Quaternion.identity, null);
        newBanana.SetAsGroundBanana();
        newBanana.gameObject.SetActive(true);

        dropBananaTransformDictionary.Add(newBanana.transform, newBanana);
        if (_stopInstantiating)
        {
            yield break;
        }

        StartCoroutine(InstantiateBanana());
    }

    public void DropBananaFromDictionary(Transform transform)
    {
        if (dropBananaTransformDictionary.ContainsKey(transform))
        {
            dropBananaTransformDictionary.Remove(transform);
        }
        Debug.Log($"DropBananaFromDictionaryLength {dropBananaTransformDictionary.Count}");
    }

    public Transform GetRandomAvailableTransformPositionWhereThereisNoBanana()
    {
        List<Transform> availablePositions = new List<Transform>();
        foreach (var dropBananaTransformPosition in dropBananaTransformPositions)
        {
            if (!dropBananaTransformDictionary.ContainsKey(dropBananaTransformPosition))
            {
                availablePositions.Add(dropBananaTransformPosition);
            }
        }
        if (availablePositions.Count == 0)
        {
            return null;
        }
        return availablePositions[Random.Range(0, availablePositions.Count)];
    }
}