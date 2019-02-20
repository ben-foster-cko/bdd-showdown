# BDD Showdown

This repository compares different BDD frameworks for .NET. 

[BDD](https://docs.cucumber.io/bdd/overview/) promotes a behaviour focussed approach to testing, often providing extremely readable output that can often be presented and understood by project stakeholders.

BDD adds additional context when writing tests, something that is often lost when writing tests with traditional _unit_ testing frameworks. 

Finding a good balance between a test framework that offers the expressiveness of BDD without bloating tests or requiring a lot of ceremony is a challenge, especially in .NET since most frameworks are really being driven by the underlying test runners (XUnit/NUnit).

## System under test

The system being tested is a simplified component taken from a payment gateway. `AuthorizationProcessor` is responsible for dispatching a payment request to one or more acquirers. The business requires that failures are handled gracefully and in certain circumstances we can cascade from one acquirer to another.

## Frameworks covered

- [NSpec](http://nspec.org/)
- [XBehave](http://xbehave.github.io/)
- [NUnit](https://nunit.org/)
- [XUnit](https://xunit.github.io/)

### NSpec

Browse to the NSpec tests directory and run `dotnet run`

### XBehave

Browse to the XBehave directory and run `dotnet test`. This uses the XUnit test runner. 
To see more details output run `dotnet test --verbosity normal`

### NUnit

Browse to the NUnit directory and run `dotnet test`. This uses the NUnit test runner. 

### XUnit

Browse to the XUnit directory and run `dotnet test`. This uses the XUnit test runner. 

## Other BDD Frameworks

- [NBehave](https://github.com/nbehave/NBehave)
- [BDDfy](https://github.com/TestStack/TestStack.BDDfy)
- [LightBDD](https://github.com/LightBDD/LightBDD)
- [MSpec](https://github.com/machine/machine.specifications)
- [SpecFlow](https://specflow.org/)
