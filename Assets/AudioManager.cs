using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip menuTheme;
    [SerializeField] AudioClip gameTheme;

    AudioSource audioSource;

    public static AudioManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = menuTheme;
        audioSource.volume = GameManager.Instance.LoadVolumeValue();
    }

    public void SetMenuTheme()
    {
        audioSource.clip = menuTheme;
    }

    public void SetGameTheme()
    {
        audioSource.clip = gameTheme;
    }

    public void SetVolume(float value)
    {
        Instance.audioSource.volume = value;
    }
}
