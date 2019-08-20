using System;
using Common.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; set; } = null;

        [Header("Префабы")]
        public GameObject platformPrefab = null;
        public GameObject ballPrefab = null;
        public GameObject brickPrefab = null;
        public GameObject borderPrefab = null;
        
        [Header("Объекты")]
        public Camera mainCamera = null;
        public GameObject endGamePanel = null;
        public Text endGameText = null;
        public GameObject startMenu = null;
        public Slider gridSizeSlider = null;
        public Text gridSizeText = null;

        public int BrickSizeX { get; } = 2;
        public int BrickSizeY { get; } = 1;

        private int _gridSize = 5;

        public float BallForce { get; private set; } = 100.0f;

        private float _startForce = 100.0f;

        private float _platformOffset = 1.0f;
        
        private GameObject _ball  = null;
        private GameObject _platform = null;
        private GameObject _grid = null;
        private GameObject _border = null;
        
        private bool _isBallLaunched = false;

        public GameObject Ball => _ball;

        public bool IsBallLaunched => _isBallLaunched;

        // Start is called before the first frame update
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
            
            SetupUi();
        }
        
        private void Update()
        {
            CheckKeys();

            if (_isBallLaunched && _grid.transform.childCount == 0)
            {
                EndGame(true);
            }
        }
        
        private void SetupUi()
        {
            gridSizeSlider.value = gridSizeSlider.minValue;
            
            startMenu.SetActive(true);
            endGamePanel.SetActive(false);
            UpdateSliderValue();
        }

        public void UpdateSliderValue()
        {
            gridSizeText.text = $"Grid size: {gridSizeSlider.value}";
        }

        private void CheckKeys()
        {
            if (Input.GetKeyUp(KeyCode.R))
            {
                RestartGame();
            }

            if (Input.GetKey(KeyCode.Escape) && !startMenu.activeSelf)
            {
                RestartGame();
            }
        }

        public void NewGame()
        {
            startMenu.SetActive(false);

            _gridSize = (int)gridSizeSlider.value;
            
            // adjust force to level size
            BallForce = _startForce * _gridSize;
            
            SetupLevel();
        }

        public void ExitGame()
        {
            Application.Quit();
        }
        
        private void SetupLevel()
        {
            SetupBricks();
            SetupCamera();
            SetupPlatform();
            SetupBorder();
        }

        private void SetupCamera()
        {
            mainCamera.orthographicSize = _gridSize * BrickSizeY;
        
            // align camera to the center of grid bottom line
            mainCamera.transform.position = new Vector3(_gridSize * BrickSizeX / 2.0f,
                _gridSize * BrickSizeY, -10);
        }
        
        private void SetupBricks()
        {
            _grid = new GameObject {name = "Bricks"};

            for (int i = 0; i < _gridSize; i++)
            {
                for (int j = 0; j < _gridSize; j++)
                {
                    var offsetX = BrickSizeX / 2.0f;
                    var offsetY = BrickSizeY / 2.0f;
                
                    // bring bricks up on gridSizeY cell count
                    var brick = Instantiate(brickPrefab, 
                        new Vector2(i * BrickSizeX + offsetX, 
                            j * BrickSizeY + offsetY + _gridSize * BrickSizeY), 
                        Quaternion.identity, 
                        _grid.transform);

                    brick.name = $"Brick X{i} Y{j}";
                }
            }
        }

        private void SetupBorder()
        {
            _border = Instantiate(borderPrefab, mainCamera.transform.position, Quaternion.identity);

            var width = mainCamera.GetWidth();
            var height = mainCamera.GetHeight();
            
            _border.transform.localScale = new Vector2(width, height);
        }
        
        private void SetupPlatform()
        {
            // game field center
            var centerX = _gridSize * BrickSizeX / 2.0f;

            _platform = Instantiate(platformPrefab, 
                new Vector2(centerX, _platformOffset), 
                Quaternion.identity);
            _platform.name = "Platform";
            
            _ball = Instantiate(ballPrefab, Vector2.zero, Quaternion.identity);
            _ball.name = "Ball";
        }

        public void StartGame()
        {
            _isBallLaunched = true;
        }

        public void EndGame(bool isWin)
        {
            string endText;
            
            if (isWin)
            {
                endText = "You Win";
                AudioManager.Instance.WinSound();
            }
            else
            {
                endText = "You Lose";
                AudioManager.Instance.LoseSound();
            }
            
            _isBallLaunched = false;
            Destroy(_ball.gameObject);

            endGameText.text = endText;
            endGamePanel.SetActive(true);
        }

        private void RestartGame()
        {
            _isBallLaunched = false;
            
            Destroy(_grid.gameObject);
            Destroy(_ball.gameObject);
            Destroy(_platform.gameObject);
            Destroy(_border.gameObject);
            
            SetupUi();
        }
    }
}
