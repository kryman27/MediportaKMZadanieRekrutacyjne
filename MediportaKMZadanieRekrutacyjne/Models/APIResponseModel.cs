using System.Text.Json.Serialization;

namespace MediportaKMZadanieRekrutacyjne.Models
{
    public class APIResponseModel
    {
        public APIResponseModel()
        {
            
        }

        [JsonPropertyName("items")]
        public List<TagModel>? Items { get; set; }

        [JsonPropertyName("has_more")]
        public bool HasMore {  get; set; }

        [JsonPropertyName("quora_max")]
        public int QuotaMax { get; set; }

        [JsonPropertyName("quota_remaining")]
        public int QuotaRemaining {  get; set; }
    }

    public class TagModel
    {
        public TagModel()
        {
            
        }

        [JsonPropertyName("has_synonyms")]
        public bool HasSynonyms { get; set; }


        [JsonPropertyName("is_moderator_only")]
        public bool IsModeratorOnly { get; set; }


        [JsonPropertyName("is_required")]
        public bool IsRequired { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
