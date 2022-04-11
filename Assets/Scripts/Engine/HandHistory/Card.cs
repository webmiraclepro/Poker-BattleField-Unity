// Decompiled with JetBrains decompiler
// Type: PokerHandHistory.Card
// Assembly: HandHistory, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 438952A3-4B0D-46BF-9953-3F34AC441EEE
// Assembly location: C:\Users\rdp\Documents\HandHistory.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;

namespace PokerHandHistory
{
  [DesignerCategory("code")]
  [GeneratedCode("xsd", "2.0.50727.42")]
  [DebuggerStepThrough]
  [Serializable]
  public class Card
  {
    private Rank rankField;
    private Suit suitField;

    public Card()
    {
      this.rankField = Rank.None;
      this.suitField = Suit.None;
    }

    public Card(string card)
    {
      switch (card[0])
      {
        case '2':
          this.rankField = Rank.Two;
          break;
        case '3':
          this.rankField = Rank.Three;
          break;
        case '4':
          this.rankField = Rank.Four;
          break;
        case '5':
          this.rankField = Rank.Five;
          break;
        case '6':
          this.rankField = Rank.Six;
          break;
        case '7':
          this.rankField = Rank.Seven;
          break;
        case '8':
          this.rankField = Rank.Eight;
          break;
        case '9':
          this.rankField = Rank.Nine;
          break;
        case 'A':
          this.rankField = Rank.Ace;
          break;
        case 'J':
          this.rankField = Rank.Jack;
          break;
        case 'K':
          this.rankField = Rank.King;
          break;
        case 'Q':
          this.rankField = Rank.Queen;
          break;
        case 'T':
          this.rankField = Rank.Ten;
          break;
        case 'W':
          this.rankField = Rank.Joker;
          break;
        default:
          this.rankField = Rank.None;
          break;
      }
      switch (card[1])
      {
        case 'c':
          this.suitField = Suit.Clubs;
          break;
        case 'd':
          this.suitField = Suit.Diamonds;
          break;
        case 'h':
          this.suitField = Suit.Hearts;
          break;
        case 's':
          this.suitField = Suit.Spades;
          break;
        default:
          this.suitField = Suit.None;
          break;
      }
    }

    public static Rank GetRank(char rank)
    {
      switch (rank)
      {
        case '2':
          return Rank.Two;
        case '3':
          return Rank.Three;
        case '4':
          return Rank.Four;
        case '5':
          return Rank.Five;
        case '6':
          return Rank.Six;
        case '7':
          return Rank.Seven;
        case '8':
          return Rank.Eight;
        case '9':
          return Rank.Nine;
        case 'A':
          return Rank.Ace;
        case 'J':
          return Rank.Jack;
        case 'K':
          return Rank.King;
        case 'Q':
          return Rank.Queen;
        case 'T':
          return Rank.Ten;
        case 'W':
          return Rank.Joker;
        default:
          return Rank.None;
      }
    }

    public static Suit GetSuit(char suit)
    {
      switch (suit)
      {
        case 'c':
          return Suit.Clubs;
        case 'd':
          return Suit.Diamonds;
        case 'h':
          return Suit.Hearts;
        case 's':
          return Suit.Spades;
        default:
          return Suit.None;
      }
    }

    public static string RankString(Rank rank)
    {
      switch (rank)
      {
        case Rank.None:
          return "X";
        case Rank.Two:
          return "2";
        case Rank.Three:
          return "3";
        case Rank.Four:
          return "4";
        case Rank.Five:
          return "5";
        case Rank.Six:
          return "6";
        case Rank.Seven:
          return "7";
        case Rank.Eight:
          return "8";
        case Rank.Nine:
          return "9";
        case Rank.Ten:
          return "T";
        case Rank.Jack:
          return "J";
        case Rank.Queen:
          return "Q";
        case Rank.King:
          return "K";
        case Rank.Ace:
          return "A";
        case Rank.Joker:
          return "W";
        default:
          return "";
      }
    }

    public static string SuitString(Suit suit)
    {
      switch (suit)
      {
        case Suit.None:
          return "x";
        case Suit.Clubs:
          return "c";
        case Suit.Diamonds:
          return "d";
        case Suit.Hearts:
          return "h";
        case Suit.Spades:
          return "s";
        default:
          return "";
      }
    }

    public static string ToString(Card[] cards)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < cards.Length; ++index)
      {
        stringBuilder.Append(cards[index].ToString());
        if (index < cards.Length - 1)
          stringBuilder.Append(" ");
      }
      return stringBuilder.ToString();
    }

    public override string ToString() => Card.RankString(this.rankField) + Card.SuitString(this.suitField);

    public override int GetHashCode() => base.GetHashCode();

    public override bool Equals(object obj)
    {
      if ((object) (obj as Card) != null)
        return this.rankField == ((Card) obj).rankField && this.suitField == ((Card) obj).suitField;
      switch (obj)
      {
        case string _:
          return (string) obj == this.ToString();
        default:
          return base.Equals(obj);
      }
    }

    public static bool operator ==(Card c, string s) => c.Equals((object) s);

    public static bool operator !=(Card c, string s) => !c.Equals((object) s);

    public static bool operator ==(Card c1, Card c2) => c1.Equals((object) c2);

    public static bool operator !=(Card c1, Card c2) => !c1.Equals((object) c2);

    [XmlAttribute]
    public Rank Rank
    {
      get => this.rankField;
      set => this.rankField = value;
    }

    [XmlAttribute]
    public Suit Suit
    {
      get => this.suitField;
      set => this.suitField = value;
    }
  }
}
