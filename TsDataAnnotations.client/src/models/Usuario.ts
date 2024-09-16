import { Required, StringLength, Email } from "../Validators/validators"
import { BaseViewModel } from  "../Validators/validators"
export class Usuario extends BaseViewModel {
    public Id: BigInt = BigInt(0);
    @Required()
    @StringLength(32, "O username deve ter no maximo 32 caracteres")
    public Username: string = "";
    @Required()
    @StringLength(64, "O Nome deve ter no maximo 64 caracteres")
    public Nome: string = "";
    @Required()
    @StringLength(80, "O Email deve ter no maximo 128 caracteres")
    @Email()
    public Email: string = "";
    @Required()
    @StringLength(32, "O celular deve ter no maximo 128 caracteres")
    public Celular: string = "";
    @Required()
    public HashSenha: string = "";
}
