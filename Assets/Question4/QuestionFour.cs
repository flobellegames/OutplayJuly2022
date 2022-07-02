using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.flobelle
{
    public class QuestionFour : MonoBehaviour
    {
        //Create a simple project in Unity using C# with the following requirements:
        //In the scene there are 100 objects that are spawned at random positions that can collide with a main game object.
        //The main game object moves at a constant speed, starting at (0, 0, 0), towards 3 points(P1, P2, P3) that can be changed by a designer.
        //At arrival at each point, the game object should begin movement toward the next point.
        //On arrival at the final point or if a collision is detected, the main game object should be removed from the scene,
        //play a sound effect and show a particle effect.
        //Note that the game uses the physics system provided by Unity and the collisions are reported by Unity.

        [SerializeField] GameObject _randomPlacedCubePrefab;
        [SerializeField] Vector2 _minMaxWidth;
        [SerializeField] Vector2 _minMaxHeight;
        [SerializeField] Vector2 _minMaxDepth;
        [SerializeField] int _numberOfCubesToSpawn = 100;

        [Space(10)]
        [SerializeField] ParticleSystem _playerExplosion;
        [SerializeField] AudioClip _audioClip;
        [SerializeField] AudioSource _audioSource;

        private void OnEnable()
        {
            QuestionFour_Player.OnPlayerDied += PlayerDied;
        }
        private void OnDisable()
        {
            QuestionFour_Player.OnPlayerDied -= PlayerDied;
        }

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < _numberOfCubesToSpawn; i++)
            {
                float xR = Random.Range(_minMaxWidth.x, _minMaxWidth.y);
                float yR = Random.Range(_minMaxHeight.x, _minMaxHeight.y);
                float zR = Random.Range(_minMaxDepth.x, _minMaxDepth.y);

                GameObject cube = Instantiate(_randomPlacedCubePrefab, this.transform);
                cube.transform.position = new Vector3(xR, yR, zR);
            }
        }

        void PlayerDied(Vector3 position)
        {
            _audioSource.PlayOneShot(_audioClip);
            _playerExplosion.transform.position = position;
            _playerExplosion.Play();
        }
    }

}