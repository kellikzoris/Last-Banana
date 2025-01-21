using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [SerializeField] private List<Banana> _yellowBananasInBag;
    [SerializeField] private List<Banana> _greenBananasInBag;
    [SerializeField] private List<Banana> _redBananasInBag;

    [SerializeField] private TMP_Text _yellowBananasLeft;
    [SerializeField] private TMP_Text _greenBananasLeft;
    [SerializeField] private TMP_Text _redBananasLeft;

    [SerializeField] private TMP_Text _yellowBananaAmountLeft;
    [SerializeField] private TMP_Text _greenBananaAmountLeft;
    [SerializeField] private TMP_Text _redBananaAmountLeft;

    [Header("UI Buttons To Change Weapons")]
    [SerializeField] private Button _yellowBananaButton;

    [SerializeField] private Button _greenBananaButton;
    [SerializeField] private Button _redBananaButton;

    public enum BananaType
    {
        yellow,
        green,
        red
    }

    public BananaType _selectedBananaType;

    private void Start()
    {
        _selectedBananaType = BananaType.yellow;
        foreach (var banana in _yellowBananasInBag)
        {
            banana.gameObject.SetActive(false);
        }
        foreach (var banana in _greenBananasInBag)
        {
            banana.gameObject.SetActive(false);
        }
        foreach (var banana in _redBananasInBag)
        {
            banana.gameObject.SetActive(false);
        }
    }

    public Banana GetAvailableBananaFromTheBagAndReduceAmountInBag()
    {
        Debug.Log("GetAvailableBananaFromTheBagCalled");

        if (_selectedBananaType == BananaType.yellow)
        {
            if (_yellowBananasInBag.Count == 0)
            {
                Debug.Log("There are no bananas in the bag");
                return null;
            }
            Banana banana = _yellowBananasInBag[_yellowBananasInBag.Count - 1];
            _yellowBananasInBag.Remove(banana);
            _yellowBananaAmountLeft.text = $"{_yellowBananasInBag.Count}/5";
            banana.gameObject.SetActive(true);
            return banana;
        }
        else if (_selectedBananaType == BananaType.green)
        {
            if (_greenBananasInBag.Count == 0)
            {
                Debug.Log("There are no bananas in the bag");
                return null;
            }
            Banana banana = _greenBananasInBag[_greenBananasInBag.Count - 1];
            _greenBananasInBag.Remove(banana);
            _greenBananaAmountLeft.text = $"{_greenBananasInBag.Count}/5";
            banana.gameObject.SetActive(true);
            return banana;
        }
        else if (_selectedBananaType == BananaType.red)
        {
            if (_redBananasInBag.Count == 0)
            {
                Debug.Log("There are no bananas in the bag");
                return null;
            }
            Banana banana = _redBananasInBag[_redBananasInBag.Count - 1];
            _redBananasInBag.Remove(banana);
            _redBananaAmountLeft.text = $"{_redBananasInBag.Count}/5";
            banana.gameObject.SetActive(true);
            return banana;
        }
        return null;
    }


    
    public void CollectBananaFromGround(Banana banana)
    {
        
        banana.GetComponent<Rigidbody2D>().excludeLayers = LayerMask.GetMask("Player");
        

        if (banana.GetBananaType() == Banana.BananaType.yellow)
        {
            _yellowBananasInBag.Add(banana);
            banana.gameObject.SetActive(false);
            _yellowBananaAmountLeft.text = $"{_yellowBananasInBag.Count}/5";
        }
        else if (banana.GetBananaType() == Banana.BananaType.green)
        {
            _greenBananasInBag.Add(banana);
            banana.gameObject.SetActive(false);
            _greenBananaAmountLeft.text = $"{_greenBananasInBag.Count}/5";
        }
        else if (banana.GetBananaType() == Banana.BananaType.red)
        {
            _redBananasInBag.Add(banana);
            banana.gameObject.SetActive(false);
            _redBananaAmountLeft.text = $"{_redBananasInBag.Count}/5";
        }
    }

    private void ChangeBananaTypes(int i)
    {
        if (i == 1)
        {
            _selectedBananaType = BananaType.yellow;
            _yellowBananaButton.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
            _greenBananaButton.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
            _redBananaButton.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
        }
        if (i == 2)
        {
            _selectedBananaType = BananaType.green;

            _yellowBananaButton.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
            _greenBananaButton.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
            _redBananaButton.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
        }
        if (i == 3)
        {
            _selectedBananaType = BananaType.red;

            _yellowBananaButton.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
            _greenBananaButton.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
            _redBananaButton.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeBananaTypes(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeBananaTypes(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeBananaTypes(3);
        }
    }
}