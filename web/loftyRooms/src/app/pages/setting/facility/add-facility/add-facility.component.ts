import { Component, OnInit } from '@angular/core';
import { GenericServiceService } from 'src/app/common/services/generic-service.service';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
@Component({
  selector: 'app-add-facility',
  templateUrl: './add-facility.component.html',
  styleUrls: ['./add-facility.component.css']
})
export class AddFacilityComponent implements OnInit {
  facilityModal: FormGroup = new FormGroup({});
  innerHTML: string = 'Add Facility';
  Sumbit_btn_text: string = 'Save';
  IsShowImage = true;
  PictureView: any = "assets/dist/img/no-image.jpeg";
  Id_Param: string = '';
  constructor(public service: GenericServiceService, private router: Router, private route: ActivatedRoute, private navig: Router, private fb: FormBuilder) {
    this.service.setHeaderName('Add Facility');
  }
  ngOnInit(): void {
    this.resetadsModal();
    this.Id_Param = this.route.snapshot.paramMap.get('id') || '';
    this.facilityModal = this.fb.group({
      FacilityId: new FormControl(0),
      FacilityName: new FormControl(''),
      Image: new FormControl(''),
      ImagePath: new FormControl(''),
      Count: new FormControl(0)


    });
    setTimeout(() => {
      this.getFacilityById();
    }, 500);
  }

  get f() {
    return this.facilityModal.controls;
  }
  resetadsModal() {
    this.PictureView = "assets/dist/img/no-image.jpeg";
    this.facilityModal.reset();
    this.facilityModal.patchValue({
      FacilityId: 0,
      FacilityName: '',
      CreatedBy: environment.GET_USERID(),
      Count: 0,
    });
  }
  AddUpdateFacility() {
    if (this.facilityModal.invalid) {
      this.facilityModal.markAllAsTouched();
      return;
    }
    const formdata = new FormData();
    formdata.append('ImagePath', this.facilityModal.get('ImagePath')?.value);
    var modelString = JSON.stringify(this.facilityModal.value);

    formdata.append('Model', modelString);
    this.service.post('Settings/AddUpdateFacility', formdata).subscribe(res => {
      this.service.setLoading(false);
      if (this.service.IsSaveData(res)) {
        this.navig.navigate(['/facilities']);
      }
    });
  }

  getFacilityById() {
    if (parseInt(this.Id_Param) > 0) {
      var data = { FacilityId: this.Id_Param };
      this.service.getDatatable('Settings/GetFacilityById', data).subscribe(res => {
        this.service.setLoading(false);
        this.innerHTML = 'Update Ad';
        this.Sumbit_btn_text = 'Update';
        var Detail = res.successList[0];
        if (Detail.image != "") {
          this.PictureView = environment.API_FILE_URL + Detail.image;
        }
        setTimeout(() => {
          this.facilityModal.patchValue({
            FacilityId: Detail.facilityId,
            FacilityName: Detail.facilityName,
            Image: Detail.image,
            Count: Detail.count,
          });
        }, 100);
      });
    }
  }
  onImageChange(event: any) {
    if (event.target.files.length > 0) {
      if (event.target.files[0].type == "image/png" || event.target.files[0].type == "image/jpg" || event.target.files[0].type == "image/jpeg") {
        const file = event.target.files[0];
        const reader = new FileReader();
        reader.onload = e => this.PictureView = reader.result;
        reader.readAsDataURL(file);
        this.facilityModal.patchValue({
          ImagePath: file
        });
      }
      else {
        this.PictureView = "assets/dist/img/no-image.jpeg";
        this.IsShowImage = false;
        this.facilityModal.patchValue({
          PromotionalImage: ""
        });
      }

    }
  }
}
