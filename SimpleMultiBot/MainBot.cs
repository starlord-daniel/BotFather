using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Bot.Samples
{
    public class MainBot : IBot
    {
        public async Task OnTurn(ITurnContext context)
        {
            if (context.Activity.Type is ActivityTypes.Message)
            {
                if (BotFather.CurrentBot)
                {
                    // Put in an exit statement
                    if(context.Activity.Text == "exit")
                    {
                        BotFather.ExitCurrentBot();
                    }
                    else
                    {
                        BotFather.HandleBotMessage();
                    }
                    
                }
                else
                {
                    BotFather.DisplaySelectionMessage();
                }
            }
        }
    }
}
