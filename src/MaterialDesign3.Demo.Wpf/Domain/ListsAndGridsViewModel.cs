﻿using System.Collections.ObjectModel;
using MaterialDesignDemo.Shared.Domain;

namespace MaterialDesign3Demo.Domain;

public class ListsAndGridsViewModel : ViewModelBase
{
    public ListsAndGridsViewModel()
    {
        Items1 = CreateData();
        Items2 = CreateData();
        Items3 = CreateData();

        foreach (var model in Items1)
        {
            model.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SelectableViewModel.IsSelected))
                    OnPropertyChanged(nameof(IsAllItems1Selected));
            };
        }
    }

    public bool? IsAllItems1Selected
    {
        get
        {
            var selected = Items1.Select(item => item.IsSelected).Distinct().ToList();
            return selected.Count == 1 ? selected.Single() : null;
        }
        set
        {
            if (value.HasValue)
            {
                SelectAll(value.Value, Items1);
                OnPropertyChanged();
            }
        }
    }

    private static void SelectAll(bool select, IEnumerable<SelectableViewModel> models)
    {
        foreach (var model in models)
        {
            model.IsSelected = select;
        }
    }

    private static ObservableCollection<SelectableViewModel> CreateData()
    {
        return new ObservableCollection<SelectableViewModel>
        {
            new SelectableViewModel
            {
                Code = 'M',
                Name = "Material Design",
                Description = "Material Design in XAML Toolkit"
            },
            new SelectableViewModel
            {
                Code = 'D',
                Name = "Dragablz",
                Description = "Dragablz Tab Control",
                Food = "Fries"
            },
            new SelectableViewModel
            {
                Code = 'P',
                Name = "Predator",
                Description = "If it bleeds, we can kill it"
            }
        };
    }

    public ObservableCollection<SelectableViewModel> Items1 { get; }
    public ObservableCollection<SelectableViewModel> Items2 { get; }
    public ObservableCollection<SelectableViewModel> Items3 { get; }

    public IEnumerable<string> Foods => new[] { "Burger", "Fries", "Shake", "Lettuce" };
}
