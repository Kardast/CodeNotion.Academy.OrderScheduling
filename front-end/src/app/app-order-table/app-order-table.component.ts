import { Component, Input } from '@angular/core';
import { BehaviorSubject, combineLatest, Observable, switchMap } from 'rxjs';
import { Order, OrderClient } from '../api.service';

@Component({
  selector: 'app-app-order-table',
  templateUrl: './app-order-table.component.html',
  styleUrls: ['./app-order-table.component.scss']
})
export class AppOrderTableComponent {
  @Input() focusedOrder = new BehaviorSubject<Order | null>(null);
  @Input() orderCreate$ = new BehaviorSubject<Order | null>(null);
  @Input() orderUpdate$ = new BehaviorSubject<Order | null>(null);

  searchCustomer: string = '';
  searchOrderNumber: string = '';
  columnsToDisplay = ['id', 'customer', 'orderNumber', 'cuttingDate', 'preparationDate', 'bendingDate', 'assemblyDate', 'action'];

  searchFilter$ = new BehaviorSubject<{ customer?: string; orderNumber?: string }>({});
  orderDelete$ = new BehaviorSubject<Order | null>(null);
  orders$ = new Observable<Order[]>;

  ngOnChanges(): void {
    this.orders$ = combineLatest([this.searchFilter$, this.orderCreate$, this.orderUpdate$, this.orderDelete$])
      .pipe(switchMap(([filter]) => this.orderClient.list(filter.customer, filter.orderNumber)));
  }

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
}
