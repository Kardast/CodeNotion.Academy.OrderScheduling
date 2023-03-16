import { Component, OnInit } from '@angular/core';
import { Order, OrderClient } from './api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'order-scheduling-angular';
  list: Order[] = [];
  filteredData: any[] = [];
  searchCustomer: string = '';
  searchOrderNumber: string = '';
  columnsToDisplay = ['id', 'customer', 'orderNumber', 'cuttingDate', 'preparationDate', 'bendingDate', 'assemblyDate'];

  constructor(private orderClient: OrderClient) { }

  ngOnInit(): void {
    this.fetchData();
  }

  fetchData() {
    this.orderClient.list(this.searchCustomer, this.searchOrderNumber).subscribe((data: any) => {
      this.list = data;
      this.filteredData = this.list;
    });
  }
}
