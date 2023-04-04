using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using DataforgedGen;
using DataforgedGen.Shared;
using MudBlazor;
using TheOracle2.Data;
using Newtonsoft.Json;
using System.Linq;
using System.Text;

namespace DataforgedGen.Pages
{
    public partial class AssetPage
    {
        private Asset asset = new();
        bool enableCondition = false;

        [Inject]
        public IJSRuntime? JS { get; set; }

        public AssetPage()
        {
            LoadAsset();
        }

        private void LoadAsset()
        {
            string text = """{ "Category": "Ironsworn/Moves/Adventure", "Name": "Face Danger", "Text": "When **you attempt something risky or react to an imminent threat**, envision your action and roll. If you act...\n\n * With speed, agility, or precision: Roll +edge.\n * With charm, loyalty, or courage: Roll +heart.\n * With aggressive action, forceful defense, strength, or endurance: Roll +iron.\n * With deception, stealth, or trickery: Roll +shadow.\n * With expertise, insight, or observation: Roll +wits.\n\nOn a **strong hit**, you are successful. Take +1 momentum.\n\nOn a **weak hit**, you succeed, but face a troublesome cost. Choose one.\n\n * You are delayed, lose advantage, or face a new danger: Suffer -1 momentum.\n * You are tired or hurt: [Endure Harm](Ironsworn/Moves/Suffer/Endure_Harm) (1 harm).\n * You are dispirited or afraid: [Endure Stress](Ironsworn/Moves/Suffer/Endure_Stress) (1 stress).\n * You sacrifice resources: Suffer -1 supply.\n\nOn a **miss**, you fail, or your progress is undermined by a dramatic and costly turn of events. [Pay the Price](Ironsworn/Moves/Fate/Pay_the_Price).", "Outcomes": { "Strong Hit": { "Text": "You are successful. Take +1 momentum." }, "Weak Hit": { "Text": "You succeed, but face a troublesome cost. Choose one.\n\n * You are delayed, lose advantage, or face a new danger: Suffer -1 momentum.\n * You are tired or hurt: [Endure Harm](Ironsworn/Moves/Suffer/Endure_Harm) (1 harm).\n * You are dispirited or afraid: [Endure Stress](Ironsworn/Moves/Suffer/Endure_Stress) (1 stress).\n * You sacrifice resources: Suffer -1 supply." }, "Miss": { "Text": "You fail, or your progress is undermined by a dramatic and costly turn of events. [Pay the Price](Ironsworn/Moves/Fate/Pay_the_Price)." } }}""";
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

        private void ClearData()
        {
            asset = new()
            {
                Abilities = new(),
                Source = new(),
                Display = new(),
                Attachments = new(),
                ConditionMeter = new(),
                Inputs= new(),
                States= new(),
                Usage=new(),
            };
        }

        void FormatAsset()
        {
            asset.Id = $"{asset.AssetType.Replace(" ", "_")}/{asset.Name.Replace(" ", "_")}";
        }
    }
}