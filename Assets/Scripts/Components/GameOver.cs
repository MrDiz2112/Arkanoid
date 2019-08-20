using Managers;
using UnityEngine;

namespace Components
{
    public class GameOver : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (GameManager.Instance.IsBallLaunched && other.gameObject.CompareTag("Ball"))
            {
                GameManager.Instance.EndGame(false);
            }
        }
    }
}
