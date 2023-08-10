import { Component, OnInit } from '@angular/core';
import { GenericServiceService } from 'src/app/common/services/generic-service.service';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
@Component({
  selector: 'app-add-package',
  templateUrl: './add-package.component.html',
  styleUrls: ['./add-package.component.css']
})
export class AddPackageComponent implements OnInit {
  packageModal: FormGroup = new FormGroup({});
  innerHTML: string = 'Add Package';
  Sumbit_btn_text: string = 'Save';
  Package_Id_Param: string = '';
  PromotionalPictureView: any = "assets/dist/img/no-image.jpeg";
  IsShowImage = true;
  constructor(public service: GenericServiceService, private route: ActivatedRoute, private router: Router, private navig: Router, private fb: FormBuilder) {
    this.service.setHeaderName('Add Package');
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        if (this.route.snapshot.routeConfig?.path === 'add_package/:id') {
          this.Sumbit_btn_text = 'Save'
          this.packageModal = this.fb.group({
            PackageId: new FormControl(0),
            PromotionalText: new FormControl('', [Validators.required]),
            DiscountPercentage: new FormControl('', [Validators.required]),
            PromotionalImage: new FormControl(''),
            PromotionalImagePath: new FormControl(''),

          });
          this.PromotionalPictureView = "assets/dist/img/no-image.jpeg";
        }
      }
    });
  }


  ngOnInit(): void {
    this.Package_Id_Param = this.route.snapshot.paramMap.get('id') || '';

    this.packageModal = this.fb.group({
      PackageId: new FormControl(0),
      PromotionalText: new FormControl('', [Validators.required]),
      DiscountPercentage: new FormControl('', [Validators.required]),
      PromotionalImage: new FormControl(''),
      PromotionalImagePath: new FormControl('')

    });

    // this.resetjobModal();
    this.getPackageById();
  }
  resetjobModal() {
    this.packageModal.reset();


    this.packageModal.patchValue({
      PackageId: 0,
      PromotionalText: '',
      DiscountPercentage: '',
      CreatedBy: environment.GET_USERID(),
    });
  }

  get f() {
    return this.packageModal.controls;
  }
  AddUpdatePackage() {
    if (this.packageModal.invalid) {
      this.packageModal.markAllAsTouched();
      return;
    }
    const formdata = new FormData();
    formdata.append('PromotionalImagePath', this.packageModal.get('PromotionalImagePath')?.value);

    this.packageModal.patchValue({
      CreatedBy: environment.GET_USERID()
    });
    var modelString = JSON.stringify(this.packageModal.value);
    formdata.append('Model', modelString);
    this.service.post('Settings/AddUpdatePackage', formdata).subscribe(res => {
      this.service.setLoading(false);
      if (this.service.IsSaveData(res)) {
        this.navig.navigate(['/package_list']);
        // this.resetjobModal();
      }
    });

  }
  onFileChange(event: any) {
    if (event.target.files.length > 0) {
      if (event.target.files[0].type == "image/png" || event.target.files[0].type == "image/jpg" || event.target.files[0].type == "image/jpeg") {
        const file = event.target.files[0];
        const reader = new FileReader();
        reader.onload = e => this.PromotionalPictureView = reader.result;
        reader.readAsDataURL(file);
        this.packageModal.patchValue({
          PromotionalImagePath: file
        });
      }
      else {
        this.PromotionalPictureView = "assets/dist/img/no-image.jpeg";
        this.IsShowImage = false;
        this.packageModal.patchValue({
          PromotionalImage: ""
        });
      }

    }
  }
  getPackageById() {
    if (parseInt(this.Package_Id_Param) > 0) {
      var date = { PackageId: this.Package_Id_Param };
      this.service.getDatatable('Settings/GetPackageById', date).subscribe(res => {
        this.service.setLoading(false);
        this.innerHTML = 'Update Package';
        this.Sumbit_btn_text = 'Update';
        var packageData = res.successList[0];

        if (packageData.promotionalImage != "") {
          this.PromotionalPictureView = environment.API_FILE_URL + packageData.promotionalImage;
        }
        setTimeout(() => {
          this.packageModal.patchValue({
            PackageId: packageData.packageId,
            PromotionalText: packageData.promotionalText,
            DiscountPercentage: packageData.discountPercentage,
          });
        }, 1000);
      });
    }
    else {
      this.PromotionalPictureView = "assets/dist/img/no-image.jpeg";
    }
  }
}
