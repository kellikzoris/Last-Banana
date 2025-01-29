using UnityEngine;
using UnityEngine.UI;

public class EatABananaFromBag : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (GetComponent<Button>().interactable)
            {
                GetComponent<Button>().onClick.Invoke();
            }
        }
    }
}