namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Metrics
    {
        /// <summary>The number of received messages that were new for the node.</summary>
        [JsonProperty("newMessages", Required = Required.Always)]
        public int NewMessages { get; set; }

        /// <summary>The number of received messages that already were known to the node.</summary>
        [JsonProperty("knownMessages", Required = Required.Always)]
        public int KnownMessages { get; set; }

        /// <summary>The number of received messages from the peer.</summary>
        [JsonProperty("receivedMessages", Required = Required.Always)]
        public int ReceivedMessages { get; set; }

        /// <summary>The number of received message requests from the peer.</summary>
        [JsonProperty("receivedMessageRequests", Required = Required.Always)]
        public int ReceivedMessageRequests { get; set; }

        /// <summary>The number of received milestone requests from the peer.</summary>
        [JsonProperty("receivedMilestoneRequests", Required = Required.Always)]
        public int ReceivedMilestoneRequests { get; set; }

        /// <summary>The number of received heartbeats from the peer.</summary>
        [JsonProperty("receivedHeartbeats", Required = Required.Always)]
        public int ReceivedHeartbeats { get; set; }

        /// <summary>The number of sent messages to the peer.</summary>
        [JsonProperty("sentMessages", Required = Required.Always)]
        public int SentMessages { get; set; }

        /// <summary>The number of sent message requests to the peer.</summary>
        [JsonProperty("sentMessageRequests", Required = Required.Always)]
        public int SentMessageRequests { get; set; }

        /// <summary>The number of sent milestone requests to the peer.</summary>
        [JsonProperty("sentMilestoneRequests", Required = Required.Always)]
        public int SentMilestoneRequests { get; set; }

        /// <summary>The number of sent heartbeats to the peer.</summary>
        [JsonProperty("sentHeartbeats", Required = Required.Always)]
        public int SentHeartbeats { get; set; }

        /// <summary>The number of dropped packets.</summary>
        [JsonProperty("droppedPackets", Required = Required.Always)]
        public int DroppedPackets { get; set; }

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
