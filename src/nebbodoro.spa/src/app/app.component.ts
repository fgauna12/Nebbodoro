import { Component, OnInit } from '@angular/core';
import { environment } from '../environments/environment';
import { RuntimeSettingService } from './runtime-setting.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  
  title = 'app';
  apiUrl = 'nothing';

  constructor(private runtTimeSettings: RuntimeSettingService){

  }

  ngOnInit(): void {
    this.runtTimeSettings.load().then(env => {
      this.apiUrl = environment.runTimeSettings.apiUrl || env.apiUrl;
    });    
  }
}
