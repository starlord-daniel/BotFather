using Bot.Unification.Api;
using Bot.Unification.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Bot.Unification.Main
{
    public static class BotFather
    {
        private static string _botDataUrl = null;
        private static BotData _botData = null;
        private static string _currentBot = null;

        public static async Task StartMultiBotConversation(ITurnContext context)
        {
            if (context.Activity.Type is ActivityTypes.Message)
            {
                var userInput = context.Activity.Text;

                if (String.IsNullOrEmpty(_currentBot))
                {
                    if(_botData.directLine.Where(x => x.name.ToLower() == userInput.ToLower())?.FirstOrDefault() != null)
                    {
                        await StartNewBotConversationAsync(context);
                    }
                    else
                    {
                        await context.SendActivity(ShowInitialMessage(context));
                    }
                }
                else
                {
                    // Put in an exit statement
                    if (context.Activity.Text == "exit")
                    {
                        await ExitCurrentBotAsync(context);
                    }
                    else if(_botData.directLine.Where(x => x.name.ToLower() == userInput.ToLower())?.FirstOrDefault() != null)
                    {
                        await StartNewBotConversationAsync(context);
                    }
                    else
                    {
                        await ForwardConversationToBotAsync(context);
                    }
                }
            }
        }

        private static async Task ForwardConversationToBotAsync(ITurnContext context)
        {
            await DirectLineApi.SendActivityToBot(context.Activity);
            var answer = await DirectLineApi.ReceiveActivity();
            await context.SendActivity(answer);
        }

        private static async Task StartNewBotConversationAsync(ITurnContext context)
        {
            _currentBot = context.Activity.Text.ToLower();

            await DirectLineApi.StartConversation(_botData.directLine.Where(x => x.name.ToLower() == _currentBot).Select(x => x.secret).First());
            await context.SendActivity($"You are now connected to {_currentBot}!");
        }

        public static Task PopulateBotData(string botDataUrl)
        {
            _botDataUrl = botDataUrl;

            string json = new WebClient().DownloadString(new Uri(_botDataUrl));
            BotData botData = JsonConvert.DeserializeObject<BotData>(json);
            _botData = botData;

            return Task.CompletedTask;
        }

        public static async Task ExitCurrentBotAsync(ITurnContext context)
        {
            _currentBot = null;
            await context.SendActivity(ShowInitialMessage(context));
        }

        private static Activity ShowInitialMessage(ITurnContext context, 
            string greeting = "Hello, I'm the Bot father. Tell me, who do you want to talk to?",
            string heroCardTitle = "Menu")
        {
            Activity activity = context.Activity;
            activity.Text = "";
            activity.Attachments = new List<Attachment>();

            List<CardAction> buttons = new List<CardAction>();
            foreach (var name in _botData.directLine.Select(x => x.name))
            {
                buttons.Add(new CardAction
                {
                    Title = name,
                    DisplayText = name,
                    Type = ActionTypes.ImBack,
                    Text = name,
                    Value = name
                });
            }

            HeroCard cardMessage = new HeroCard()
            {
                Text = greeting,
                Title = heroCardTitle,
                Buttons = buttons
            };

            activity.Attachments.Add(cardMessage.ToAttachment());
            return activity;
        }
    }
}
