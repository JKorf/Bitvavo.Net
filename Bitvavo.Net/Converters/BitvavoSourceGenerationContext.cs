using Bitvavo.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Converters
{
    [JsonSerializable(typeof(BitvavoTrade[]))]
    [JsonSerializable(typeof(BitvavoOrderBook))]
    [JsonSerializable(typeof(BitvavoOrderBookReport))]
    [JsonSerializable(typeof(BitvavoTradeReport[]))]
    [JsonSerializable(typeof(BitvavoKline[]))]
    [JsonSerializable(typeof(BitvavoTickerPrice))]
    [JsonSerializable(typeof(BitvavoTickerPrice[]))]
    [JsonSerializable(typeof(BitvavoBookTicker))]
    [JsonSerializable(typeof(BitvavoBookTicker[]))]
    [JsonSerializable(typeof(BitvavoTicker))]
    [JsonSerializable(typeof(BitvavoTicker[]))]

    [JsonSerializable(typeof(BitvavoAccountFeesResponse))]
    [JsonSerializable(typeof(BitvavoBalance[]))]
    [JsonSerializable(typeof(BitvavoStakingBalance[]))]
    [JsonSerializable(typeof(BitvavoSymbolFees))]
    [JsonSerializable(typeof(BitvavoTransactionHistory))]
    [JsonSerializable(typeof(BitvavoServerTime))]
    [JsonSerializable(typeof(IDictionary<string, object>))]
    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(int?))]
    [JsonSerializable(typeof(int))]
    [JsonSerializable(typeof(long?))]
    [JsonSerializable(typeof(long))]
    [JsonSerializable(typeof(decimal))]
    [JsonSerializable(typeof(decimal?))]
    [JsonSerializable(typeof(DateTime))]
    [JsonSerializable(typeof(DateTime?))]
    internal partial class BitvavoSourceGenerationContext : JsonSerializerContext
    {
    }
}
