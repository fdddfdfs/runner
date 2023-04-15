using System;
using Newtonsoft.Json;

[Serializable]
public sealed class InGameTextLocalization
{
    [JsonProperty("language")]
    public string Language { get; set; }
    
    [JsonProperty("texts")]
    public string[] Texts { get; set; }
}