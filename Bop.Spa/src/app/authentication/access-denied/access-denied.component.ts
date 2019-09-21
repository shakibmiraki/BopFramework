import { UserService } from 'src/app/core/services/user.service';
import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-access-denied',
  templateUrl: './access-denied.component.html',
  styleUrls: ['./access-denied.component.scss']
})
export class AccessDeniedComponent implements OnInit {
  isAuthenticated = false;

  constructor(private location: Location, private userService: UserService) {}

  ngOnInit() {
    this.isAuthenticated = this.userService.isAuthUserLoggedIn();
  }

  goBack() {
    this.location.back(); // <-- go back to previous location on cancel
  }
}
