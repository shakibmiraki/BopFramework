import { Component, OnInit } from '@angular/core';
import * as AOS from 'aos';

@Component({
  selector: 'app-home',
  templateUrl: './blank.component.html',
  styleUrls: ['./blank.component.scss']
})
export class BlankComponent implements OnInit {
  ngOnInit(): void {
    AOS.init({
      duration: 900,
      delay: 100,
      once: true
    });
  }
}
