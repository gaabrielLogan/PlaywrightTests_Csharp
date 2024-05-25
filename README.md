# PlaywrightTests

This repository contains automated tests written in C# using the [Playwright](https://playwright.dev/dotnet/docs/intro) framework and [NUnit](https://nunit.org/). The tests are performed on the [app-car](https://gaabrielogan2.github.io/app-car) application and verify various functionalities of the application.

## Project Structure

The project is structured as follows:

- **CarFormTests.cs**: Contains the test definitions and test environment configuration.
- **Pages**: Folder containing page classes used to organize and abstract interactions with different parts of the UI.

## Environment Setup

### Dependencies

- .NET SDK
- Microsoft.Playwright
- NUnit

### Installation

To install the dependencies, run the following command in the terminal:

```sh
dotnet add package Microsoft.Playwright.NUnit
dotnet build
pwsh bin/Debug/net8.0/playwright.ps1 install
```

## Test Structure

### Setup and Teardown

- **OneTimeSetUp**: Global setup, such as starting Playwright and launching the browser in headless mode.
- **SetUp**: Setup before each test, such as creating a new browser context and opening a new page.
- **TearDown**: Cleanup after each test, closing the page and context.
- **OneTimeTearDown**: Global cleanup, closing the browser and disposing of Playwright.

### Test Examples

#### Filling in Mandatory Fields and Checking the Success Message

```csharp
[Test]
public async Task FillingInMandatoryFieldsAndCheckingTheSuccessMessage()
{
    var fillingInMandatoryFields = new FillingInMandatoryFields(_page);

    await fillingInMandatoryFields.FillInMandatoryFieldsAndSubmitAsync(
        model: "Chevette",
        brand: "Chevrolet",
        year: "1973",
        password: "password",
        country: "Germany"
    );

    var successMessage = await fillingInMandatoryFields.GetSuccessMessageAsync();

    Assert.That(successMessage, Is.EqualTo("Order placed successfully!"));
}
```

#### Enabling a Disabled Field and Filling It Out

```csharp
[Test]
public async Task EnablingADisabledFieldAndFillingItOut()
{
    var disabledField = await _page.QuerySelectorAsync("#disabledField");
    Assert.NotNull(disabledField);
    Assert.That(await disabledField.InputValueAsync(), Is.Empty);
    await disabledField.EvaluateAsync("element => element.removeAttribute('disabled')");
    await disabledField.FillAsync("50%");
    Assert.That(await disabledField.InputValueAsync(), Is.EqualTo("50%"));
}
```

#### Interacting with a Date Field

```csharp
[Test]
public async Task InteractingWithADateField()
{
    await _page.FillAsync("#time", "23:23");
    Assert.That(await _page.InputValueAsync("#time"), Is.EqualTo("23:23"));
}
```

### Running the Tests

To run the tests, use the following command:

```sh
dotnet test
```

The tests will be executed, and the results will be displayed in the console.

## Contribution

To contribute to this project, follow these steps:

1. Fork this repository.
2. Create a branch for your feature (`git checkout -b feature/new-feature`).
3. Commit your changes (`git commit -m 'Add new feature'`).
4. Push to the branch (`git push origin feature/new-feature`).
5. Open a Pull Request.

## License

This project is licensed under the terms of the MIT license.

---

**Note:** This is an example documentation for your automated testing project using Playwright and NUnit. Make sure to adapt the information as necessary to reflect the specifics of your project.
