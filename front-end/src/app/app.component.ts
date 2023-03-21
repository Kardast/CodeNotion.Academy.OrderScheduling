import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject, combineLatest, Observable, Observer, switchMap } from 'rxjs';
import { Order, OrderClient } from './api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  searchCustomer: string = '';
  searchOrderNumber: string = '';
  orderForm!: FormGroup;
  orderId = 0;
  columnsToDisplay = ['id', 'customer', 'orderNumber', 'cuttingDate', 'preparationDate', 'bendingDate', 'assemblyDate', 'action'];

  searchFilter$ = new BehaviorSubject<{ customer?: string; orderNumber?: string }>({});
  orderCreate$ = new BehaviorSubject<Order | null>(null);
  orderUpdate$ = new BehaviorSubject<Order | null>(null);
  orderDelete$ = new BehaviorSubject<Order | null>(null);
  orders$ = combineLatest([this.searchFilter$, this.orderCreate$, this.orderUpdate$, this.orderDelete$])
    .pipe(switchMap(([filter]) => this.orderClient.list(filter.customer, filter.orderNumber)));

  constructor(private fb: FormBuilder, private orderClient: OrderClient) {
    this.clearOrderForm();
  }

  clearOrderForm() {
    this.orderForm = this.fb.group({
      id: [0, Validators.required],
      customer: [null, Validators.required],
      orderNumber: [null, Validators.required],
      cuttingDate: [null],
      preparationDate: [null],
      bendingDate: [null],
      assemblyDate: [null]
    });
  }

  fillUpdateForm(row: Order) {
    this.orderForm.setValue({ ...row });
    this.orderId = row.id ?? 0;
  }

  onSubmit() {
    if (!this.orderForm.valid) {
      this.clearOrderForm();
      return;
    }

    const payload = Object.assign({}, this.orderForm.getRawValue()) as Order;
    // payload.cuttingDate = serializeDateOnly(payload.cuttingDate);
    // payload.preparationDate = serializeDateOnly(payload.preparationDate);
    // payload.bendingDate = serializeDateOnly(payload.bendingDate);
    // payload.assemblyDate = serializeDateOnly(payload.assemblyDate);

    if (this.orderId) {
      this.orderClient
        .update(payload)
        .subscribe(() => this.orderUpdate$.next(payload));
        this.clearOrderForm();
        this.orderId = 0;
      return;
    }

    this.orderClient
      .createOrder(payload)
      .subscribe(() => this.orderCreate$.next(payload));

    this.clearOrderForm();
    this.orderId = 0;
  }

  deleteOrder(order: any) {
    if (!order?.id || order?.id === 0) {
      return;
    }
    this.orderClient
      .delete(order.id)
      .subscribe(() => this.orderDelete$.next(order));
  }

  searchCustomerKeyUp() {
    this.searchFilter$.next({ ...this.searchFilter$.value, customer: this.searchCustomer });
  }

  searchOrderNumberKeyUp() {
    this.searchFilter$.next({ ...this.searchFilter$.value, orderNumber: this.searchOrderNumber });
  }
}
