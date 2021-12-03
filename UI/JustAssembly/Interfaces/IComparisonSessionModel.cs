using JustAssembly.Controls;

namespace JustAssembly.Interfaces
{
    interface IComparisonSessionModel
    {
        string Header { get; }

        SelectedItemType SelectedItemType { get; }

        ITabSourceItem GetTabSourceItem();
    }
}