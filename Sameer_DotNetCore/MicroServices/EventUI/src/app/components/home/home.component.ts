import { Component, OnInit } from '@angular/core';
import { EventInfo } from 'src/app/models/event-info';
import { EventService } from 'src/app/services/event.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  /* events: EventInfo[] = [
     {
       id: 1,
       title: "App modernization",
       startDate: new Date(),
       endDate: new Date(),
       speaker: "Sameer",
       location: "Mumbai",
       organizer: "Microsoft",
       registrationUrl: "https://www.google.com"
     },
     {
       id: 2,
       title: "App modernization",
       startDate: new Date(),
       endDate: new Date(),
       speaker: "Kholi",
       location: "Pune",
       organizer: "ASUS",
       registrationUrl: "https://www.google.com"
     }
   ];*/

  events: EventInfo[];

  constructor(private eventSvc: EventService) { }

  ngOnInit() {
    this.eventSvc.getEvents()
      .subscribe(res => this.events = res, //Success CallBack
        error => console.log(error) //Error Callback
      );
  }

}
