// Decompiled with JetBrains decompiler
// Type: PokerHandHistory.Context
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
  public class Context
  {
    private bool onlineField;
    private string siteField;
    private string idField;
    private string tableField;
    private DateTime timeStampField;
    private GameFormat formatField;
    private int buttonField;
    private Decimal bigBlindField;
    private Decimal smallBlindField;
    private Decimal anteField;
    private BettingType bettingTypeField;
    private bool cappedField;
    private Decimal capAmountField;
    private bool capAmountFieldSpecified;
    private PokerVariant pokerVariantField;
    private string currency;

    public Context()
    {
      this.onlineField = true;
      this.anteField = 0.00M;
      this.cappedField = false;
      this.currency = "USD";
    }

    [XmlAttribute]
    [DefaultValue(true)]
    public bool Online
    {
      get => this.onlineField;
      set => this.onlineField = value;
    }

    [XmlAttribute]
    public string Site
    {
      get => this.siteField;
      set => this.siteField = value;
    }

    [XmlAttribute]
    public string Currency
    {
      get => this.currency;
      set => this.currency = value;
    }

    [XmlAttribute(DataType = "ID")]
    public string ID
    {
      get => this.idField;
      set => this.idField = value;
    }

    [XmlAttribute]
    public string Table
    {
      get => this.tableField;
      set => this.tableField = value;
    }

    [XmlAttribute]
    public DateTime TimeStamp
    {
      get => this.timeStampField;
      set => this.timeStampField = value;
    }

    [XmlAttribute]
    public GameFormat Format
    {
      get => this.formatField;
      set => this.formatField = value;
    }

    [XmlAttribute]
    public int Button
    {
      get => this.buttonField;
      set => this.buttonField = value;
    }

    [XmlAttribute]
    public Decimal BigBlind
    {
      get => this.bigBlindField;
      set => this.bigBlindField = value;
    }

    [XmlAttribute]
    public Decimal SmallBlind
    {
      get => this.smallBlindField;
      set => this.smallBlindField = value;
    }

    [XmlAttribute]
    [DefaultValue(typeof (Decimal), "0.00")]
    public Decimal Ante
    {
      get => this.anteField;
      set => this.anteField = value;
    }

    [XmlAttribute]
    public BettingType BettingType
    {
      get => this.bettingTypeField;
      set => this.bettingTypeField = value;
    }

    [XmlAttribute]
    [DefaultValue(false)]
    public bool Capped
    {
      get => this.cappedField;
      set => this.cappedField = value;
    }

    [XmlAttribute]
    public Decimal CapAmount
    {
      get => this.capAmountField;
      set => this.capAmountField = value;
    }

    [XmlIgnore]
    public bool CapAmountSpecified
    {
      get => this.capAmountFieldSpecified;
      set => this.capAmountFieldSpecified = value;
    }

    [XmlAttribute]
    public PokerVariant PokerVariant
    {
      get => this.pokerVariantField;
      set => this.pokerVariantField = value;
    }
  }
}
