// Decompiled with JetBrains decompiler
// Type: PokerHandHistory.Round
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
  public class Round
  {
    private Card[] communityCardsField;
    private Action[] actionsField;

    [XmlElement("CommunityCards")]
    public Card[] CommunityCards
    {
      get => this.communityCardsField;
      set => this.communityCardsField = value;
    }

    [XmlElement("Actions")]
    public Action[] Actions
    {
      get => this.actionsField;
      set => this.actionsField = value;
    }
  }
}
