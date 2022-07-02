using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.flobelle
{
    // Imagine a 2D game where the playing area is an enclosed rectangle with width w and infinite height.In the playing area there is a ball.
    // The ball is subject to a gravitational acceleration G. The ball starts at position p and has an initial velocity of v.
    // Write an implementation for a reusable function with the signature below which predicts the position the ball will be at if and when it reaches the specified height h.
    // bool TryCalculateXPositionAtHeight(float h, Vector2 p, Vector2 v, float G, float w, ref float xPosition);

    public class QuestionTwo : MonoBehaviour
    {
        [SerializeField] Vector2 _startPosition = new Vector2(5f, 5f);
        [SerializeField] Vector2 _velocity = new Vector2(2f, 2f);

        [Header("Other properties")]
        [SerializeField] float _width = 10f;
        [SerializeField] float _height = 2f;
        [SerializeField] float _gravity = -9.8f;

        [Header("Check this to refresh plot with new settings")]
        [SerializeField] bool _checkThisToPlot = true;

        [Header("for debug and display")]
        [SerializeField] float _maxDuration = 2f;
        [SerializeField] Transform _cameraTransform;
        [SerializeField] LineRenderer _predictedPath;
        [SerializeField] LineRenderer _heightLine;
        [SerializeField] LineRenderer _rightWallLine;

        List<Vector3> _points = new List<Vector3>();
        float _xPosition;

		private void Update()
		{
            if (_checkThisToPlot)
            {
                _checkThisToPlot = false;
                SetLines();

                if (TryCalculateXPositionAtHeight(_height, _startPosition, _velocity, _gravity, _width, ref _xPosition))
				{
                    Debug.Log($"_xPosition = {_xPosition}");
                }
            }
        }

		bool TryCalculateXPositionAtHeight(float h, Vector2 p, Vector2 v, float G, float w, ref float xPosition)
		{
            float _timeStep = Time.fixedDeltaTime;
            bool crossedHline = p.y > h;
            bool xPositionSet = false;
            Vector2 currentPosition = p;

            _points.Clear();
            _points.Add(p);

            float fStepsInDuration = _maxDuration / _timeStep;
            int iterations = Mathf.CeilToInt(fStepsInDuration);

            for (int i = 0; i <= iterations; i++)
            {
                Vector2 vel = (_timeStep * v);
                Vector2 rawPosition = currentPosition + vel;
                rawPosition.y += (float)(0.5 * G * Mathf.Pow(i * _timeStep, 2));

                Vector2 deltaPos = rawPosition - currentPosition;
                Vector2 predictedPosition = currentPosition + deltaPos;

                Vector2 horizontalCollision(float xPos)
				{
                    float xDis = xPos - currentPosition.x;
                    float xRatio = xDis / deltaPos.x;
                    float yCollisionHeight = currentPosition.y + (deltaPos.y * xRatio);
                    _points.Add(new Vector2(xPos, yCollisionHeight));
                    return new Vector2(xPos - (deltaPos.x * (1 - xRatio)), predictedPosition.y);
                }

                float xPositionOfCollisionWithHeight(float h)
				{
                    float yDis = predictedPosition.y - h;
                    float yRatio = Mathf.Abs(yDis / deltaPos.y);
                    float xCollisionWidth = predictedPosition.x + (deltaPos.x * yRatio);
                    _points.Add(new Vector2(xCollisionWidth, h));
                    return xCollisionWidth;
                }

                if (predictedPosition.x > w)
                {
                    predictedPosition = horizontalCollision(w);
                    v.x *= -1f;
                }
                else if (predictedPosition.x < 0)
				{
                    predictedPosition = horizontalCollision(0);
                    v.x *= -1f;
                }

                if (!crossedHline && predictedPosition.y > h)
				{
                    crossedHline = true;
                }

                if (crossedHline && predictedPosition.y < h)
				{
                    xPosition = xPositionOfCollisionWithHeight(h);
                    xPositionSet = true;
                    break;
                }
				else
                {
                    _points.Add(predictedPosition);
                    currentPosition = predictedPosition;
                }

                if (predictedPosition.y < 0)
                {
                    return false;
                }

            }

            _predictedPath.positionCount = _points.Count;
            _predictedPath.SetPositions(_points.ToArray());

            return xPositionSet;
		}


        void SetLines()
        {
            _heightLine.positionCount = 2;
            _heightLine.SetPosition(0, new Vector3(-5f, _height, 0f));
            _heightLine.SetPosition(1, new Vector3(_width + 5f, _height, 0f));

            _rightWallLine.SetPosition(0, new Vector3(_width, -5f, 0f));
            _rightWallLine.SetPosition(1, new Vector3(_width, 50f, 0f));
            _cameraTransform.position = new Vector3(_width / 2f, _cameraTransform.position.y, _cameraTransform.position.z);
        }
    }
}
