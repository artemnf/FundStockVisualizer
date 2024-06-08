import { Injectable } from '@angular/core';
import { Stock } from './models/stock';
import { HistporicalPrice } from './models/historical-price';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  readonly baseUrl = "http://localhost:5092";
  
  async getFundStocks(fundId: number, searchTerm? : string): Promise<Stock[]> {
    const data = await fetch(`${this.baseUrl}/funds/${fundId}/stocks` + (searchTerm ? `?searchTerm=${searchTerm}` : ''));
    return (await data.json()) ?? [];
  }

  async getStockHistoricalPrices(stockId: number):Promise<HistporicalPrice[]> {
    const data = await fetch(`${this.baseUrl}/stocks/${stockId}/historical-prices`);
    return (await data.json() ?? []);
  }
}
