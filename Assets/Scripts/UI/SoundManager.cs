using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;      // For clicks
    [SerializeField] private AudioSource musicSource;    // For background music

    [Header("Click Sounds (SFX)")]
    [SerializeField] private AudioClip buttonOpen;
    [SerializeField] private AudioClip buttonClose;
    [SerializeField] private AudioClip creatureClick;
    [SerializeField] private AudioClip planetClick;
    [SerializeField] private AudioClip sendOffClick;

    [Header("Background Music")]
    [SerializeField] private AudioClip bgMusic1;
    [SerializeField] private AudioClip bgMusic2;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // --- CLICK SOUNDS ---
    public void PlayClick(int index)
    {
        switch (index)
        {
            case 1: sfxSource.PlayOneShot(buttonOpen); break;
            case 2: sfxSource.PlayOneShot(buttonClose); break;
            case 3: sfxSource.PlayOneShot(creatureClick); break;
            case 4: sfxSource.PlayOneShot(planetClick); break;
            case 5: sfxSource.PlayOneShot(sendOffClick, 20f); break;
            default: Debug.LogWarning("Invalid click sound index"); break;
        }
    }

    // --- BACKGROUND MUSIC ---
    public void PlayBackground(int index)
    {
        musicSource.Stop();
        switch (index)
        {
            case 1: musicSource.clip = bgMusic1; break;
            case 2: musicSource.clip = bgMusic2; break;
            default: Debug.LogWarning("Invalid background index"); return;
        }
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopBackground()
    {
        musicSource.Stop();
    }

    public void ToggleMuteMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleMuteSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }
}
