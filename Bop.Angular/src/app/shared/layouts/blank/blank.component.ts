import { Component, OnInit } from '@angular/core';
import * as AOS from 'aos';

@Component({
  selector: 'app-blank-layout',
  templateUrl: './blank.component.html',
  styleUrls: ['./blank.component.scss']
})
export class BlankComponent implements OnInit {
  ngOnInit(): void {
    console.log('blank component');
    AOS.init({
      duration: 900,
      delay: 100,
      once: true
    });
  }
}
