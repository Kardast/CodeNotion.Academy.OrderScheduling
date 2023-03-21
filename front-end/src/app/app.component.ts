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
  columnsToDisplay = ['id', 'customer', 'orderNumber', 'cuttingDate', 'preparationDate', 'bendingDate', 'assemblyDate'];

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

  fillUpdateForm(row: Order) {
    this.orderForm.patchValue({
      id: row.id,
      customer: row.customer,
      orderNumber: row.orderNumber,
      cuttingDate: row.cuttingDate ? new Date(row.cuttingDate) : null,
      preparationDate: row.preparationDate ? new Date(row.preparationDate) : null,
      bendingDate: row.bendingDate ? new Date(row.bendingDate) : null,
      assemblyDate: row.assemblyDate ? new Date(row.assemblyDate) : null
    });
    this.orderId = row.id ?? 0;
  }

  onSubmit() {
    if (!this.orderForm.valid) {
      this.clearOrderForm();
      return;
    }

    const payload = Object.assign({}, this.orderForm.getRawValue()) as Order;
    if (this.orderId) {
      this.orderClient
        .update(this.orderId, payload)
        .subscribe(() => this.orderUpdate$.next(payload));
    }
    else {
      this.orderClient
        .createOrder(payload)
        .subscribe(() => this.orderCreate$.next(payload));
    }
    this.clearOrderForm();
  }

  deleteOrder(listOrder : any){
    console.log(listOrder.id);
    this.orderClient.delete(listOrder.id).subscribe(() => this.orderDelete$.next(listOrder));
  }
}
