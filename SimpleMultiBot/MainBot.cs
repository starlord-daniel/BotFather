using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Unification.Main;

namespace Microsoft.Bot.Samples
{
    public class MainBot : IBot
    {
        public async Task OnTurn(ITurnContext context)
        {
            await BotFather.StartMultiBotConversation(context);
        }
    }
}
