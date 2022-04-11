// Decompiled with JetBrains decompiler
// Type: PokerHandHistory.PokerHand
// Assembly: HandHistory, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 438952A3-4B0D-46BF-9953-3F34AC441EEE
// Assembly location: C:\Users\rdp\Documents\HandHistory.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace PokerHandHistory
{
  [DesignerCategory("code")]
  [DebuggerStepThrough]
  [GeneratedCode("xsd", "2.0.50727.42")]
  [Serializable]
  public class PokerHand
  {
    private Blind[] blindsField;
    private Card[] holeCardsField;
    private Round[] roundsField;
    private Context contextField;
    private HandResult[] resultsField;
    private Player[] playersField;
    private Decimal rakeField;
    private string heroField;

    public PokerHand() => this.contextField = new Context();

    [XmlIgnore]
    public Round Preflop
    {
      get => this.roundsField[0];
      set => this.roundsField[0] = value;
    }

    [XmlIgnore]
    public Round Flop
    {
      get => this.roundsField[1];
      set => this.roundsField[1] = value;
    }

    [XmlIgnore]
    public Round Turn
    {
      get => this.roundsField[2];
      set => this.roundsField[2] = value;
    }

    [XmlIgnore]
    public Round River
    {
      get => this.roundsField[3];
      set => this.roundsField[3] = value;
    }

    public HandResult PlayerResult(string name)
    {
      foreach (HandResult handResult in this.resultsField)
      {
        if (handResult.Player == name)
          return handResult;
      }
      return (HandResult) null;
    }

    [XmlElement("Blinds")]
    public Blind[] Blinds
    {
      get => this.blindsField;
      set => this.blindsField = value;
    }

    [XmlElement("HoleCards")]
    public Card[] HoleCards
    {
      get => this.holeCardsField;
      set => this.holeCardsField = value;
    }

    [XmlElement("Rounds")]
    public Round[] Rounds
    {
      get => this.roundsField;
      set => this.roundsField = value;
    }

    public Context Context
    {
      get => this.contextField;
      set => this.contextField = value;
    }

    [XmlElement("Results")]
    public HandResult[] Results
    {
      get => this.resultsField;
      set => this.resultsField = value;
    }

    [XmlElement("Players")]
    public Player[] Players
    {
      get => this.playersField;
      set => this.playersField = value;
    }

    public Decimal Rake
    {
      get => this.rakeField;
      set => this.rakeField = value;
    }

    public string Hero
    {
      get => this.heroField;
      set => this.heroField = value;
    }
  }
}
