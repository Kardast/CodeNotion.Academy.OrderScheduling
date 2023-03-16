import { Component } from '@angular/core';
import { BehaviorSubject, combineLatest, filter, Observable, Subject, switchMap, tap } from 'rxjs';
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
  orders$ = combineLatest([this.customerFilter$, this.searchOrderFilter$]).pipe(
    switchMap(([customer, order]) => this.orderClient.list(customer ?? undefined, order ?? undefined))
  );

  constructor(private orderClient: OrderClient) {
  }

  searchOrderKeyUp() {
    this.searchOrderFilter$.next(this.searchOrderNumber);
  }

  searchCustomerKeyUp() {
    this.customerFilter$.next(this.searchCustomer);
  }
}
