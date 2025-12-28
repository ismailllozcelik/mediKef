using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LisBoxSimulator;

class Program
{
    private static readonly HttpClient httpClient = new HttpClient();
    private const string API_URL = "http://localhost:5000/api/LisBox/receive-results";
    private const string API_KEY = "LISBOX_SECRET_KEY_2024";

    static async Task Main(string[] args)
    {
        Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║         🔬 LisBox Cihaz Simülatörü v1.0                 ║");
        Console.WriteLine("║         Laboratuvar Cihazlarından Veri Gönderimi        ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
        Console.WriteLine();

        // HTTP Client yapılandırması
        httpClient.DefaultRequestHeaders.Add("X-API-Key", API_KEY);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        while (true)
        {
            Console.WriteLine("\n📋 MENÜ:");
            Console.WriteLine("1. Biyokimya Cihazı (Cobas c 311) - Glukoz, BUN, Kreatinin");
            Console.WriteLine("2. Hemogram Cihazı (Sysmex XN-550) - Tam Kan Sayımı");
            Console.WriteLine("3. Hormon Cihazı (Cobas e 411) - TSH, FT3, FT4");
            Console.WriteLine("4. Rastgele Test Sonucu Gönder");
            Console.WriteLine("5. Çıkış");
            Console.Write("\nSeçiminiz (1-5): ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await SendBiochemistryResults();
                    break;
                case "2":
                    await SendHematologyResults();
                    break;
                case "3":
                    await SendHormoneResults();
                    break;
                case "4":
                    await SendRandomResults();
                    break;
                case "5":
                    Console.WriteLine("\n👋 Çıkış yapılıyor...");
                    return;
                default:
                    Console.WriteLine("\n❌ Geçersiz seçim!");
                    break;
            }
        }
    }

    static async Task SendBiochemistryResults()
    {
        Console.Write("\n📊 Numune Barkodu (örn: BAR2024001): ");
        var barcode = Console.ReadLine() ?? "BAR2024001";

        var result = new
        {
            DeviceId = "COBAS_C311_01",
            SampleBarcode = barcode,
            TestResults = new[]
            {
                new
                {
                    TestCode = "GLU",
                    TestName = "Glukoz",
                    ResultValue = "95",
                    ResultNumeric = 95.0m,
                    Unit = "mg/dL",
                    ReferenceRange = "70-110",
                    Flag = "N",
                    ResultDateTime = DateTime.Now
                },
                new
                {
                    TestCode = "BUN",
                    TestName = "Üre",
                    ResultValue = "28",
                    ResultNumeric = 28.0m,
                    Unit = "mg/dL",
                    ReferenceRange = "10-50",
                    Flag = "N",
                    ResultDateTime = DateTime.Now
                },
                new
                {
                    TestCode = "CREA",
                    TestName = "Kreatinin",
                    ResultValue = "0.9",
                    ResultNumeric = 0.9m,
                    Unit = "mg/dL",
                    ReferenceRange = "0.6-1.2",
                    Flag = "N",
                    ResultDateTime = DateTime.Now
                }
            },
            Status = "Final",
            Timestamp = DateTime.Now
        };

        await SendToApi(result, "Biyokimya");
    }

    static async Task SendHematologyResults()
    {
        Console.Write("\n🩸 Numune Barkodu (örn: BAR2024002): ");
        var barcode = Console.ReadLine() ?? "BAR2024002";

        var result = new
        {
            DeviceId = "SYSMEX_XN550_01",
            SampleBarcode = barcode,
            TestResults = new[]
            {
                new
                {
                    TestCode = "WBC",
                    TestName = "Lökosit",
                    ResultValue = "7.5",
                    ResultNumeric = 7.5m,
                    Unit = "10^3/µL",
                    ReferenceRange = "4.0-10.0",
                    Flag = "N",
                    ResultDateTime = DateTime.Now
                },
                new
                {
                    TestCode = "RBC",
                    TestName = "Eritrosit",
                    ResultValue = "4.8",
                    ResultNumeric = 4.8m,
                    Unit = "10^6/µL",
                    ReferenceRange = "4.5-5.5",
                    Flag = "N",
                    ResultDateTime = DateTime.Now
                },
                new
                {
                    TestCode = "HGB",
                    TestName = "Hemoglobin",
                    ResultValue = "14.2",
                    ResultNumeric = 14.2m,
                    Unit = "g/dL",
                    ReferenceRange = "13.0-17.0",
                    Flag = "N",
                    ResultDateTime = DateTime.Now
                }
            },
            Status = "Final",
            Timestamp = DateTime.Now
        };

        await SendToApi(result, "Hemogram");
    }

    static async Task SendHormoneResults()
    {
        Console.Write("\n💉 Numune Barkodu (örn: BAR2024003): ");
        var barcode = Console.ReadLine() ?? "BAR2024003";

        var result = new
        {
            DeviceId = "COBAS_E411_01",
            SampleBarcode = barcode,
            TestResults = new[]
            {
                new
                {
                    TestCode = "TSH",
                    TestName = "TSH",
                    ResultValue = "2.1",
                    ResultNumeric = 2.1m,
                    Unit = "mIU/L",
                    ReferenceRange = "0.4-4.0",
                    Flag = "N",
                    ResultDateTime = DateTime.Now
                }
            },
            Status = "Final",
            Timestamp = DateTime.Now
        };

        await SendToApi(result, "Hormon");
    }

    static async Task SendRandomResults()
    {
        Console.Write("\n🎲 Numune Barkodu: ");
        var barcode = Console.ReadLine() ?? $"BAR{DateTime.Now:yyyyMMddHHmmss}";

        var random = new Random();
        var testCodes = new[] { "GLU", "BUN", "CREA", "WBC", "RBC", "HGB" };
        var selectedTest = testCodes[random.Next(testCodes.Length)];

        var result = new
        {
            DeviceId = "SIMULATOR_01",
            SampleBarcode = barcode,
            TestResults = new[]
            {
                new
                {
                    TestCode = selectedTest,
                    TestName = selectedTest,
                    ResultValue = random.Next(50, 150).ToString(),
                    ResultNumeric = (decimal)random.Next(50, 150),
                    Unit = "mg/dL",
                    ReferenceRange = "70-110",
                    Flag = "N",
                    ResultDateTime = DateTime.Now
                }
            },
            Status = "Final",
            Timestamp = DateTime.Now
        };

        await SendToApi(result, "Rastgele");
    }

    static async Task SendToApi(object data, string deviceType)
    {
        try
        {
            Console.WriteLine($"\n⏳ {deviceType} sonuçları gönderiliyor...");

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            Console.WriteLine("\n📤 Gönderilen Veri:");
            Console.WriteLine(json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(API_URL, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("\n✅ BAŞARILI!");
                Console.WriteLine($"Status Code: {(int)response.StatusCode} {response.StatusCode}");
                Console.WriteLine($"Yanıt: {responseContent}");
            }
            else
            {
                Console.WriteLine("\n❌ HATA!");
                Console.WriteLine($"Status Code: {(int)response.StatusCode} {response.StatusCode}");
                Console.WriteLine($"Hata Mesajı: {responseContent}");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"\n❌ Bağlantı Hatası: {ex.Message}");
            Console.WriteLine("💡 API sunucusunun çalıştığından emin olun (http://localhost:5000)");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Beklenmeyen Hata: {ex.Message}");
        }
    }
}
