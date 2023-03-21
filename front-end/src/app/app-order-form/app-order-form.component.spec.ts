import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppOrderFormComponent } from './app-order-form.component';

describe('AppOrderFormComponent', () => {
  let component: AppOrderFormComponent;
  let fixture: ComponentFixture<AppOrderFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AppOrderFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AppOrderFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
