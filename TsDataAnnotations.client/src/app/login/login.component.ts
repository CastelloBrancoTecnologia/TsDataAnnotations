import { Component } from '@angular/core';
import { FormBuilder } from "@angular/forms";
import { HttpClient } from '@angular/common/http';
import { LoginModel } from "../../models/LoginModel";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent
{
  public viewmodel: LoginModel = new LoginModel();

  mensagem: string = "";

  constructor(private http: HttpClient)
  {

  }

  login()
  {
    if (this.viewmodel.IsValid()) {
      this.http.post("/api/Login",
        this.viewmodel).subscribe(next => this.mensagem = "Login feito com sucesso!",
          error => this.mensagem = "E-mail ou senha esta errado ! \n" + JSON.stringify(error));
    }
    else {
      this.mensagem = this.viewmodel.ErrorMessages(); 
    }
  }
}
