#nullable disable

using AutoFixture.Kernel;
using System.Reflection;

namespace MicroBlog.Data.EF.Tests.AutoFixture
{
    public class IgnoreVirtualMembers : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var pi = request as PropertyInfo;
            if (pi == null)
            {
                return new NoSpecimen();
            }

            if (pi.GetGetMethod().IsVirtual)
            {
                return null;
            }
            return new NoSpecimen();
        }
    }
}
