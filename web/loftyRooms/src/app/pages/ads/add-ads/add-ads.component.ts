import { Component, OnInit } from '@angular/core';
import { GenericServiceService } from 'src/app/common/services/generic-service.service';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
@Component({
  selector: 'app-add-ads',
  templateUrl: './add-ads.component.html',
  styleUrls: ['./add-ads.component.css'],

})
export class AddAdsComponent implements OnInit {
  adsModal: FormGroup = new FormGroup({});
  innerHTML: string = 'Add Ads';
  Sumbit_btn_text: string = 'Save';
  Ad_Id_Param: string = '';
  person_list: any;
  partner_list: any;
  roomType_list: any;
  adsTypes_list: any;
  packages_list: any;
  facility_list: any;
  AdsPictureView: any = "assets/dist/img/no-image.jpeg";
  AdsPictureView2: any = "assets/dist/img/no-image.jpeg";
  AdsPictureView3: any = "assets/dist/img/no-image.jpeg";
  AdsPictureView4: any = "assets/dist/img/no-image.jpeg";
  IsShowImage = true;
  IsShowImage2 = true;
  IsShowImage3 = true;
  IsShowImage4 = true;
  selectedOption: string = '';
  facilities = [];

  constructor(public service: GenericServiceService, private router: Router, private route: ActivatedRoute, private navig: Router, private fb: FormBuilder) {
    this.service.setHeaderName('Add Ads');
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        if (this.route.snapshot.routeConfig?.path === 'add_ads/:id') {
          this.Sumbit_btn_text = 'Save'
          this.selectedOption = '2';
          this.resetadsModal();
          this.getAllDropDowns();
        }
      }
    });
  }

  onAdTypeChange(event: any) {
    this.selectedOption = event.target.value;
    if (this.selectedOption == '1') {
      this.adsModal.patchValue({
        PackageId: 0,
        PackageStartDate: '',
        PackageEndDate: '',
      });
    }
    else {
      this.adsModal.patchValue({
        PackageId: new FormControl(0),
        PackageStartDate: new FormControl((new Date()).toISOString().substring(0, 10)),
        PackageEndDate: new FormControl((new Date()).toISOString().substring(0, 10)),
      });
    }
  }
  ngOnInit(): void {
    this.resetadsModal();
    this.Ad_Id_Param = this.route.snapshot.paramMap.get('id') || '';
    this.selectedOption = '2';
    if (parseInt(this.Ad_Id_Param) > 0) {
      this.adsModal = this.fb.group({
        AdId: new FormControl(0),
        PersonId: new FormControl(0),
        NoOfBed: new FormControl(0, [Validators.required]),
        NoOfPerson: new FormControl(0, [Validators.required]),
        PartnerId: new FormControl(0),
        Price: new FormControl('', [Validators.required]),
        BasePrice: new FormControl('', [Validators.required]),
        RoomId: new FormControl(0),
        AdTypeId: new FormControl(0),
        AdImage1: new FormControl(''),
        AdImage1Path: new FormControl(''),
        AdImage2: new FormControl(''),
        AdImage2Path: new FormControl(''),
        AdImage3: new FormControl(''),
        AdImage3Path: new FormControl(''),
        AdImage4: new FormControl(''),
        AdImage4Path: new FormControl(''),
        Location: new FormControl('', [Validators.required]),
        PackageId: new FormControl(0),
        PackageStartDate: new FormControl((new Date()).toISOString().substring(0, 10)),
        PackageEndDate: new FormControl((new Date()).toISOString().substring(0, 10)),
        Description: new FormControl(''),
        Facilities: [[]],
        RoomNo: new FormControl('', [Validators.required]),

      });
    } else {
      this.adsModal = this.fb.group({
        AdId: new FormControl(0),
        PersonId: new FormControl(0),
        NoOfBed: new FormControl(0, [Validators.required]),
        NoOfPerson: new FormControl(0, [Validators.required]),
        PartnerId: new FormControl(0),
        Price: new FormControl('', [Validators.required]),
        BasePrice: new FormControl('', [Validators.required]),
        RoomId: new FormControl(0),
        AdTypeId: new FormControl(0),
        AdImage1: new FormControl('', [Validators.required]),
        AdImage1Path: new FormControl('', [Validators.required]),
        AdImage2: new FormControl('', [Validators.required]),
        AdImage2Path: new FormControl('', [Validators.required]),
        AdImage3: new FormControl('', [Validators.required]),
        AdImage3Path: new FormControl('', [Validators.required]),
        AdImage4: new FormControl('', [Validators.required]),
        AdImage4Path: new FormControl('', [Validators.required]),
        Location: new FormControl('', [Validators.required]),
        PackageId: new FormControl(0),
        PackageStartDate: new FormControl((new Date()).toISOString().substring(0, 10)),
        PackageEndDate: new FormControl((new Date()).toISOString().substring(0, 10)),
        Description: new FormControl(''),
        Facilities: [[]],
        RoomNo: new FormControl(''),

      });
    }

    this.getAllDropDowns();
    setTimeout(() => {
      this.getAdById();
    }, 500);
  }

  resetadsModal() {
    this.adsModal.reset();
    this.adsModal.patchValue({

      AdId: 0,
      PersonId: 0,
      NoOfBed: 0,
      NoOfPerson: 0,
      PartnerId: 0,
      Price: '',
      BasePrice: '',
      RoomId: 0,
      AdTypeId: 0,
      PackageId: 0,
      CreatedBy: environment.GET_USERID(),
      PackageStartDate: new FormControl((new Date()).toISOString().substring(0, 10)),
      PackageEndDate: new FormControl((new Date()).toISOString().substring(0, 10)),
      Description: '',
      RoomNo: ''

    });
  }

  getAllDropDowns() {
    this.service.get('Ads/GetAllAdsDropDowns', '').subscribe(res => {
      this.person_list = res.successList.persons;
      this.partner_list = res.successList.partners;
      this.roomType_list = res.successList.roomsType;
      this.adsTypes_list = res.successList.adsTypes;
      this.packages_list = res.successList.packages;
      this.facility_list = res.successList.facilities;
      this.service.setLoading(false);
    });
  }
  get f() {
    return this.adsModal.controls;
  }
  AddUpdateAds() {
    if (this.adsModal.invalid) {
      this.adsModal.markAllAsTouched();
      return;
    }
    const formdata = new FormData();
    formdata.append('AdImage1Path', this.adsModal.get('AdImage1Path')?.value);
    formdata.append('AdImage2Path', this.adsModal.get('AdImage2Path')?.value);
    formdata.append('AdImage3Path', this.adsModal.get('AdImage3Path')?.value);
    formdata.append('AdImage4Path', this.adsModal.get('AdImage4Path')?.value);
    this.adsModal.patchValue({
      CreatedBy: environment.GET_USERID()
    });
    var modelString = JSON.stringify(this.adsModal.value);

    formdata.append('Model', modelString);
    this.service.post('Ads/AddUpdateAds', formdata).subscribe(res => {
      this.service.setLoading(false);
      if (this.service.IsSaveData(res)) {
        this.navig.navigate(['/ads_list']);
        this.resetadsModal();
        this.getAllDropDowns();
      }
    });

  }

  getAdById() {
    if (parseInt(this.Ad_Id_Param) > 0) {
      var date = { AdId: this.Ad_Id_Param };
      this.service.getDatatable('Ads/GetAdById', date).subscribe(res => {
        this.service.setLoading(false);
        this.innerHTML = 'Update Ad';
        this.Sumbit_btn_text = 'Update';
        var adDetail = res.successList.OriginalAd[0];
        var adFacilityData = res.successList.fac;
        if (adDetail.adImage1 != "") {
          this.AdsPictureView = environment.API_FILE_URL + adDetail.adImage1;
        }
        if (adDetail.adImage2 != "") {
          this.AdsPictureView2 = environment.API_FILE_URL + adDetail.adImage2;
        }
        if (adDetail.adImage3 != "") {
          this.AdsPictureView3 = environment.API_FILE_URL + adDetail.adImage3;
        }
        if (adDetail.adImage4 != "") {
          this.AdsPictureView4 = environment.API_FILE_URL + adDetail.adImage4;
        }
        if (adDetail.packageStartDate == null) {
          this.adsModal.patchValue({
            PackageStartDate: '',
          });
        }
        else if (adDetail.packageStartDate != null) {
          this.adsModal.patchValue({
            PackageStartDate: adDetail.packageStartDate.slice(0, 10),
          });
        }
        if (adDetail.packageEndDate == null) {
          this.adsModal.patchValue({
            PackageEndDate: '',
          });
        }
        else if (adDetail.packageEndDate != null) {
          this.adsModal.patchValue({
            PackageEndDate: adDetail.packageEndDate.slice(0, 10),
          });
        }
        if (adFacilityData.length > 0) {
          for (var i = 0; i < adFacilityData.length; i++) {
            const facilitiesControl = this.adsModal.get('Facilities');
            const facilityId = adFacilityData[i].facilityId;
            if (facilitiesControl) {
              facilitiesControl.patchValue([...facilitiesControl.value, facilityId]);
            }
          }
        }

        setTimeout(() => {
          this.adsModal.patchValue({
            AdId: adDetail.adId,
            PersonId: adDetail.personId,
            NoOfBed: adDetail.noOfBed,
            NoOfPerson: adDetail.noOfPerson,
            PartnerId: adDetail.partnerId,
            Price: adDetail.price,
            BasePrice: adDetail.basePrice,
            RoomId: adDetail.roomId,
            AdTypeId: adDetail.adTypeId,
            PackageId: adDetail.packageId,
            Location: adDetail.location,
            // PackageStartDate: adDetail.packageStartDate.slice(0, 10),
            // PackageEndDate: adDetail.packageEndDate.slice(0, 10),
            Description: adDetail.description,
            RoomNo: adDetail.roomNo
          });
        }, 1000);
      });
    }
  }
  onAdChange1(event: any) {
    if (event.target.files.length > 0) {
      if (event.target.files[0].type == "image/png" || event.target.files[0].type == "image/jpg" || event.target.files[0].type == "image/jpeg") {
        const file = event.target.files[0];
        const reader = new FileReader();
        reader.onload = e => this.AdsPictureView = reader.result;
        reader.readAsDataURL(file);
        this.adsModal.patchValue({
          AdImage1Path: file
        });
      }
      else {
        this.AdsPictureView = "assets/dist/img/no-image.jpeg";
        this.IsShowImage = false;
        this.adsModal.patchValue({
          PromotionalImage: ""
        });
      }

    }
  }

  onAdChange2(event: any) {
    if (event.target.files.length > 0) {
      if (event.target.files[0].type == "image/png" || event.target.files[0].type == "image/jpg" || event.target.files[0].type == "image/jpeg") {
        const file = event.target.files[0];
        const reader = new FileReader();
        reader.onload = e => this.AdsPictureView2 = reader.result;
        reader.readAsDataURL(file);
        this.adsModal.patchValue({
          AdImage2Path: file
        });
      }
      else {
        this.AdsPictureView2 = "assets/dist/img/no-image.jpeg";
        this.IsShowImage2 = false;
        this.adsModal.patchValue({
          PromotionalImage: ""
        });
      }

    }
  }
  onAdChange3(event: any) {
    if (event.target.files.length > 0) {
      if (event.target.files[0].type == "image/png" || event.target.files[0].type == "image/jpg" || event.target.files[0].type == "image/jpeg") {
        const file = event.target.files[0];
        const reader = new FileReader();
        reader.onload = e => this.AdsPictureView3 = reader.result;
        reader.readAsDataURL(file);
        this.adsModal.patchValue({
          AdImage3Path: file
        });
      }
      else {
        this.AdsPictureView3 = "assets/dist/img/no-image.jpeg";
        this.IsShowImage3 = false;
        this.adsModal.patchValue({
          PromotionalImage: ""
        });
      }

    }
  }

  onAdChange4(event: any) {
    if (event.target.files.length > 0) {
      if (event.target.files[0].type == "image/png" || event.target.files[0].type == "image/jpg" || event.target.files[0].type == "image/jpeg") {
        const file = event.target.files[0];
        const reader = new FileReader();
        reader.onload = e => this.AdsPictureView4 = reader.result;
        reader.readAsDataURL(file);
        this.adsModal.patchValue({
          AdImage4Path: file
        });
      }
      else {
        this.AdsPictureView4 = "assets/dist/img/no-image.jpeg";
        this.IsShowImage4 = false;
        this.adsModal.patchValue({
          PromotionalImage: ""
        });
      }

    }
  }
}
