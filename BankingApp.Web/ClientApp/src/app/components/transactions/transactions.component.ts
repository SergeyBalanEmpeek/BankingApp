import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from "@angular/router";
import * as Globals from '../../app.globals';

@Component({
  selector: 'page-transactions',
  templateUrl: './transactions.component.html'
})

export class TransactionsComponent implements OnInit {
  transactions: any;

  loadingMessage: string;
  loadingShowMessage: boolean;

  constructor(private router: Router, private http: HttpClient) { }

  ngOnInit() {
    //Load required data on page load
    this.http.get("/api/transaction", { headers: Globals.getAuthHttpHeaders() }).subscribe(
      response => { this.transactions = response; console.log(this.transactions) },
      err => {
        if (err.status == 401) this.router.navigate(["/"]);;      // if not authorized - redirect to login 
        this.loadingMessage = "Unable to load data";
        this.loadingShowMessage = true;
        console.log(err);
      }
    );
  }
}
