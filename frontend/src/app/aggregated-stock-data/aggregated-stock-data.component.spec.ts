import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AggregatedStockDataComponent } from './aggregated-stock-data.component';

describe('AggregatedStockDataComponent', () => {
  let component: AggregatedStockDataComponent;
  let fixture: ComponentFixture<AggregatedStockDataComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AggregatedStockDataComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AggregatedStockDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
