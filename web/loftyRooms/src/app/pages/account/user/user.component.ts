import { Component, OnInit, ViewChild } from '@angular/core';
import { GenericServiceService } from 'src/app/common/services/generic-service.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})

export class UserComponent implements OnInit {

  constructor(public service: GenericServiceService, private modalService: NgbModal) {
    this.service.setHeaderName('User');
  }
  userList: any;
  total_user: any;
  userType_list: any;
  role_List: Array<any> = new Array<any>();
  public temp_var: Object = false;
  public temp_var2: Object = false;
  button_text = "";
  rolesString = "";

  closeResult = '';
  openPopup(content: any) {
    this.button_text = "Add";
    this.resetModal();
    this.modalService.open(content,
      { ariaLabelledBy: 'modal-basic-title' });
  }
  closePopup() {
    this.modalService.dismissAll();
  }

  userModal = new FormGroup({
    UserId: new FormControl(0),
    UserName: new FormControl('', [Validators.required]),
    LastName: new FormControl('', [Validators.required]),
    Email: new FormControl('', [Validators.required, Validators.email]),
    Password: new FormControl('', [Validators.required, Validators.minLength(4)]),
    Cnic: new FormControl(''),
    Mobile: new FormControl('')
    // RoleId: new FormControl('', [Validators.required])
  });

  get f() {
    return this.userModal.controls;
  }

  ngOnInit(): void {
    //  this.getUserType();
    this.getUserList();
  }


  resetModal() {
    this.userModal.reset();
    this.userModal.setValue({
      UserId: 0,
      UserName: '',
      LastName: '',
      Email: '',
      Password: '',
      Cnic: '',
      Mobile: '',
      // RoleId: 0,

    });
  }

  getUserList() {
    this.service.get('Account/GetUserList', '')
      .subscribe(resp => {
        this.userList = resp.successList;
        this.total_user = this.userList.length;
        this.service.setLoading(false);
      });
  }

  getUserType() {
    this.service.get('Account/GetUserDropDowns', '').subscribe(res => {
      this.userType_list = res.successList.userTypes;
      this.service.setLoading(false);
    });
  }




  AddUpdateUser() {
    if (this.userModal.invalid) {
      this.userModal.markAllAsTouched();
      return;
    }

    this.service.post('Account/AddUpdateUser', this.userModal.value).subscribe(res => {
      this.service.setLoading(false);
      if (this.service.IsSaveData(res)) {
        this.closePopup();
        this.getUserList();
      }
    });
  }

  EditUser(data: any, content: any) {
    this.openPopup(content);
    this.button_text = "Update";
    this.userModal.setValue({
      UserId: data.userId,
      UserName: data.userName,
      LastName: data.lastName,
      Email: data.email,
      Password: '',
      Cnic: data.cnic,
      Mobile: data.mobile
      // RoleId: data.roleId

    });
  }

  DeleteUser(UserId: any) {
    if (confirm('Are you sure you want to delete this User.')) {
      this.userModal.setValue({
        UserId: UserId,
        UserName: '',
        LastName: '',
        Email: '',
        Password: '',
        Cnic: '',
        Mobile: '',
        // RoleId: 0
      });
      this.service.post('Account/DeleteUser', this.userModal.value).subscribe(res => {
        this.service.setLoading(false);
        if (this.service.IsSaveData(res)) {
          this.getUserList();
        }
      });
    }
  }


  /////////////////////Assign Roles To User/////////////////////////
  GetUserRoleList(UserId: any, content2: any) {
    this.modalService.open(content2, { ariaLabelledBy: 'modal-basic-title' });
    this.userModal.controls['UserId'].setValue(UserId);
    console.log(this.userModal.value);
    this.service.getDatatable('Account/GetUserRoleList', this.userModal.value).subscribe(res => {
      this.service.setLoading(false);
      this.role_List = res.successList
    });
  }
  IsItemSelected(e: any, item: any) {
    let indexToUpdate = this.role_List.findIndex(d => d.roleId === item.roleId);
    if (e.checked) {
      this.role_List[indexToUpdate].isSelected = 1;
    } else {
      this.role_List[indexToUpdate].isSelected = 0;
    }
  }
  AssignRoles() {
    this.rolesString = '';
    var coma = '';
    var t = this.role_List.filter(opt => opt.isSelected).map(opt => {
      this.rolesString += coma + opt.roleId;
      coma = ',';
    });
    var userid = this.userModal.value.UserId;
    var data = {
      UserId: userid, Roles: this.rolesString
    };
    this.service.post('Account/AddUpdateUserRole', data).subscribe(res => {
      this.service.setLoading(false);
      if (this.service.IsSaveData(res)) {
      }
    });
  }

}
