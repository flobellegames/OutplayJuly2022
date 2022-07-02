using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.flobelle
{
    public class QuestionFour_CameraFollow : MonoBehaviour
    {

        [SerializeField] Transform _toFollow;
        [SerializeField] Vector3 _cameraOffset = new Vector3(0,0,-10f);

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

        }

        // Update is called once per frame
        void Update()
        {
            if (_toFollow != null)
			{
                this.transform.position = _toFollow.position + _cameraOffset;
            }
        }

        void PlayerDied(Vector3 position)
		{
            _toFollow = null;
            this.transform.position = position + _cameraOffset;
        }
    }

}