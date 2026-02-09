using Marqdouj.DotNet.AzureMaps.Blazor.Models.Events;
using System.Collections.ObjectModel;

namespace DemoApp.Shared.Components.Pages.BlazorMaps.Events
{
    public class MapEventArgsViewModelManager
    {
        private readonly List<MapEventArgsViewModel> _items = [];

        public MapEventArgsViewModelManager(int maxItems = 25)
        {
            Items = new ReadOnlyCollection<MapEventArgsViewModel>(_items);
            MaxItems = maxItems;
        }

        public void Add(MapEventArgs args)
        {
            _items.Insert(0, new MapEventArgsViewModel(args));
            while (_items.Count > MaxItems)
            {
                _items.RemoveAt(_items.Count - 1);
            }
        }

        public void Clear()
        {
            _items.Clear();
        }

        public IReadOnlyCollection<MapEventArgsViewModel> Items { get; }
        public int MaxItems { get; }
    }

    public class MapEventArgsViewModel(MapEventArgs args)
    {
        public MapEventArgs Args { get; } = args;
        public string Value { get; set; } = args.ToJsonMin();
        public TimeSpan TimeStamp { get; set; } = new TimeSpan(DateTime.Now.Ticks);
    }
}
