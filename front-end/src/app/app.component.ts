import { Component } from '@angular/core';
import { combineLatest, Observable, Observer, switchMap } from 'rxjs';
import { OrderClient } from './api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  searchCustomer: string = '';
  searchOrderNumber: string = '';
  columnsToDisplay = ['id', 'customer', 'orderNumber', 'cuttingDate', 'preparationDate', 'bendingDate', 'assemblyDate'];
  customerFilterObserver!: Observer<string>;
  orderNumberFilterObserver!: Observer<string>;
  customerFilter$: Observable<string> = new Observable(observer => {
    this.customerFilterObserver = observer;
    observer.next()
  });
  orderNumberFilter$: Observable<string> = new Observable(observer => {
    this.orderNumberFilterObserver = observer;
    observer.next()
  });
  orders$ = combineLatest([this.customerFilter$, this.orderNumberFilter$]).pipe(
    switchMap(([customer, order]) => this.orderClient.list(customer ?? undefined, order ?? undefined))
  );

  constructor(private orderClient: OrderClient) {

  }

  searchCustomerKeyUp() {
    this.customerFilterObserver.next(this.searchCustomer);
  }

  searchOrderNumberKeyUp() {
    this.orderNumberFilterObserver.next(this.searchOrderNumber);
  }
}
