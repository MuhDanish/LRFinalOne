import { Component, OnInit, ViewChild } from '@angular/core';
import { GenericServiceService } from 'src/app/common/services/generic-service.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-partner-notifications',
  templateUrl: './partner-notifications.component.html',
  styleUrls: ['./partner-notifications.component.css']
})
export class PartnerNotificationsComponent implements OnInit {

  constructor(public service: GenericServiceService, private modalService: NgbModal) {
    this.service.setHeaderName('Partner Notifications');
    this.getAllPartnerNotificationList();
  }
  partnerNotificationList: any;
  total_notification: any;
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

  partnerNotificationModal = new FormGroup({
    NotificationId: new FormControl(0),
    Title: new FormControl('', [Validators.required]),
    Body: new FormControl('', [Validators.required]),
  });

  get f() {
    return this.partnerNotificationModal.controls;
  }

  ngOnInit(): void {
    this.getAllPartnerNotificationList();
  }

  resetModal() {
    this.partnerNotificationModal.reset();
    this.partnerNotificationModal.setValue({
      NotificationId: 0,
      Title: '',
      Body: ''
    });
  }
  getAllPartnerNotificationList() {
    this.temp_var = false;
    this.service.get('Partners/GetAllPartnerFireBaseNotification', '').subscribe(res => {
      this.partnerNotificationList = res.data;
      this.total_notification = this.partnerNotificationList.length;
      this.temp_var = true;
      this.service.setLoading(false);
    });
  }

  AddNotification() {
    if (this.partnerNotificationModal.invalid) {
      this.partnerNotificationModal.markAllAsTouched();
      return;
    }
    this.service.post('Partners/SendNotificationToPartner', this.partnerNotificationModal.value).subscribe(res => {
      this.service.setLoading(false);
      this.closePopup();
      if (this.service.IsNotificationSend(res)) {
        this.getAllPartnerNotificationList();
      }
    });
  }

}
