using System;
namespace CardGameWebApi.Models
{
	public class GameStatus
	{
        public bool IsRunning { get; set; }
		public string GameType { get; set; }
		public DateTime? StartTime { get; set; } = null;
		public DateTime? EndTime { get; set; } = null;

        public bool StartGame(string gameType)
        {
            if (!IsRunning)
            {
                IsRunning = true;
                GameType = gameType;
                StartTime = DateTime.Now;
                return true;
            }

            return false;
        }

        public bool EndGame()
        {
            if (IsRunning)
            {
                IsRunning = false;
                EndTime = DateTime.Now;
                return true;
            }

            return false;
        }

    }
}

