using System;
using System.IO;
using System.Text;
using HoldemEngine;

public class ConsolePlayer : IPlayer
{

	#region IPlayer implementation

	public void GetAction (HandHistory history, out HoldemEngine.Action.ActionTypes action, out double amount)
	{
		action = getAction();
		amount = 0;
	}

	private HoldemEngine.Action.ActionTypes getAction()
	{
		string cmd = "bet";
		switch(cmd)
		{
		case "raise":
		case "bet":
		case "b":
		case "r": return HoldemEngine.Action.ActionTypes.Raise;
		case "fold":
		case "f": return HoldemEngine.Action.ActionTypes.Fold;
		case "call":
		case "check":
		case "c": return HoldemEngine.Action.ActionTypes.Call;
		default: return getAction();
		}
	}

	#endregion
}


