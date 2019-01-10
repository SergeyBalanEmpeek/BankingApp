import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { NgForm } from '@angular/forms';

@Component({
  selector: 'page-login',
  templateUrl: './login.component.html'
})

export class LoginComponent {
  error: boolean;
  errorMessage: string;

  constructor(private router: Router, private http: HttpClient) { }

  //User pressed 'Log In' button
  login(form: NgForm) {
    //send data to a server
    this.http.post("/api/auth", JSON.stringify(form.value), { headers: new HttpHeaders({ "Content-Type": "application/json"}) })
    .subscribe(response => {
        //Success
      this.error = false;    
        localStorage.setItem("jwt", (<any>response).token);   //store token to local storage
        this.router.navigate(["/home"]);                      //redirect to home page
      },
      err => {
        //Fail
        switch (err.status) {
          case 401:   //wrong login/password combination
          case 422:   //bad login/password fields value
            this.errorMessage = err.error;
            break;
          default:
            this.errorMessage = "Unable to login. Please try again later.";
            //console.log(err);
            break;
        }
        this.error = true;
      }
    );
  }

}
