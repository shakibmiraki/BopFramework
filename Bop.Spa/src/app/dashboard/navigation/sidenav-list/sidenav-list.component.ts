import { UserService } from 'src/app/core/services/user.service';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-sidenav-list',
  templateUrl: './sidenav-list.component.html',
  styleUrls: ['./sidenav-list.component.css']
})
export class SidenavListComponent implements OnInit {
  @Output() drawerClose = new EventEmitter();

  constructor(private userService: UserService) {
  }

  ngOnInit() {}

  public onDrawerClose = () => {
    this.drawerClose.emit();
  };
}
