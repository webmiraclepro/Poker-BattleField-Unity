// Decompiled with JetBrains decompiler
// Type: PokerHandHistory.HandResult
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
  public class HandResult
  {
    private Card[] holeCardsField;
    private Pot[] wonPotsField;
    private string playerField;

    public HandResult()
    {
    }

    public HandResult(string player) => this.playerField = player;

    [XmlElement("HoleCards")]
    public Card[] HoleCards
    {
      get => this.holeCardsField;
      set => this.holeCardsField = value;
    }

    [XmlElement("WonPots")]
    public Pot[] WonPots
    {
      get => this.wonPotsField;
      set => this.wonPotsField = value;
    }

    [XmlAttribute]
    public string Player
    {
      get => this.playerField;
      set => this.playerField = value;
    }
  }
}
