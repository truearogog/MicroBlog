using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace MicroBlog.Data.EF.ValueGenerators
{
    public class CurrentTimeValueGenerator : ValueGenerator<DateTime>
    {
        public override bool GeneratesTemporaryValues => false;

        public override DateTime Next(EntityEntry entry)
        {
            return DateTime.UtcNow;
        }
    }
}
