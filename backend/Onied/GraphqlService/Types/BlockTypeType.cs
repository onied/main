using Courses.Data.Models;

namespace GraphqlService.Types;

public class BlockTypeType : EnumType<BlockType>
{
    protected override void Configure(IEnumTypeDescriptor<BlockType> descriptor)
    {
        descriptor.BindValuesExplicitly();
    }
}
