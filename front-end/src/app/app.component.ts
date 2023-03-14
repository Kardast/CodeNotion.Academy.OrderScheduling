import { Component } from '@angular/core';
import { Order, OrderClient } from './api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'order-scheduling-angular';
  list: Order[] = [];
  constructor(private orderClient:OrderClient){
    orderClient.list().subscribe(list=>this.list = list);
  }
}
