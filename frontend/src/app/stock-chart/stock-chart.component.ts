import { Component, Input, OnChanges, OnInit, SimpleChanges, inject } from '@angular/core';
import { Stock } from '../models/stock';
import { ApiService } from '../api.service';
import { ChartModule, StockChart} from 'angular-highcharts';
import { HistporicalPrice } from '../models/historical-price';

@Component({
  selector: 'app-stock-chart',
  standalone: true,
  imports: [ChartModule],
  templateUrl: './stock-chart.component.html',
  styleUrl: './stock-chart.component.scss'
})
export class StockChartComponent implements OnInit, OnChanges {
  
  @Input() stock!: Stock;

  private readonly apiService: ApiService = inject(ApiService);

  stockChart?: StockChart;

  ngOnInit() {
    
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.apiService.getStockHistoricalPrices(this.stock.stockId).then((historicalPrices : HistporicalPrice[]) => {
      this.stockChart = new StockChart({
        rangeSelector: {
          selected: 1
        },
        title: {
          text: `${this.stock.symbol.toUpperCase()} Stock Price`
        },
        series: [
          {
            tooltip: {
              valueDecimals: 2
            },
            name: this.stock.symbol.toUpperCase(),
            type: 'line',
            data: historicalPrices.map(p => [p.millisecondsUnixEpoch, p.price])
          }
        ]
      });
    })
  }


}
