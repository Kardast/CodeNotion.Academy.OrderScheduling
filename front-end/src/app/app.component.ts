import { Component } from '@angular/core';
import { BehaviorSubject, combineLatest, switchMap } from 'rxjs';
import { Order, OrderClient } from './api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  focusedOrder = new BehaviorSubject<Order | null>(null);
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

  deleteOrder(order: any, event: MouseEvent) {
    event.stopPropagation();

    if (!order?.id || order?.id === 0) {
      return;
    }

    this.orderClient
      .delete(order.id)
      .subscribe(() => this.orderDelete$.next(order));

    this.focusedOrder.next(null);
  }

  searchCustomerKeyUp() {
    this.searchFilter$.next({ ...this.searchFilter$.value, customer: this.searchCustomer });
  }

  searchOrderNumberKeyUp() {
    this.searchFilter$.next({ ...this.searchFilter$.value, orderNumber: this.searchOrderNumber });
  }

  refresh(payload: Order) {
    if (payload.id) {
      this.orderUpdate$.next(payload);
      return;
    }

    this.orderCreate$.next(payload);
  }
}
