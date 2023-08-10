import { Component, OnInit, ViewChild } from '@angular/core';
import { GenericServiceService } from 'src/app/common/services/generic-service.service';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
@Component({
  selector: 'app-facility',
  templateUrl: './facility.component.html',
  styleUrls: ['./facility.component.css']
})
export class FacilityComponent implements OnInit {
  facilityModal: FormGroup = new FormGroup({});
  constructor(public service: GenericServiceService, private modalService: NgbModal, private fb: FormBuilder, private route: Router) {
    this.service.setHeaderName('Facility');
    this.getFacilityList();
  }

  facilitiesList: any;
  total_facility: any;
  public temp_var: Object = false;
  button_text = "";

  displayStyle = "none";
  openPopup(content: any) {
    this.button_text = "Add";
    this.resetModal();
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' });
  }
  closePopup() {
    this.modalService.dismissAll();
  }

  get f() {
    return this.facilityModal.controls;
  }

  ngOnInit(): void {
    this.facilityModal = this.fb.group({
      FacilityId: new FormControl(0),
      FacilityName: new FormControl('', [Validators.required]),
      Image: new FormControl(''),

    });
    this.getFacilityList();
  }

  resetModal() {
    this.facilityModal.reset();
    this.facilityModal.setValue({
      FacilityId: 0,
      FacilityName: '',
    });
  }
  getFacilityList() {
    this.temp_var = false;
    this.service.get('Settings/GetFacilityList', '').subscribe(res => {
      this.facilitiesList = res.successList;
      this.total_facility = this.facilitiesList.length;
      this.temp_var = true;
      this.service.setLoading(false);
    });
  }


  EditFac(data: any) {
    this.route.navigate(['/add-facility/', data.data.facilityId]);
  }


  DeleteFacility(data: any) {
    if (confirm('Are you sure you want to delete this Facility.')) {
      var facdata = { FacilityId: data.facilityId };
      this.service.post('Settings/DeleteFacility', facdata).subscribe(res => {
        this.service.setLoading(false);
        if (this.service.IsSaveData(res)) {
          this.getFacilityList();
        }
      });
    }
  }


}
