import { Component } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Order } from './api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  focusedOrder = new BehaviorSubject<Order | null>(null);
  orderCreate$ = new BehaviorSubject<Order | null>(null);
  orderUpdate$ = new BehaviorSubject<Order | null>(null);
}
