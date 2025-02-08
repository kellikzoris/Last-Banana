using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorillaTrapPlanter : MonoBehaviour
{
    [SerializeField] private List<Transform> _placesWhereTheTrapCanBePlaced;
    [SerializeField] private List<Transform> _plantedTrapPositions;
    [SerializeField] private Player _player;
    [SerializeField] private Transform _gorillaTrap;
    [SerializeField] float _timeBetweenTraps = 1;
    private bool _trapCycleFinished = false;

    public void StartPlantingATrap(int trapAmount, float timeBetweenTraps)
    {
        DamageZone[] damageZones = FindObjectsOfType<DamageZone>();
        foreach (DamageZone damageZone in damageZones)
        {
            if(damageZone.transform.tag == "GorillaTrap")
            {
                damageZone.DisableThisGameObject(0);
            }
        }
        _timeBetweenTraps = timeBetweenTraps;
        StartCoroutine(TeleportAndPlantTheTrap(trapAmount));
    }

    private Transform ReturnTheDistantPositionOfTheTrapPositions()
    {
        Transform selectedTransform = null;
        float distanceToPos = 0;
        foreach (Transform transform in _placesWhereTheTrapCanBePlaced)
        {
            if (Vector2.Distance(_player.transform.position, transform.position) > distanceToPos &&
                !_plantedTrapPositions.Contains(transform))
            {
                distanceToPos = Vector2.Distance(_player.transform.position, transform.position);
                selectedTransform = transform;
            }
        }
        return selectedTransform;
    }

    private void TeleportEnemyToTrapPosition()
    {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        Transform selectedTransform = ReturnTheDistantPositionOfTheTrapPositions();
        _plantedTrapPositions.Add(selectedTransform);
        this.transform.position = selectedTransform.position;
        GetComponentInChildren<SpriteRenderer>().enabled = true;
    }

    public void StartPlantingTheTrap()
    {
        GetComponentInChildren<Animator>().SetBool("isEating", true);
    }

    public void PlantTheTrap()
    {
        //Don't call this method within this game object.
        if (GetComponentInChildren<Animator>().transform.localScale.x < 0)
        {
            Transform _gorillaTrapPlanted = Instantiate(_gorillaTrap, this.transform.position + new Vector3(-9, 0, 0), Quaternion.identity, null);
            _gorillaTrapPlanted.gameObject.SetActive(true);
            FindObjectOfType<SoundManager>().PlayTrapPlantedSound();
            _gorillaTrapPlanted.GetComponent<DamageZone>().CallDisableGameObjectWithDelay(false);
        }
        else
        {
            Transform _gorillaTrapPlanted = Instantiate(_gorillaTrap, this.transform.position + new Vector3(9, 0, 0), Quaternion.identity, null);
            _gorillaTrapPlanted.gameObject.SetActive(true);
            FindObjectOfType<SoundManager>().PlayTrapPlantedSound();
            _gorillaTrapPlanted.GetComponent<DamageZone>().CallDisableGameObjectWithDelay(false);
        }
        GetComponentInChildren<Animator>().SetBool("isEating", false);
    }

    public bool IsTheCycleFinised()
    {
        return _trapCycleFinished;
    }

    public void ResetTrapCycleFinished()
    {
        _trapCycleFinished = false;
    }

    private IEnumerator TeleportAndPlantTheTrap(int trapAmmount)
    {
        for (int i = 0; i < trapAmmount; i++)
        {
            TeleportEnemyToTrapPosition();
            yield return new WaitForSeconds(_timeBetweenTraps);
            StartPlantingTheTrap();
            yield return new WaitForSeconds(1);
        }
        _trapCycleFinished = true;
        _plantedTrapPositions.Clear();
    }
}