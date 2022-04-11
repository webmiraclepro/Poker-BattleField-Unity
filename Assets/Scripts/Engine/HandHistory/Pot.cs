// Decompiled with JetBrains decompiler
// Type: PokerHandHistory.Pot
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
  public class Pot
  {
    private Decimal amountField;
    private int numberField;

    public Pot()
    {
      this.amountField = 0.00M;
      this.numberField = 0;
    }

    [DefaultValue(typeof (Decimal), "0.00")]
    [XmlAttribute]
    public Decimal Amount
    {
      get => this.amountField;
      set => this.amountField = value;
    }

    [XmlAttribute]
    [DefaultValue(0)]
    public int Number
    {
      get => this.numberField;
      set => this.numberField = value;
    }
  }
}
