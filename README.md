# bdd-showdown

This repository compares different BDD frameworks for .NET. BDD provides a behaviour focussed approach to testing, often producing extremely readable output that can be presented to stakeholders to verify a system. 

BDD adds additional context when writing tests, something that is often lost when writing tests with _unit_ testing frameworks. 

Finding a good balance between a test framework that offers the expressiveness of BDD without bloating tests or requiring a lot of ceremony, especially in .NET since most frameworks are really being driven by the underlying test runners (XUnit/NUnit) which do not really have a sense of heirarchy.

## System under test

The system being tested is a simplified component taken from a payment gateway. `AuthorizationProcessor` is responsible for dispatching a payment request to one or more acquirers. The business requires that failures are handled gracefully and in certain circumstances we can cascade from one acquirer to another.

## Frameworks covered

- [NSpec](http://nspec.org/)
- [XBehave](http://xbehave.github.io/)

### NSpec

Browse to the NSpec tests directory and run `dotnet run`

### XBehave

Browse to the XBehave directory and run `dotnet test`. This uses the XUnit test runner. 

To see more details output run `dotnet test --verbosity normal`

## Other BDD Frameworks

- [NBehave](https://github.com/nbehave/NBehave)
- [BDDfy](https://github.com/TestStack/TestStack.BDDfy)
- [LightBDD](https://github.com/LightBDD/LightBDD)
- [MSpec](https://github.com/machine/machine.specifications)
- [SpecFlow](https://specflow.org/)
