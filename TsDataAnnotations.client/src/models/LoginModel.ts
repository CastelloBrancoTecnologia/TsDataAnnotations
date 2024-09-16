import { Required, Length } from "../Validators/validators"
import { BaseViewModel } from  "../Validators/validators"
export class LoginModel extends BaseViewModel {
    @Required()
    @Length(4, 64, "O nome de usuario deve ter no minimo 4 e no maximo 32 letras ou numeros, sem espaços")
    public Username: string = "";
    @Required()
    @Length(8, 16, "A senha deve ter no minimo 8 e no maximo 16 caracteres")
    public Senha: string = "";
    @Required()
    @Length(8, 8, "A senha otp deve ter 8 caracteres")
    public Otp: string = "";
}
