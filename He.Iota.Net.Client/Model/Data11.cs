namespace He.Iota.Net.Client.Model
{
    using Newtonsoft.Json;

    public class Data11
    {
        [JsonProperty("receipts", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<ReceiptTuple> Receipts { get; set; } = new System.Collections.ObjectModel.Collection<ReceiptTuple>();

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
