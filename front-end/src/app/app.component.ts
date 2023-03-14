import { Component, OnInit } from '@angular/core';
import { Order, OrderClient } from './api.service';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'order-scheduling-angular';
  list: Order[] = [];
  // constructor(private orderClient: OrderClient) {
  //   orderClient.list().subscribe(list => this.list = list);
  // }
  columnsToDisplay = ['id', 'customer', 'orderNumber', 'cuttingDate', 'preparationDate', 'bendingDate', 'assemblyDate'];

  filteredData: any[] = []; // This will hold the filtered data
  searchTerm: string = ''; // This will hold the search term
  constructor(private route: ActivatedRoute, private orderClient: OrderClient) { }
  ngOnInit(): void{
    // Read the data from the API
    this.fetchData();
    // Subscribe to changes in the query parameters
    this.route.queryParams.subscribe(params => {
      this.searchTerm = params['searchTerm'] || '';
      this.filterData();
    });

    // Filter the data initially
    this.filterData();
  }

  fetchData() {
    // Fetch the data from the API using the service
    this.orderClient.list(this.searchTerm).subscribe((data: any) => {
      this.list = data;
      this.filterData();
    });
  }

  filterData() {
    // Apply the filter function to the raw data and update the filteredData array
    this.filteredData = this.list.filter(item => {
      return item.customer?.includes(this.searchTerm);
    });
  }


  log(){
    console.log(this.filteredData);

  }

  // searchTerm: string = '';

  // filterTableData(tableData: any[], searchTerm: string): any[] {
  //   if (!searchTerm) {
  //     return tableData;
  //   }
  //   searchTerm = searchTerm.toLowerCase();
  //   return tableData.filter(item => {
  //     return item.customer.toLowerCase().indexOf(searchTerm) !== -1;
  //   });
  // }
}
