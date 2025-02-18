using FluentValidation;

namespace FlashApp.ArchitectureTests.Application;
public class ApplicationTests : BaseTest
{
    [Fact]
    public void Validator_Should_HaveNameEndingWith_Validator()
    {
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .Should()
            .HaveNameEndingWith("Validator")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
