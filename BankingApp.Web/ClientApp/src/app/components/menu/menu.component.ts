import { Component } from '@angular/core';
import { Router } from "@angular/router";

@Component({
  selector: 'main-menu',
  templateUrl: './menu.component.html'
})

export class MenuComponent {

  constructor(private router: Router) {
  }

  //User pressed Logout button
  logOut() {
    localStorage.removeItem("jwt");     //Remove JSON Web Token from local storage
    this.router.navigate(["/"]);        //Redirect user to start page
  }

}
