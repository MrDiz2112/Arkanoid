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
        private Sprite[] brickSprites;

        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _collider;
        
        private void Start()
        {
            transform.localScale = new Vector2(GameManager.Instance.BrickSizeX, GameManager.Instance.BrickSizeY);

            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<BoxCollider2D>();

            _animator.enabled = false;
            
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
                
                _spriteRenderer.sprite = sprite;
                _spriteRenderer.size = new Vector2(1, 1);
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError("Textures for bricks not set");
                throw;
            }
        }

        private IEnumerator DestroyBrick()
        {
            _animator.enabled = true;
            _collider.enabled = false;
            
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
            
            Destroy(gameObject);
        }
    }
}
