using UnityEngine;

namespace Managers
{
    
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager: MonoBehaviour
    {
        public static AudioManager Instance { get; set; } = null;

        [Header("Звуки")] 
        [SerializeField]
        private AudioClip hitSound;
        [SerializeField]
        private AudioClip loseSound;
        [SerializeField]
        private AudioClip winSound;

        private AudioSource _audioSource;
        
        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

            _audioSource = GetComponent<AudioSource>();
        }

        public void HitSound()
        {
            _audioSource.PlayOneShot(hitSound);
        }
        
        public void LoseSound()
        {
            _audioSource.PlayOneShot(loseSound);
        }
        
        public void WinSound()
        {
            _audioSource.PlayOneShot(winSound);
        }
    }
}
