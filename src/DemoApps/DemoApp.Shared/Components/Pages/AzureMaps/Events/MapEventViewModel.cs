using Marqdouj.DotNet.AzureMaps.Map.Events;

namespace DemoApp.Shared.Components.Pages.AzureMaps.Events
{
    internal class MapEventViewModel(MapEventDef eventDef)
    {
        public MapEventDef EventDef { get; } = eventDef;
        public string? Name => EventDef.Type.ToString();
        public MapEventType? Type => EventDef.Type;
        public bool IsChecked { get; set; }
        public bool IsNotChecked => !IsChecked;
        public bool IsLoaded { get; set; }
        public bool IsNotLoaded => !IsLoaded;
    }
}
