namespace He.Iota.Net.Client.Model
{
    using Newtonsoft.Json;

    /// <summary>Returns the balance of an address.</summary>
    public class BalanceAddressResponse
    {
        [JsonProperty("data", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Data9 Data { get; set; } = new Data9();

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
