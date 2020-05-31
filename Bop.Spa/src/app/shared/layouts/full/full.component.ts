import { Component, OnInit } from "@angular/core";
import * as AOS from "aos";
import { Router } from "@angular/router";

@Component({
  selector: "app-full-layout",
  templateUrl: "./full.component.html",
  styleUrls: ["./full.component.scss"],
})
export class FullComponent implements OnInit {
  constructor(public router: Router) {}

  ngOnInit(): void {
    console.log('full component');
    AOS.init({
      duration: 900,
      delay: 100,
      once: true,
    });

    if (this.router.url === "/") {
      this.router.navigate(["/dashboard"]);
    }
  }
}
