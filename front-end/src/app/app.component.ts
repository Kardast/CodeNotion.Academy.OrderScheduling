import { Component, OnInit } from '@angular/core';
import { Order, OrderClient } from './api.service';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'order-scheduling-angular';
  list: Order[] = [];
  columnsToDisplay = ['id', 'customer', 'orderNumber', 'cuttingDate', 'preparationDate', 'bendingDate', 'assemblyDate'];
  filteredData: any[] = [];
  searchCustomer: string = '';
  searchOrderNumber: string = '';
  //Provides access to information about a route associated (private orderClient: OrderClient)
  constructor(private route: ActivatedRoute, private orderClient: OrderClient) { }

  ngOnInit(): void {
    // Read the data from the API
    this.fetchData();
  }

  fetchData() {
    // Fetch the data from the API using the service
    this.orderClient.list(this.searchCustomer, this.searchOrderNumber).subscribe((data: any) => {
      this.list = data;
      this.filterData();
    });
  }

  filterData() {
    // Apply the filter function to the raw data and update the filteredData array
    this.filteredData = this.list.filter(item => {
      if (item.customer?.includes(this.searchCustomer)) {
        return item.customer?.includes(this.searchCustomer);
      }
      return item.orderNumber?.includes(this.searchOrderNumber);
    });
  }
}
