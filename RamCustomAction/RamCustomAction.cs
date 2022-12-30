using AdaptiveExpressions.Properties;
using Azure.Identity;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using RamCustomAction.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RamCustomAction
{
    //Ram Graph Custom action
    public class RamCustomAction : Dialog
    {
        public RamCustomAction([CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) : base()
        {
            RegisterSourceLocation(sourceFilePath, sourceLineNumber);
        }

        [JsonProperty("$Kind")]
        public const string Kind = "RamCustomAction";

        [JsonProperty("fromId")]
        public StringExpression FromId { get; set; }

        [JsonProperty("trackUserName")]
        public BoolExpression TrackUserName { get; set; } = false;

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var fromId = FromId.GetValue(dc.State);
            var graphClient = new GraphClient();
            var me = await graphClient.GetMeAsync(fromId);

            var properties = new Dictionary<string, string>();

            var trackUsername = TrackUserName.GetValue(dc);
            if(trackUsername && !string.IsNullOrWhiteSpace(me.DisplayName))
            {
                properties.Add(TelemetryConstants.FromNameProperty, me.DisplayName);
            }

            TelemetryClient.TrackEvent(TelemetryLoggerConstants.BotMsgSendEvent, properties);

            return await dc.EndDialogAsync(result: me, cancellationToken: cancellationToken);
        }
    }
}
