using UnityEngine;

public class Door : MonoBehaviour
{
    public SceneManagerOfGorilla.ScenesToLoad thisDoorLeadsTo;

    [SerializeField] private Sprite _openDoorSprite;
    [SerializeField] private Sprite _closedDoorSprite;

    [SerializeField] private bool _isDoorOpened;

    public void SetDoorOpen(bool doorOpen)
    {
        _isDoorOpened = doorOpen;
        this.GetComponent<SpriteRenderer>().sprite = doorOpen ? _openDoorSprite : _closedDoorSprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log("You're touching the door and do you want to proceed");
            collision.transform.GetComponent<Player>().DisableControls();
            FindObjectOfType<LineRendererDrawer>().DisableAttacks();
            if (_isDoorOpened)
            {
                string message = "";
                if (thisDoorLeadsTo == SceneManagerOfGorilla.ScenesToLoad.TigerFightScene)
                {
                    message = "You already beat the Tiger";
                }
                else
                {
                    message = "You already beat the Bull";
                }
                FindObjectOfType<SceneManagerOfGorilla>().ShowWelcomeText(message, false, true);
                return;
            }
            else
            {
                FindObjectOfType<SceneManagerOfGorilla>().SetSelectedScene(thisDoorLeadsTo);
            }
        }
    }
}