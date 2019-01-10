import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { TransactionsComponent } from './components/transactions/transactions.component';

import { MainLayoutComponent } from './layout/main/main.component';
import { MiniLayoutComponent } from './layout/mini/mini.component';
import { MenuComponent } from './components/menu/menu.component';

import { AuthService } from './services/auth.service';

@NgModule({
  declarations: [
    AppComponent,
    MainLayoutComponent,
    MiniLayoutComponent,
    MenuComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    TransactionsComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,

    RouterModule.forRoot([
      
      //Mini Layout for guests
      {
        path: '',
        component: MiniLayoutComponent,
        children: [
          { path: '', component: LoginComponent },
          { path: 'register', component: RegisterComponent },
        ]
      },

      //Main Layout for logged in users
      {
        path: '',
        component: MainLayoutComponent,
        canActivate: [AuthService],
        children: [
          { path: 'home', component: HomeComponent },
          { path: 'transactions', component: TransactionsComponent },
        ]
      },

      //Unknown pages - redirect to root
      { path: '**', redirectTo: '' },
    ])
  ],
  providers: [AuthService],
  bootstrap: [AppComponent]
})
export class AppModule { }
