using UnityEngine;

namespace Core.Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [SerializeField]private AudioSource bgmSource;
        private AudioSource[] _sfxSources;

        [Header("BGM Clips")]
        public AudioClip bullet;       // 타이틀 BGM

        public AudioClip windBullet;

        public AudioClip jump;
        public AudioClip water;
        public AudioClip die;
        public AudioClip bgm;
        public AudioClip run;
        public AudioClip clear;
        public AudioClip ouch;
        public AudioClip boom;
        public AudioClip detect;
        public AudioClip razor;
        public AudioClip thunder;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            _sfxSources = new AudioSource[10];
            for (int i = 0; i < _sfxSources.Length; i++)
            {
                _sfxSources[i] = gameObject.AddComponent<AudioSource>();
            }
            DontDestroyOnLoad(gameObject);
        }

        public void PlayBgm(AudioClip clip)
        {
            if (bgmSource.clip == clip && bgmSource.isPlaying) return;

            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }

        public void PlaySfx(AudioClip clip)
        {
            foreach (var source in _sfxSources)
            {
                if (!source.isPlaying)
                {
                    source.PlayOneShot(clip);
                    return;
                }
            }
        }

        public void StopBGM() {
            bgmSource.Stop();
        }

        public void PauseBGM() {
            bgmSource.Pause();
        }

        public void ResumeBGM() {
            bgmSource.UnPause();
        }
    }
}