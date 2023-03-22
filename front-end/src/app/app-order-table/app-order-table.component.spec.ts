import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppOrderTableComponent } from './app-order-table.component';

describe('AppOrderTableComponent', () => {
  let component: AppOrderTableComponent;
  let fixture: ComponentFixture<AppOrderTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AppOrderTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AppOrderTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
