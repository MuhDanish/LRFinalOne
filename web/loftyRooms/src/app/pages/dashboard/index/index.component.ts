import { Component, OnInit } from '@angular/core';
// import { Chart, registerables } from 'chart.js';
import Chart from 'chart.js/auto';
import { GenericServiceService } from 'src/app/common/services/generic-service.service';
import { BarController, BarElement, CategoryScale, LinearScale, Title } from 'chart.js';
Chart.register(BarController, BarElement, CategoryScale, LinearScale, Title);


@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})

export class IndexComponent implements OnInit {
  constructor(public service: GenericServiceService) {
    this.service.setHeaderName('DashBoard');
  }
  public chart: any;
  pieChartData: any;
  statusData: any;
  activeCount: number = 0;
  bookedCount: number = 0;
  pastCount: number = 0;
  rejectCount: number = 0;
  pieChartCount: number[] | undefined;
  pieChartLabel: string[] | undefined;

  bookingList: any;
  rejectedbookingList: any;
  total_booking: any;
  total_rejectedbooking: any;
  ngOnInit() {
    this.getAllDashBoardData();
    const canvas: HTMLCanvasElement = document.getElementById('myChart') as HTMLCanvasElement;
    const ctx = canvas.getContext('2d')!;
  }


  createChart() {
    this.chart = new Chart("MyChart", {
      type: 'doughnut',
      data: {// values on X-Axis
        // labels:  [this.pieChartLabel],
        labels: this.pieChartLabel,
        datasets: [{
          label: 'My First Dataset',
          // data: this.pieChartCount,
          data: this.pieChartCount,
          backgroundColor: [
            '#baa77f',
            '#254e70',
            '#9fa896',
            '#8ba69f'
          ],
          hoverOffset: 4
        }],
      },
      options: {
        aspectRatio: 2.5
      }

    });
  }
  getAllDashBoardData() {
    this.service.get('DashBoard/DashBoardData', '').subscribe(res => {
      this.activeCount = res.data.activeCount;
      this.bookedCount = res.data.bookedCount;
      this.pastCount = res.data.pastCount;
      this.rejectCount = res.data.rejectCount;
      this.pieChartData = res.data.pieChartData;
      this.pieChartLabel = res?.data.pieChartData.map((s: { label: any; }) => s.label);
      this.pieChartCount = res?.data.pieChartData.map((s: { count: any; }) => s.count);
      this.bookingList = res?.data.totalBooking;
      this.rejectedbookingList = res?.data.customerRejectedBooking;
      this.total_booking = this.bookingList.length;
      this.total_rejectedbooking = this.rejectedbookingList.length;
      this.service.setLoading(false);
      this.createChart();
    });
  }
  checkStatus(data: any) {
    var res = null;
    if (data.data.status == 1) {
      this.statusData = "Active";
    }
    if (data.data.status == 2) {
      this.statusData = "Booked";
    }
    if (data.data.status == 3) {
      this.statusData = "Past";
    }
    if (data.data.status == 4) {
      this.statusData = "Rejected";
    }
    return this.statusData;
  }
}
