import { Component, OnInit } from '@angular/core';
import { GenericServiceService } from 'src/app/common/services/generic-service.service';
@Component({
  selector: 'app-helpandsupport',
  templateUrl: './helpandsupport.component.html',
  styleUrls: ['./helpandsupport.component.css']
})
export class HelpandsupportComponent implements OnInit {
  helpandsupportdata: any;
  total_data: any;
  innerHTML: string = 'Help & Support';
  constructor(public service: GenericServiceService) {
    this.service.setHeaderName('Help & Support');
  }

  ngOnInit(): void {
    this.getAllHelpAndSupportData();
  }


  getAllHelpAndSupportData() {
    this.service.get('Customer/CustomerHelpAndSupportMessageList', '').subscribe(res => {
      this.helpandsupportdata = res.data;
      this.total_data = this.helpandsupportdata.length;
      this.service.setLoading(false);
    });
  }
}
