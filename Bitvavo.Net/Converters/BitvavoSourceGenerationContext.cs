using Bitvavo.Net.Objects.Models;
using System;
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
