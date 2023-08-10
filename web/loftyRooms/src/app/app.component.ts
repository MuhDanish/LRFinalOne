import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { GenericServiceService } from './common/services/generic-service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'loftyRooms';
  constructor(private loader:GenericServiceService){
  }
  IsShowLoader(){
    return this.loader.getLoading();
  }
}
