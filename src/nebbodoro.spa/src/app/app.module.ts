import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { RuntimeSettingService } from './runtime-setting.service';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule
  ],
  providers: [
    RuntimeSettingService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
