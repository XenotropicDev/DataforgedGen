using DataswornPoco;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using System.Text;
using TheOracle2.Data;

namespace DataforgedGen.Pages;

public partial class MovePage
{
    private Move move = new();

    [Inject]
    public IJSRuntime? JS { get; set; }

    [Inject]
    public ISnackbar Snackbar { get; set; }

    public MovePage()
    {
        LoadMove();
    }

    private void LoadMove()
    {
        string text = """{ "Category": "Ironsworn/Moves/Adventure", "Name": "Face Danger", "Text": "When **you attempt something risky or react to an imminent threat**, envision your action and roll. If you act...\n\n * With speed, agility, or precision: Roll +edge.\n * With charm, loyalty, or courage: Roll +heart.\n * With aggressive action, forceful defense, strength, or endurance: Roll +iron.\n * With deception, stealth, or trickery: Roll +shadow.\n * With expertise, insight, or observation: Roll +wits.\n\nOn a **strong hit**, you are successful. Take +1 momentum.\n\nOn a **weak hit**, you succeed, but face a troublesome cost. Choose one.\n\n * You are delayed, lose advantage, or face a new danger: Suffer -1 momentum.\n * You are tired or hurt: [Endure Harm](Ironsworn/Moves/Suffer/Endure_Harm) (1 harm).\n * You are dispirited or afraid: [Endure Stress](Ironsworn/Moves/Suffer/Endure_Stress) (1 stress).\n * You sacrifice resources: Suffer -1 supply.\n\nOn a **miss**, you fail, or your progress is undermined by a dramatic and costly turn of events. [Pay the Price](Ironsworn/Moves/Fate/Pay_the_Price).", "Outcomes": { "Strong Hit": { "Text": "You are successful. Take +1 momentum." }, "Weak Hit": { "Text": "You succeed, but face a troublesome cost. Choose one.\n\n * You are delayed, lose advantage, or face a new danger: Suffer -1 momentum.\n * You are tired or hurt: [Endure Harm](Ironsworn/Moves/Suffer/Endure_Harm) (1 harm).\n * You are dispirited or afraid: [Endure Stress](Ironsworn/Moves/Suffer/Endure_Stress) (1 stress).\n * You sacrifice resources: Suffer -1 supply." }, "Miss": { "Text": "You fail, or your progress is undermined by a dramatic and costly turn of events. [Pay the Price](Ironsworn/Moves/Fate/Pay_the_Price)." } }}""";
        move = JsonConvert.DeserializeObject<Move>(text) ?? new();
    }

    private async Task DownloadJson()
    {
        FormatMove();

        var json = JsonConvert.SerializeObject(move, Formatting.Indented);
        var fileName = $"{move.Name}.json";

        var bytes = Encoding.UTF8.GetBytes(json);
        var stream = new MemoryStream(bytes);

        using var streamRef = new DotNetStreamReference(stream);

        await JS!.InvokeVoidAsync("window.downloadFileFromStream", fileName, streamRef);
    }

    private void ClearData()
    {
        move = new()
        {
            Outcomes = new()
            {
                StrongHit = new() {Reroll = new(), WithAMatch = new() },
                WeakHit = new() { Reroll = new() },
                Miss = new() { Reroll = new(), WithAMatch = new() },
            },
            Source = new(),
            Display = new(),
            Oracles = new(),
            Trigger = new(),
        };
    }

    void FormatMove()
    {
        move.Id = $"{move.Category.Replace(" ","_")}/{move.Name.Replace(" ", "_")}";

        move.Outcomes.Id = $"{move.Id}/Outcomes";
        move.Outcomes.StrongHit.Id = move.Outcomes.Id + "/Strong_Hit";
        move.Outcomes.WeakHit.Id = move.Outcomes.Id + "/Weak_Hit";
        move.Outcomes.Miss.Id = move.Outcomes.Id + "/Miss";
    }

    private async Task UploadFiles(IBrowserFile file)
    {
        using var text = file.OpenReadStream();
        using var streamReader = new StreamReader(text);

        var read = await streamReader.ReadToEndAsync();

        try
        {
            move = JsonConvert.DeserializeObject<Move>(read) ?? new();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Couldn't process the json file:\n{ex.Message}");
        }
    }
}