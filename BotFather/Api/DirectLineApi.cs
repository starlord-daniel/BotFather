using BotFather.Model;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BotFather.Api
{
    public static class DirectLineApi
    {
        private const string DIRECT_LINE_URL = "https://directline.botframework.com/v3/directline/conversations";
        private const string DIRECT_LINE_SEND_ACTIVITY_URL = "https://directline.botframework.com/v3/directline/conversations/{conversationId}/activities";

        private static string _token = "";
        private static string _conversationId = "";

        public static async Task StartConversation(string key)
        {
            //POST https://directline.botframework.com/v3/directline/conversations
            //Authorization: Bearer SECRET_OR_TOKEN

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {key}");

            HttpResponseMessage response = await client.PostAsync(DIRECT_LINE_URL, null);

            string result = await response.Content.ReadAsStringAsync();

            var conversationInfo = JsonConvert.DeserializeObject<ConversationInfo>(result);

            _token = conversationInfo.token;
            _conversationId = conversationInfo.conversationId;
        }

        public static async Task SendActivityToBot(Activity message)
        {
            //POST https://directline.botframework.com/v3/directline/conversations/{conversationId}/activities
            //Authorization: Bearer SECRET
            //Content - Type: application / json

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");

            var url = DIRECT_LINE_SEND_ACTIVITY_URL.Replace("{conversationId}", _conversationId);

            HttpContent content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);

            string result = await response.Content.ReadAsStringAsync();
        }

        public static async Task<Activity> ReceiveActivity()
        {
            //GET https://directline.botframework.com/v3/directline/conversations/abc123/activities?watermark=0001a-94
            //Authorization: Bearer SECRET

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");

            var url = DIRECT_LINE_SEND_ACTIVITY_URL.Replace("{conversationId}", _conversationId);

            HttpResponseMessage response = await client.GetAsync(url);

            string result = await response.Content.ReadAsStringAsync();

            ActivityList activities = JsonConvert.DeserializeObject<ActivityList>(result);

            return activities.activities.Last();
        }
    }
}
