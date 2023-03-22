import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Order, OrderClient } from '../api.service';
import { serializeDateOnly } from '../dateonly.utils';

@Component({
  selector: 'app-app-order-form',
  templateUrl: './app-order-form.component.html',
  styleUrls: ['./app-order-form.component.scss']
})
export class AppOrderFormComponent {
  @Output() orderManagerOutput = new EventEmitter<Order>();

  orderForm!: FormGroup;

  constructor(private fb: FormBuilder, private orderClient: OrderClient) {
    this.clearOrderForm();
  }

  clearOrderForm() {
    this.orderForm = this.fb.group({
      id: [0, Validators.required],
      customer: [null, Validators.required],
      orderNumber: [null, Validators.required],
      cuttingDate: [null],
      preparationDate: [null],
      bendingDate: [null],
      assemblyDate: [null]
    });
  }

  fillUpdateForm(row: Order) {
    if (!row.id) {
      return;
    }

    this.orderForm.setValue({ ...row });
  }

  onSubmit() {
    if (!this.orderForm.valid) {
      this.clearOrderForm();
      return;
    }

    const payload = Object.assign({}, this.orderForm.getRawValue()) as Order;
    payload.cuttingDate = serializeDateOnly(payload.cuttingDate);
    payload.preparationDate = serializeDateOnly(payload.preparationDate);
    payload.bendingDate = serializeDateOnly(payload.bendingDate);
    payload.assemblyDate = serializeDateOnly(payload.assemblyDate);

    if (payload.id) {
      this.orderClient
        .update(payload)
        .subscribe(() => this.orderManagerOutput.emit(payload));
      this.clearOrderForm();
      return;
    }

    this.orderClient
      .createOrder(payload)
      .subscribe(() => this.orderManagerOutput.emit(payload));

    this.clearOrderForm();
  }
}
