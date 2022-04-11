using System;
using System.Collections.Generic;
using System.Text;

namespace HoldemEngine
{
    public class TournamentHandHistory : HandHistory
    {
        #region Fields
        private ulong tourneyNumber;
        private double buyIn;
        private double fee;
        private int level;
        private int tableNumber;
        #endregion

        #region Properties
        public int TableNumber
        {
            get { return tableNumber; }
            set { tableNumber = value; }
        }

        public ulong TournamentNumber
        {
            get { return tourneyNumber; }
            set { tourneyNumber = value; }
        }

        public double Fee
        {
            get { return fee; }
            set { fee = value; }
        }

        public double BuyIn
        {
            get { return buyIn; }
            set { buyIn = value; }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }
        #endregion

        public TournamentHandHistory(Seat[] players, ulong handNumber, uint button, double[] blinds, double ante, BettingStructure bs)
            : base(players, handNumber, button, blinds, ante, bs)
        {
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append(string.Format("{0} Game #{1}: Tournament #{2}, ${3}+${4} Hold'em ",
                   Site, HandNumber,
                   tourneyNumber, buyIn, fee));

            #region Betting Structure
            switch (this.BettingStructure)
            {
                case BettingStructure.Limit: result.Append("Limit");
                    break;
                case BettingStructure.PotLimit: result.Append("Pot Limit");
                    break;
                case BettingStructure.NoLimit: result.Append("No Limit");
                    break;
                default: throw new Exception("Unknown betting structure");
            }
            #endregion

            result.AppendLine(string.Format(" - Level {0} ({1}/{2} - {3} - {4}", LEVEL_NAMES[level], SmallBlind, BigBlind, 
                                        DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()) );

            result.AppendLine(string.Format("Table '{0} {1}' {2}-max Seat #{3} is the button", 
                                        tourneyNumber, tableNumber,
                                        MaxPlayersPerTable, Button));

            for (int i = 0; i < Players.Length; i++)
            {
                result.AppendLine(string.Format("Seat {0}: {1} ({2} in chips)", Players[i].SeatNumber,
                                                                      Players[i].Name, StartingChips[i]));
                
            }

            foreach (Action action in PredealActions)
                result.AppendLine(action.ToString());

            result.AppendLine("*** HOLE CARDS ***");

            for (int i = 0; i < Players.Length; i++)
                if (HoleCards[i] != 0UL)
                    result.AppendLine(string.Format("Dealt to {0} [{1}]", Players[i].Name, 
                                                    HoldemHand.Hand.MaskToString(HoleCards[i])));

            foreach(Action action in PreflopActions)
                result.AppendLine(action.ToNoDollarSignString());

            #region Print post-flop actions and board cards.
            if(Flop != 0UL)
            {
                        result.AppendLine("*** Flop *** [" + HoldemHand.Hand.MaskToString(Flop) + "]");

                        foreach (Action action in FlopActions)
                            result.AppendLine(action.ToNoDollarSignString());
            }
            if (Turn != 0UL)
            {
                result.AppendLine("*** Turn *** [" + HoldemHand.Hand.MaskToString(Flop) + "] ["
                                                   + HoldemHand.Hand.MaskToString(Turn) + "]");

                foreach (Action action in TurnActions)
                    result.AppendLine(action.ToNoDollarSignString());
            }
            if (River != 0UL)
            {
                result.AppendLine("*** River *** ["
                                      + HoldemHand.Hand.MaskToString(Flop | Turn)
                                      + "] ["
                                      + HoldemHand.Hand.MaskToString(River) + "]");

                foreach (Action action in TurnActions)
                    result.AppendLine(action.ToNoDollarSignString());
            }
            #endregion

            //TODO: output hand showdowns and strength descriptions, also determine if there was a showdown or not
            foreach (Winner winner in Winners)
                result.AppendFormat("{0} collected {1} from {2}", winner.Player, winner.Amount, winner.Pot);

            //TODO: output summaries

            return result.ToString();
        }

