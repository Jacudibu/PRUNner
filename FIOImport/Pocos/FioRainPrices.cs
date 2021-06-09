// ReSharper disable All
using Newtonsoft.Json;

namespace FIOImport.Pocos
{
    public class FioRainPrices
    {
        public string Ticker { get; set; }
        public double? MMBuy { get; set; }
        public double? MMSell { get; set; }

        [JsonProperty("AI1-Average")]
        public double AI1Average { get; set; }

        [JsonProperty("AI1-AskAmt")]
        public int? AI1AskAmt { get; set; }

        [JsonProperty("AI1-AskPrice")]
        public double? AI1AskPrice { get; set; }

        [JsonProperty("AI1-AskAvail")]
        public int AI1AskAvail { get; set; }

        [JsonProperty("AI1-BidAmt")]
        public int? AI1BidAmt { get; set; }

        [JsonProperty("AI1-BidPrice")]
        public double? AI1BidPrice { get; set; }

        [JsonProperty("AI1-BidAvail")]
        public int AI1BidAvail { get; set; }

        [JsonProperty("CI1-Average")]
        public double CI1Average { get; set; }

        [JsonProperty("CI1-AskAmt")]
        public int? CI1AskAmt { get; set; }

        [JsonProperty("CI1-AskPrice")]
        public double? CI1AskPrice { get; set; }

        [JsonProperty("CI1-AskAvail")]
        public int CI1AskAvail { get; set; }

        [JsonProperty("CI1-BidAmt")]
        public int? CI1BidAmt { get; set; }

        [JsonProperty("CI1-BidPrice")]
        public double? CI1BidPrice { get; set; }

        [JsonProperty("CI1-BidAvail")]
        public int CI1BidAvail { get; set; }

        [JsonProperty("NC1-Average")]
        public double NC1Average { get; set; }

        [JsonProperty("NC1-AskAmt")]
        public int? NC1AskAmt { get; set; }

        [JsonProperty("NC1-AskPrice")]
        public double? NC1AskPrice { get; set; }

        [JsonProperty("NC1-AskAvail")]
        public int NC1AskAvail { get; set; }

        [JsonProperty("NC1-BidAmt")]
        public int? NC1BidAmt { get; set; }

        [JsonProperty("NC1-BidPrice")]
        public double? NC1BidPrice { get; set; }

        [JsonProperty("NC1-BidAvail")]
        public int NC1BidAvail { get; set; }

        [JsonProperty("IC1-Average")]
        public double IC1Average { get; set; }

        [JsonProperty("IC1-AskAmt")]
        public int? IC1AskAmt { get; set; }

        [JsonProperty("IC1-AskPrice")]
        public double? IC1AskPrice { get; set; }

        [JsonProperty("IC1-AskAvail")]
        public int IC1AskAvail { get; set; }

        [JsonProperty("IC1-BidAmt")]
        public int? IC1BidAmt { get; set; }

        [JsonProperty("IC1-BidPrice")]
        public double? IC1BidPrice { get; set; }

        [JsonProperty("IC1-BidAvail")]
        public int IC1BidAvail { get; set; }
    }
}