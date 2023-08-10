import { Component, OnInit } from '@angular/core';
import { GenericServiceService } from 'src/app/common/services/generic-service.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
@Component({
  selector: 'app-add-partner',
  templateUrl: './add-partner.component.html',
  styleUrls: ['./add-partner.component.css']
})
export class AddPartnerComponent implements OnInit {
  partnerModal: FormGroup = new FormGroup({});
  innerHTML: string = 'Add Partner';
  Sumbit_btn_text: string = 'Save';
  Partner_Id_Param: string = '';
  roomType_list: any;

  constructor(public service: GenericServiceService, private router: Router, private route: ActivatedRoute, private navig: Router, private fb: FormBuilder) {
    this.service.setHeaderName('Add Partner');
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        if (this.route.snapshot.routeConfig?.path === 'add_partner/:id') {
          this.Sumbit_btn_text = 'Save'

          this.resetpartnerModal();
          this.getRoomType();
        }
      }
    });
  }

  ngOnInit(): void {
    this.resetpartnerModal();
    this.Partner_Id_Param = this.route.snapshot.paramMap.get('id') || '';
    this.partnerModal = this.fb.group({
      PartnerId: new FormControl(0),
      FirstName: new FormControl('', [Validators.required]),
      LastName: new FormControl('', [Validators.required]),
      Email: new FormControl('', [Validators.required]),
      Password: new FormControl('', [Validators.required]),
      Address: new FormControl(''),
      City: new FormControl(''),
      ZipCode: new FormControl(''),
      State: new FormControl(''),
      Phone: new FormControl('', [Validators.required]),
      RoomId: new FormControl(0),
      HotelName: new FormControl('', [Validators.required]),
      DateEntry: new FormControl((new Date()).toISOString().substring(0, 10)),
      AllowLogin: new FormControl(false),
      Latitude: new FormControl('', [Validators.required]),
      Longitude: new FormControl('', [Validators.required])
    });
    this.getRoomType();
    setTimeout(() => {
      this.getPartnerById();
    }, 500);
  }

  resetpartnerModal() {
    this.partnerModal.reset();
    this.partnerModal.patchValue({
      PartnerId: 0,
      CreatedBy: environment.GET_USERID(),
      FirstName: '',
      LastName: '',
      Email: '',
      Password: '',
      Address: '',
      City: '',
      State: '',
      ZipCode: '',
      Phone: '',
      RoomId: 0,
      HotelName: '',
      DateEntry: new FormControl((new Date()).toISOString().substring(0, 10)),
      AllowLogin: false,
      Latitude: '',
      Longitude: ''
    });
  }

  getRoomType() {
    this.service.get('Partners/GetRoomTypeList', '').subscribe(res => {
      this.roomType_list = res.successList.roomTypes;
      this.service.setLoading(false);
    });
  }
  get f() {
    return this.partnerModal.controls;
  }
  AddUpdatePartner() {
    if (this.partnerModal.invalid) {
      this.partnerModal.markAllAsTouched();
      return;
    }

    const formdata = new FormData();
    this.partnerModal.patchValue({
      CreatedBy: environment.GET_USERID()
    });
    var modelString = JSON.stringify(this.partnerModal.value);

    formdata.append('Model', modelString);
    this.service.post('Partners/AddUpdatePartner', formdata).subscribe(res => {
      this.service.setLoading(false);
      if (this.service.IsSaveData(res)) {
        this.navig.navigate(['/partner_list']);
        this.resetpartnerModal();
        this.getRoomType();
      }
    });

  }

  getPartnerById() {
    if (parseInt(this.Partner_Id_Param) > 0) {
      var date = { PartnerId: this.Partner_Id_Param };
      this.service.getDatatable('Partners/GetPartnerById', date).subscribe(res => {
        this.service.setLoading(false);
        this.innerHTML = 'Update Partner';
        this.Sumbit_btn_text = 'Update';

        var partnerDetail = res.successList;

        setTimeout(() => {
          this.partnerModal.patchValue({
            PartnerId: partnerDetail.partnerId,
            FirstName: partnerDetail.firstName,
            LastName: partnerDetail.lastName,
            Email: partnerDetail.email,
            Address: partnerDetail.address,
            City: partnerDetail.city,
            State: partnerDetail.state,
            ZipCode: partnerDetail.zipCode,
            Phone: partnerDetail.phone,
            RoomId: partnerDetail.roomId,
            HotelName: partnerDetail.hotelName,
            DateEntry: partnerDetail.dateEntry.slice(0, 10),
            AllowLogin: partnerDetail.allowLogin,
            Latitude: partnerDetail.latitude,
            Longitude: partnerDetail.longitude,
          });
        }, 1000);
      });
    }
  }
}
