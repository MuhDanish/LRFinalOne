import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GenericServiceService } from 'src/app/common/services/generic-service.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class LayoutComponent implements OnInit {

  constructor(private route: Router, public sevice: GenericServiceService) {
    this.sevice.setHeaderName('DashBoard');
  }
  userName = "";
  currentDate = new Date();

  ngOnInit(): void {
    var token = environment.GET_TOKEN();
    this.userName = environment.GET_USERNAME() || '';
    if (token == '' || token == null) {
      window.location.href = '/#/login';
    }
  }
}
