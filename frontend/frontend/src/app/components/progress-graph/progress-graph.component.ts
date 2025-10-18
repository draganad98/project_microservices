import { Component, Input, OnChanges, SimpleChanges, ViewChild, ElementRef } from '@angular/core';
import { Chart } from 'chart.js';

export interface Attempt {
  id: number;
  score: number;
  startedAt: string;
}

@Component({
  selector: 'app-progress-graph',
  templateUrl: './progress-graph.component.html',
  styleUrls: ['./progress-graph.component.css']
})
export class ProgressGraphComponent implements OnChanges {
  @Input() attempts: Attempt[] = [];
  @ViewChild('progressCanvas') progressCanvas!: ElementRef<HTMLCanvasElement>;

  private chart: Chart | null = null;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['attempts'] && this.attempts.length > 0) {
      console.log('[ProgressGraph] attempts changed:', this.attempts);
      this.createChart();
    }
  }

  private createChart(): void {
    if (!this.progressCanvas) return;

    const ctx = this.progressCanvas.nativeElement.getContext('2d');
    if (!ctx) return;

    
    if (this.chart) this.chart.destroy();

    const labels = this.attempts.map(a => new Date(a.startedAt).toLocaleString());
    const scores = this.attempts.map(a => a.score);

    this.chart = new Chart(ctx, {
      type: 'line',
      data: {
        labels: labels,
        datasets: [{
          label: 'Score',
          data: scores,
          borderColor: 'blue',
          backgroundColor: 'rgba(0,0,255,0.2)',
          fill: false,
          tension: 0.3,
          pointRadius: 5,
          pointBackgroundColor: 'blue'
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: true },
          title: { display: true, text: 'Progress over Attempts' }
        },
        scales: {
          y: { beginAtZero: true },
          x: { title: { display: true, text: 'Attempts' } }
        }
      }
    });
  }
}

