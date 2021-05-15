namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    /// <summary>A message is the object nodes gossip around in the network. It always references two other messages that are known as parents. It is stored as a vertex on the tangle data structure that the nodes maintain. A message can have a maximum size of 32Kb.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Message
    {
        /// <summary>Network identifier. This field signifies for which network the message is meant for. It also tells which protocol rules apply to the message. It is computed out of the first 8 bytes of the `BLAKE2b-256` hash of the concatenation of the network type and protocol version string.</summary>
        [JsonProperty("networkId", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string NetworkId { get; set; }

        /// <summary>The identifiers of the messages this message references.</summary>
        [JsonProperty("parentMessageIds", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<string> ParentMessageIds { get; set; } = new System.Collections.ObjectModel.Collection<string>();

        [JsonProperty("payload", Required = Required.Always)]
        public TransactionPayload Payload { get; set; }

        /// <summary>The nonce which lets this message fulfill the Proof-of-Work requirement.</summary>
        [JsonProperty("nonce", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Nonce { get; set; }

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
