using System;
using System.Collections.Generic;
using System.Text;

namespace HoldemEngine
{
    /// <summary>
    /// Simple data class representing the winner of a Pot.
    /// 
    /// Author: Wesley Tansey
    /// </summary>
    public class Winner
    {
        public int SeatNumber { get; set; }
        public double Amount { get; set; }
        public string Player { get; set; }
        public string Pot { get; set; }

        public Winner(int seatNumber, string player, string pot, double amount)
        {
            SeatNumber = seatNumber;
            Player = player;
            Pot = pot;
            Amount = amount;            
        }
    }
}
