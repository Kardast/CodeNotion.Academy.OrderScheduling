import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { combineLatest, Observable, Observer, switchMap } from 'rxjs';
import { OrderClient } from './api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  searchCustomer: string = '';
  searchOrderNumber: string = '';
  orderForm!: FormGroup;
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

  constructor(private fb: FormBuilder, private orderClient: OrderClient) {}

  ngOnInit(): void {
    this.clearOrderForm();
  }

  clearOrderForm(){
    this.orderForm = this.fb.group({
      // Define your form fields here
      customer: ['', Validators.required],
      orderNumber: ['', Validators.required],
      cuttingDate: ['', Validators.required],
      preparationDate: ['', Validators.required],
      bendingDate: ['', Validators.required],
      assemblyDate: ['', Validators.required],
    });
  }

  searchCustomerKeyUp() {
    this.customerFilterObserver.next(this.searchCustomer);
  }

  searchOrderNumberKeyUp() {
    this.orderNumberFilterObserver.next(this.searchOrderNumber);
  }

  onSubmit(){
    console.log(this.orderForm);
    if(this.orderForm.valid){
      let formData = {
        customer: this.orderForm.value.customer,
        orderNumber: this.orderForm.value.orderNumber,
        cuttingDate: this.orderForm.value.cuttingDate,
        preparationDate: this.orderForm.value.preparationDate,
        bendingDate: this.orderForm.value.bendingDate,
        assemblyDate: this.orderForm.value.assemblyDate
      };

      this.orderClient.createOrder(formData).subscribe(
        response => console.log(response),
        error => console.log(error)
      );

    }
    this.clearOrderForm();
  }
}
