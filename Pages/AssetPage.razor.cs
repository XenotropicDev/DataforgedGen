using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Text;
using TheOracle2.Data;

namespace DataforgedGen.Pages
{
    public partial class AssetPage
    {
        private Asset asset = new();
        private int cardWidth = 380;
        private int cardHeight = 500;

        [Inject]
        public IJSRuntime? JS { get; set; }

        public AssetPage()
        {
            LoadAsset();
        }

        private void LoadAsset()
        {
            string text = """{ "Asset Type": "Ironsworn/Assets/Companion", "Name": "Cave Lion", "Display": { "Title": "Cave Lion" }, "Inputs": [ { "Input Type": "Text", "Name": "Name", "Adjustable": false } ], "Requirement": "Your cat takes down its prey.", "Condition Meter": { "Min": 0, "Value": 4, "Name": "Health", "Max": 4, "Conditions": [], "Aliases": [ "Companion Health" ] }, "Abilities": [ { "$id": "Ironsworn/Assets/Companion/Cave_Lion/Abilities/1", "Name": "Eager", "Text": "When your cat chases down big game, you may [Resupply](Ironsworn/Moves/Adventure/Resupply) with +edge (instead of +wits). If you do, take +1 supply or +1 momentum on a strong hit.", "Enabled": false }, { "$id": "Ironsworn/Assets/Companion/Cave_Lion/Abilities/2", "Name": "Inescapable", "Text": "When you [Enter the Fray](Ironsworn/Moves/Combat/Enter_the_Fray) or [Strike](Ironsworn/Moves/Combat/Strike) by sending your cat to attack, roll +edge. On a hit, take +2 momentum.", "Enabled": false }, { "$id": "Ironsworn/Assets/Companion/Cave_Lion/Abilities/3", "Name": "Protective", "Text": "When you [Make Camp](Ironsworn/Moves/Adventure/Make_Camp), your cat is alert to trouble. If you or an ally choose to relax, take +1 spirit. If you focus, take +1 momentum.", "Enabled": false } ]}""";
            asset = JsonConvert.DeserializeObject<Asset>(text) ?? new();
        }

        private async Task DownloadJson()
        {
            FormatAsset();

            var json = JsonConvert.SerializeObject(asset, Formatting.Indented);
            var fileName = $"{asset.Name}.json";

            var bytes = Encoding.UTF8.GetBytes(json);
            var stream = new MemoryStream(bytes);

            using var streamRef = new DotNetStreamReference(stream);

            await JS!.InvokeVoidAsync("window.downloadFileFromStream", fileName, streamRef);
        }

        private async Task printCardButton()
        {
            await JS!.InvokeVoidAsync("PrintElem", "assetCard");
        }

        private void ClearData()
        {
            asset = new()
            {
                Abilities = new(),
                Source = new(),
                Display = new(),
                Attachments = new(),
                ConditionMeter = new(),
                Inputs = new(),
                States = new(),
                Usage = new(),
            };
        }

        private void FormatAsset()
        {
            asset.Id = $"{asset.AssetType.Replace(" ", "_")}/{asset.Name.Replace(" ", "_")}";
            foreach(var ability in asset.Abilities)
            {
                ability.Id = asset.Id + $"/{ability.Name.Replace(" ", "_")}";
            }
            if (asset.ConditionMeter != null)
            {
                asset.ConditionMeter.Id = asset.Id + $"/{asset.ConditionMeter.Name.Replace(" ", "_")}";
            }
            foreach (var input in asset.Inputs ?? new())
            {
                input.Id = asset.Id + $"/{input.Name.Replace(" ", "_")}";
                input.InputType = AssetInput.Text;
            }
        }

        private void RemoveFromTable(Ability value)
        {
            var removed = asset.Abilities.Remove(value);
            asset.Abilities = new(asset.Abilities);
        }

        private Ability abilityToAdd = new();

        private void AddAbility()
        {
            asset.Abilities.Add(abilityToAdd);
            abilityToAdd = new();
        }

        private void toggleCondition()
        {
            if (asset.ConditionMeter == null)
            {
                asset.ConditionMeter = new() { Max = 5, Value = 5, Name = "Health" };
            }
            else
            {
                asset.ConditionMeter = null;
            }
        }
    }
}