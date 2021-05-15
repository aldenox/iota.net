namespace He.Iota.Net.Client.Model
{
    using Newtonsoft.Json;

    public partial class Data12
    {
        [JsonProperty("milestoneId", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string MilestoneId { get; set; }

        [JsonProperty("amount", Required = Required.Always)]
        public int Amount { get; set; }

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
