import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';


import {RoundProgressEase} from 'angular-svg-round-progressbar';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  current: number = 10;
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

  constructor(private _ease: RoundProgressEase, private route: ActivatedRoute, private router: Router) {    
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
    let transform = (isSemi ? '' : 'translateY(-50%) ') + 'translateX(-50%)';

    return {
      'top': isSemi ? 'auto' : '50%',
      'bottom': isSemi ? '5%' : 'auto',
      'left': '50%',
      'transform': transform,
      '-moz-transform': transform,
      '-webkit-transform': transform,
      'font-size': this.radius / 3.5 + 'px'
    };
  }

  ngOnInit() {
  }

  gotoPomodoro(){
    this.router.navigate(['pomodoro']);
  }

}
