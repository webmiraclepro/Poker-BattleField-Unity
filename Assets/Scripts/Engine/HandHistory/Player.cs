// Decompiled with JetBrains decompiler
// Type: PokerHandHistory.Player
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
  public class Player
  {
    private string nameField;
    private Decimal stackField;
    private int seatField;

    [XmlAttribute]
    public string Name
    {
      get => this.nameField;
      set => this.nameField = value;
    }

    [XmlAttribute]
    public Decimal Stack
    {
      get => this.stackField;
      set => this.stackField = value;
    }

    [XmlAttribute]
    public int Seat
    {
      get => this.seatField;
      set => this.seatField = value;
    }
  }
}
