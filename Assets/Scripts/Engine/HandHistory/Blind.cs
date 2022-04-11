// Decompiled with JetBrains decompiler
// Type: PokerHandHistory.Blind
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
  [DesignerCategory("code")]
  [GeneratedCode("xsd", "2.0.50727.42")]
  [Serializable]
  public class Blind
  {
    private string playerField;
    private BlindType typeField;
    private Decimal amountField;
    private bool allInField;

    public Blind()
    {
      this.typeField = BlindType.None;
      this.amountField = 0.00M;
      this.allInField = false;
    }

    [XmlAttribute]
    public string Player
    {
      get => this.playerField;
      set => this.playerField = value;
    }

    [DefaultValue(BlindType.None)]
    [XmlAttribute]
    public BlindType Type
    {
      get => this.typeField;
      set => this.typeField = value;
    }

    [XmlAttribute]
    [DefaultValue(typeof (Decimal), "0.00")]
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
