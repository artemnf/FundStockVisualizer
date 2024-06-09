import { Component, Input, OnChanges, SimpleChanges, inject } from '@angular/core';
import { Stock } from '../models/stock';
import { ApiService } from '../api.service';
import { AggregatedStockData } from '../models/aggregated-stock-data';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-aggregated-stock-data',
  standalone: true,
  imports: [MatTableModule, CommonModule],
  templateUrl: './aggregated-stock-data.component.html',
  styleUrl: './aggregated-stock-data.component.scss'
})


export class AggregatedStockDataComponent implements OnChanges {
  @Input() stock!: Stock;

  
  datasource = new MatTableDataSource<any>(); /*any[] = [{
    title: 'Test',
    y1y: 23,
    y2y: 2,
    y5y: 224
  }];*/

  displayedColumns = ['title', '1year', '2years', '5years'];

  private readonly apiService: ApiService = inject(ApiService);

  ngOnChanges(): void {
    this.getDataForTimeFrame(1);
    this.getDataForTimeFrame(2);
    this.getDataForTimeFrame(5);
  }

  getDataForTimeFrame(years: number) {
    this.apiService.getAggregatedStockData(this.stock.stockId, years)
      .then((x: AggregatedStockData) => {
        const key = `${years}y`;

        const newData = [...this.datasource.data];

        (newData[0] || (newData[0] = {title: 'Maximum Price'}))[key] = x.maxPrice;
        (newData[1] || (newData[1] = {title: 'Minimum Price'}))[key] = x.minPrice;
        (newData[2] || (newData[2] = {title: 'Average Price'}))[key] = x.avgPrice;
        (newData[3] || (newData[3] = {title: 'Average Volume'}))[key] = x.avgVolume;

        this.datasource.data = newData;
      })
  }
}
