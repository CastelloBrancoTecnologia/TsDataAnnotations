// https://github.com/dboikliev/ValidatorTS

import "reflect-metadata";

let validatorKey = Symbol("validator");

interface IValidator<T> {
    validate: (obj: T) => boolean;
    message: string;
    property: string;
}

export function Time(is24hFormat: boolean, message: string = "Hora Invalida") {
  return defineValidator(
    (obj: any, propertyKey) => obj[propertyKey] && typeof obj[propertyKey] == "string" &&
      (is24hFormat ? new RegExp ("^([0-1][0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?$") : new RegExp ("(((0[1-9])|(1[0-2])):([0-5])(0|5)(:[0-5][0-9])\\s(A|P|a|p)(M|m))") ).test(obj[propertyKey]),
    message);
}

export function Guid(message: string = "Guid Invalida") {
  return defineValidator(
    (obj: any, propertyKey) => obj[propertyKey] && typeof obj[propertyKey] == "string" &&
      /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i.test(obj[propertyKey]),
    message);
}

export function Email(message: string = "Email Invalido") {
  return defineValidator(
    (obj: any, propertyKey) => obj[propertyKey] && typeof(obj[propertyKey]) == "string" &&       
         obj[propertyKey].IndexOf('@') > 0 &&
         obj[propertyKey].IndexOf('@') != obj[propertyKey].Length - 1,
    message);
}

export function AllowedValues(values: any[], message: string = "valor nao permitido") {
  return defineValidator(
    (obj: any, propertyKey) => obj[propertyKey] &&
      values.indexOf (obj[propertyKey]) >= 0,
    message);
}

export function DeniedValues(values: any[], message: string = "valor nao permitido") {
  return defineValidator(
    (obj: any, propertyKey) => obj[propertyKey] &&
      values.indexOf(obj[propertyKey]) < 0,
    message
  );
}

export function Required(message: string = "requerido") {
  return defineValidator((obj, propertyKey) => Reflect.has(obj, propertyKey), message);
}

export function Compare(propertyName: string, message: string = "nao sao iguais") {
  return defineValidator((obj: any, propertyKey) => obj[propertyName] === obj[propertyKey], message);
}

export function Range(min: number, max: number, message: string = "fora da faixa permitida") {
    return defineValidator(
      (obj: any, propertyKey) => obj[propertyKey] >= min && obj[propertyKey] <= max, message);
}

export function MinValue(min: number, message: string = "menor que valor minimo") {
    return defineValidator(
      (obj: any, propertyKey) => obj[propertyKey] >= min, message);
}

export function MaxValue(max: number, message: string = "maior que valor maximo") {
    return defineValidator(
      (obj: any, propertyKey) => obj[propertyKey] <= max, message);
}

export function MaxLength(max: number, message: string = "tamanho maximo ultrapassado") {
    return defineValidator(
      (obj: any, propertyKey) => obj[propertyKey] && obj[propertyKey].length <= max,
      message );
}

export function StringLength(max: number, message: string = "fora da faixa de tamanho permitida") {
    return defineValidator(
      (obj: any, propertyKey) => typeof obj[propertyKey] == "string" && obj[propertyKey].length <= max, message
    );
}

export function MinLength(max: number, message: string = "menor que tamanho minimo") {
    return defineValidator(
      (obj: any, propertyKey) => obj[propertyKey] && obj[propertyKey].length <= max, message );
}

export function Length(min: number, max: number, message: string = "fora da faixa de tamanho permitida") {
    return defineValidator(
      (obj: any, propertyKey) => obj[propertyKey] &&
            obj[propertyKey].length >= min &&
            obj[propertyKey].length <= max,
      message );
}

export function RegularExpression(expression: RegExp, message: string = "valor invalido") {
    return defineValidator(
      (obj: any, propertyKey) => typeof obj[propertyKey] == "string" &&
            expression.test(obj[propertyKey]),
      message);
}

export function Base64String(message: string = "nao eh uma string base64 valida") {
    return defineValidator(
      (obj: any, propertyKey) => typeof obj[propertyKey] == "string" &&
            /^[-A-Za-z0-9+/]*={0,3}$/g.test(obj[propertyKey]),
      message);
}

