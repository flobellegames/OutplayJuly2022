using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.flobelle
{

    //You are working on a match-3 game with the following rules
    // - Pairs of jewels adjacent vertically and horizontally can be swapped.
    // - You can only swap jewels when this will result in a match being created.
    // - A match happens when there are 3 or more jewels of the same kind adjacent vertically or horizontally.
    // - All jewels involved in matches are set to JewelKind::Empty after each move.
    // - One point is given for each jewel that has been removed. The best move for a given board is thus the one that will remove the most jewels.
    // - The initial board state contains no matches; therefore swapping jewels is the only way matches can be created.

    //Given the code below implement the CalculateBestMoveForBoard function.

    public class Board
    {
        List<List<JewelKind>> BoardLayout = new List<List<JewelKind>>();

        int width = 0;
        int height = 0;

        public Board(List<int> rawJewels, int w = 4, int h = 4)
		{
            BoardLayout.Clear();
            width = w;
            height = h;
            int totalCells = width * height;
            int cellCount = 0;
            for (int x = 0; x < width; x++)
			{
                List<JewelKind> column = new List<JewelKind>();
                for (int y = 0; y < height; y++)
				{
                    if (cellCount < rawJewels.Count)
					{
                        column.Add((JewelKind)rawJewels[cellCount]);
                    }
					else
					{
                        column.Add(JewelKind.Empty);
                    }
                    cellCount++;
				}
                BoardLayout.Add(column);
            }
        }

        public enum JewelKind
        {
            Empty,
            Red,
            Orange,
            Yellow,        
            Green,
            Blue,
            Indigo,
            Violet
        };

        public enum MoveDirection
        {
            Up,
            Down,
            Left,
            Right
        };

        public struct Move
        {
            public int x;
            public int y;
            public MoveDirection direction;
            public int score;
        };

        int GetWidth() { return width; }
        int GetHeight() { return height; }

        public JewelKind GetJewel(int x, int y)  { return BoardLayout[x][y]; }

        void SetJewel(int x, int y, JewelKind kind) { }

        List<JewelKind> foundInVertical = new List<JewelKind>();
        List<JewelKind> foundInHorizontal = new List<JewelKind>();


        //Implement this function
        public Move CalculateBestMoveForBoard()
		{
            Move bestMove = new Move();

            int bestMoveScore = 0;

            for (int x = 0; x < GetWidth(); x++)
			{
                for(int y = 0; y < GetHeight(); y++)
				{
                    JewelKind jKind = GetJewel(x, y);

                    int scoreUp = PerformTestMove(jKind, MoveDirection.Up, x, y);
                    int scoreDown = PerformTestMove(jKind, MoveDirection.Down, x, y);
                    int scoreLeft = PerformTestMove(jKind, MoveDirection.Left, x, y);
                    int scoreRight = PerformTestMove(jKind, MoveDirection.Right, x, y);

                    if (scoreUp > 0 && scoreUp > bestMoveScore)
                    {
                        bestMoveScore = scoreUp;
                        bestMove = new Move() { direction = MoveDirection.Up, x = x, y = y, score = bestMoveScore };
                    }
                    if (scoreDown > 0 && scoreDown > bestMoveScore)
                    {
                        bestMoveScore = scoreDown;
                        bestMove = new Move() { direction = MoveDirection.Down, x = x, y = y, score = bestMoveScore };
                    }
                    if (scoreLeft > 0 && scoreLeft > bestMoveScore)
                    {
                        bestMoveScore = scoreLeft;
                        bestMove = new Move() { direction = MoveDirection.Left, x = x, y = y, score = bestMoveScore };
                    }
                    if (scoreRight > 0 && scoreRight > bestMoveScore)
                    {
                        bestMoveScore = scoreRight;
                        bestMove = new Move() { direction = MoveDirection.Right, x = x, y = y, score = bestMoveScore };
                    }
                }
            }

            return bestMove;
		}

        int PerformTestMove(JewelKind jewelToMatch, MoveDirection direction, int x, int y)
		{
            foundInVertical.Clear();
            foundInHorizontal.Clear();

            foundInVertical.Add(jewelToMatch);
            foundInHorizontal.Add(jewelToMatch);

            switch (direction)
			{
                case MoveDirection.Up:
					{
                        y++;
                        if (y >= GetHeight()) return 0;
                        break;
					}
                case MoveDirection.Down:
                    {
                        y--;
                        if (y < 0) return 0;
                        break;
                    }
                case MoveDirection.Left:
                    {
                        x--;
                        if (x < 0) return 0;
                        break;
                    }
                case MoveDirection.Right:
                    {
                        x++;
                        if (x >= GetWidth()) return 0;
                        break;
                    }
            }

            JewelKind jewelToSwapKind = GetJewel(x, y);
            if (jewelToSwapKind == jewelToMatch) return 0;

            // don't check the direction we have just come from
            if (direction != MoveDirection.Up) CheckForMatch(jewelToMatch, MoveDirection.Down, x, y);
            if (direction != MoveDirection.Down) CheckForMatch(jewelToMatch, MoveDirection.Up, x, y);
            if (direction != MoveDirection.Left) CheckForMatch(jewelToMatch, MoveDirection.Right, x, y);
            if (direction != MoveDirection.Right) CheckForMatch(jewelToMatch, MoveDirection.Left, x, y);

            return (foundInHorizontal.Count > foundInVertical.Count) ? foundInHorizontal.Count : foundInVertical.Count;
        }

        void CheckForMatch(JewelKind jewelToMatch, MoveDirection direction, int x, int y)
		{
            JewelKind jCheck = JewelKind.Empty;

            if (direction == MoveDirection.Down)
			{
                y--;
                if (y < 0) return;
                jCheck = GetJewel(x, y);
                if (jCheck != jewelToMatch)
                    return;

                foundInVertical.Add(jCheck);
                CheckForMatch(jewelToMatch, direction, x, y);
            }
            else if (direction == MoveDirection.Up)
            {
                y++;
                if (y >= GetHeight()) return;
                jCheck = GetJewel(x, y);
                if (jCheck != jewelToMatch)
                    return;

                foundInVertical.Add(jCheck);
                CheckForMatch(jewelToMatch, direction, x, y);
            }
            else if (direction == MoveDirection.Left)
            {
                x--;
                if (x < 0) return;
                jCheck = GetJewel(x, y);
                if (jCheck != jewelToMatch)
                    return;

                foundInHorizontal.Add(jCheck);
                CheckForMatch(jewelToMatch, direction, x, y);
            }
            else if (direction == MoveDirection.Right)
            {
                x++;
                if (x >= GetWidth()) return;
                jCheck = GetJewel(x, y);
                if (jCheck != jewelToMatch)
                    return;

                foundInHorizontal.Add(jCheck);
                CheckForMatch(jewelToMatch, direction, x, y);
            }
        }


        public Move TestSinglePosition(int x, int y)
        {
            Move bestMove = new Move();
            int bestMoveScore = 0;

            JewelKind jKind = GetJewel(x, y);

            int scoreUp = PerformTestMove(jKind, MoveDirection.Up, x, y);
            int scoreDown = PerformTestMove(jKind, MoveDirection.Down, x, y);
            int scoreLeft = PerformTestMove(jKind, MoveDirection.Left, x, y);
            int scoreRight = PerformTestMove(jKind, MoveDirection.Right, x, y);

            if (scoreUp > 0 && scoreUp > bestMoveScore)
            {
                bestMoveScore = scoreUp;
                bestMove = new Move() { direction = MoveDirection.Up, x = x, y = y, score = bestMoveScore };
            }
            if (scoreDown > 0 && scoreDown > bestMoveScore)
            {
                bestMoveScore = scoreDown;
                bestMove = new Move() { direction = MoveDirection.Down, x = x, y = y, score = bestMoveScore };
            }
            if (scoreLeft > 0 && scoreLeft > bestMoveScore)
            {
                bestMoveScore = scoreLeft;
                bestMove = new Move() { direction = MoveDirection.Left, x = x, y = y, score = bestMoveScore };
            }
            if (scoreRight > 0 && scoreRight > bestMoveScore)
            {
                bestMoveScore = scoreRight;
                bestMove = new Move() { direction = MoveDirection.Right, x = x, y = y, score = bestMoveScore };
            }

            return bestMove;
        }

    };

}