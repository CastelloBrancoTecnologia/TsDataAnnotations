import {
    Required,
    Range,
    Length,
    RegularExpression,
    Validate,
    Compare,
    // Validated
} from "./validators";

type Gender = "male" | "female" | "";

class Human {
    @Required("gender is required.")
    gender: Gender = "";
}

class Person extends Human {
    @Required("name is required.")
    name: string = "";

    @Range(1, 10, "age must be in the range [1, 10].")
    age: number =0;

    @Required("data is required.")
    @Length(10, 10, "data must be of length 10.")
    @RegularExpression(/1+/, "data must be a string of 1s.")
    data: string = "";

    @Required("password is required.")
    password : string = "1";

    @Compare("password", "both passwords must be the same.")
    passwordRepeat : string = "1";

    public IsValid(): boolean {
        return Validate(this).length == 0;
    }

    public GetErrors(): { [key: string]: string }[] {
        return Validate(this);
    }
}

let person = new Person();

console.log(Validate(person));

