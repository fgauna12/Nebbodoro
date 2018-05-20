import { BrowserModule } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { NgModule, NO_ERRORS_SCHEMA, APP_INITIALIZER } from '@angular/core';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { RoundProgressModule } from 'angular-svg-round-progressbar';

import { HttpServiceService } from './pomodoro/http-service.service';
import { AppComponent } from './app.component';
import { PomodoroComponent } from './pomodoro/pomodoro.component';
import { HomeComponent } from './home/home.component';
import { RuntimeSettingService } from './runtime-setting.service';
import { FormsModule } from '@angular/forms';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'home', component: HomeComponent },
  { path: 'pomodoro', component: PomodoroComponent }
];


@NgModule({
  declarations: [
    AppComponent,
    PomodoroComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    MDBBootstrapModule.forRoot(),
    RoundProgressModule,
    RouterModule.forRoot(routes),
    HttpClientModule,
    FormsModule
  ],
  providers: [
    HttpServiceService,
    RuntimeSettingService   
  ],
  bootstrap: [AppComponent]
})


export class AppModule {
  
}
