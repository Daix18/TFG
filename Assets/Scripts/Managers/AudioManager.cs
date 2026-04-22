using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager THIS;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip audioClip;
        [Range(0f, 1f)] public float volume;
        public bool loop;
    }

    public Sound[] _sonidos;

    private GameObject _soundManager;
    private AudioSource[] _soundSource;

    bool _isPlaying;

    private void Awake()
    {
        if (THIS == null)
        {
            THIS = this;

            _soundManager = transform.Find("Sounds")?.gameObject;

            //Inicializamos los sonidos que estan dentro del array
            InitializeSoundSources();
        }
    }

    public string PlayRandomSound()
    {
        int index = Random.Range(0, _soundSource.Length);
        AudioSource source = _soundSource[index];
        source.Play();
        return _sonidos[index].name;
    }

    public string PlaySound(string soundName)
    {
        if (_isPlaying) return null;

        for (int i = 0; i < _sonidos.Length; i++)
        {
            if (_sonidos[i].name == soundName)
            {
                _soundSource[i].Play();
                _isPlaying = true;
                StartCoroutine(ResetSound(_sonidos[i].audioClip.length));
                return _sonidos[i].name;
            }
        }
        return null; // Retorna null si no se encuentra el sonido
    }

    void InitializeSoundSources()
    {
        _soundSource = new AudioSource[_sonidos.Length];

        for (int i = 0; i < _sonidos.Length; i++)
        {
            AudioSource newSource = _soundManager.AddComponent<AudioSource>();
            newSource.clip = _sonidos[i].audioClip;
            newSource.volume = _sonidos[i].volume;
            newSource.loop = _sonidos[i].loop;
            newSource.spatialBlend = 0f;
            _soundSource[i] = newSource;
        }
    }

    IEnumerator ResetSound(float duration)
    {
        yield return new WaitForSeconds(duration);
        _isPlaying = false;
    }
}
