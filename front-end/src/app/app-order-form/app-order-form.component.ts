import { Component, Input, OnChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { Order, OrderClient } from '../api.service';
import { serializeDateOnly } from '../dateonly.utils';

@Component({
  selector: 'app-app-order-form',
  templateUrl: './app-order-form.component.html',
  styleUrls: ['./app-order-form.component.scss']
})
export class AppOrderFormComponent implements OnChanges {
  @Input() focusedOrder = new BehaviorSubject<Order | null>(null);
  @Input() orderCreate$ = new BehaviorSubject<Order | null>(null);
  @Input() orderUpdate$ = new BehaviorSubject<Order | null>(null);

  orderForm: Observable<FormGroup | null> | null = null;

  constructor(private fb: FormBuilder, private orderClient: OrderClient) {
  }

  ngOnChanges(): void {
    this.orderForm = this.focusedOrder.pipe(
      map(order => this.buildForm(order!)));
  }

  buildForm(order: Order | null): FormGroup {
    return this.fb.group({
      id: [order?.id ?? 0, Validators.required],
      customer: [order?.customer, Validators.required],
      orderNumber: [order?.orderNumber, Validators.required],
      cuttingDate: [order?.cuttingDate],
      preparationDate: [order?.preparationDate],
      bendingDate: [order?.bendingDate],
      assemblyDate: [order?.assemblyDate]
    });
  }

  onSubmit(orderForm: FormGroup) {
    if (!orderForm!.valid) {
      return;
    }

    const payload = Object.assign({}, orderForm!.getRawValue()) as Order;
    payload.cuttingDate = serializeDateOnly(payload.cuttingDate);
    payload.preparationDate = serializeDateOnly(payload.preparationDate);
    payload.bendingDate = serializeDateOnly(payload.bendingDate);
    payload.assemblyDate = serializeDateOnly(payload.assemblyDate);

    if (payload.id) {
      this.orderClient
        .update(payload)
        .subscribe(() => this.orderUpdate$.next(payload));
      this.focusedOrder.next(null);
      return;
    }

    this.orderClient
      .createOrder(payload)
      .subscribe(() => this.orderCreate$.next(payload));
    this.focusedOrder.next(null);
  }
}
