import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { combineLatest, BehaviorSubject, switchMap } from 'rxjs';
import { Order, OrderClient } from './api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  searchCustomer: string = '';
  searchOrderNumber: string = '';
  inputType = ["text", "text", "date", "date", "date", "date"];
  orderForm!: FormGroup;
  columnsToDisplay = ['id', 'customer', 'orderNumber', 'cuttingDate', 'preparationDate', 'bendingDate', 'assemblyDate'];
  
  searchFilter$ = new BehaviorSubject<{ customer?: string; orderNumber?: string }>({});
  
  orderCreate$ = new BehaviorSubject<Order | null>(null);

  orders$ = combineLatest([this.searchFilter$, this.orderCreate$])
    .pipe(switchMap(([filter]) => this.orderClient.list(filter.customer, filter.orderNumber)));

  constructor(private fb: FormBuilder, private orderClient: OrderClient) {
    this.clearOrderForm();
  }

  clearOrderForm() {
    this.orderForm = this.fb.group({
      customer: [null, Validators.required],
      orderNumber: [null, Validators.required],
      cuttingDate: [null],
      preparationDate: [null],
      bendingDate: [null],
      assemblyDate: [null]
    });
  }

  searchCustomerKeyUp() {
    this.searchFilter$.next({ ...this.searchFilter$.value, customer: this.searchCustomer });
  }

  searchOrderNumberKeyUp() {
    this.searchFilter$.next({ ...this.searchFilter$.value, orderNumber: this.searchOrderNumber });
  }

  onSubmit() {
    if (!this.orderForm.valid) {
      this.clearOrderForm();
      return;
    }

    const payload = Object.assign({}, this.orderForm.getRawValue()) as Order;
    this.orderClient
      .createOrder(payload)
      .subscribe(() => this.orderCreate$.next(payload));

    this.clearOrderForm();
  }
}
