﻿using System;
using Common.Extensions;
using Managers;
using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Platform : MonoBehaviour
    {
        private float _positionX = 1;
        private float _positionY = 1;

        private float _sizeX = 2.0f;
        private float _sizeY = 0.5f;

        private Camera _mainCamera;
        
        private float _cameraWidth;
        private float _cameraHeight;
        private Vector3 _cameraPosition;
        

        // Start is called before the first frame update
        void Start()
        {
            var brickSizeX = GameManager.Instance.BrickSizeX;
            var brickSizeY = GameManager.Instance.BrickSizeY;

            _sizeX *= brickSizeX;
            _sizeY *= brickSizeY;

            var platformTransform = transform;
            platformTransform.localScale = new Vector2( _sizeX, _sizeY);
            
            _mainCamera = GameManager.Instance.mainCamera.GetComponent<Camera>();
            _cameraHeight = _mainCamera.GetHeight();
            _cameraWidth = _mainCamera.GetWidth();
            _cameraPosition = _mainCamera.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (GameManager.Instance.IsBallLaunched && other.gameObject.CompareTag("Ball"))
            {
                var contact = other.GetContact(other.contactCount - 1);

                var distanceFromCenter = (contact.point.x - _positionX) / _sizeX;
                var newDirection = new Vector3(distanceFromCenter, 1);
                newDirection.Normalize();

                var rb = GameManager.Instance.Ball.GetComponent<Rigidbody2D>();
                
                rb.velocity = Vector2.zero;
                rb.AddForce(newDirection * GameManager.Instance.BallForce);
            }
        }

        private void Move()
        {
            var ball = GameManager.Instance.Ball;
        
            var mouseX = _mainCamera.ScreenToWorldPoint(Input.mousePosition).x;

            var offsetX = _sizeX / 2.0f;

            _positionX = Mathf.Clamp(mouseX, 
                _cameraPosition.x - _cameraWidth / 2.0f + offsetX, 
                _cameraPosition.x + _cameraWidth / 2.0f - offsetX);
        
            transform.position = new Vector2(_positionX, _positionY);

            if (!GameManager.Instance.IsBallLaunched && ball != null)
            {
                ball.transform.position = new Vector2(_positionX,
                    _positionY + _sizeY / 2.0f + ball.transform.localScale.y / 2.0f);
            }
        }
    }
}