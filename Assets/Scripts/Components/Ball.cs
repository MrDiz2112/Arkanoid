using Managers;
using UnityEngine;

namespace Components
{
    [RequireComponent((typeof(Rigidbody2D)))]
    public class Ball : MonoBehaviour
    {
        void Update()
        {
            var rb = GetComponent<Rigidbody2D>();
        
            if (Input.GetMouseButtonDown(0) && !GameManager.Instance.IsBallLaunched)
            {
                GameManager.Instance.StartGame();

                var direction = new Vector2(Random.value, 1);
                direction.Normalize();
            
                rb.AddForce(direction * GameManager.Instance.BallForce);
            }
        }
    }
}
