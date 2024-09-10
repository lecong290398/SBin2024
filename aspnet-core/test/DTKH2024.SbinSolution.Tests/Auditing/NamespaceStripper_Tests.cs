using DTKH2024.SbinSolution.Auditing;
using DTKH2024.SbinSolution.Test.Base;
using Shouldly;
using Xunit;

namespace DTKH2024.SbinSolution.Tests.Auditing
{
    // ReSharper disable once InconsistentNaming
    public class NamespaceStripper_Tests: AppTestBase
    {
        private readonly INamespaceStripper _namespaceStripper;

        public NamespaceStripper_Tests()
        {
            _namespaceStripper = Resolve<INamespaceStripper>();
        }

        [Fact]
        public void Should_Stripe_Namespace()
        {
            var controllerName = _namespaceStripper.StripNameSpace("DTKH2024.SbinSolution.Web.Controllers.HomeController");
            controllerName.ShouldBe("HomeController");
        }

        [Theory]
        [InlineData("DTKH2024.SbinSolution.Auditing.GenericEntityService`1[[DTKH2024.SbinSolution.Storage.BinaryObject, DTKH2024.SbinSolution.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null]]", "GenericEntityService<BinaryObject>")]
        [InlineData("CompanyName.ProductName.Services.Base.EntityService`6[[CompanyName.ProductName.Entity.Book, CompanyName.ProductName.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[CompanyName.ProductName.Services.Dto.Book.CreateInput, N...", "EntityService<Book, CreateInput>")]
        [InlineData("DTKH2024.SbinSolution.Auditing.XEntityService`1[DTKH2024.SbinSolution.Auditing.AService`5[[DTKH2024.SbinSolution.Storage.BinaryObject, DTKH2024.SbinSolution.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[DTKH2024.SbinSolution.Storage.TestObject, DTKH2024.SbinSolution.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],]]", "XEntityService<AService<BinaryObject, TestObject>>")]
        public void Should_Stripe_Generic_Namespace(string serviceName, string result)
        {
            var genericServiceName = _namespaceStripper.StripNameSpace(serviceName);
            genericServiceName.ShouldBe(result);
        }
    }
}
