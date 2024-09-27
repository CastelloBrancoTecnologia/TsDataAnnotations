# TsDataAnnotations - C# System.ComponentModel.DataAnnotations - attributes and validators for Typescript, generated from c# model classes

1) Separate yours models in an separate C# class library project. Use it on server side or on desktops apps projects
2) use all good things of **CommunityToolkit.MVVM**, include source generators 
3) Call TypeScript Model Generator from visual studio project automaticaly after suscessfull build
4) Typescript classes equivalent of your datamodels are atomaticaly generated inside Angular frontend project
5) Use the same validators and datamodels of c# inside typescript. Also serialize it from typescript to pass to json webservices and to c# - equivalent c# classes are desserialized inside backend

   Enjoy ! 

# TypeScript Model Generator

This project provides a **TypeScript Model Generator** tool that automatically generates TypeScript class models equivalent to C# classes decorated with specific attributes. It is designed to integrate seamlessly with .NET projects, executing after the Visual Studio build process.

## Key Attributes

1. **`GenerateTypeScriptAttribute`**
   - Applied to a C# class to mark it for TypeScript model generation.

2. **`TypeScriptIgnoreAttribute`**
   - Applied to a class or property to exclude it from the generated TypeScript model.
   

## Usage in .NET Project

This project integrates with a .NET build process. After the build completes, the `TsModelGenerator.EXE` tool runs, generating TypeScript classes based on the C# classes decorated with the `GenerateTypeScriptAttribute`.

### Visual Studio Project Configuration

To ensure TypeScript models are generated after building the project in Visual Studio, include the following section in your `.csproj` file:

```xml
<Target Name="GenerateTypescriptModels" AfterTargets="AfterBuild">
    <Exec ConsoleToMsBuild="true" 
          Command="$(SolutionDir)TsModelGenerator\bin\Debug\net8.0\TsModelGenerator.exe $(TargetPath) $(SolutionDir)TsDataAnnotations.client\src\models" />
</Target>
```

This command does the following:
- **`$(TargetPath)`**: Refers to the compiled output of the current project.
- **`$(SolutionDir)TsDataAnnotations.client\src\models`**: Specifies the folder where the TypeScript models will be placed.

## Angular Integration

The generated TypeScript models from c# models can be used in an Angular project, with validation provided by decorators. For example:

```c#
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.MVVM.DataAnnotations;

class MyModel : BaseViewModel
{
    [ObjectProperty]
    [Required("This field is required")]
    private string myProperty = "";

    [ObjectProperty]
    [Email("Invalid email format")]
    private string email = "";
}
```

Generated class from ModelGenerator after sucefull buil of c#

```typescript
import { Required, Email } from "../Validators/validators"
import { BaseViewModel } from  "../Validators/validators"

export class MyModel extends BaseViewModel {
    @Required("This field is required")
    public myProperty: string = "";

    @Email("Invalid email format")
    public email: string = "";
}
```

### Validator Decorators

The project includes a set of decorators to apply validation rules to the generated TypeScript properties:

- **`Required`**: Ensures the field is not null.
- **`Email`**: Validates if a string is in email format.
- **`Range`**: Ensures a number falls within a specified range.
- **`Guid`**: Validates if a string follows a GUID format.
- **`MaxLength`**: Limits the length of a string.
- 
- validators in System.ComponentModel.DataAnnotations 

For the complete list of validators and their usage, refer to the included TypeScript file.

### Example Usage

```typescript
export class UserModel extends BaseViewModel {
    @Required("Username is required")
    public username: string;

    @Email("Invalid email address")
    public email: string;

    @Range(18, 99, "Age must be between 18 and 99")
    public age: number;
}

let user = new UserModel();
user.username = "JohnDoe";
user.email = "john.doe@example.com";
user.age = 25;

console.log(user.IsValid());  // true
console.log(user.GetErrors());  // No errors
```

## Running the Generator

To manually run the generator:

```bash
TsModelGenerator.exe <CSharpProject.dll> <OutputDirectory>
```

Example:
```bash
TsModelGenerator.exe MyProject.dll ./src/models
```

## Source Code

The source code for `TsModelGenerator` is attached in the project. You can modify or extend it to suit your requirements.

---

Feel free to update this README as the project evolves!
