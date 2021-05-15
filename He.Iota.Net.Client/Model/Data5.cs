namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data5
    {
        /// <summary>The identifier of the message.</summary>
        [JsonProperty("messageId", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string MessageId { get; set; }

        /// <summary>The identifiers of the messages this message references.</summary>
        [JsonProperty("parentMessageIds", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<string> ParentMessageIds { get; set; } = new System.Collections.ObjectModel.Collection<string>();

        /// <summary>Tells if the message could get solidified by the node or not.</summary>
        [JsonProperty("isSolid", Required = Required.Always)]
        public bool IsSolid { get; set; }

        /// <summary>Tells which milestone references this message. If `null` the message was not referenced by a milestone yet.</summary>
        [JsonProperty("referencedByMilestoneIndex", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? ReferencedByMilestoneIndex { get; set; }

        /// <summary>If set, this message can be considered as a valid milestone message. This field therefore describes the milestone index of the involved milestone. A message can be considered as a valid milestone message if the milestone payload is valid and if the referenced parents in the milestone payload do match the referenced parents in the message itself. Note it's possible to have different milestone messages that all represent the same milestone.</summary>
        [JsonProperty("milestoneIndex", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int MilestoneIndex { get; set; }

        /// <summary>If `included`, the message contains a transaction that has been included in the ledger. If `conflicitng`, the message contains a transaction that has not been included in the ledger because it conflicts with another transaction. If the message does not contain a transaction, `ledgerInclusionState` is set to `noTransaction`.</summary>
        [JsonProperty("ledgerInclusionState", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public Data5LedgerInclusionState LedgerInclusionState { get; set; }

        /// <summary>Defines the reason why a message is marked as conflicting. Value `1` denotes that the referenced UTXO was already spent. Value `2`denotes that the referenced UTXO was already spent while confirming this milestone. Value `3` denotes that the referenced UTXO cannot be found. Value `4` denotes that the sum of the inputs and output values does not match. Value `5` denotes that the unlock block signature is invalid. Value `6` denotes that the input or output type used is unsupported. Value `7` denotes that the used address type is unsupported. Value `8` denotes that the dust allowance for the address is invalid. Value `9` denotes that the semantic validation failed.</summary>
        [JsonProperty("conflictReason", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int ConflictReason { get; set; }

        /// <summary>Tells if the message should be promoted to get more likely picked up by the Coordinator.</summary>
        [JsonProperty("shouldPromote", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool ShouldPromote { get; set; }

        /// <summary>Tells if the message should be reattached.</summary>
        [JsonProperty("shouldReattach", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool ShouldReattach { get; set; }

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
