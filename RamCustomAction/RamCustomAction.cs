using AdaptiveExpressions.Properties;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using System;
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

        [JsonProperty("token")]
        public StringExpression Token { get; set; }

        [JsonProperty("username")]
        public BoolExpression Username { get; set; }

        [JsonProperty("resultProperty")]
        public StringExpression ResultProperty { get; set; }

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var token = Token.GetValue(dc.State);

            var result = token;

            if (ResultProperty != null)
            {
                dc.State.SetValue(this.ResultProperty.GetValue(dc.State), result);
            }

            await dc.Context.SendActivityAsync($"response : {result}");

            return await dc.EndDialogAsync(result: result, cancellationToken: cancellationToken);
        }
    }
}
