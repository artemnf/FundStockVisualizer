import { Routes } from '@angular/router';
import { StockVisualizerComponent } from './stock-visualizer/stock-visualizer.component';

export const routes: Routes = [
    {
        path: '',
        component: StockVisualizerComponent,
        title: 'Stock Visualizer'
    }
];