export function Url(message: string = "url invalida") {
    return defineValidator(
        (obj : any, propertyKey) => typeof obj[propertyKey] == "string" &&
            /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/g.test(obj[propertyKey]),
      message);
}

export function FileExtensions(extensions: string, message: string = "tipo de arquivo nao permitido") {        
    return defineValidator(
        (obj: any, propertyKey) => obj[propertyKey] == null ||
                              (typeof obj[propertyKey] == "string" &&
                               ((extensions == null || extensions == "") ? "png,jpg,jpeg,gif" :
                                extensions.replace(/\s|./g, "").toLowerCase()).split(",").includes(obj[propertyKey])),
      message);
}

export function CreditCard(message: string = "no de cartao invalido") {    
    function IsValid(value: string): boolean
    {
        let checksum: number = 0;
        let evenDigit: boolean = false;

        for (let i: number = value.length - 1; i >= 0; i--)
        {
            let ch: string = value[i];

            if (!(ch >= '0' && ch <= '9'))
            {
                if (ch == '-' || ch == ' ')
                {
                    continue;
                }

                return false;
            }

            let digit: number = ch.charCodeAt(0) - ('0'.charCodeAt(0));

            let digitValue: number = digit * (evenDigit ? 2 : 1);

            evenDigit = !evenDigit;

            while (digitValue > 0)
            {
                checksum += digitValue % 10;
                digitValue /= 10;
            }
        }

        return (checksum % 10) == 0;
    }

  return defineValidator((obj: any, propertyKey) => IsValid(obj[propertyKey]), message);
}

export function EnumDataType(EnumType: Object, message: string = "valor invalido") {
    function IsValid(value: any): boolean {
        if (EnumType == null) {
            throw new Error("EnumType nao pode ser nula");
        }

        if (value == null) {
            return true;
        }

        if (typeof (value) == "string" && value.length == 0)
        {
            return true;
        }

        return value in EnumDataType;        
    }

    return defineValidator((obj : any, propertyKey : string | symbol) => IsValid(obj[propertyKey]), message );
}

export function defineValidator(validatorPredicate: (obj: Object, propertyKey: string | symbol) => boolean, message: string)
    : (target: any, propertyKey: string | symbol) => void {
    return function (target: any, propertyKey: string | symbol) {
        let symbol = Symbol();
        Reflect.defineMetadata(symbol, {
            [validatorKey]: {
                validate: (obj : any) => validatorPredicate(obj, propertyKey),
                message: message,
                property: propertyKey
            }
        }, target);
    };
}

export function Validate(target: any): { [key: string]: string }[] {
    if (typeof target === "object") {
        let result = Reflect.getMetadataKeys(target)
            .map(key => Reflect.getMetadata(key, target))
            .map(metadata => metadata[validatorKey] as IValidator<any>)
            .filter(metadata => metadata && !metadata.validate(target))
            .map(metadata => ({ property: metadata.property, message: metadata.message }));

        return result;
    }

    return [];
};

// DataTypeAttribute
// DisplayAttribute
// DisplayFormatAttribute
// UIHintAttribute
// EditableAttribute
// PhoneNumber
// Numeric
// DateTime
// DateOnly
// TimeSpan
// CustomValidation

export class BaseViewModel
{
  public IsValid(): boolean {
    return Validate(this).length == 0;
  }

  public GetErrors(): { [key: string]: string }[] {
    return Validate(this);
  }

  public ErrorMessages(): string {

    let s: string = "";

    let errors : Iterator<{[key:string]:string}> = Validate(this).values();

    for (let value in errors) {
      s = s + `${value}\r\n<\br>`;
    }

    return s;
  }
}


//export function Validated(target: Object, key: string | symbol, descriptor: PropertyDescriptor) {
//    let original: Function = descriptor.value;

//    descriptor.value = function (...args: any[]) {
//        let modelState = args.map(arg => Validate(arg));
//        args.push(modelState);
//        original.apply(this, args);
//    };
//    return descriptor;
//}


