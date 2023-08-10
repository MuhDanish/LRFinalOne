import { Component, OnInit, ViewChild } from '@angular/core';
import { GenericServiceService } from 'src/app/common/services/generic-service.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-customer-notifications',
  templateUrl: './customer-notifications.component.html',
  styleUrls: ['./customer-notifications.component.css']
})
export class CustomerNotificationsComponent implements OnInit {

  constructor(public service: GenericServiceService, private modalService: NgbModal) {
    this.service.setHeaderName('Customer Notifications');
    this.getAllCustomerNotificationList();
  }
  customerNotificationList: any;
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

  customerNotificationModal = new FormGroup({
    NotificationId: new FormControl(0),
    Title: new FormControl('', [Validators.required]),
    Body: new FormControl('', [Validators.required]),
  });

  get f() {
    return this.customerNotificationModal.controls;
  }

  ngOnInit(): void {
    this.getAllCustomerNotificationList();
  }

  resetModal() {
    this.customerNotificationModal.reset();
    this.customerNotificationModal.setValue({
      NotificationId: 0,
      Title: '',
      Body: ''
    });
  }
  getAllCustomerNotificationList() {
    this.temp_var = false;
    this.service.get('Customer/GetAllCustomerFireBaseNotification', '').subscribe(res => {
      this.customerNotificationList = res.data;
      this.total_notification = this.customerNotificationList.length;
      this.temp_var = true;
      this.service.setLoading(false);
    });
  }

  AddNotification() {
    if (this.customerNotificationModal.invalid) {
      this.customerNotificationModal.markAllAsTouched();
      return;
    }
    console.log(this.customerNotificationModal.value);
    this.service.post('Customer/SendNotificationToCustomer', this.customerNotificationModal.value).subscribe(res => {
      this.service.setLoading(false);
      this.closePopup();
      if (this.service.IsNotificationSend(res)) {
        this.getAllCustomerNotificationList();
      }
    });
  }

}
