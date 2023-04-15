using _2DGameMaker.GAME_NAME.GUI.GameUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME.PLAYER_NAME
{
    public struct PlayerStats
    {
        public int Score { get; private set; }

        public void SetScore(int score)
        {
            Score = score;

            StatsDisplay.UpdateScore(score);
        }

        public void AddScore(int score)
        {
            Score += score;
            StatsDisplay.UpdateScore(Score);
        }
    }
}
