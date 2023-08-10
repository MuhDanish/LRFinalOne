import { Component, OnInit } from '@angular/core';
import { GenericServiceService } from 'src/app/common/services/generic-service.service';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
@Component({
  selector: 'app-package-list',
  templateUrl: './package-list.component.html',
  styleUrls: ['./package-list.component.css']
})
export class PackageListComponent implements OnInit {

  constructor(public service: GenericServiceService, private route: Router) {
    this.service.setHeaderName('Package List');
    this.getPackageList();
  }

  packageList: any;
  total_package: any;
  public temp_var: Object = false;
  api_url = environment.API_FILE_URL;
  ngOnInit(): void {

  }
  getPackageList() {
    this.temp_var = false;
    this.service.get('Settings/GetPackageList', '').subscribe(res => {
      this.packageList = res.successList.package;
      this.total_package = this.packageList.length;
      this.temp_var = true;
      this.service.setLoading(false);
    });
  }

  AddPackage() {
    this.route.navigate(['/add_package', '']);
  }
  EditPackage(data: any) {
    this.route.navigate(['/add_package', data.data.packageId]);
  }

  DeletePackage(data: any) {
    if (confirm('Are you sure you want to delete this Package.')) {
      var packagedata = { PackageId: data.data.packageId };
      this.service.post('Settings/DeletePackage', packagedata).subscribe(res => {
        this.service.setLoading(false);
        if (this.service.IsSaveData(res)) {
          this.getPackageList();
        }
      });
    }
  }
  checkImagePath(data: any) {
    var res = null;
    if (data.data.promotionalImage != null || data.data.promotionalImage != '') {
      res = this.api_url + data.data.promotionalImage;
    }
    if (data.data.promotionalImage == null || data.data.promotionalImage == '') {
      res = null;
    }

    return res;
  }
}
