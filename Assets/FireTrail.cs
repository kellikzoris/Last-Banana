using UnityEngine;

public class FireTrail : MonoBehaviour
{
    public void DisableWithADelay()
    {
        Invoke("DisableThisGameObject", 3);
    }

    public void DisableThisGameObject()
    {
        this.gameObject.SetActive(false);
    }
}