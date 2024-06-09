import { Routes } from '@angular/router';
import { StockVisualizerComponent } from './stock-visualizer/stock-visualizer.component';
import { DataManagementComponent } from './data-management/data-management.component';

export const routes: Routes = [
    {
        path: '',
        component: StockVisualizerComponent,
        title: 'Stock Visualizer'
    },
    {
        path: 'data-management',
        component: DataManagementComponent,
        title: 'Data Management'
    }
];
