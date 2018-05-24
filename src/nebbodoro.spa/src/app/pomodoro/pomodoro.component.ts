import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {Observable} from 'rxjs/Rx';
import {RoundProgressEase} from 'angular-svg-round-progressbar';

import { HttpServiceService } from './http-service.service';
import { IPomodoro } from '../models/Pomodoro';
import { map } from 'rxjs/operator/map';
import { RuntimeSettingService } from '../runtime-setting.service';

@Component({
  selector: 'app-pomodoro',
  templateUrl: './pomodoro.component.html',
  styleUrls: ['./pomodoro.component.scss']
})
export class PomodoroComponent implements OnInit {

  current: number = 0;
  max: number = 25;
  stroke: number = 15;
  radius: number = 125;
  semicircle: boolean = false;
  rounded: boolean = true;
  responsive: boolean = false;
  clockwise: boolean = true;
  color: string = '#72BD49';
  background: string = '#404040';
  duration: number = 800;
  animation: string = 'easeOutCubic';
  animationDelay: number = 0;
  animations: string[] = [];
  gradient: boolean = false;
  realCurrent: number = 0;
  currentUserEmail: string = 'esteban@nebbiatech.com';

  pomodoros: IPomodoro[];
  
  constructor(private _ease: RoundProgressEase, private route: ActivatedRoute, private router: Router, private http: HttpServiceService, private runtimeSettingService: RuntimeSettingService) {    
    for (let prop in _ease) {
      if (prop.toLowerCase().indexOf('ease') > -1) {
        this.animations.push(prop);
      };
    }
  }

  increment(amount = 1) {
    this.current += amount;
  }

  getOverlayStyle() {
    let isSemi = this.semicircle;
    let transform = (isSemi ? '' : 'translateY(-10%) ') + 'translateX(-10%)';

    return {
      'top': isSemi ? 'auto' : '10%',
      'bottom': isSemi ? '5%' : 'auto',
      'left': '10%',
      'transform': transform,
      '-moz-transform': transform,
      '-webkit-transform': transform,
      'font-size': this.radius / 3.5 + 'px'
    };
  }

  ngOnInit() {
    if (this.runtimeSettingService.isInitialized){
      this.http.getPomodoros(this.currentUserEmail).subscribe(data => {
        this.pomodoros = data;
        console.log(data);
      });
    }
    else {
      setTimeout(() => {
        this.http.getPomodoros(this.currentUserEmail).subscribe(data => {
          this.pomodoros = data;
          console.log(data);
        });
      }, 500);
    }
    
  }


  //Timer Component Methods
  /// Start the timer
  start() {
    const interval = Observable.interval(100);
    
    interval
      .takeWhile(_ => !this.isFinished )
      .do(i => this.current += 0.1)
      .subscribe(() => {
        if (this.isFinished)
        {
          console.info('Pomodoro done');
          this.http.postPomodoroDone(this.currentUserEmail).subscribe();
        }
      });
  }
   /// finish timer
  finish() {
    this.current = this.max;
  }
  /// reset timer
  reset() {
    this.current = 0;
  }
  /// Getters to prevent NaN errors
  get maxVal() {
    return isNaN(this.max) || this.max < 0.1 ? 0.1 : this.max;
  }

  get currentVal() {
    return isNaN(this.current) || this.current < 0 ? 0 : this.current;
  }
  get isFinished() {
    return this.currentVal >= this.maxVal;
  }

}
