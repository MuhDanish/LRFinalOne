import { Component, OnInit } from '@angular/core';
import { GenericServiceService } from 'src/app/common/services/generic-service.service';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-ads-list',
  templateUrl: './ads-list.component.html',
  styleUrls: ['./ads-list.component.css']
})
export class AdsListComponent implements OnInit {

  constructor(public service: GenericServiceService, private route: Router, private modalService: NgbModal) {
    this.service.setHeaderName('Ads List');
    this.getAdsList();
  }

  adsList: any;
  api_url = environment.API_FILE_URL;
  canExportgroups = false;
  list_name = '';
  totalRecords = 0;
  ngOnInit(): void {
  }
  getAdsList() {
    this.service.get('Ads/GetAdsList', '')
      .subscribe(resp => {
        this.adsList = resp.data;
        this.totalRecords = this.adsList.length;
        this.service.setLoading(false);
      });
  }
  EditAds(data: any) {
    this.route.navigate(['/add_ads', data.data.adId]);
  }
  DeleteAds(data: any) {
    if (confirm('Are you sure you want to delete this Ad.')) {
      var adddata = { AdId: data.data.adId };
      this.service.post('Ads/DeleteAds', adddata).subscribe(res => {
        this.service.setLoading(false);
        if (this.service.IsSaveData(res)) {
          this.getAdsList();
        }
      });
    }
  }
}


