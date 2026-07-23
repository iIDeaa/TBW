using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    [Header("------ Audio Source ------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("------ Audio Clip ------")]
    public AudioClip background;
    public AudioClip Setting_but;

    private void Awake()
    {
        SFXSource.playOnAwake = false;
        SFXSource.Stop();
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayButtonSFX()
    {
        if (Setting_but != null)
        {
            PlaySFX(Setting_but);
        }
        else
        {
            Debug.LogWarning("Setting_but AudioClip is not assigned in Audio_Manager.");
        }
    }
}
