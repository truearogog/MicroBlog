using AutoFixture;

namespace MicroBlog.Data.EF.Tests.AutoFixture
{
    internal class IgnoreVirtualMembersCustomisation : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IgnoreVirtualMembers());
        }
    }
}
