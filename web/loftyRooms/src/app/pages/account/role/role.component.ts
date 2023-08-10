import { Component, OnInit, ViewChild } from '@angular/core';
import { GenericServiceService } from 'src/app/common/services/generic-service.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DataTableDirective } from 'angular-datatables';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.css']
})
export class RoleComponent implements OnInit {

  constructor(private service: GenericServiceService, private modalService: NgbModal) {
    this.service.setHeaderName('Role');
    this.getRoleList();
  }

  dtOptions: DataTables.Settings = {};
  roleList: any;
  total_roles: any;
  public temp_var: Object = false;
  button_text = "";

  Role_Id: string = "";
  Role_Name: string = "";

  @ViewChild(DataTableDirective)
  datatableElement!: DataTableDirective;

  displayStyle = "none";
  closeResult = '';
  openPopup(content: any) {
    this.button_text = "Add";
    this.resetModal();
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' });
  }
  closePopup() {
    this.roleModal.reset();
    this.modalService.dismissAll();
  }

  roleModal = new FormGroup({
    RoleId: new FormControl(0),
    RoleName: new FormControl('', [Validators.required]),
  });
  get f() {
    return this.roleModal.controls;
  }


  ngOnInit(): void {
    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 10,
      processing: true,
      order: [0]
    };
  }

  resetModal() {
    this.roleModal.reset();
    this.roleModal.setValue({
      RoleId: 0,
      RoleName: ''
    });
  }

  getRoleList() {
    this.temp_var = false;
    this.service.get('Account/GetRoleList', '').subscribe(res => {
      this.roleList = res.successList;
      this.total_roles = this.roleList.length;
      this.temp_var = true;
      this.service.setLoading(false);
    });
  }

  async searchColumn() {
    const dataTable = await this.datatableElement.dtInstance;
    dataTable.column(0).search(this.Role_Id);
    dataTable.column(1).search(this.Role_Name);
    dataTable.draw();
  }

  AddUpdateRole() {
    if (this.roleModal.invalid) {
      this.roleModal.markAllAsTouched();
      return;
    }
    this.service.post('Account/AddUpdateRole', this.roleModal.value).subscribe(res => {
      this.service.setLoading(false);
      if (this.service.IsSaveData(res)) {
        this.closePopup();
        this.getRoleList();
      }
    });
  }

  EditRole(data: any, content: any) {
    this.openPopup(content);
    this.button_text = "Update";
    this.roleModal.setValue({
      RoleId: data.roleId,
      RoleName: data.roleName
    });
  }

}
