using System;
using System.IO;
using System.Text;
using HoldemEngine;
using System.Threading;
using UnityEngine;

class Main: MonoBehaviour
{
	public void Start()
	{
		Debug.Log("Loading opponents...");
		var seats = new Seat[5];
		seats[0] = new Seat(1, "Human1", 1000);
		seats[1] = new Seat(2, "Human2", 1000);
		seats[2] = new Seat(3, "Human3", 1000);
		seats[3] = new Seat(4, "Human4", 1000);
		seats[4] = new Seat(5, "Human5", 1000);
		var blinds = new double[] { 10, 20 };
		uint handNumber = 0;

		HandHistory history = new HandHistory(seats, handNumber, handNumber % (uint)seats.Length + 1, blinds, 0, BettingStructure.Limit);
		HandEngine engine = new HandEngine(history);

		while(true)
		{
			Debug.Log("==================== Player =========================");
			foreach(HoldemEngine.Action action in engine.GetAbleActions())
			{
				Debug.Log(action.ActionType + ": " + action.Amount);
			}

			bool gameOver = engine.Bet(HoldemEngine.Action.ActionTypes.Bet, 0);

			if (gameOver)
			{
				break;
			}
		}

		Debug.Log(history.ToString(true));
	}
}

