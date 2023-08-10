import { Component, OnInit, ViewChild } from '@angular/core';
import { GenericServiceService } from 'src/app/common/services/generic-service.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { groupBy } from 'lodash';

@Component({
  selector: 'app-role-claims',
  templateUrl: './role-claims.component.html',
  styleUrls: ['./role-claims.component.css']
})
export class RoleClaimsComponent implements OnInit {

  constructor(private service: GenericServiceService, private modalService: NgbModal) {
    this.service.setHeaderName('Role Claims');
    this.getRoleList();
  }

  roleList: any;
  roleId: any;
  claimsList: Array<any> = new Array<any>();
  groupClaims: any = [];
  claimsString: string = '';
  groupbyModules: any;
  IsShowBtn: boolean = true;
  ngOnInit(): void {

  }

  roleClaimsModal = new FormGroup({
    RoleId: new FormControl(0),
    Claims: new FormControl(''),
  });

  getRoleList() {
    this.service.get('Account/GetRoleList', '').subscribe(res => {
      this.roleList = res.successList;
      this.service.setLoading(false);
    });
  }
  OnRoleChange(e: any) {
    this.roleClaimsModal.setValue({
      RoleId: e.target['value'],
      Claims: ''
    });
    this.service.getDatatable('Account/GetClaimsList', this.roleClaimsModal.value).subscribe(res => {
      this.claimsList = res.successList;
      this.groupClaims = groupBy(this.claimsList, 'moduleName');
      this.IsShowBtn = false;
      this.service.setLoading(false);
    });
  }

  AddUpdateRoleClaims() {
    this.service.post('Account/AddUpdateRoleClaims', this.roleClaimsModal.value).subscribe(res => {
      this.service.setLoading(false);
      if (this.service.IsSaveData(res)) {
      }
    });
  }

  IsItemSelected(e: any, item: any) {
    let indexToUpdate = this.claimsList.findIndex(d => d.claimId === item.claimId);
    if (e.checked) {
      this.claimsList[indexToUpdate].isSelected = 1;
    } else {
      this.claimsList[indexToUpdate].isSelected = 0;
    }
  }

  UpdateRoleClaims() {
    this.claimsString = '';
    var coma = '';
    var t = this.claimsList.filter(opt => opt.isSelected).map(opt => {
      this.claimsString += coma + opt.claimId;
      coma = ',';
    });
    var role = this.roleClaimsModal.value.RoleId;
    this.roleClaimsModal.setValue({
      RoleId: role,
      Claims: this.claimsString
    });
    this.service.post('Account/AddUpdateRoleClaims', this.roleClaimsModal.value).subscribe(res => {
      this.service.setLoading(false);
      if (this.service.IsSaveData(res)) {
      }
    });
  }
}
