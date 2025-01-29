using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrailManager : MonoBehaviour
{
    [SerializeField] private List<FireTrail> fireTrails;
    [SerializeField] private FireTrail _fireTrailReference;

    private void Start()
    {
        fireTrails = new List<FireTrail>();
    }

    public void SpawnAvailableFireTrailOnThePositionOf(Vector3 position)
    {
        FireTrail availableFireTrail = GetAvailableFireTrailFromList();
        if (availableFireTrail == null)
        {
            availableFireTrail = Instantiate(_fireTrailReference, position, Quaternion.identity, null);
            availableFireTrail.gameObject.SetActive(true);
            availableFireTrail.DisableWithADelay();
            fireTrails.Add(availableFireTrail);
        }
        else
        {
            availableFireTrail.transform.position = position;
            availableFireTrail.gameObject.SetActive(true);
            availableFireTrail.DisableWithADelay();
        }
    }

    private FireTrail GetAvailableFireTrailFromList()
    {
        foreach (FireTrail fireTrail in fireTrails)
        {
            if (fireTrail.gameObject.activeInHierarchy == false)
            {
                return fireTrail;
            }
        }
        return null;
    }
}