        #region Level names (roman numerals)
        private static readonly string[] LEVEL_NAMES = new string[] { "0",
            "I","II","III","IV","V","VI","VII","VIII","IX","X",
"XI","XII","XIII","XIV","XV","XVI","XVII","XVIII","XIX","XX",
"XXI","XXII","XXIII","XXIV","XXV","XXVI","XXVII","XXVIII","XXIX","XXX",
"XXXI","XXXII","XXXIII","XXXIV","XXXV","XXXVI","XXXVII","XXXVIII","XXXIX","XL",
"XLI","XLII","XLIII","XLIV","XLV","XLVI","XLVII","XLVIII","XLIX","L",
"LI","LII","LIII","LIV","LV","LVI","LVII","LVIII","LIX","LX",
"LXI","LXII","LXIII","LXIV","LXV","LXVI","LXVII","LXVIII","LXIX","LXX",
"LXXI","LXXII","LXXIII","LXXIV","LXXV","LXXVI","LXXVII","LXXVIII","LXXIX","LXXX",
"LXXXI","LXXXII","LXXXIII","LXXXIV","LXXXV","LXXXVI","LXXXVII","LXXXVIII","LXXXIX","XC",
"XCI","XCII","XCIII","XCIV","XCV","XCVI","XCVII","XCVIII","XCIX","C",
"CI","CII","CIII","CIV","CV","CVI","CVII","CVIII","CIX","CX",
"CXI","CXII","CXIII","CXIV","CXV","CXVI","CXVII","CXVIII","CXIX","CXX",
"CXXI","CXXII","CXXIII","CXXIV","CXXV","CXXVI","CXXVII","CXXVIII","CXXIX","CXXX",
"CXXXI","CXXXII","CXXXIII","CXXXIV","CXXXV","CXXXVI","CXXXVII","CXXXVIII","CXXXIX","CXL",
"CXLI","CXLII","CXLIII","CXLIV","CXLV","CXLVI","CXLVII","CXLVIII","CXLIX","CL",
"CLI","CLII","CLIII","CLIV","CLV","CLVI","CLVII","CLVIII","CLIX","CLX",
"CLXI","CLXII","CLXIII","CLXIV","CLXV","CLXVI","CLXVII","CLXVIII","CLXIX","CLXX",
"CLXXI","CLXXII","CLXXIII","CLXXIV","CLXXV","CLXXVI","CLXXVII","CLXXVIII","CLXXIX","CLXXX",
"CLXXXI","CLXXXII","CLXXXIII","CLXXXIV","CLXXXV","CLXXXVI","CLXXXVII","CLXXXVIII","CLXXXIX","CXC",
"CXCI","CXCII","CXCIII","CXCIV","CXCV","CXCVI","CXCVII","CXCVIII","CXCIX","CC",
"CCI","CCII","CCIII","CCIV","CCV","CCVI","CCVII","CCVIII","CCIX","CCX",
"CCXI","CCXII","CCXIII","CCXIV","CCXV","CCXVI","CCXVII","CCXVIII","CCXIX","CCXX",
"CCXXI","CCXXII","CCXXIII","CCXXIV","CCXXV","CCXXVI","CCXXVII","CCXXVIII","CCXXIX","CCXXX",
"CCXXXI","CCXXXII","CCXXXIII","CCXXXIV","CCXXXV","CCXXXVI","CCXXXVII","CCXXXVIII","CCXXXIX","CCXL",
"CCXLI","CCXLII","CCXLIII","CCXLIV","CCXLV","CCXLVI","CCXLVII","CCXLVIII","CCXLIX","CCL",
"CCLI","CCLII","CCLIII","CCLIV","CCLV","CCLVI","CCLVII","CCLVIII","CCLIX","CCLX",
"CCLXI","CCLXII","CCLXIII","CCLXIV","CCLXV","CCLXVI","CCLXVII","CCLXVIII","CCLXIX","CCLXX",
"CCLXXI","CCLXXII","CCLXXIII","CCLXXIV","CCLXXV","CCLXXVI","CCLXXVII","CCLXXVIII","CCLXXIX","CCLXXX",
"CCLXXXI","CCLXXXII","CCLXXXIII","CCLXXXIV","CCLXXXV","CCLXXXVI","CCLXXXVII","CCLXXXVIII","CCLXXXIX","CCXC",
"CCXCI","CCXCII","CCXCIII","CCXCIV","CCXCV","CCXCVI","CCXCVII","CCXCVIII","CCXCIX","CCC",
"CCCI","CCCII","CCCIII","CCCIV","CCCV","CCCVI","CCCVII","CCCVIII","CCCIX","CCCX",
"CCCXI","CCCXII","CCCXIII","CCCXIV","CCCXV","CCCXVI","CCCXVII","CCCXVIII","CCCXIX","CCCXX",
"CCCXXI","CCCXXII","CCCXXIII","CCCXXIV","CCCXXV","CCCXXVI","CCCXXVII","CCCXXVIII","CCCXXIX","CCCXXX",
"CCCXXXI","CCCXXXII","CCCXXXIII","CCCXXXIV","CCCXXXV","CCCXXXVI","CCCXXXVII","CCCXXXVIII","CCCXXXIX","CCCXL",
"CCCXLI","CCCXLII","CCCXLIII","CCCXLIV","CCCXLV","CCCXLVI","CCCXLVII","CCCXLVIII","CCCXLIX","CCCL",
"CCCLI","CCCLII","CCCLIII","CCCLIV","CCCLV","CCCLVI","CCCLVII","CCCLVIII","CCCLIX","CCCLX",
"CCCLXI","CCCLXII","CCCLXIII","CCCLXIV","CCCLXV","CCCLXVI","CCCLXVII","CCCLXVIII","CCCLXIX","CCCLXX",
"CCCLXXI","CCCLXXII","CCCLXXIII","CCCLXXIV","CCCLXXV","CCCLXXVI","CCCLXXVII","CCCLXXVIII","CCCLXXIX","CCCLXXX",
"CCCLXXXI","CCCLXXXII","CCCLXXXIII","CCCLXXXIV","CCCLXXXV","CCCLXXXVI","CCCLXXXVII","CCCLXXXVIII","CCCLXXXIX","CCCXC",
"CCCXCI","CCCXCII","CCCXCIII","CCCXCIV","CCCXCV","CCCXCVI","CCCXCVII","CCCXCVIII","CCCXCIX","CD",
"CDI","CDII","CDIII","CDIV","CDV","CDVI","CDVII","CDVIII","CDIX","CDX",
"CDXI","CDXII","CDXIII","CDXIV","CDXV","CDXVI","CDXVII","CDXVIII","CDXIX","CDXX",
"CDXXI","CDXXII","CDXXIII","CDXXIV","CDXXV","CDXXVI","CDXXVII","CDXXVIII","CDXXIX","CDXXX",
"CDXXXI","CDXXXII","CDXXXIII","CDXXXIV","CDXXXV","CDXXXVI","CDXXXVII","CDXXXVIII","CDXXXIX","CDXL",
"CDXLI","CDXLII","CDXLIII","CDXLIV","CDXLV","CDXLVI","CDXLVII","CDXLVIII","CDXLIX","CDL",
"CDLI","CDLII","CDLIII","CDLIV","CDLV","CDLVI","CDLVII","CDLVIII","CDLIX","CDLX",
"CDLXI","CDLXII","CDLXIII","CDLXIV","CDLXV","CDLXVI","CDLXVII","CDLXVIII","CDLXIX","CDLXX",
"CDLXXI","CDLXXII","CDLXXIII","CDLXXIV","CDLXXV","CDLXXVI","CDLXXVII","CDLXXVIII","CDLXXIX","CDLXXX",
"CDLXXXI","CDLXXXII","CDLXXXIII","CDLXXXIV","CDLXXXV","CDLXXXVI","CDLXXXVII","CDLXXXVIII","CDLXXXIX","CDXC",
"CDXCI","CDXCII","CDXCIII","CDXCIV","CDXCV","CDXCVI","CDXCVII","CDXCVIII","CDXCIX","D",
"DI","DII","DIII","DIV","DV","DVI","DVII","DVIII","DIX","DX",
"DXI","DXII","DXIII","DXIV","DXV","DXVI","DXVII","DXVIII","DXIX","DXX",
"DXXI","DXXII","DXXIII","DXXIV","DXXV","DXXVI","DXXVII","DXXVIII","DXXIX","DXXX",
"DXXXI","DXXXII","DXXXIII","DXXXIV","DXXXV","DXXXVI","DXXXVII","DXXXVIII","DXXXIX","DXL",
"DXLI","DXLII","DXLIII","DXLIV","DXLV","DXLVI","DXLVII","DXLVIII","DXLIX","DL",
"DLI","DLII","DLIII","DLIV","DLV","DLVI","DLVII","DLVIII","DLIX","DLX",
"DLXI","DLXII","DLXIII","DLXIV","DLXV","DLXVI","DLXVII","DLXVIII","DLXIX","DLXX",
"DLXXI","DLXXII","DLXXIII","DLXXIV","DLXXV","DLXXVI","DLXXVII","DLXXVIII","DLXXIX","DLXXX",
"DLXXXI","DLXXXII","DLXXXIII","DLXXXIV","DLXXXV","DLXXXVI","DLXXXVII","DLXXXVIII","DLXXXIX","DXC",
"DXCI","DXCII","DXCIII","DXCIV","DXCV","DXCVI","DXCVII","DXCVIII","DXCIX","DC",
"DCI","DCII","DCIII","DCIV","DCV","DCVI","DCVII","DCVIII","DCIX","DCX",
"DCXI","DCXII","DCXIII","DCXIV","DCXV","DCXVI","DCXVII","DCXVIII","DCXIX","DCXX",
"DCXXI","DCXXII","DCXXIII","DCXXIV","DCXXV","DCXXVI","DCXXVII","DCXXVIII","DCXXIX","DCXXX",
"DCXXXI","DCXXXII","DCXXXIII","DCXXXIV","DCXXXV","DCXXXVI","DCXXXVII","DCXXXVIII","DCXXXIX","DCXL",
"DCXLI","DCXLII","DCXLIII","DCXLIV","DCXLV","DCXLVI","DCXLVII","DCXLVIII","DCXLIX","DCL",
"DCLI","DCLII","DCLIII","DCLIV","DCLV","DCLVI","DCLVII","DCLVIII","DCLIX","DCLX",
"DCLXI","DCLXII","DCLXIII","DCLXIV","DCLXV","DCLXVI","DCLXVII","DCLXVIII","DCLXIX","DCLXX",
"DCLXXI","DCLXXII","DCLXXIII","DCLXXIV","DCLXXV","DCLXXVI","DCLXXVII","DCLXXVIII","DCLXXIX","DCLXXX",
"DCLXXXI","DCLXXXII","DCLXXXIII","DCLXXXIV","DCLXXXV","DCLXXXVI","DCLXXXVII","DCLXXXVIII","DCLXXXIX","DCXC",
"DCXCI","DCXCII","DCXCIII","DCXCIV","DCXCV","DCXCVI","DCXCVII","DCXCVIII","DCXCIX","DCC",
"DCCI","DCCII","DCCIII","DCCIV","DCCV","DCCVI","DCCVII","DCCVIII","DCCIX","DCCX",
"DCCXI","DCCXII","DCCXIII","DCCXIV","DCCXV","DCCXVI","DCCXVII","DCCXVIII","DCCXIX","DCCXX",
"DCCXXI","DCCXXII","DCCXXIII","DCCXXIV","DCCXXV","DCCXXVI","DCCXXVII","DCCXXVIII","DCCXXIX","DCCXXX",
"DCCXXXI","DCCXXXII","DCCXXXIII","DCCXXXIV","DCCXXXV","DCCXXXVI","DCCXXXVII","DCCXXXVIII","DCCXXXIX","DCCXL",
"DCCXLI","DCCXLII","DCCXLIII","DCCXLIV","DCCXLV","DCCXLVI","DCCXLVII","DCCXLVIII","DCCXLIX","DCCL",
"DCCLI","DCCLII","DCCLIII","DCCLIV","DCCLV","DCCLVI","DCCLVII","DCCLVIII","DCCLIX","DCCLX",
"DCCLXI","DCCLXII","DCCLXIII","DCCLXIV","DCCLXV","DCCLXVI","DCCLXVII","DCCLXVIII","DCCLXIX","DCCLXX",
"DCCLXXI","DCCLXXII","DCCLXXIII","DCCLXXIV","DCCLXXV","DCCLXXVI","DCCLXXVII","DCCLXXVIII","DCCLXXIX","DCCLXXX",
"DCCLXXXI","DCCLXXXII","DCCLXXXIII","DCCLXXXIV","DCCLXXXV","DCCLXXXVI","DCCLXXXVII","DCCLXXXVIII","DCCLXXXIX","DCCXC",
"DCCXCI","DCCXCII","DCCXCIII","DCCXCIV","DCCXCV","DCCXCVI","DCCXCVII","DCCXCVIII","DCCXCIX","DCCC",
"DCCCI","DCCCII","DCCCIII","DCCCIV","DCCCV","DCCCVI","DCCCVII","DCCCVIII","DCCCIX","DCCCX",
"DCCCXI","DCCCXII","DCCCXIII","DCCCXIV","DCCCXV","DCCCXVI","DCCCXVII","DCCCXVIII","DCCCXIX","DCCCXX",
"DCCCXXI","DCCCXXII","DCCCXXIII","DCCCXXIV","DCCCXXV","DCCCXXVI","DCCCXXVII","DCCCXXVIII","DCCCXXIX","DCCCXXX",
"DCCCXXXI","DCCCXXXII","DCCCXXXIII","DCCCXXXIV","DCCCXXXV","DCCCXXXVI","DCCCXXXVII","DCCCXXXVIII","DCCCXXXIX","DCCCXL",
"DCCCXLI","DCCCXLII","DCCCXLIII","DCCCXLIV","DCCCXLV","DCCCXLVI","DCCCXLVII","DCCCXLVIII","DCCCXLIX","DCCCL",
"DCCCLI","DCCCLII","DCCCLIII","DCCCLIV","DCCCLV","DCCCLVI","DCCCLVII","DCCCLVIII","DCCCLIX","DCCCLX",
"DCCCLXI","DCCCLXII","DCCCLXIII","DCCCLXIV","DCCCLXV","DCCCLXVI","DCCCLXVII","DCCCLXVIII","DCCCLXIX","DCCCLXX",
"DCCCLXXI","DCCCLXXII","DCCCLXXIII","DCCCLXXIV","DCCCLXXV","DCCCLXXVI","DCCCLXXVII","DCCCLXXVIII","DCCCLXXIX","DCCCLXXX",
"DCCCLXXXI","DCCCLXXXII","DCCCLXXXIII","DCCCLXXXIV","DCCCLXXXV","DCCCLXXXVI","DCCCLXXXVII","DCCCLXXXVIII","DCCCLXXXIX","DCCCXC",
"DCCCXCI","DCCCXCII","DCCCXCIII","DCCCXCIV","DCCCXCV","DCCCXCVI","DCCCXCVII","DCCCXCVIII","DCCCXCIX","CM",
"CMI","CMII","CMIII","CMIV","CMV","CMVI","CMVII","CMVIII","CMIX","CMX",
"CMXI","CMXII","CMXIII","CMXIV","CMXV","CMXVI","CMXVII","CMXVIII","CMXIX","CMXX",
"CMXXI","CMXXII","CMXXIII","CMXXIV","CMXXV","CMXXVI","CMXXVII","CMXXVIII","CMXXIX","CMXXX",
"CMXXXI","CMXXXII","CMXXXIII","CMXXXIV","CMXXXV","CMXXXVI","CMXXXVII","CMXXXVIII","CMXXXIX","CMXL",
"CMXLI","CMXLII","CMXLIII","CMXLIV","CMXLV","CMXLVI","CMXLVII","CMXLVIII","CMXLIX","CML",
"CMLI","CMLII","CMLIII","CMLIV","CMLV","CMLVI","CMLVII","CMLVIII","CMLIX","CMLX",
"CMLXI","CMLXII","CMLXIII","CMLXIV","CMLXV","CMLXVI","CMLXVII","CMLXVIII","CMLXIX","CMLXX",
"CMLXXI","CMLXXII","CMLXXIII","CMLXXIV","CMLXXV","CMLXXVI","CMLXXVII","CMLXXVIII","CMLXXIX","CMLXXX",
"CMLXXXI","CMLXXXII","CMLXXXIII","CMLXXXIV","CMLXXXV","CMLXXXVI","CMLXXXVII","CMLXXXVIII","CMLXXXIX","CMXC",
"CMXCI","CMXCII","CMXCIII","CMXCIV","CMXCV","CMXCVI","CMXCVII","CMXCVIII","CMXCIX","M"
};
        #endregion
    }
}
