using Shared.Kernel.Entities;

namespace Shared.Kernel.UnitTests.EventSourcing.TestDomain;

public class Tree : Entity<TreeId>
{
    public string TreeType { get; private set; }
    
    public Tree(TreeId id, string treeType) : base(id)
    {
        TreeType = treeType;
    }
}