using UnityEngine;

namespace Managers
{
    
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager: MonoBehaviour
    {
        public static AudioManager Instance { get; set; } = null;

        [Header("Звуки")] 
        [SerializeField]
        private AudioClip hitSound = null;
        [SerializeField]
        private AudioClip loseSound = null;
        [SerializeField]
        private AudioClip winSound = null;

        [SerializeField]
        private AudioSource audioSource = null;
        
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
        }

        public void HitSound()
        {
            audioSource.PlayOneShot(hitSound);
        }
        
        public void LoseSound()
        {
            audioSource.PlayOneShot(loseSound);
        }
        
        public void WinSound()
        {
            audioSource.PlayOneShot(winSound);
        }
    }
}
