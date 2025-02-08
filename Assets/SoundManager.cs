using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource bananaAttackSound;
    [SerializeField] AudioSource eatingBananaSound;
    [SerializeField] AudioSource meleeAttackGorilla;
    [SerializeField] AudioSource clickButtonSound;
    [SerializeField] AudioSource collectBananaSound;
    [SerializeField] AudioSource bullMovementSound;
    [SerializeField] AudioSource tigerAttackSound;
    [SerializeField] AudioSource trapPlantedSound;
    [SerializeField] AudioSource trapClosedSound;
    [SerializeField] AudioSource bullKickingGround;

    [SerializeField] AudioSource lostSound;
    [SerializeField] AudioSource winSound;







    public void PlayBananaAttackSound()
    {
        bananaAttackSound.Play();
    }

    public void PlayEatingBananaSound()
    {
        eatingBananaSound.Play();
    }

    public void PlayMeleeAttackGorilla()
    {
        meleeAttackGorilla.Play();
    }

    public void PlayClickButtonSound()
    {
        clickButtonSound.Play();
    }

    public void PlayCollectingBananaSound()
    {
        collectBananaSound.Play();
    }

    public void PlayBullFeetSound()
    {
        bullMovementSound.Play();
    }

    public void PlayTigerAttackSound()
    {
        tigerAttackSound.Play();
    }

    public void PlayTrapPlantedSound()
    {
        trapPlantedSound.Play();
    }

    public void PlayTrapClosedSound()
    {
        trapClosedSound.Play();
    }

    public void PlayBullKickingGround()
    {
        bullKickingGround.Play();
    }

    public void PlayLostSound()
    {
        lostSound.Play();
    }
    public void PlayWinSound()
    {
        winSound.Play();
    }

}
