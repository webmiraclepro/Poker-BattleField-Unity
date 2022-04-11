// Decompiled with JetBrains decompiler
// Type: PokerHandHistory.PokerHandXML
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
  [XmlRoot(IsNullable = false, Namespace = "")]
  [DebuggerStepThrough]
  [GeneratedCode("xsd", "2.0.50727.42")]
  [DesignerCategory("code")]
  [Serializable]
  public class PokerHandXML
  {
    private PokerHand[] handsField;

    public PokerHand this[int index]
    {
      get => this.handsField[index];
      set => this.handsField[index] = value;
    }

    [XmlElement("Hands")]
    public PokerHand[] Hands
    {
      get => this.handsField;
      set => this.handsField = value;
    }
  }
}
