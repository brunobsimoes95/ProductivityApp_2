using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

//Classe que interage com a API da OpenAI
public class CoachAI
{
    private readonly HttpClient _http;

    public CoachAI(HttpClient http)
    {
        _http = http;
    }

    public async Task<string> GerarSugestaoAsync(double mediaSono, double mediaStress, double mediaProd, List<string> emocoes, string apiKey)
    {
        string prompt = $@"
Estes são os dados da semana de um utilizador:
- Sono médio: {mediaSono:F1}h
- Produtividade média: {mediaProd:F1}%
- Stress médio: {mediaStress:F1}%
- Emoções mais frequentes: {string.Join(", ", emocoes)}

Gera uma sugestão curta, empática e útil, como se fosses um coach emocional digital, em português de portugal.
";

        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "user", content = prompt }
            },
            temperature = 0.7
        };

        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var response = await _http.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestBody);

        if (!response.IsSuccessStatusCode)
        {
            var erro = await response.Content.ReadAsStringAsync();
            Console.WriteLine("❌ Erro da OpenAI: " + erro);
            return $"❌ Erro da OpenAI: {erro}";
        }

        var raw = await response.Content.ReadAsStringAsync();
        Console.WriteLine("🔍 Resposta da OpenAI: " + raw);


        try
        {
            var result = JsonSerializer.Deserialize<ChatGPTResponse>(raw);
            return result?.choices?[0]?.message?.content?.Trim() ?? "❌ Resposta vazia da AI.";
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Erro ao interpretar JSON: " + ex.Message);
            return "❌ Erro ao interpretar a resposta da AI.";
        }

    }

    public class ChatGPTResponse
    {
        public List<Choice>? choices { get; set; }

        public class Choice
        {
            public Message? message { get; set; }
        }

        public class Message
        {
            public string? role { get; set; }
            public string? content { get; set; }
        }
    }
}
