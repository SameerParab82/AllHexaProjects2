import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { EventService } from 'src/app/services/event.service';

@Component({
  selector: 'app-add-event',
  templateUrl: './add-event.component.html',
  styleUrls: ['./add-event.component.css']
})
export class AddEventComponent implements OnInit {
  form: FormGroup;
  status = undefined;
  constructor(private fb: FormBuilder, private router: Router, private eventSvc: EventService) { }

  ngOnInit() {
    this.form = this.fb.group({
      title: ["", Validators.required],
      speaker: ["", Validators.required],
      organizer: ["", Validators.required],
      startDate: ["", Validators.required],
      endDate: ["", Validators.required],
      location: ["", Validators.required],
      registrationURL: ["", Validators.required]
    });
  }

  addEvent() {
    //alert("Done");
    this.status = undefined;
    if (this.form.valid) {

      this.eventSvc.addEvent(this.form.value)
        .subscribe(
          result => {
            console.log(result);
           // this.router.navigate(['/']);
          }, err => {
            console.log(err);
            this.status = { success: false, class: "alert-danger", message: "Failed to add new event" }
          }
        )
    }
    else {
      this.status = { success: false, class: "alert-danger", message: "Invalid Data" }
    }
  }

}
