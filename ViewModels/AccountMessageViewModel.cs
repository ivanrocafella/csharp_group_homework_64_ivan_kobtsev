using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace csharp_group_homework_64_ivan_kobtsev.ViewModels
{
    public class AccountMessageViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("message_text")]
        public string TextOfMessage { get; set; }
        [JsonPropertyName("date_create")]
        public string DateCreate { get; set; }
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }
    }
}
