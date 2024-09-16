import {  } from "../Validators/validators"
import { BaseViewModel } from  "../Validators/validators"
export class TokenModel extends BaseViewModel {
    public Username: string = "";
    public Roles: string [] = [];
    public Expires: Date = new Date();
    public Token: string = "";
    public Message: string = "";
}
