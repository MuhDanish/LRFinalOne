import { Component, OnInit } from '@angular/core';
import { GenericServiceService } from 'src/app/common/services/generic-service.service';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormControl, FormGroup, Validators } from '@angular/forms';
@Component({
  selector: 'app-partner-list',
  templateUrl: './partner-list.component.html',
  styleUrls: ['./partner-list.component.css']
})
export class PartnerListComponent implements OnInit {

  constructor(public service: GenericServiceService, private route: Router, private modalService: NgbModal) {
    this.service.setHeaderName('Partner List');
    this.getPartnerList();
  }


  partnerList: any;
  api_url = environment.API_FILE_URL;
  canExportgroups = false;
  list_name = '';
  totalRecords = 0;
  ngOnInit(): void {
  }


  getPartnerList() {
    this.service.get('Partners/GetPartnerList', '')
      .subscribe(resp => {
        this.partnerList = resp.successList;
        this.totalRecords = this.partnerList.length;
        this.service.setLoading(false);
      });
  }

  EditPartner(data: any) {
    this.route.navigate(['/add_partner', data.data.partnerId]);
  }
  DeletePartner(data: any) {
    if (confirm('Are you sure you want to delete this Partner.')) {
      var partnerdata = { PartnerId: data.data.partnerId };
      this.service.post('Partners/DeletePartner', partnerdata).subscribe(res => {
        this.service.setLoading(false);
        if (this.service.IsSaveData(res)) {
          this.getPartnerList();
        }
      });
    }
  }
  openPopup(data: any, content: any) {
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' });
    this.partnerModal.setValue({
      FirstName: data.firstName,
      LastName: data.lastName,
      Email: data.email,
      Address: data.address,
      City: data.city,
      State: data.state,
      ZipCode: data.zipCode,
      Phone: data.phone
    });
  }
  closePopup() {
    this.modalService.dismissAll();
  }
  partnerModal = new FormGroup({
    FirstName: new FormControl(''),
    LastName: new FormControl(''),
    Email: new FormControl(''),
    Address: new FormControl(''),
    City: new FormControl(''),
    State: new FormControl(''),
    ZipCode: new FormControl(''),
    Phone: new FormControl(''),
  });
  get f() {
    return this.partnerModal.controls;
  }

}
