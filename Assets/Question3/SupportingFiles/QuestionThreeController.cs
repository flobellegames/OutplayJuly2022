using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace com.flobelle
{
    public class QuestionThreeController : MonoBehaviour
    {
        [Header("Board setup")]
        [SerializeField] int _width = 4;
        [SerializeField] int _height = 4;

        [SerializeField] string _playBoardAsString = "1,1,2,7,4,5,7,6,3,2,4,7,7,1,1,7";
        [SerializeField] bool _refreshBoard = true;

		
		[Header("Display")]
        [SerializeField] GameObject _container;
        [SerializeField] QuestionThreeText _text;
        [SerializeField] Text _bestMoveResult;

        float _cellSize = 30f;
        Board board;

        // Start is called before the first frame update
        void Start()
        {
        }

		private void Update()
		{
			if (_refreshBoard)
			{
                _refreshBoard = false;
                CreateBoard();
            }
		}

		void CreateBoard()
        {
            List<int> rawJewels = _playBoardAsString.Split(',').Select(s => Convert.ToInt32(s.Trim())).ToList();

            board = new Board(rawJewels);
            Board.Move move = board.CalculateBestMoveForBoard();
            if (move.score >= 3)
			{
                _bestMoveResult.text = $"Move {(int)board.GetJewel(move.x, move.y)} x:{move.x}, y:{move.y} {move.direction} for Best Score:{move.score}";
            }
			else
			{
                _bestMoveResult.text = "No best move found";
            }

            // for test display purposes.
            float col = 0f;
            float row = 0f;
            int jewelCount = 0;


            foreach (Transform child in _container.transform)
            {
                Destroy(child.gameObject);
            }


            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    QuestionThreeText textItem = Instantiate(_text, _container.transform);
                    textItem.Setup(rawJewels[jewelCount], row, col);

                    jewelCount++;
                    col += _cellSize;
                }
                col = 0f;
                row += _cellSize;
            }
        }
    }
}