using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip menuTheme;
    [SerializeField] AudioClip gameTheme;

    [SerializeField] AudioMixer audioMixer;

    AudioSource audioSource;
    

    public static AudioManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = menuTheme;
        audioMixer.SetFloat("MasterVolume", CalcVolume(LoadVolumeValue()));
    }

    public void SetMenuTheme()
    {
        audioSource.clip = menuTheme;
        audioSource.Play();
    }

    public void SetGameTheme()
    {
        audioSource.clip = gameTheme;
        audioSource.Play();
    }

    public void SetVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", CalcVolume(value));
        PlayerPrefs.SetFloat("volume", value);
    }

    float CalcVolume(float raw)
    {
        return Mathf.Log10(raw) * 20;
    }
    public float LoadVolumeValue()
    {
        float retVal = 0.5f;
        if (PlayerPrefs.HasKey("playername"))
        {
            retVal = PlayerPrefs.GetFloat("volume");
        }
        return retVal;
    }
}
