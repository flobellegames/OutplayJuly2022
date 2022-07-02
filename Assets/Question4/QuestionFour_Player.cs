using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.flobelle
{
    public class QuestionFour_Player : MonoBehaviour
    {
        public delegate void PlayerDiedDelegate(Vector3 position);
        public static event PlayerDiedDelegate OnPlayerDied;

        [SerializeField] float _speed = 500f;
        [SerializeField] QuestionFour_PlayerTargetPoints _targetPositions;
        int _targetPositionIndex = 0;

        Rigidbody _rigidBody;
        Vector3 _targetPosition;
        Vector3 _direction;
        bool _move = false;

        // Start is called before the first frame update
        void Start()
        {
            _targetPosition = _targetPositions.Positions[_targetPositionIndex];
            _rigidBody = GetComponent<Rigidbody>();

            StartCoroutine(StartCountdown(2f));  // wait 2 seconds from scene load before starting to move, just give the player time to orientate themselves 
        }

        // Update is called once per frame
        void Update()
        {
            if (_move)
			{
                if (Vector3.Distance(this.transform.position, _targetPosition) < 0.25f)
				{
                    _targetPositionIndex++;
                    if (_targetPositionIndex < _targetPositions.Positions.Count)
					{
                        _targetPosition = _targetPositions.Positions[_targetPositionIndex];
                    }
					else
					{
                        OnPlayerDied?.Invoke(this.transform.position);
                        GameObject.Destroy(this.gameObject);
                    }
                }

                _direction = (_targetPosition - this.transform.position).normalized;

            }
        }

		private void FixedUpdate()
		{
            if (_move)
			{
                _rigidBody.velocity = _speed * Time.fixedDeltaTime * _direction;
            }
        }

		IEnumerator StartCountdown(float waitTime)
		{
            yield return new WaitForSeconds(waitTime);
            _move = true;
		}

		private void OnCollisionEnter(Collision collision)
		{
            OnPlayerDied?.Invoke(this.transform.position);
            GameObject.Destroy(this.gameObject);
        }


    }
}