import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NgForm } from '@angular/forms';
import { Router } from "@angular/router";
import * as Globals from '../../app.globals';

@Component({
  selector: 'page-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {
  account: any;
  recipients: any;

  loadingMessage: string;
  loadingShowMessage: boolean;

  balanceMessage: string;
  balanceShowMessage: boolean;

  @ViewChild('withdrawfield') withdrawElement: ElementRef;
  @ViewChild('depositfield') depositElement: ElementRef;
  @ViewChild('transferfield') transferElement: ElementRef;
  @ViewChild('transferreceiverfield') transferReceiverElement: ElementRef;

  constructor(private router: Router, private http: HttpClient) { }

  ngOnInit() {
    //Load required data on page load
    this.http.get("/api/account", { headers: Globals.getAuthHttpHeaders() }).subscribe(
      response => {
        this.account = (<any>response).account;
        this.recipients = (<any>response).recipients;
      },
      err => {
        if (err.status == 401) this.router.navigate(["/"]);;      // if not authorized - redirect to login 
        this.loadingMessage = "Unable to load data";
        this.loadingShowMessage = true;
      }
    );
  }

  //User wants to deposit or withdraw money
  changeBalance(form: NgForm, withdraw: boolean) {
    this.balanceShowMessage = false;

    //Prepare data
    let formData = form.value;                            //current form values
    formData.balance = this.account.balance;              //Add current balance
    formData.withdraw = withdraw;                         //Is withdraw
    
    //Send them to server
    this.http.post("/api/operation/balance", JSON.stringify(formData), { headers: Globals.getAuthHttpHeaders() })
      .subscribe(response => {
        //Success
        this.account = response;    //Update data

        //Clean field values
        if (withdraw)
          this.withdrawElement.nativeElement.value = "";            //(<HTMLInputElement>document.getElementById("withdraw")).value = "";
        else
          this.depositElement.nativeElement.value = "";             //(<HTMLInputElement>document.getElementById("deposit")).value = "";
      },
        err => {
          //Fail
          switch (err.status) {
            case 401:
              this.router.navigate(["/"]);    // if not authorized - redirect to login
              break;
            case 409: //not enough money
              this.balanceMessage = "Your balance must be positive.";
              this.account = err.error;   //updated data goes here
              break;
            case 422: //model error
              this.balanceMessage = err.error;
              break;
            default:
              this.balanceMessage = "Unable to change balance. Please try again later.";
              //console.log(err);
              break;
          }
          this.balanceShowMessage = true;
          //console.log(err);
        }
      );
  }

  //User wants to transfer money to another account
  transferMoney(form: NgForm) {
    this.balanceShowMessage = false;

    //Send them to server
    this.http.post("/api/operation/transfer", JSON.stringify(form.value), { headers: Globals.getAuthHttpHeaders() })
      .subscribe(response => {
        //Success
        this.account = response;    //Update data

        //Clean field values
        this.transferElement.nativeElement.value = "";
        this.transferReceiverElement.nativeElement.selectedIndex = -1;
      },
        err => {
          //Fail
          switch (err.status) {
            case 401:
              this.router.navigate(["/"]);    // if not authorized - redirect to login
              break;
            case 409: //not enough money
              this.balanceMessage = "Your balance must be positive.";
              this.account = err.error;   //updated data goes here
              break;
            case 422: //model error
              this.balanceMessage = err.error;    
              break;
            default:
              this.balanceMessage = "Unable to transfer. Please try again later.";
              //console.log(err);
              break;
          }
          this.balanceShowMessage = true;
          console.log(err);
        }
      );
  }
}
