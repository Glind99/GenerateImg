using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;


class Program
{
    
    private static string subscriptionKey = "22520de46ffa475ba564518efc028613";
    private static string endpoint = "https://imagegenerate.cognitiveservices.azure.com/";

    static async Task Main(string[] args)
    {
        ComputerVisionClient client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(subscriptionKey))
        {
            Endpoint = endpoint
        };

        Console.WriteLine("Vill du analysera en bild från URL eller en lokal filsökväg? (Skriv 'url' eller 'lokal')");
        string inputType = Console.ReadLine();

        if (inputType == "url")
        {
            Console.WriteLine("Ange bildens URL:");
            string imageUrl = Console.ReadLine();
            await AnalyzeImageUrlAsync(client, imageUrl);
        }
        else if (inputType == "lokal")
        {
            Console.WriteLine("Ange den lokala filsökvägen till bilden:");
            string imagePath = Console.ReadLine();
            await AnalyzeImageFileAsync(client, imagePath);
        }
        else
        {
            Console.WriteLine("Ogiltigt val, avslutar programmet.");
        }
    }

    private static async Task AnalyzeImageUrlAsync(ComputerVisionClient client, string imageUrl)
    {
        Console.WriteLine($"Analyserar bild från URL: {imageUrl}...");

        var features = new List<VisualFeatureTypes?> { VisualFeatureTypes.Objects, VisualFeatureTypes.Tags };
        ImageAnalysis analysis = await client.AnalyzeImageAsync(imageUrl, features);

        DisplayAnalysisResult(analysis);
    }

    private static async Task AnalyzeImageFileAsync(ComputerVisionClient client, string imagePath)
    {
        Console.WriteLine($"Analyserar bild från lokal filsökväg: {imagePath}...");

        using (Stream imageStream = File.OpenRead(imagePath))
        {
            var features = new List<VisualFeatureTypes?> { VisualFeatureTypes.Objects, VisualFeatureTypes.Tags };
            ImageAnalysis analysis = await client.AnalyzeImageInStreamAsync(imageStream, features);

            DisplayAnalysisResult(analysis);
        }
    }

    private static void DisplayAnalysisResult(ImageAnalysis analysis)
    {
        Console.WriteLine("Taggar funna i bilden:");
        foreach (var tag in analysis.Tags)
        {
            Console.WriteLine($"- {tag.Name} (Confidence: {tag.Confidence})");
        }

        Console.WriteLine("Objekt funna i bilden:");
        foreach (var detectedObject in analysis.Objects)
        {
            Console.WriteLine($"- {detectedObject.ObjectProperty} at location {detectedObject.Rectangle.X}, {detectedObject.Rectangle.Y}, {detectedObject.Rectangle.W}, {detectedObject.Rectangle.H}");
        }
    }
}
