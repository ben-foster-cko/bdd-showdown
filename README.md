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

### NSpec

Browse to the NSpec tests directory and run `dotnet run`

#### Basic debugging

- For VSCode tag the test you want to debug with "debug"
- Start a debugging session using the "NSpec debug" launch profile

#### Generate code coverage report

- Run the [gen-coverage](./gen-coverage.sh) script

This outputs the test report and code coverage report to the console. Beside that is generates the raw code coverage report under the ./coverage directory.

Using this generated report a tool like [Coverage Gutters](https://marketplace.visualstudio.com/items?itemName=ryanluker.vscode-coverage-gutters) to highlight the
code coverage for each file.


### XBehave

Browse to the XBehave directory and run `dotnet test`. This uses the XUnit test runner. 

To see more details output run `dotnet test --verbosity normal`

## Other BDD Frameworks

- [NBehave](https://github.com/nbehave/NBehave)
- [BDDfy](https://github.com/TestStack/TestStack.BDDfy)
- [LightBDD](https://github.com/LightBDD/LightBDD)
- [MSpec](https://github.com/machine/machine.specifications)
- [SpecFlow](https://specflow.org/)
