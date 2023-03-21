import { Component, ViewChild } from '@angular/core';
import { BehaviorSubject, combineLatest, switchMap } from 'rxjs';
import { Order, OrderClient } from './api.service';
import { AppOrderFormComponent } from './app-order-form/app-order-form.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  @ViewChild(AppOrderFormComponent) childComponent!: AppOrderFormComponent;
  searchCustomer: string = '';
  searchOrderNumber: string = '';
  columnsToDisplay = ['id', 'customer', 'orderNumber', 'cuttingDate', 'preparationDate', 'bendingDate', 'assemblyDate', 'action'];

  searchFilter$ = new BehaviorSubject<{ customer?: string; orderNumber?: string }>({});
  orderCreate$ = new BehaviorSubject<Order | null>(null);
  orderUpdate$ = new BehaviorSubject<Order | null>(null);
  orderDelete$ = new BehaviorSubject<Order | null>(null);
  orders$ = combineLatest([this.searchFilter$, this.orderCreate$, this.orderUpdate$, this.orderDelete$])
    .pipe(switchMap(([filter]) => this.orderClient.list(filter.customer, filter.orderNumber)));

  constructor(private orderClient: OrderClient) { }

  triggerChildFunction(row: Order) {
    this.childComponent.fillUpdateForm(row);
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

  ///
  /// FROM ORDER FORM COMPONENT
  ///
  handLeOrderManager(payload: Order) {
    if (payload.id) {
      this.orderUpdate$.next(payload);
    }

    this.orderCreate$.next(payload);
  }
}
