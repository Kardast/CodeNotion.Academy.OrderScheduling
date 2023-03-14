import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { API_BASE_URL } from './api.service';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from "@angular/router/testing";

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule, HttpClientModule, BrowserAnimationsModule, MatTableModule,
    MatPaginatorModule, MatInputModule, FormsModule, ReactiveFormsModule, RouterTestingModule
  ],
  providers: [{ provide: API_BASE_URL, useValue: "http://localhost:5075" }],
  bootstrap: [AppComponent],
})
export class AppModule { }
