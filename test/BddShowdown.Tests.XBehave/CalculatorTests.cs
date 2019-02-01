namespace BddShowdown.Tests
{
    using System;
    using Shouldly;
    using Xbehave;
    using Xunit.Abstractions;

    public class Calculator
    {
        public int Add(int x, int y) => x + y;
    }

    public class CalculatorFeature : NSpecShim
    {
        private Calculator calculator;
        private readonly ITestOutputHelper output;

        public CalculatorFeature(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Background]
        public void Background() // this method can have any name
        {
            "Given a calculator"
                .x(() => this.calculator = new Calculator());
        }


        [Scenario(DisplayName = "Adding two numbers")]
        public void Addition(int x, int y, int answer)
        {
            "Given the number 1"
                .x(() =>
                {
                    x = 1; output.WriteLine("Ctor");
                });

            "And the number 2"
                .x(() => y = 2);

            // "And a calculator"
            //     .x(() => calculator = new Calculator());

            "When I add the numbers together"
                .x(() => answer = calculator.Add(x, y));

            "Then the answer is 3"
                .x(() => answer.ShouldBe(3));
        }

        [Scenario(DisplayName = "Adding two numbers (shim)")]
        public void Addition2(int x, int y, int answer)
        {
            Given("the number 1", () => x = 1);
            And("the number 2", () => y = 2);

            When("I add the numbers together", () => answer = calculator.Add(x, y));
            Then("the answer is 3", () => answer.ShouldBe(3));
        }
    }
}