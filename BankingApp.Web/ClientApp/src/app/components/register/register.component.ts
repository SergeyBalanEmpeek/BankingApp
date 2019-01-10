import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, ViewChild, ElementRef } from '@angular/core';
import { Router } from "@angular/router";
import { NgForm } from '@angular/forms';

@Component({
  selector: 'page-register',
  templateUrl: './register.component.html',
  styleUrls: [ './register.component.css']
})

export class RegisterComponent {
  error: boolean;   
  errorMessage: string;

  constructor(private router: Router, private http: HttpClient) { }

  @ViewChild('password') password: ElementRef;

  //User pressed 'Register' button
  register(form: NgForm) {
    //send data to a server
    this.http.post("/api/auth/create", JSON.stringify(form.value), { headers: new HttpHeaders({ "Content-Type": "application/json"}) })
    .subscribe(response => {
        //Success
        this.error = false;    
        localStorage.setItem("jwt", (<any>response).token);   //store token to local storage
        this.router.navigate(["/home"]);                      //redirect to home page
      },
      err => {
        //Fail
        switch (err.status) {
          case 409:   //login is in use
          case 422:   //bad login/password fields value
            this.errorMessage = err.error;
            break;
          default:
            this.errorMessage = "Unable to register. Please try again later.";
            //console.log(err);
            break;
        }
        this.error = true;
      }
    );
  }

  //User toggled Password field visibility
  flipPassword() {
    
    //let password = <HTMLInputElement>document.getElementById("new-password");
    if (this.password === null) return;

    if (this.password.nativeElement.type === 'password')
      this.password.nativeElement.type = 'text';
    else
      this.password.nativeElement.type = 'password';
  }

}
