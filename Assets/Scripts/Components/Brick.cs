using System;
using System.Collections;
using Common.Enums;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Components
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent((typeof(SpriteRenderer)))]
    [RequireComponent((typeof(BoxCollider2D)))]
    public class Brick: MonoBehaviour
    {
        [SerializeField]
        private Sprite[] brickSprites = null;

        [SerializeField]
        private Animator animator = null;
        
        [SerializeField]
        private SpriteRenderer spriteRenderer = null;
        
        [SerializeField]
        private BoxCollider2D boxCollider2D = null;
        
        private void Start()
        {
            transform.localScale = new Vector2(GameManager.Instance.BrickSizeX, GameManager.Instance.BrickSizeY);

            animator.enabled = false;
            
            SetColor();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!GameManager.Instance.IsBallLaunched)
            {
                return;
            }
            
            if (other.gameObject.CompareTag("Ball"))
            {
                AudioManager.Instance.HitSound();
                StartCoroutine(DestroyBrick());
            }
        }

        private void SetColor()
        {
            var colors = Enum.GetValues(typeof(BrickColor));
            var color = (BrickColor) colors.GetValue(Random.Range(0, colors.Length - 1));

            try
            {
                var sprite = brickSprites[(int) color];
                
                spriteRenderer.sprite = sprite;
                spriteRenderer.size = new Vector2(1, 1);
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError("Textures for bricks not set");
                throw;
            }
        }

        private IEnumerator DestroyBrick()
        {
            animator.enabled = true;
            boxCollider2D.enabled = false;
            
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            
            Destroy(gameObject);
        }
    }
}
