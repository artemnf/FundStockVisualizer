import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { ApiService } from '../api.service';
import { CommonModule } from '@angular/common';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
@Component({
  selector: 'app-data-management',
  standalone: true,
  imports: [
    MatCardModule,
    MatButtonModule,
    MatSnackBarModule,
    CommonModule
  ],
  templateUrl: './data-management.component.html',
  styleUrl: './data-management.component.scss'
})
export class DataManagementComponent  {
  private readonly apiService: ApiService = inject(ApiService);
  private readonly snackBar: MatSnackBar = inject(MatSnackBar);

  latestLoadedDate?: Date;
  apiLoading: boolean = false;

  constructor() {
    this.apiService.getLatestLoadedDate().then((date: Date) => {
      this.latestLoadedDate = date;
    })
  }

  async loadData() {
    this.apiLoading = true;
    const result = await this.apiService.sendLoadDataCommand();
    this.apiLoading = false;
    this.latestLoadedDate = result.latestLoadedDate;

    if (result.failedSymbols.length > 0) {
      this.snackBar.open(`Failed to retrive data for following symbols: ${result.failedSymbols.join(', ')}`, "Dismiss", {
        duration: 5000
      });
    }
  }
}
