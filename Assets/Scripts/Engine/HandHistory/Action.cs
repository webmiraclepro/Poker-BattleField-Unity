// Decompiled with JetBrains decompiler
// Type: PokerHandHistory.Action
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
  [DebuggerStepThrough]
  [GeneratedCode("xsd", "2.0.50727.42")]
  [DesignerCategory("code")]
  [Serializable]
  public class Action
  {
    private string playerField;
    private ActionType typeField;
    private Decimal amountField;
    private bool allInField;

    public Action(string player, ActionType type)
    {
      this.playerField = player;
      this.typeField = type;
    }

    public Action(string player, ActionType type, Decimal amount)
    {
      this.playerField = player;
      this.typeField = type;
      this.amountField = amount;
    }

    public Action()
    {
      this.typeField = ActionType.None;
      this.amountField = 0.00M;
      this.allInField = false;
    }

    [XmlAttribute]
    public string Player
    {
      get => this.playerField;
      set => this.playerField = value;
    }

    [XmlAttribute]
    [DefaultValue(ActionType.None)]
    public ActionType Type
    {
      get => this.typeField;
      set => this.typeField = value;
    }

    [DefaultValue(typeof (Decimal), "0.00")]
    [XmlAttribute]
    public Decimal Amount
    {
      get => this.amountField;
      set => this.amountField = value;
    }

    [XmlAttribute]
    [DefaultValue(false)]
    public bool AllIn
    {
      get => this.allInField;
      set => this.allInField = value;
    }
  }
}
