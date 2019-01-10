import { JwtHelperService } from '@auth0/angular-jwt';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable()
export class AuthService implements CanActivate {

  constructor(private router: Router) {
  }

  //Check is token valid
  canActivate() {
    const jwtHelper = new JwtHelperService();

    //Get token from local storage
    var token = localStorage.getItem("jwt");

    if (token && !jwtHelper.isTokenExpired(token)) {
      //Token is valid
      return true;
    }

    //Token is not exists or invalid. Redirect to start page
    this.router.navigate(["/"]);
    return false;
  }
}
