using System.Windows.Automation.Peers;

namespace MaterialDesignThemes.Wpf.Automation.Peers;

public class TreeListViewAutomationPeer(TreeListView owner) : ListViewAutomationPeer(owner)
{
    protected override AutomationControlType GetAutomationControlTypeCore() => AutomationControlType.Tree;

    protected override string GetClassNameCore() => "TreeListView";

    protected override List<AutomationPeer>? GetChildrenCore()
    {
        var owner = (ItemsControl)Owner;
        ItemCollection items = owner.Items;
        List<AutomationPeer>? children = null;
        
        if (items.Count > 0)
        {
            children = new List<AutomationPeer>(items.Count);
            for (int i = 0; i < items.Count; i++)
            {
                var treeViewItem = owner.ItemContainerGenerator.ContainerFromIndex(i) as TreeListViewItem;
                //We grab top level items only
                if (treeViewItem is { Level: 0 })
                {
                    children.Add(CreateItemAutomationPeer(items[i]));
                }
            }
        }
        return children;
    }

    protected override ItemAutomationPeer CreateItemAutomationPeer(object item) => new TreeListViewItemAutomationPeer(item, this);
}

public class TreeListViewItemAutomationPeer(object owner, SelectorAutomationPeer selectorAutomationPeer) : ListBoxItemAutomationPeer(owner, selectorAutomationPeer)
{
    protected override List<AutomationPeer> GetChildrenCore()
    {
        List<AutomationPeer> rv = base.GetChildrenCore();

        if (ItemsControlAutomationPeer is TreeListViewAutomationPeer { Owner: TreeListView treeListView } itemAutomationPeer &&
            treeListView.ItemContainerGenerator.ContainerFromItem(Item) is TreeListViewItem treeListViewItem)
        {
            int index = treeListView.ItemContainerGenerator.IndexFromContainer(treeListViewItem);
            if (index >= 0)
            {
                foreach (int childIndex in treeListView.InternalItemsSource?.GetDirectChildrenIndexes(index) ?? Array.Empty<int>())
                {
                    rv.Add(new TreeListViewItemAutomationPeer(treeListView.Items[childIndex], itemAutomationPeer));
                }
            }
        }

        return rv;
    }

    public override object GetPattern(PatternInterface patternInterface)
    {
        if (patternInterface == PatternInterface.ExpandCollapse)
        {
            return this;
        }
        return base.GetPattern(patternInterface);
    }

    protected override AutomationControlType GetAutomationControlTypeCore() => AutomationControlType.TreeItem;
    protected override string GetClassNameCore() => "TreeListViewItem";
}
