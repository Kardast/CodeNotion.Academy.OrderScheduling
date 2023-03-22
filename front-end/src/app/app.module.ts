import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { API_BASE_URL } from './api.service';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { AppOrderFormComponent } from './app-order-form/app-order-form.component';
import { AppOrderTableComponent } from './app-order-table/app-order-table.component';

@NgModule({
  declarations: [
    AppComponent,
    AppOrderFormComponent,
    AppOrderTableComponent
  ],
  imports: [
    BrowserModule, HttpClientModule, BrowserAnimationsModule, MatTableModule,
    MatPaginatorModule, MatInputModule, FormsModule, ReactiveFormsModule, MatButtonModule,
    MatFormFieldModule, MatDatepickerModule, MatNativeDateModule, MatIconModule
  ],
  providers: [{ provide: API_BASE_URL, useValue: "http://localhost:5075" }, {provide: MAT_DATE_LOCALE, useValue: 'af'}],
  bootstrap: [AppComponent],
})
export class AppModule { }
