import { Component, OnInit, ViewChild } from '@angular/core';
import { GenericServiceService } from 'src/app/common/services/generic-service.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-room-types',
  templateUrl: './room-types.component.html',
  styleUrls: ['./room-types.component.css']
})
export class RoomTypesComponent implements OnInit {

  constructor(public service: GenericServiceService, private modalService: NgbModal) {
    this.service.setHeaderName('Room Type');
    this.getRoomTypeList();
  }
  roomTypeList: any;
  total_room: any;
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

  roomTypeModal = new FormGroup({
    RoomId: new FormControl(0),
    RoomType: new FormControl('', [Validators.required]),
  });

  get f() {
    return this.roomTypeModal.controls;
  }

  ngOnInit(): void {
    this.getRoomTypeList();
  }

  resetModal() {
    this.roomTypeModal.reset();
    this.roomTypeModal.setValue({
      RoomId: 0,
      RoomType: ''
    });
  }
  getRoomTypeList() {
    this.temp_var = false;
    this.service.get('Settings/GetRoomTypeList', '').subscribe(res => {
      this.roomTypeList = res.successList.roomTypes;
      this.total_room = this.roomTypeList.length;
      this.temp_var = true;
      this.service.setLoading(false);
    });
  }

  AddUpdateRoomType() {
    if (this.roomTypeModal.invalid) {
      this.roomTypeModal.markAllAsTouched();
      return;
    }
    this.service.post('Settings/AddUpdateRoomType', this.roomTypeModal.value).subscribe(res => {
      this.service.setLoading(false);
      if (this.service.IsSaveData(res)) {
        this.closePopup();
        this.getRoomTypeList();
      }
    });
  }

  EditRoomType(data: any, content: any) {
    this.openPopup(content);
    this.button_text = "Update";
    this.roomTypeModal.setValue({
      RoomId: data.roomId,
      RoomType: data.roomType
    });
  }

  DeleteRoomType(roomId: any) {
    if (confirm('Are you sure you want to delete this Room.')) {
      this.roomTypeModal.setValue({
        RoomId: roomId,
        RoomType: ''

      });
      this.service.post('Settings/DeleteRoomType', this.roomTypeModal.value).subscribe(res => {
        this.service.setLoading(false);
        if (this.service.IsSaveData(res)) {
          this.getRoomTypeList();
        }
      });
    }
  }
}
