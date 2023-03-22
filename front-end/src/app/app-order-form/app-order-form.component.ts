import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Order, OrderClient } from '../api.service';
import { serializeDateOnly } from '../dateonly.utils';

@Component({
  selector: 'app-app-order-form',
  templateUrl: './app-order-form.component.html',
  styleUrls: ['./app-order-form.component.scss']
})
export class AppOrderFormComponent implements OnChanges {
  @Input() order: Order | null = null;
  @Output() onOrderUpdated = new EventEmitter<Order>();
  orderForm: FormGroup | null = null;

  constructor(private fb: FormBuilder, private orderClient: OrderClient) {
  }

  ngOnChanges(): void {
    this.buildForm();
  }

  buildForm() {
    this.orderForm = this.fb.group({
      id: [this.order?.id ?? 0, Validators.required],
      customer: [this.order?.customer, Validators.required],
      orderNumber: [this.order?.orderNumber, Validators.required],
      cuttingDate: [this.order?.cuttingDate],
      preparationDate: [this.order?.preparationDate],
      bendingDate: [this.order?.bendingDate],
      assemblyDate: [this.order?.assemblyDate]
    });
  }

  onSubmit() {
    if (!this.orderForm!.valid) {
      return;
    }

    const payload = Object.assign({}, this.orderForm!.getRawValue()) as Order;
    payload.cuttingDate = serializeDateOnly(payload.cuttingDate);
    payload.preparationDate = serializeDateOnly(payload.preparationDate);
    payload.bendingDate = serializeDateOnly(payload.bendingDate);
    payload.assemblyDate = serializeDateOnly(payload.assemblyDate);

    if (payload.id) {
      this.orderClient
        .update(payload)
        .subscribe(() => this.onOrderUpdated.emit(payload));
      return;
    }

    this.orderClient
      .createOrder(payload)
      .subscribe(() => this.onOrderUpdated.emit(payload));
  }
}
