import { Component, OnInit, ViewChild } from '@angular/core';
import { GenericServiceService } from 'src/app/common/services/generic-service.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-person',
  templateUrl: './person.component.html',
  styleUrls: ['./person.component.css']
})
export class PersonComponent implements OnInit {

  constructor(public service: GenericServiceService, private modalService: NgbModal) {
    this.service.setHeaderName('Person');
    this.getPersonList();
  }
  personList: any;
  total_person: any;
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

  personModal = new FormGroup({
    PersonId: new FormControl(0),
    NoOfPerson: new FormControl('', [Validators.required]),
  });

  get f() {
    return this.personModal.controls;
  }


  ngOnInit(): void {
    this.getPersonList();
  }

  resetModal() {
    this.personModal.reset();
    this.personModal.setValue({
      PersonId: 0,
      NoOfPerson: ''
    });
  }
  getPersonList() {
    this.temp_var = false;
    this.service.get('Settings/GetPersonList', '').subscribe(res => {
      this.personList = res.successList.persons;
      this.total_person = this.personList.length;
      this.temp_var = true;
      this.service.setLoading(false);
    });
  }

  AddUpdatePerson() {
    if (this.personModal.invalid) {
      this.personModal.markAllAsTouched();
      return;
    }
    this.service.post('Settings/AddUpdatePerson', this.personModal.value).subscribe(res => {
      this.service.setLoading(false);
      if (this.service.IsSaveData(res)) {
        this.closePopup();
        this.getPersonList();
      }
    });
  }

  EditPerson(data: any, content: any) {
    this.openPopup(content);
    this.button_text = "Update";
    this.personModal.setValue({
      PersonId: data.personId,
      NoOfPerson: data.noOfPerson
    });
  }

  DeletePerson(personId: any) {
    if (confirm('Are you sure you want to delete this Person.')) {
      this.personModal.setValue({
        PersonId: personId,
        NoOfPerson: ''

      });
      this.service.post('Settings/DeletePerson', this.personModal.value).subscribe(res => {
        this.service.setLoading(false);
        if (this.service.IsSaveData(res)) {
          this.getPersonList();
        }
      });
    }
  }
}
