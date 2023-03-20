import {ChangeDetectionStrategy, Component, EventEmitter, Input, Output} from '@angular/core';
import {Contact} from "../../models/contact";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ContactsComponent {
  @Input() contact: Contact = {
    id: undefined,
    name: '',
    email: '',
    phone: ''
  };

  @Output() save = new EventEmitter<Contact>();

  form: FormGroup;

  constructor(public formBuilder: FormBuilder) {
    this.form = this.formBuilder.group({
      id: [this.contact.id],
      name: [this.contact.name, Validators.required],
      email: [this.contact.email, Validators.required],
      phone: [this.contact.phone]
    });
  }

  ngOnInit() {

  }

  ngOnChanges() {
    if (this.contact) {
      this.form.patchValue({...this.contact});
    }
  }

  submit() {
    if (this.form.valid) {
      this.save.emit(this.form.value);
    }

  }

}
