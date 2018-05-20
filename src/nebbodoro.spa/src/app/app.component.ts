import { Component } from '@angular/core';
import { RuntimeSettingService } from './runtime-setting.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  /**
   *
   */
  constructor(private runTimeSettingService: RuntimeSettingService) {
    this.runTimeSettingService.init();
  }
}
