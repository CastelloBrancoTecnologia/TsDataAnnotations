import { Required, StringLength } from "../Validators/validators"
import { BaseViewModel } from  "../Validators/validators"
export class UsuariosRole extends BaseViewModel {
    public IdUsuario: BigInt = BigInt(0);
    @Required()
    @StringLength(64, "A role deve ter no maximo 64 caracteres")
    public Role: string = "";
}
