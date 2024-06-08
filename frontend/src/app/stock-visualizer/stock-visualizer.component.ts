import { Component, inject } from '@angular/core';

import { ReactiveFormsModule, FormBuilder, Validators, FormControl } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatAutocompleteModule, MatOption } from '@angular/material/autocomplete';
import { ApiService } from '../api.service';
import { Stock } from '../models/stock';
import { Observable, debounceTime, startWith, switchMap } from 'rxjs';
import { CommonModule } from '@angular/common';
import { StockChartComponent } from '../stock-chart/stock-chart.component';


@Component({
  selector: 'app-stock-visualizer',
  templateUrl: './stock-visualizer.component.html',
  styleUrl: './stock-visualizer.component.scss',
  standalone: true,
  imports: [
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatAutocompleteModule,
    MatOption,
    ReactiveFormsModule,
    CommonModule,
    StockChartComponent
  ]
})
export class StockVisualizerComponent {
  private readonly apiService: ApiService = inject(ApiService)
  private fb = inject(FormBuilder);
  symbolSearch = new FormControl<Stock | string>('', Validators.required)
  searchForm = this.fb.group({
    symbol: this.symbolSearch,
  });

  stocks: Stock[] = []
  stocks$: Observable<Stock[]>;
  selectedStock?: Stock;

  constructor() {
    this.stocks$ = this.symbolSearch.valueChanges.pipe(
      // startWith(''),
       debounceTime(500),
       switchMap(value => {
        const term = typeof value === 'string' ? value : value?.symbol;
        return this.apiService.getFundStocks(1, term)
       })
     );
  }

  onSubmit(): void {
     this.selectedStock = this.instanceOfStock(this.symbolSearch.value) ? this.symbolSearch.value : undefined;
  }

  displayFn(stock: Stock): string {
    return stock && stock.symbol ? stock.symbol : '';
  }

  instanceOfStock(object: any): object is Stock {
    return (object as Stock).stockId !== undefined && (object as Stock).symbol !== undefined;
  }
}
