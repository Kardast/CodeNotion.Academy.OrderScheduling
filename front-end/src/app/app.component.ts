import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, combineLatest, Observable, Subject, switchMap } from 'rxjs';
import { Order, OrderClient } from './api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  list: Order[] = [];
  searchCustomer: string = '';
  searchOrderNumber: string = '';
  columnsToDisplay = ['id', 'customer', 'orderNumber', 'cuttingDate', 'preparationDate', 'bendingDate', 'assemblyDate'];

  customerFilter$: Subject<string | null> = new BehaviorSubject<string | null>(null);
  searchOrderFilter$: Subject<string | null> = new BehaviorSubject<string | null>(null);
  orderApi$: Observable<Order[]> = new Subject<Order[]>();
  orders$ = combineLatest([this.customerFilter$, this.searchOrderFilter$]).pipe(
    switchMap(([customer, order]) => this.orderClient.list(customer ?? undefined, order ?? undefined)),
  );

  constructor(private orderClient: OrderClient) {
    this.orderApi$ = this.orderClient.list(this.searchCustomer, this.searchOrderNumber);
    this.orderApi$.subscribe();
  }

  searchOrderKeyUp() {
    this.searchOrderFilter$.next(this.searchOrderNumber);
  }

  searchCustomerKeyUp() {
    this.customerFilter$.next(this.searchCustomer);
  }
}
